"""Reconcile the required check with the routing decision and platform jobs."""

import argparse
from collections import Counter
from datetime import datetime
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import subprocess

from github_artifacts import repository_json, repository_pages


ROOT = Path(__file__).resolve().parents[2]
PLAN = Path("artifacts/validation/ci-plan")
BOOTSTRAP = Path("artifacts/validation/bootstrap/outputs")
RECEIVED = Path("artifacts/validation/received")


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def digest(path):
    with path.open("rb") as stream:
        return hashlib.file_digest(stream, "sha256").hexdigest()


def publish(name, value):
    with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
        print(f"{name}={value}", file=stream)


def append_summary(message):
    if "GITHUB_STEP_SUMMARY" in os.environ:
        with open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as stream:
            print(message, file=stream)


def run_logged(command, path):
    with path.open("wb") as log:
        result = subprocess.run(command, stdout=log, stderr=subprocess.STDOUT)
    if result.returncode:
        print(path.read_text(encoding="utf-8", errors="replace")[-12000:], flush=True)
        result.check_returncode()


def workloads(plan, sdk):
    targets = read(ROOT / ".github/native-targets.json")
    registered = {item["target"]: item["workloads"] for item in targets}
    if len(registered) != len(targets):
        raise ValueError("Duplicate native target registration")
    required = {name for request in (plan["nativeRequests"] or []) for name in registered[request["target"]]}
    if not required:
        return False
    installed = json.loads(subprocess.check_output([sdk, "workload", "list", "--machine-readable"]))["installed"]
    missing = sorted(required - set(installed))
    if missing:
        subprocess.run([sdk, "workload", "install", *missing, "--skip-manifest-update", "--disable-parallel"], check=True)
    return bool(missing)


def freeze():
    commit = os.environ["EXPECTED_COMMIT"]
    if subprocess.check_output(["git", "rev-parse", "HEAD"], text=True).strip() != commit:
        raise ValueError("Checkout differs from the requested candidate")
    sdk = shutil.which("dotnet")
    if sdk is None:
        raise RuntimeError("Missing selected SDK")
    profile = os.environ["PROFILE"]
    base = os.environ["BASE_REVISION"]
    if os.environ["GITHUB_EVENT_NAME"] == "workflow_dispatch" and profile == "pull-request":
        base = "HEAD^"
    elif base == "0" * 40:
        base = "origin/master"
    scheduled = os.environ.get("SCHEDULED") == "true"
    matched = False
    plan_path = PLAN / "plan.json"
    project = ROOT / "tools/Inno.Tooling.Cli/Inno.Tooling.Cli.csproj"
    entry = BOOTSTRAP / "bin/Inno.Tooling.Cli/debug/Inno.Tooling.Cli.dll"
    if scheduled:
        plan = read(plan_path)
        merged = subprocess.check_output(["git", "merge-base", base, "HEAD"], text=True).strip()
        if plan["baseline"] != merged or plan["commit"] != commit:
            raise ValueError("Scheduled plan has a different baseline or candidate")
        receipt = read(PLAN / "entry.json")
        if Path(receipt["assembly"]).resolve() != entry.resolve() or digest(entry).upper() != receipt["sha256"]:
            raise ValueError("Scheduled validation entry changed")
        workloads(plan, sdk)
        status = subprocess.run([sdk, entry, "validate", "plan", "--engine-root", ROOT, "--dotnet", sdk,
                                 "--plan", plan_path, "--profile", profile, "--output", PLAN]).returncode
        if status not in (0, 3):
            raise RuntimeError(f"Cannot bind scheduled plan ({status})")
        matched = status == 0
        if not matched:
            plan_path.rename(PLAN / "plan-before-environment-change.json")
    else:
        PLAN.mkdir(parents=True, exist_ok=False)
        run_logged([sdk, "build", project, "-c", "Debug", "--artifacts-path", BOOTSTRAP.resolve(),
                    "--disable-build-servers", "-m:4", "-nr:false"], PLAN / "bootstrap.log")
        (PLAN / "entry.json").write_text(json.dumps({"assembly": entry.as_posix(), "sha256": digest(entry).upper()}), encoding="utf-8")
    if not matched:
        command = [sdk, entry, "validate", "plan", "--engine-root", ROOT, "--dotnet", sdk, "--base", base,
                   "--output", PLAN, "--profile", profile, "--shared-managed", str(os.name == "nt"),
                   "--backend-store", "artifacts/b"]
        if os.getenv("REBUILD_BACKENDS"):
            command.extend(["--rebuild-backends", os.environ["REBUILD_BACKENDS"]])
        run_logged(command, PLAN / "planning.log")
        plan = read(plan_path)
        if workloads(plan, sdk):
            shutil.copyfile(plan_path, PLAN / "plan-before-workloads.json")
            run_logged(command, PLAN / "planning-prepared.log")
            plan = read(plan_path)
    if not plan["checks"] or not plan["shards"]:
        raise ValueError("Required hosted validation discovery is empty")
    active = [item for item in plan["checks"] if not item["exclusion"]]
    tooling = [item for item in active if item["kind"] == "ci-contracts" or
               item["kind"] == "test" and item["owner"].startswith(("tests/tools/", "tests/build/"))]
    engine = [item for item in active if item["kind"] in ("products", "test") and item not in tooling]
    label = "Engine tests and tooling tests" if engine and tooling else "Engine tests" if engine else "Tooling tests" if tooling else "Repository checks"
    publish("execution_label", label)
    publish("native", str(bool(plan["nativeRequests"])).lower())
    publish("shards", json.dumps([item["id"] for item in plan["shards"]]))
    append_summary(f"### Plan for {os.environ['PLATFORM']}\n\n" + "\n".join(f"- {item['owner']} ({item['kind']})" for item in engine + tooling))


