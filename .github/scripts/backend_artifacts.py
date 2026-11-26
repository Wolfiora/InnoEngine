"""Receive a certified backend candidate from GitHub Actions."""

import argparse
import base64
import gzip
import io
import hashlib
import json
import os
import re
import shutil
import subprocess
import sys
import tempfile
import zipfile
from pathlib import Path

from github_artifacts import download, repository_json, repository_pages


ROOT = Path(__file__).resolve().parents[2]
PLANS = ROOT / "artifacts/validation/ci-plan"
REQUEST = ROOT / "artifacts/backend-request"
RELEASE = re.compile(r"backend-prebuilts-\d{4}\.\d{2}\.\d{2}\.[1-9]\d*")


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def write(path, value):
    path.write_text(json.dumps(value, separators=(",", ":")), encoding="utf-8")


def registrations():
    targets = read(ROOT / ".github/native-targets.json")
    registered = {item["target"]: item for item in targets}
    if len(registered) != len(targets):
        raise ValueError("Duplicate registered native target")
    return registered


def validate(*arguments):
    entry = read(PLANS / "entry.json")
    sdk = shutil.which("dotnet")
    if not sdk:
        raise RuntimeError("Missing selected .NET SDK")
    subprocess.run([sdk, entry["assembly"], "validate", *arguments], check=True)


def catalog(release):
    tag = release["tag_name"]
    if release["draft"] or not release["prerelease"] or not RELEASE.fullmatch(tag):
        raise ValueError("Only a published versioned prerelease can supply a backend catalog")
    ref = repository_json(f"git/ref/tags/{tag}")["object"]
    if ref["type"] != "tag" or not re.fullmatch("[0-9a-f]{40}", ref["sha"]):
        raise ValueError("Expected an annotated backend tag")
    obj = repository_json(f"git/tags/{ref['sha']}")
    message = obj["message"]
    prefix = "InnoEngine backend prebuilt catalog v1\n\n"
    if len(message) > 8 * 1024**2 or not message.startswith(prefix):
        raise ValueError("Invalid backend tag catalog header")
    encoded = base64.b64decode(message[len(prefix):].rstrip(), validate=True)
    with gzip.GzipFile(fileobj=io.BytesIO(encoded)) as compressed:
        body = compressed.read(32 * 1024**2 + 1)
    if len(body) > 32 * 1024**2:
        raise ValueError("Backend tag catalog exceeds its size limit")
    value = json.loads(body)
    if (value["version"] != 1 or not re.fullmatch("[0-9a-f]{40}", value["revision"]) or not value["platforms"]
            or obj["tag"] != tag or obj["object"]["type"] != "commit" or obj["object"]["sha"] != value["revision"]):
        raise ValueError("Backend tag metadata and source disagree")
    names = set()
    for target, platform in value["platforms"].items():
        asset = platform["asset"]
        if (not re.fullmatch("[a-z0-9-]+", target) or platform["target"] != target or asset in names
                or not re.fullmatch(r"[a-z0-9]+-prebuilt-backends\.zip", asset)
                or not re.fullmatch("[a-f0-9]{64}", platform["sha256"])):
            raise ValueError("Invalid catalog platform or asset identity")
        names.add(asset)
        matches = [item for item in release["assets"] if item["name"] == asset]
        if (len(matches) != 1 or matches[0]["state"] != "uploaded"
                or matches[0]["digest"] != "sha256:" + platform["sha256"]):
            raise ValueError(f"The release and catalog disagree about {asset}")
    if len(names) != len(release["assets"]):
        raise ValueError("The release has assets outside the catalog")
    return value["platforms"]


