"""Coordinate hosted API export artifacts with a trusted completed baseline."""

import argparse
import json
import os
import re
import subprocess
from pathlib import Path

from github_artifacts import repository_pages


def inventory(revision):
    entries = subprocess.check_output(["git", "ls-tree", "-rz", revision])
    result = {}
    for entry in entries.split(b"\0"):
        if not entry:
            continue
        metadata, name = entry.split(b"\t", 1)
        mode, kind, identity = metadata.decode("ascii").split()
        path = name.decode("utf-8")
        if kind not in ("blob", "commit") or not re.fullmatch("[0-9a-f]{40}", identity):
            raise ValueError(f"Invalid Git input: {path}")
        if (path.startswith("benchmark/") or path in ("README.md", "CONTRIBUTING.md", "LICENSE")
                or re.fullmatch(r"\.agents/records/.*\.(md|png|jpg|json|csv|trx|log)", path)):
            continue
        result[path] = identity
    if not result:
        raise ValueError("API input discovery is empty")
    return result


def trusted_baseline():
    runs = repository_pages("actions/workflows/documentation-api.yml/runs", key="workflow_runs",
                            filters={"branch": "master", "event": "push", "status": "success"})
    for run in runs:
        if (run["head_repository"]["full_name"] != "Wolfiora/InnoEngine"
                or run["path"] != ".github/workflows/documentation-api.yml"
                or str(run["id"]) == os.environ["GITHUB_RUN_ID"]):
            continue
        run_path = f"actions/runs/{run['id']}"
        attempt = run["run_attempt"]
        jobs = repository_pages(f"{run_path}/attempts/{attempt}/jobs", key="jobs")
        if sum(job["name"] == "API export validation" and job["conclusion"] == "success" for job in jobs) != 1:
            continue
        packages = repository_pages(f"{run_path}/artifacts", key="artifacts")
        if sum(item["name"] == f"documentation-api-package-{attempt}" and not item["expired"]
               and bool(re.fullmatch("sha256:[0-9a-f]{64}", item.get("digest") or "")) for item in packages) == 1:
            baseline = run["head_sha"]
            if not re.fullmatch("[0-9a-f]{40}", baseline):
                raise ValueError("Invalid baseline revision")
            return baseline
    return None


def preflight():
    revision = subprocess.check_output(["git", "rev-parse", "HEAD"], text=True).strip()
    current = inventory(revision)
    baseline = trusted_baseline() if os.getenv("GH_TOKEN") else None
    changed = []
    if baseline:
        subprocess.run(["git", "fetch", "--depth=1", "--no-tags", "origin", baseline], check=True)
        previous = inventory(baseline)
        changed = sorted(path for path in current.keys() | previous.keys() if current.get(path) != previous.get(path))
    required = baseline is None or bool(changed)
    status = "EvaluationRequired" if required else "NoRelevantChanges"
    if baseline is None:
        reason = "No trusted complete export baseline."
    elif changed:
        reason = "API input files or membership changed."
    else:
        reason = "The input manifest matches the trusted export."
    result = {"schema": "inno-api-preflight/1", "revision": revision, "baseline": baseline, "inputs": current,
              "status": status, "changed": changed, "reason": reason}
    destination = Path("artifacts/documentation-export/preflight.json")
    destination.parent.mkdir(parents=True, exist_ok=True)
    destination.write_text(json.dumps(result), encoding="utf-8")
    if "GITHUB_OUTPUT" in os.environ:
        with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
            print(f"required={str(required).lower()}", file=stream)
    print(f"{status}: {reason}")


ROOT = Path(__file__).resolve().parents[2]
ENGINE = "https://github.com/Wolfiora/InnoEngine"
DOCS = ENGINE + ".Docs.git"
EXPORT = ROOT / "artifacts/documentation-export"
CLI_PROJECT = ROOT / "tools/Inno.Tooling.Cli/Inno.Tooling.Cli.csproj"


def git(directory, *args, env=None):
    return subprocess.check_output(["git", "-C", directory, *args], env=env, encoding="utf-8").strip()


def ancestor(directory, older, newer):
    status = subprocess.run(["git", "-C", directory, "merge-base", "--is-ancestor", older, newer])
    if status.returncode not in (0, 1):
        status.check_returncode()
    return status.returncode == 0


def collect():
    commit = os.environ["EXPECTED_COMMIT"]
    if git(ROOT, "rev-parse", "HEAD") != commit:
        raise ValueError("The collection checkout differs from its producers")
    subprocess.run(["dotnet", "run", "--project", CLI_PROJECT, "-c", "Release", "--", "docs", "collect-producers", ROOT,
                    EXPORT / "producers", EXPORT / "package", commit, git(ROOT, "rev-parse", "HEAD^{tree}"),
                    "Debug", "Release"], check=True)