def artifact(name):
    attempt = int(os.environ["GITHUB_RUN_ATTEMPT"])
    matches = {}
    for path in RECEIVED.iterdir():
        match = re.fullmatch(re.escape(name) + r"-attempt-([1-9][0-9]*)", path.name)
        if match and path.is_dir():
            identity = int(match[1])
            if identity > attempt or identity in matches:
                raise ValueError(f"Invalid attempt for {path.name}")
            matches[identity] = path
    if not matches:
        raise FileNotFoundError(f"Missing artifact {name}")
    return matches[max(matches)]


def join():
    verify_route()
    if os.environ["SHOULD_RUN"] == "false":
        return
    hosts = [item["runner"] for item in json.loads(os.environ["HOSTS"])["include"]]
    if not hosts or len(hosts) != len(set(hosts)):
        raise ValueError("The required host matrix is empty or duplicated")
    for host in hosts:
        planned = artifact(f"validation-plan-{host}")
        certified = artifact(f"validation-joined-{host}")
        plan = read(planned / "plan.json")
        proof = read(certified / "receipt.json")
        entry = read(planned / "entry.json")
        attempt = int(certified.name.rsplit("-attempt-", 1)[1])
        if (int(planned.name.rsplit("-attempt-", 1)[1]) != attempt or proof["attempt"] != attempt
                or str(proof["run"]) != os.environ["GITHUB_RUN_ID"]):
            raise ValueError(f"Plan and certification were uploaded by different runs or attempts: {host}")
        if (proof["platform"] != host or proof["commit"] != os.environ["EXPECTED_COMMIT"]
                or plan["commit"] != proof["commit"] or not re.fullmatch("[A-F0-9]{64}", entry["sha256"])
                or proof["executor"] != entry["sha256"] or proof["planIdentity"] != digest(planned / "plan.json")
                or proof["resultsIdentity"] != digest(certified / "results.json")):
            raise ValueError(f"Changed or foreign certification for {host}")
        shards = [item["id"] for item in plan["shards"]]
        hashes = {item["id"]: item["sha256"] for item in proof["shards"]}
        if not shards or len(set(shards)) != len(shards) or len(proof["shards"]) != len(shards) or set(shards) != hashes.keys():
            raise ValueError(f"Different shard matrix for {host}")
        for shard in shards:
            receipt = artifact(f"validation-result-{host}-{shard}") / "results.json"
            if int(receipt.parent.name.rsplit("-attempt-", 1)[1]) != attempt or digest(receipt) != hashes[shard]:
                raise ValueError(f"The joined receipt differs from the uploaded {host}/{shard} result")
        append_summary(f"{host}: {proof['passed']} passed, {proof['reused']} reused, {proof['notApplicable']} not applicable.")