def test_evidence(plan):
    identity = plan["environment"]
    host = plan["backendHost"]
    if not re.fullmatch("[a-z0-9-]+", host) or not re.fullmatch("[a-f0-9]{64}", identity):
        raise ValueError("Invalid backend evidence scope")
    repository = int(os.environ["GITHUB_REPOSITORY_ID"])
    branch = read(Path(os.environ["GITHUB_EVENT_PATH"]))["repository"]["default_branch"]
    name = f"backend-tests-{host}-{identity}"
    inspected = 0
    limit = 4 * 1024**2
    for item in repository_pages("actions/artifacts", key="artifacts", filters={"name": name}):
        brief = item["workflow_run"]
        if (item["expired"] or item["name"] != name or not re.fullmatch("sha256:[0-9a-f]{64}", item.get("digest") or "")
                or item["size_in_bytes"] > limit or brief["head_branch"] != branch
                or brief["repository_id"] != repository or brief["head_repository_id"] != repository):
            continue
        inspected += 1
        if inspected > 3:
            break
        run = repository_json(f"actions/runs/{brief['id']}")
        if (run["conclusion"] != "success" or run["status"] != "completed" or run["head_sha"] != brief["head_sha"]
                or run["head_branch"] != branch or run["head_repository"]["id"] != repository
                or run["event"] not in ("push", "workflow_dispatch") or run["path"] != ".github/workflows/engine-ci.yml"):
            continue
        with tempfile.TemporaryDirectory() as directory:
            archive = Path(directory) / "tests.zip"
            download(f"actions/artifacts/{item['id']}/zip", archive, item["digest"][7:])
            with zipfile.ZipFile(archive) as package:
                if package.namelist() != ["backend-tests.json"] or package.getinfo("backend-tests.json").file_size > limit:
                    raise ValueError("Invalid backend test evidence payload")
                proof = json.loads(package.read("backend-tests.json"))
        if (proof["version"] == 2 and proof["commit"] == run["head_sha"] and proof["attempt"] == run["run_attempt"]
                and proof["environment"] == identity and proof["hostTarget"] == host):
            yield from proof["tests"]


def resolve():
    plan_path = PLANS / "plan.json"
    plan = read(plan_path)
    if not plan["backendStore"] or plan["backendPackages"] is not None:
        raise ValueError("Expected an unbound CI plan")
    targets = registrations()
    REQUEST.mkdir(parents=True, exist_ok=True)
    sdk = shutil.which("dotnet")

    def capture(target, configuration, selection=None):
        output = REQUEST / f"{target}-{configuration}.json"
        extra = ["--selection", selection] if selection else []
        validate("backend-inputs", "--plan", plan_path, "--engine-root", ROOT, "--dotnet", sdk, "--target", target,
                 "--configuration", configuration, "--output", output, *extra)
        return read(output)

    requests = plan["nativeRequests"] or []
    if any(item["target"] not in targets for item in requests):
        raise ValueError("Unregistered native request")
    packages = {}
    if plan["backendRequired"]:
        for target in targets:
            requested = {item["configuration"] for item in requests if item["target"] == target}
            configurations = set(requested)
            if target == plan["backendHost"]:
                configurations.update(context["properties"]["Configuration"] for context in plan["contexts"].values())
                configurations.add(plan["configuration"])
            changed = {}
            for configuration in sorted(configurations):
                inputs = capture(target, configuration)
                if inputs["required"] or configuration in requested:
                    changed[configuration] = inputs["required"]
            if changed:
                packages[target] = {"target": target, "operation": "produce", "tag": None,
                                    "sha256": None, "assetId": None, "changed": changed}
    certificates = {}
    if packages:
        releases = (r for r in repository_pages("releases") if r["prerelease"] and not r["draft"] and RELEASE.fullmatch(r["tag_name"]))
        for release in sorted(releases, key=lambda r: (r["published_at"], r["id"]), reverse=True):
            platforms = catalog(release)
            selection = REQUEST / (release["tag_name"] + ".json")
            write(selection, {key: [item for platform in platforms.values() for item in platform["native"].get(key, [])]
                              for key in ("assets", "requests", "aggregates")})
            for target, decision in packages.items():
                if target not in platforms or decision["operation"] == "consume":
                    continue
                platform = platforms[target]
                if platform["asset"] != targets[target]["asset"]:
                    raise ValueError(f"Unexpected asset name for {target}")
                for test in platform.get("tests", []):
                    certificates.setdefault((test["id"], test["inputIdentity"]), test)
                cost = 0
                changed = {}
                for configuration in decision["changed"]:
                    current = capture(target, configuration, selection)
                    owners = platform["configurations"].get(configuration, {})
                    changed[configuration] = [key for key in current["required"] if owners.get(key) != current["projects"][key]]
                    cost += len(changed[configuration]) + len(current["missingNativeProducts"])
                if cost < decision.get("cost", float("inf")):
                    asset = next(item for item in release["assets"] if item["name"] == platform["asset"])
                    decision.update(operation="produce" if cost else "consume", tag=release["tag_name"], assetId=asset["id"],
                                    sha256=platform["sha256"], cost=cost, changed=changed)
            if all(item["operation"] == "consume" for item in packages.values()):
                break
        if any(not item["exclusion"] and item["reusable"] and item["kind"] == "test"
               and item["owner"].startswith("tests/backends/") for item in plan["checks"]):
            for test in test_evidence(plan):
                certificates.setdefault((test["id"], test["inputIdentity"]), test)
    write(REQUEST / "acquisition.json", {"commit": plan["commit"], "portableTestChanges": plan["portableTestChanges"],
                                         "packages": list(packages.values()), "testEvidence": list(certificates.values())})