def publish(engine, assembly, package, workspace):
    packet = json.loads(package.read_bytes())
    revision = packet["revision"]
    if (packet["schema"] != "inno-api-package/1" or packet["repository"] != ENGINE
            or not re.fullmatch("[a-f0-9]{40}", revision) or not re.fullmatch("[a-f0-9]{40}", packet["sourceTree"])
            or git(engine, "config", "--get", "remote.origin.url") not in {ENGINE, ENGINE + ".git"}):
        raise ValueError("Invalid producer identity or engine origin")
    if not assembly.is_file():
        raise FileNotFoundError(assembly)

    def current_master():
        git(engine, "fetch", "--no-tags", "origin", "master")
        if git(engine, "rev-parse", "FETCH_HEAD") != revision:
            raise ValueError("The package was superseded by newer master inputs")

    current_master()
    if git(engine, "rev-parse", revision + "^{tree}") != packet["sourceTree"]:
        raise ValueError("API package has the wrong source tree")
    workspace.mkdir(parents=True, exist_ok=False)
    hooks = workspace / "disabled-hooks"
    hooks.mkdir()
    git_env = os.environ.copy()
    token = git_env.pop("DOCS_TOKEN", None)
    if token:
        import base64
        index = int(git_env.get("GIT_CONFIG_COUNT", 0))
        header = base64.b64encode(("x-access-token:" + token).encode()).decode()
        git_env.update({"GIT_CONFIG_COUNT": str(index + 1), f"GIT_CONFIG_KEY_{index}": f"http.{DOCS}.extraheader",
                        f"GIT_CONFIG_VALUE_{index}": "Authorization: Basic " + header})
    tool_env = os.environ.copy()
    tool_env.pop("DOCS_TOKEN", None)

    def docs(*args, stdout=None):
        subprocess.run(["dotnet", assembly, "docs", *args], check=True, env=tool_env, stdout=stdout)

    for attempt in range(1, 4):
        checkout = workspace / f"attempt-{attempt}"
        git(workspace, "-c", f"core.hooksPath={hooks}", "clone", "--no-checkout", "--single-branch", "--branch",
            "draft", DOCS, checkout, env=git_env)
        git(checkout, "config", "core.hooksPath", str(hooks))
        git(checkout, "config", "user.name", "InnoEngine API synchronization")
        git(checkout, "config", "user.email", "innoengine-api@users.noreply.github.com")
        git(checkout, "checkout", "draft")
        expected = git(checkout, "rev-parse", "HEAD")
        lock = checkout / "reference/engine.lock.json"
        if lock.exists():
            retained = workspace / f"retained-{attempt}.json"
            docs("api-package", "--docs-root", checkout, "--package", retained)
            if json.loads(retained.read_bytes())["contentIdentity"] == packet["contentIdentity"]:
                print("NoRelevantChanges: complete API facts match.")
                return
        git(checkout, "fetch", "--no-tags", "origin", "main:refs/remotes/origin/main", env=git_env)
        if not ancestor(checkout, "origin/main", "HEAD"):
            git(checkout, "merge", "--no-edit", "origin/main")
        if lock.exists():
            binding = json.loads(lock.read_bytes())
            if (binding["repository"] != ENGINE or not re.fullmatch("[0-9a-f]{40}", binding["revision"])
                    or not ancestor(engine, binding["revision"], revision)):
                raise ValueError("Unrelated or invalid retained engine binding")
        docs("api-import", "--docs-root", checkout, "--package", package)
        with (workspace / f"editorial-{attempt}.json").open("xb") as result:
            docs("api-check", "--docs-root", checkout, stdout=result)
        names = subprocess.check_output(["git", "-C", checkout, "ls-files", "-z", "--modified", "--deleted",
                                         "--others", "--exclude-standard"]).split(b"\0")
        for name in filter(None, names):
            if not re.fullmatch(rb"(content/en/.+\.md|reference/(api|projects)/.+\.json|reference/engine\.lock\.json)", name):
                raise ValueError(f"Unexpected independent file in API synchronization: {name!r}")
        if any(names):
            git(checkout, "add", "--", "content/en", "reference/api", "reference/projects", "reference/engine.lock.json")
            git(checkout, "commit", "-m", "docs(api): synchronize engine " + revision[:12])
        candidate = git(checkout, "rev-parse", "HEAD")
        if candidate == expected:
            return
        current_master()
        push = subprocess.run(["git", "-C", checkout, "push", "origin", "HEAD:refs/heads/draft"], env=git_env)
        if push.returncode == 0:
            print(f"Published draft {candidate} from engine {revision}.")
            return
        git(checkout, "fetch", "--no-tags", "origin", "draft", env=git_env)
        head = git(checkout, "rev-parse", "FETCH_HEAD")
        if head == candidate:
            return
        if head == expected:
            push.check_returncode()
        print(f"Recompute ownership after concurrent draft edit {head}.")
    raise RuntimeError("Draft changed during three publication attempts")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("operation", choices=["preflight", "collect", "publish"])
    parser.add_argument("--engine-root", type=Path, default=ROOT)
    parser.add_argument("--assembly", type=Path, default=CLI_PROJECT.parent / "bin/Release/net10.0/Inno.Tooling.Cli.dll")
    parser.add_argument("--package", type=Path, default=EXPORT / "package/api-package.json")
    parser.add_argument("--workspace", type=Path, default=EXPORT / "publication")
    args = parser.parse_args()
    if args.operation == "preflight":
        preflight()
    elif args.operation == "collect":
        collect()
    else:
        publish(args.engine_root.resolve(), args.assembly.resolve(), args.package.resolve(), args.workspace.resolve())