def summarize():
    lines = ["## Validation evidence", ""]
    for item in json.loads(os.environ["HOSTS"])["include"]:
        host = item["runner"]
        lines.append(f"### {host}")
        try:
            plan = read(artifact(f"validation-plan-{host}") / "plan.json")
            for stage, seconds in (plan.get("planningSeconds") or {}).items():
                lines.append(f"- {stage}: {seconds:.2f} s")
            lines.extend(plan.get("diagnostics", []))
            observed = {}
            failures = []
            for shard in plan["shards"]:
                try:
                    evidence = read(artifact(f"validation-result-{host}-{shard['id']}") / "results.json")
                    for failure in evidence["failures"]:
                        failures.append(f"- {failure['stage']}: {failure['detail']} ({failure.get('log', '')})")
                    for result in evidence["results"]:
                        if result["id"] in observed:
                            raise ValueError(f"Duplicate check result: {result['id']}")
                        observed[result["id"]] = result
                except (OSError, KeyError, ValueError) as error:
                    failures.append(f"- {shard['id']}: complete results unavailable: {error}")
            checks = plan["checks"]
            required = sum(check.get("exclusion") is None for check in checks)
            statuses = Counter(result["status"] for result in observed.values())
            lines.append(f"Required checks: {required}. Reused results: {statuses['Reused']}. Inapplicable checks: {statuses['NotApplicable']}.")
            timings = Counter()
            for check in checks:
                result = observed.get(check["id"])
                if result is None:
                    lines.append(f"- {check['id']}: Missing")
                    continue
                status = result["status"]
                timings[check["kind"]] += result.get("seconds", 0)
                if check["kind"] == "test" and not check.get("exclusion"):
                    cases = 0 if status == "Reused" else result.get("testCases")
                    duration = 0 if status == "Reused" else result.get("caseSeconds")
                    lines.append(f"- {check['owner']}: {status}, cases={cases}, case-seconds={duration}")
                if status in ("Failed", "Blocked"):
                    detail = result["detail"]
                    if len(detail) > 4000:
                        detail = detail[:500] + " … see results.json … " + detail[-3000:]
                    failures.append(f"- {check['id']}: {detail}")
            lines.extend(f"- {kind}: {seconds:.1f} check-seconds" for kind, seconds in sorted(timings.items()))
            lines.extend(failures)
        except (OSError, KeyError, ValueError) as error:
            lines.append(f"- Planning evidence unavailable: {error}")
    if os.getenv("GH_TOKEN"):
        endpoint = f"actions/runs/{os.environ['GITHUB_RUN_ID']}/attempts/{os.environ['GITHUB_RUN_ATTEMPT']}"
        try:
            start = datetime.fromisoformat(repository_json(endpoint)["created_at"])
            completed = sorted((job for job in repository_pages(endpoint + "/jobs", key="jobs")
                                if job.get("completed_at") and job.get("started_at") and job["conclusion"] != "skipped"),
                               key=lambda job: job["started_at"])
            busy = total = 0
            end = start
            for job in completed:
                left = datetime.fromisoformat(job["started_at"])
                right = datetime.fromisoformat(job["completed_at"])
                total += (right - left).total_seconds()
                busy += max(0, (right - max(end, left)).total_seconds())
                end = max(right, end)
                for step in job["steps"]:
                    if step["conclusion"] == "skipped" or not step.get("completed_at") or not step.get("started_at"):
                        continue
                    duration = datetime.fromisoformat(step["completed_at"]) - datetime.fromisoformat(step["started_at"])
                    lines.append(f"- [{job['name']} / {step['name']}]({job['html_url']}#step:{step['number']}:1): "
                                 f"{step['conclusion']} ({duration.total_seconds():.1f} s)")
            if completed:
                queued = (datetime.fromisoformat(completed[0]["started_at"]) - start).total_seconds()
                lines.append(f"Elapsed {(end - start).total_seconds():.0f} s, active runners {busy:.0f} s, "
                             f"cumulative {total / 60:.1f} runner-minutes, first runner started after {queued:.0f} s.")
        except (OSError, KeyError, ValueError) as error:
            lines.append(f"Hosted job timing unavailable: {error}")
    append_summary("\n\n".join(lines))