def release_archive(package):
    digest = package["sha256"]
    asset = package["assetId"]
    if not re.fullmatch("[0-9a-f]{64}", digest) or not re.fullmatch("[1-9][0-9]*", str(asset)):
        raise ValueError("Invalid backend asset digest or ID")
    directory = ROOT / "artifacts/backend-downloads"
    directory.mkdir(parents=True, exist_ok=True)
    archive = directory / (digest + ".zip")
    if archive.exists():
        with archive.open("rb") as stream:
            if hashlib.file_digest(stream, "sha256").hexdigest() != digest:
                raise ValueError("Cached backend ZIP differs from catalog digest")
    else:
        with tempfile.TemporaryDirectory(dir=directory) as temporary:
            partial = Path(temporary) / "download"
            download(f"releases/assets/{asset}", partial, digest)
            partial.rename(archive)
    return archive


def acquire_inputs(item):
    from validation import digest, workloads

    registered = registrations()
    target, host = item["target"], item["hostTarget"]
    if host not in registered or item["asset"] != registered[target]["asset"]:
        raise ValueError("Unexpected platform or asset registration")
    operation = item["operation"]
    output = ROOT / "artifacts/backend-input"
    output.mkdir()
    native = ROOT / "artifacts/backend-native"
    work = ROOT / "artifacts/bn"
    work.mkdir()
    archive = None
    identity = item.get("sha256")
    if operation == "promote":
        archive = receive(item, ROOT / "artifacts/promoted")
        identity = digest(archive)
        operation = "consume"
    elif identity:
        archive = release_archive(item)
    if operation == "consume" and not archive:
        raise ValueError("Consumption requires a certified archive")
    if operation == "produce" and archive:
        shutil.copyfile(archive, output / registered[target]["asset"])
    if operation == "consume" and not item["nativeInputs"]:
        return
    sdk = shutil.which("dotnet")
    if not sdk:
        raise RuntimeError("Missing selected SDK")
    tooling = ROOT / "tools/Inno.Tooling.Cli"
    def managed(*arguments):
        subprocess.run([sdk, "run", "--no-build", "--project", tooling, "-c", "Release", "--", "validate", *arguments], check=True)

    prior = work / "prior"
    host_seed = None
    if archive or item.get("hostSeed"):
        subprocess.run([sdk, "build", tooling, "-c", "Release", "--disable-build-servers"], check=True)
    if archive:
        managed("import-backends", "--archive", archive, "--sha256", identity, "--target", target, "--output", prior)
    if operation == "consume":
        native.mkdir()
        selection = prior / "native/selection.json"
        shutil.copyfile(selection, native / "selection.json")
        inventory = read(selection)
        for payload in inventory["assets"] + inventory["aggregates"]:
            name, expected = payload["asset"], payload["sha256"]
            if not re.fullmatch(r"[A-Za-z0-9._-]+\.zip", name) or not re.fullmatch("[a-f0-9]{64}", expected):
                raise ValueError("Invalid native asset inventory")
            destination = native / name
            if not destination.exists():
                shutil.copyfile(prior / "native" / (expected + ".zip"), destination)
            if digest(destination) != expected:
                raise ValueError(f"Conflicting native payload for {name}")
        return
    if operation != "produce":
        raise ValueError(f"Unknown acquisition operation {operation}")
    workloads({"nativeRequests": [{"target": target}]}, sdk)
    if host == target and archive:
        host_seed = prior
    elif item.get("hostSeed"):
        seed = item["hostSeed"]
        host_seed = work / "host"
        managed("import-backends", "--archive", release_archive(seed), "--sha256", seed["sha256"], "--target", host, "--output", host_seed)
    build = ROOT / "entry/InnoEngine.Build/InnoEngine.Build.csproj"
    outputs = work / "sdk"
    properties = []
    if host_seed:
        plan = work / "identity"
        managed("plan", "--engine-root", ROOT, "--dotnet", sdk, "--base", "HEAD", "--profile", "fast",
                "--backend-store", "artifacts/bn/store", "--output", plan)
        capture = work / "managed.json"
        managed("backend-inputs", "--engine-root", ROOT, "--dotnet", sdk, "--target", host, "--plan", plan / "plan.json",
                "--selection", host_seed / "native/selection.json", "--output", capture)
        current = read(capture)["projects"]
        matching = {entry["owner"] for entry in read(host_seed / "manifest.json")["files"] if entry["configuration"] == "Debug"
                    and entry["path"].startswith("managed/") and current.get(entry["owner"]) == entry["input"]}
        changed = sorted(current.keys() - matching)
        properties = [f"-p:InnoBackendPrebuiltRoot={host_seed}", f"-p:InnoBackendExportRoot={work / 'bootstrap-outputs'}",
                      f"-p:InnoBackendExportArtifactsPath={outputs}", "-p:InnoBackendCompileOwners=|" + "|".join(changed) + "|"]
    subprocess.run([sdk, "build", build, "-c", "Debug", "--artifacts-path", outputs, "--disable-build-servers", "-m:2", "-nr:false", *properties], check=True)
    entry = Path(subprocess.check_output([sdk, "msbuild", build, "-nologo", "-p:Configuration=Debug", f"-p:ArtifactsPath={outputs}",
                                         "-getProperty:TargetPath"], text=True).strip())
    if not entry.is_absolute() or not entry.resolve().is_relative_to(outputs) or not entry.is_file():
        raise ValueError("The SDK did not produce the requested Build entry within its outputs")
    command = [sdk, entry, "native-dependencies", "--operation", "prepare-platform", "--engine-root", ROOT, "--target", target,
               "--tools-target", host, "--output", native, "--configurations", ",".join(registered[target]["configurations"])]
    if archive:
        command.extend(["--input", prior])
    subprocess.run(command, check=True)


def bind(platforms=()):
    request = read(REQUEST / "acquisition.json")
    path = PLANS / "plan.json"
    plan = read(path)
    if request["commit"] != plan["commit"] or not plan["backendStore"] or plan["backendPackages"] is not None:
        raise ValueError("Unmatched request or already bound plan")
    registrations_by_target = registrations()
    receipts = []
    store = (ROOT / plan["backendStore"]).resolve()
    if not store.is_relative_to(ROOT / "artifacts"):
        raise ValueError("Invalid backend output directory")
    source = {item["target"]: item["promotion"]["commit"] for item in platforms if item["operation"] == "promote"}
    for package in request["packages"]:
        target = package["target"]
        asset = registrations_by_target[target]["asset"]
        directory = ROOT / "artifacts/candidates" / ("backend-candidate-" + target)
        if (directory / "consumption.json").exists():
            receipt = read(directory / "consumption.json")
            if receipt["target"] != target or receipt["commit"] != source.get(target, plan["commit"]):
                raise ValueError(f"Foreign candidate receipt for {target}")
            digest = receipt["sha256"]
            archive = directory / asset
        elif package["operation"] == "consume":
            digest = package["sha256"]
            archive = release_archive(package)
        else:
            raise FileNotFoundError(f"Missing produced backend: {target}")
        validate("import-backends", "--archive", archive, "--sha256", digest, "--target", target, "--output", store / target)
        receipts.append(read(store / target / "receipt.json"))
    rebuilding = set(os.getenv("REBUILD_TARGETS", "").split(","))
    clear = any(package["target"] in rebuilding for package in request["packages"])
    write(REQUEST / "packages.json", receipts)
    write(REQUEST / "backend-test-evidence.json", [] if clear else request["testEvidence"])
    validate("bind-backends", "--test-evidence", REQUEST / "backend-test-evidence.json", "--packages", REQUEST / "packages.json",
             "--plan", path, "--engine-root", ROOT, "--dotnet", shutil.which("dotnet"), "--output", path)