def run_workers():
    from concurrent.futures import ThreadPoolExecutor, as_completed

    host = os.environ["VALIDATION_PLATFORM"]
    commit = os.environ["GITHUB_SHA"]
    plan = read(PLAN / "plan.json")
    if (plan["commit"] != commit or
            subprocess.check_output(["git", "rev-parse", "HEAD"], text=True).strip() != commit):
        raise ValueError("Worker checkout differs from the planned candidate")
    shards = {item["id"]: item for item in plan["shards"]}
    if not shards or len(shards) != len(plan["shards"]) or shards.keys() - {"architecture", "managed", "native"}:
        raise ValueError("Unexpected frozen shard matrix")
    worker_root = Path(os.environ["RUNNER_TEMP"]).resolve() / "inno-shards"
    worker_root.mkdir(exist_ok=False)
    entry = read(PLAN / "entry.json")
    assembly = Path(entry["assembly"])
    if not (ROOT / assembly).resolve().is_relative_to((ROOT / BOOTSTRAP / "bin").resolve()) or digest(ROOT / assembly).upper() != entry["sha256"]:
        raise ValueError("Frozen entry does not match its registered output")
    sdk = shutil.which("dotnet")
    completed = {}
    failed = {}

    def copy_inputs(checkout):
        subprocess.run(["git", "worktree", "add", "--detach", checkout, commit], check=True)
        shutil.copytree(ROOT / PLAN, checkout / PLAN)
        shutil.copytree((ROOT / assembly).parent, (checkout / assembly).parent)
        if not plan["backendStore"] or not plan.get("nativeDependencies"):
            return
        store = (ROOT / plan["backendStore"]).resolve()
        if not store.is_relative_to(ROOT / "artifacts"):
            raise ValueError("Backend inputs escaped their owner")
        files = ["selection.json"]
        for package in plan["backendPackages"]:
            target = package["target"]
            if not re.fullmatch("[a-z0-9-]+", target):
                raise ValueError("Invalid frozen backend target")
            manifest = store / target / "manifest.json"
            if digest(manifest) != package["manifestSha256"]:
                raise ValueError(f"Backend manifest changed: {target}")
            files.extend([f"{target}/receipt.json", f"{target}/manifest.json"])
            files.extend(f"{target}/{item['path']}" for item in read(manifest)["files"])
        for name in files:
            source = (store / name).resolve()
            if not source.is_relative_to(store):
                raise ValueError("Backend input escaped its manifest")
            output = checkout / store.relative_to(ROOT) / source.relative_to(store)
            output.parent.mkdir(parents=True, exist_ok=True)
            if source.drive == output.drive:
                try:
                    os.link(source, output)
                    continue
                except OSError as error:
                    if error.errno != 18:  # EXDEV for separate mounted volumes
                        raise
            shutil.copyfile(source, output)

    def execute(shard, checkout):
        name = shard["id"]
        results = checkout / "artifacts/validation/shards" / name
        results.mkdir(parents=True, exist_ok=False)
        prerequisites = worker_root / (name + "-prerequisites")
        prerequisites.mkdir()
        for dependency in shard["prerequisites"]:
            folder = prerequisites / dependency
            folder.mkdir()
            shutil.copyfile(completed[dependency], folder / "results.json")
        env = os.environ.copy()
        env["GITHUB_WORKSPACE"] = str(checkout)
        if plan["backendStore"]:
            env.update(InnoBackendPackageDirectory=str(checkout / plan["backendStore"]), InnoBackendHostTarget=plan["backendHost"])
            if plan.get("nativeDependencies"):
                env["InnoNativeDependencyManifest"] = str(checkout / plan["nativeDependencies"])
        command = [sdk, checkout / assembly, "validate", "run", "--engine-root", checkout, "--dotnet", sdk,
                   "--plan", checkout / PLAN / "plan.json", "--output", results, "--shard", name]
        if shard["prerequisites"]:
            command.extend(["--prerequisites", prerequisites])
        with (results / "validation.log").open("wb") as log:
            with subprocess.Popen(command, cwd=checkout, env=env, stdout=subprocess.PIPE, stderr=subprocess.STDOUT) as child:
                for line in child.stdout:
                    log.write(line)
                    print(f"[{name}] {line.decode('utf-8', errors='replace').rstrip()}", flush=True)
                if child.wait() != 0:
                    raise RuntimeError(f"{name} failed; see {results / 'validation.log'}")
        if subprocess.check_output(["git", "status", "--porcelain", "--untracked-files=all"], cwd=checkout).strip():
            raise ValueError(f"Shard {name} changed repository inputs")
        receipt = results / "results.json"
        if not receipt.is_file():
            raise FileNotFoundError(receipt)
        return receipt

    while remaining := (shards.keys() - completed.keys() - failed.keys()):
        ready = sorted((name for name in remaining if set(shards[name]["prerequisites"]) <= completed.keys()),
                       key=lambda name: (name != "native", name))
        if not ready:
            raise RuntimeError(f"Blocked shards {sorted(remaining)}; first failures: {failed}")
        with ThreadPoolExecutor(max_workers=len(ready)) as pool:
            jobs = {}
            for index, name in enumerate(ready):
                checkout = ROOT if index == 0 else worker_root / name
                if index:
                    copy_inputs(checkout)
                publish(f"{name}_directory", str(checkout / "artifacts/validation/shards" / name))
                jobs[pool.submit(execute, shards[name], checkout)] = name
            results = {}
            for job in as_completed(jobs):
                name = jobs[job]
                try:
                    results[name] = job.result()
                except Exception as error:
                    failed[name] = str(error)
            completed.update(results)
    if failed:
        raise RuntimeError(f"Validation failed: {failed}")
    inputs = worker_root / "completed"
    for name, result in completed.items():
        (inputs / name).mkdir(parents=True)
        shutil.copyfile(result, inputs / name / "results.json")
    output = ROOT / "artifacts/validation/host-certification"
    output.mkdir(parents=True, exist_ok=False)
    run_logged([sdk, ROOT / assembly, "validate", "join", "--engine-root", ROOT, "--dotnet", sdk,
                "--plan", PLAN / "plan.json", "--output", output, "--prerequisites", inputs], output / "join.log")
    evidence = read(output / "results.json")
    results = evidence["results"]
    receipt = {"run": os.environ["GITHUB_RUN_ID"], "attempt": int(os.environ["GITHUB_RUN_ATTEMPT"]), "commit": commit, "platform": host, "executor": entry["sha256"], "full": plan["full"],
               "tree": plan["tree"], "environment": plan["environment"], "planIdentity": digest(PLAN / "plan.json"),
               "resultsIdentity": digest(output / "results.json"),
               "shards": [{"id": name, "sha256": digest(result)} for name, result in completed.items()],
               "passed": sum(r["status"] == "Passed" for r in results), "reused": sum(r["status"] == "Reused" for r in results),
               "notApplicable": sum(r["status"] == "NotApplicable" for r in results)}
    (output / "receipt.json").write_text(json.dumps(receipt), encoding="utf-8")