def decide():
    path = REQUEST / "acquisition.json"
    request = read(path)
    packages = request["packages"]
    forced = set(filter(None, os.environ["REBUILD_TARGETS"].split(",")))
    deferred = any(item["operation"] == "produce" or item["target"] in forced for item in packages)
    inline = not deferred and (os.environ["MULTI_HOST"].lower() != "true" or
                               (request["portableTestChanges"] and not packages))
    outputs = {"dispatch": str(not inline).lower(),
               "entry-directory": Path(read(PLANS / "entry.json")["assembly"]).parent.as_posix()}
    if len(packages) == 1 and not deferred:
        digest = packages[0]["sha256"]
        if not re.fullmatch("[a-f0-9]{64}", digest):
            raise ValueError("Direct consumption requires an exact digest")
        outputs["digest"] = digest
    write(path, {**request, "deferred": deferred, "executedLocally": inline})
    with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
        for key, value in outputs.items():
            print(f"{key}={value}", file=stream)


def promotions(revision, branch, eligible):
    from urllib.error import HTTPError

    repository = os.environ["GITHUB_REPOSITORY"]
    tree = subprocess.check_output(["git", "rev-parse", "--verify", revision + "^{tree}"], text=True).strip()
    heads = {pull["head"]["sha"] for pull in repository_pages(f"commits/{revision}/pulls") if pull["merged_at"]
             and pull["merge_commit_sha"] == revision and pull["base"]["ref"] == branch
             and pull["base"]["repo"]["full_name"] == repository and (pull["head"]["repo"] or {}).get("full_name") == repository}
    endpoint = "actions/workflows/engine-ci.yml/runs"
    runs = [run for head in heads for run in repository_pages(endpoint, key="workflow_runs",
            filters={"event": "pull_request", "head_sha": head})]
    runs.extend(run for run in repository_pages(endpoint, key="workflow_runs", filters={"event": "merge_group"})
                if (run["head_branch"] or "").startswith("gh-readonly-queue/" + branch + "/"))
    found = {}
    for run in sorted(runs, key=lambda item: item["id"], reverse=True):
        title = re.fullmatch(r"Engine validation ([0-9a-f]{40})", run["display_title"])
        if (not title or run["status"] != "completed" or run["repository"]["full_name"] != repository
                or (run["head_repository"] or {}).get("full_name") != repository or run["path"] != ".github/workflows/engine-ci.yml"):
            continue
        source = title[1]
        if not ((run["event"] == "merge_group" and source == run["head_sha"]) or
                (run["event"] == "pull_request" and run["head_sha"] in heads)):
            continue
        try:
            commit = repository_json("git/commits/" + source)
        except HTTPError as error:
            if error.code == 404:
                continue
            raise
        if commit["tree"]["sha"] != tree or (run["event"] == "pull_request" and
                run["head_sha"] not in {parent["sha"] for parent in commit["parents"]}):
            continue
        prefix = f"actions/runs/{run['id']}"
        jobs = list(repository_pages(f"{prefix}/attempts/{run['run_attempt']}/jobs", key="jobs"))
        assets = list(repository_pages(prefix + "/artifacts", key="artifacts"))
        for target, item in eligible.items():
            if target in found:
                continue
            built = [j for j in jobs if j["conclusion"] == "success" and j["name"].endswith(f" / Backend build ({target})")]
            verified = [s for j in jobs if j["conclusion"] == "success" and j["name"].endswith(f" / Engine tests ({item['runner']})")
                        for s in j["steps"] if s["name"] == "Verify new backend publication inputs" and s["conclusion"] == "success"]
            matched = [a for a in assets if a["name"] == "backend-candidate-" + target and not a["expired"]]
            if not (len(built) == len(verified) == len(matched) == 1):
                continue
            asset = matched[0]
            if (re.fullmatch(r"sha256:[a-f0-9]{64}", asset.get("digest") or "") and
                    built[0]["started_at"] <= asset["created_at"] <= min(verified[0]["started_at"], built[0]["completed_at"])):
                found[target] = {"commit": source, "tree": tree, "run": run["id"], "attempt": run["run_attempt"],
                                 "artifact": asset["id"], "digest": asset["digest"]}
        if found.keys() == eligible.keys():
            break
    return found


def matrix():
    revision = os.environ["GITHUB_SHA"]
    hosts = [item["runner"] for item in json.loads(os.environ["HOSTS"])["include"]]
    host_targets = {item["runner"]: item["target"] for item in read(ROOT / ".github/validation-hosts.json")}
    registry = registrations()
    forced = [item.strip() for item in os.getenv("REBUILD_TARGETS", "").split(",") if item]
    if len(set(forced)) != len(forced) or len(set(hosts)) != len(hosts) or not hosts:
        raise ValueError("Duplicate rebuild target or invalid host matrix")
    targets, consumers, independent, tags = {}, {}, [], set()
    for host in hosts:
        host_target = host_targets[host]
        request = read(ROOT / f"artifacts/requests/backend-request-{host}/acquisition.json")
        packages = request["packages"]
        deferred, inline = request["deferred"], request["executedLocally"]
        if (request["commit"] != revision or not isinstance(packages, list) or type(deferred) != bool or type(inline) != bool
                or (inline and (forced or deferred or (len(hosts) > 1 and (packages or not request["portableTestChanges"]))))):
            raise ValueError(f"Invalid backend decision for {host}")
        seen = set()
        for item in packages:
            name = item["target"]
            registration = registry[name]
            operation = item["operation"]
            if operation not in ("produce", "consume") or name in seen:
                raise ValueError(f"Invalid request for {host}/{name}")
            seen.add(name)
            if item.get("tag"):
                tags.add(item["tag"])
            if operation == "consume" and (not re.fullmatch("[0-9a-f]{64}", item["sha256"] or "")
                    or not re.fullmatch("[1-9][0-9]*", str(item["assetId"]))):
                raise ValueError(f"Missing immutable archive identity for {name}")
            if not deferred:
                if name in forced or operation != "consume":
                    raise ValueError(f"Missing producer for {name}")
                continue
            if name not in targets:
                targets[name] = {"target": name, "asset": registration["asset"], "runner": registration["runner"],
                                 "hostTarget": host_targets[registration["runner"]], "operation": operation,
                                 **{key: item.get(key) for key in ("sha256", "assetId", "tag")}}
            elif operation == "produce" or targets[name]["sha256"] != item["sha256"]:
                targets[name]["operation"] = "produce"
        if deferred:
            if not seen:
                raise ValueError(f"Missing deferred platform inputs for {host}")
            consumers[host] = {"runner": host, "target": host_target, "targets": sorted(seen),
                               "pattern": "", "destination": "artifacts/candidates"}
        elif not inline:
            independent.append({"runner": host, "digest": packages[0]["sha256"] if len(packages) == 1 else ""})
    bootstrap = registry.keys() == targets.keys() and not any(t["sha256"] for t in targets.values())
    for name in forced:
        targets[name].update(operation="produce", sha256=None, assetId=None, tag=None)
    eligible = {k: v for k, v in targets.items() if v["operation"] == "produce" and k not in forced}
    branch = os.environ["DEFAULT_BRANCH"]
    if eligible and os.environ["GITHUB_EVENT_NAME"] == "push" and os.environ["GITHUB_REF"] == "refs/heads/" + branch:
        for name, certificate in promotions(revision, branch, eligible).items():
            targets[name].update(operation="promote", promotion=certificate)
    produced = {k: v for k, v in targets.items() if v["operation"] == "produce"}
    for name, item in targets.items():
        item["hostSeed"] = None
        if name in produced and item["hostTarget"] != name:
            host = targets[item["hostTarget"]]
            if host["sha256"]:
                item["hostSeed"] = {key: host[key] for key in ("sha256", "assetId")}
        item["nativeInputs"] = name in produced or any(v["hostTarget"] == name for v in produced.values())
    requirements = {}
    for host, consumer in consumers.items():
        built = [name for name in consumer["targets"] if name in produced]
        if len(built) == 1:
            consumer["pattern"] = "backend-candidate-" + built[0]
            consumer["destination"] += "/" + consumer["pattern"]
        elif built:
            consumer["pattern"] = "backend-candidate-{" + ",".join(built) + "}"
        requirements[host] = set(consumer["targets"]) | {produced[name]["hostTarget"] for name in built}
    chains = []
    while requirements:
        host, closure = next(iter(requirements.items()))
        members = [host]
        del requirements[host]
        while shared := [h for h, names in requirements.items() if bootstrap or names & closure]:
            for h in shared:
                members.append(h)
                closure.update(requirements.pop(h))
        platforms = [v for k, v in targets.items() if k in closure]
        chains.append({"platforms": {"include": platforms}, "builders": {"include": [i for i in platforms if i["operation"] == "produce"]},
                       "producers": {"include": [i for i in platforms if i["operation"] != "consume"]},
                       "consumers": {"include": [consumers[h] for h in members]}})
    with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
        for name, value in {"chains": {"include": chains}, "independent": {"include": independent},
                            "reader_tags": sorted(tags), "required": bool(targets)}.items():
            print(f"{name}={json.dumps(value, separators=(',', ':'))}", file=stream)