def backend_test_receipt():
    plan = read(PLAN / "plan.json")
    required = {item["id"] for item in plan["checks"] if not item["exclusion"] and item["reusable"]
                and item["kind"] == "test" and item["owner"].startswith("tests/backends/")}
    if not required:
        return
    evidence = read(Path("artifacts/validation/host-certification/results.json"))
    tests = [item for item in evidence["results"] if item["id"] in required and item["status"] in ("Passed", "Reused")]
    if (evidence["version"] != 2 or evidence["failures"] != [] or not evidence["completed"]
            or len(tests) != len(required) or {item["id"] for item in tests} != required
            or evidence["environment"] != plan["environment"]):
        raise ValueError("Backend test certification is incomplete")
    proof = {"version": 2, "commit": plan["commit"], "environment": plan["environment"], "hostTarget": plan["backendHost"],
             "attempt": int(os.environ["GITHUB_RUN_ATTEMPT"]), "tests": tests}
    Path("artifacts/validation/backend-tests.json").write_text(json.dumps(proof), encoding="utf-8")
    publish("name", f"backend-tests-{plan['backendHost']}-{plan['environment']}")


def verify_route():
    if os.environ["ROUTE_RESULT"] != "success":
        raise RuntimeError("Event routing did not succeed")
    reason = os.environ["ROUTE_REASON"]
    run = os.environ["SHOULD_RUN"]
    platform = os.environ["PLATFORM_RESULT"]
    if run == "true" and reason == "validation":
        if platform != "success":
            raise RuntimeError(f"Required platform execution did not succeed ({platform})")
        return
    if run != "false" or platform != "skipped":
        raise RuntimeError("Unexpected platform result for this routing decision")
    event = os.environ["GITHUB_EVENT_NAME"]
    if reason == "benchmark-only" and event in ("push", "pull_request", "merge_group"):
        message = "Only manual benchmark inputs changed."
    elif reason == "open-pull-request" and event == "push" and os.environ["ROUTE_PULL"].isdecimal():
        message = f"Pull request #{os.environ['ROUTE_PULL']} owns this commit's validation."
    else:
        raise RuntimeError(f"Invalid skip reason for {event}: {reason}")
    with open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as stream:
        print(message, file=stream)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("operation", choices=["plan", "run", "join", "summary"])
    parser.add_argument("--backend-tests", action="store_true")
    args = parser.parse_args()
    {"plan": freeze, "run": backend_test_receipt if args.backend_tests else run_workers, "join": join, "summary": summarize}[args.operation]()