def receive(platform, directory):
    target = platform["target"]
    asset = platform["asset"]
    registered = json.loads((Path(__file__).resolve().parents[1] / "native-targets.json").read_text(encoding="utf-8"))
    matches = [item for item in registered if item["target"] == target]
    source = platform["promotion"]
    if (len(matches) != 1 or matches[0]["asset"] != asset or Path(asset).name != asset
            or not re.fullmatch("sha256:[a-f0-9]{64}", source["digest"])
            or not re.fullmatch("[1-9][0-9]*", str(source["artifact"]))):
        raise ValueError("Invalid promoted backend identity")
    directory.mkdir(parents=True, exist_ok=True)
    output = directory / f"backend-candidate-{target}"
    with tempfile.TemporaryDirectory(dir=directory) as temporary:
        staging = Path(temporary)
        archive = staging / "candidate.zip"
        download(f"actions/artifacts/{source['artifact']}/zip", archive, source["digest"][7:])
        contents = staging / "contents"
        contents.mkdir()
        with zipfile.ZipFile(archive) as package:
            for name in (asset, "consumption.json"):
                if package.namelist().count(name) != 1:
                    raise ValueError(f"Missing or ambiguous backend candidate file: {name}")
                with package.open(name) as incoming, (contents / name).open("xb") as outgoing:
                    shutil.copyfileobj(incoming, outgoing)
        receipt = json.loads((contents / "consumption.json").read_text(encoding="utf-8-sig"))
        with (contents / asset).open("rb") as stream:
            digest = hashlib.file_digest(stream, "sha256").hexdigest()
        if (receipt["commit"] != source["commit"] or receipt["version"] != 2
                or receipt["target"] != target or receipt["sha256"] != digest):
            raise ValueError("Invalid certified backend ZIP")
        contents.rename(output)
    print(f"Received {target} from workflow {source['run']}, attempt {source['attempt']}, SHA-256 {digest}", file=sys.stderr)
    return (output / asset).resolve()


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("operation", choices=["resolve", "receive", "decide", "matrix"])
    parser.add_argument("--directory", type=Path)
    parser.add_argument("--prepare", action="store_true")
    parser.add_argument("--bind", action="store_true")
    args = parser.parse_args()
    if args.operation == "resolve":
        resolve()
    elif args.operation == "decide":
        decide()
    elif args.operation == "matrix":
        matrix()
    elif args.prepare:
        acquire_inputs(json.loads(os.environ["SPEC"]))
    elif args.bind:
        platforms = json.loads(os.getenv("PLATFORMS", '{"include":[]}'))["include"]
        needed = set(json.loads(os.getenv("REQUIRED_TARGETS", "[]")))
        for item in platforms:
            if item["target"] in needed and item["operation"] == "promote":
                receive(item, ROOT / "artifacts/candidates")
        bind(platforms)
    elif args.directory:
        print(receive(json.load(sys.stdin), args.directory))
    else:
        parser.error("receive needs --directory or --bind")
