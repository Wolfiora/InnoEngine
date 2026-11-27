"""Replay the retained hosted validation and backend publication incidents offline."""
import base64
import copy
import gzip
import hashlib
import io
import json
import os
from pathlib import Path
import sys
import subprocess
import zipfile
import tempfile
import unittest
from unittest.mock import patch

sys.path.insert(0, str(Path(sys.argv.pop(1))))
import backend_artifacts as backends
import documentation_artifacts as documentation
import github_artifacts
import validation
import routing


class HostedReceiptTests(unittest.TestCase):
    def setUp(self):
        temporary = tempfile.TemporaryDirectory()
        self.addCleanup(temporary.cleanup)
        self.root = Path(temporary.name)
        self.summary = self.root / "summary.md"
        environment = patch.dict(os.environ, {
            "HOSTS": '{"include":[{"runner":"windows"}]}', "GITHUB_RUN_ATTEMPT": "2", "GITHUB_RUN_ID": "123",
            "GH_TOKEN": "", "GITHUB_STEP_SUMMARY": str(self.summary),
        })
        environment.start()
        self.addCleanup(environment.stop)

    def put(self, artifact, filename, value, attempt=2):
        directory = self.root / f"{artifact}-attempt-{attempt}"
        directory.mkdir(exist_ok=True)
        file = directory / filename
        file.write_text(json.dumps(value), encoding="utf-8")
        return validation.digest(file)

    def test_route_and_required_check_agree_on_manual_and_mixed_changes(self):
        revision = "a" * 40
        data = {"ref": "refs/heads/benchmark", "repository": {"default_branch": "master"}, "before": "b" * 40,
                "pull_request": {"base": {"sha": "b" * 40}}, "merge_group": {"base_sha": "b" * 40}}
        for event in ("push", "pull_request", "merge_group"):
            for paths, reason in ((b"benchmark/domain/a.cs\0", "benchmark-only"),
                                  (b"benchmark/domain/a.cs\0src/owner/b.cs\0", "validation"),
                                  (b"tests/old.cs\0benchmark/new.cs\0", "validation")):
                with patch.object(routing.subprocess, "check_output", return_value=paths), patch.object(routing, "matching_pull", return_value=None):
                    profile, result, _ = routing.select(event, data, revision)
                self.assertEqual(reason, result)
                with patch.dict(os.environ, {"GITHUB_EVENT_NAME": event, "ROUTE_RESULT": "success", "ROUTE_REASON": result,
                                             "SHOULD_RUN": str(reason == "validation").lower(),
                                             "PLATFORM_RESULT": "success" if reason == "validation" else "skipped"}):
                    validation.verify_route()
                    with patch.dict(os.environ, {"PLATFORM_RESULT": "failure"}), self.assertRaises(RuntimeError):
                        validation.verify_route()
        with patch.object(routing, "benchmark_only", return_value=False), patch.object(routing, "matching_pull", return_value=27):
            self.assertEqual(("fast", "open-pull-request", 27), routing.select("push", data, revision))
        with patch.dict(os.environ, {"GITHUB_EVENT_NAME": "push", "ROUTE_RESULT": "success", "SHOULD_RUN": "false",
                                     "PLATFORM_RESULT": "skipped", "ROUTE_REASON": "open-pull-request", "ROUTE_PULL": "27"}):
            validation.verify_route()
            self.assertIn("#27", self.summary.read_text())

    def test_rewritten_push_with_an_unadvertised_previous_commit(self):
        candidate, before = "a" * 40, "b" * 40
        data = {"ref": "refs/heads/benchmark", "repository": {"default_branch": "master"},
                "before": before, "forced": True}
        with patch.object(routing, "matching_pull", return_value=94), patch.object(routing, "ensure_push_base") as fetch:
            self.assertEqual(("fast", "open-pull-request", 94), routing.select("push", data, candidate))
            fetch.assert_not_called()
        with patch.object(routing, "matching_pull", return_value=None), patch.object(routing, "ensure_push_base", return_value=False):
            self.assertEqual(("full", "validation", None), routing.select("push", data, candidate))
        with (patch.object(routing, "matching_pull", return_value=None), patch.object(routing, "ensure_push_base", return_value=True),
              patch.object(routing, "benchmark_only", return_value=True) as compare):
            self.assertEqual(("fast", "benchmark-only", None), routing.select("push", data, candidate))
            compare.assert_called_once_with(before, candidate)

    def test_partial_doc_retry_requires_each_actual_successful_execution(self):
        revision = "a" * 40
        jobs, assets = [], []
        for i, (host, configuration) in enumerate((("macos", "Debug"), ("macos", "Release"),
                                                    ("windows", "Debug"), ("windows", "Release"))):
            original = dict(name=f"API facts / {host} / {configuration}", run_id=123, head_sha=revision,
                            run_attempt=1, status="completed", conclusion="success", runner_id=i + 1,
                            started_at="first-start", completed_at="first-end")
            retry = {**original, "run_attempt": 2}
            if i == 0:
                original["conclusion"] = "cancelled"
                retry.update(started_at="second-start", completed_at="second-end")
            jobs.extend((original, retry))
            assets.append(dict(id=i + 1, expired=False, digest="sha256:" + "b" * 64,
                               name=f"api-facts-{host}-{configuration}-{2 if i == 0 else 1}"))
        def pages(path, **kwargs):
            return jobs if path.endswith("/jobs") else assets
        with (patch.object(documentation, "EXPORT", self.root), patch.object(documentation, "repository_pages", side_effect=pages),
              patch.dict(os.environ, {"EXPECTED_COMMIT": revision, "GITHUB_OUTPUT": str(self.summary)})):
            documentation.select_producers()
            selected = json.loads((self.root / "selected-producers.json").read_bytes())
            self.assertEqual({asset["name"]: asset["id"] for asset in assets}, selected)
            for item, override in ((jobs[1], {"conclusion": "failure"}), (jobs[0], {"head_sha": "f" * 40}),
                                   (assets[0], {"name": "api-facts-macos-Debug-1"}), (assets[2], {"expired": True})):
                with patch.dict(item, override), self.assertRaises(ValueError):
                    documentation.select_producers()

    def test_first_failed_test_and_missing_owner(self):
        case = json.loads(Path(__file__).with_name("pr88-managed-failure.json").read_bytes())
        failed = case["failedOwner"]
        plan = {"checks": [{"id": owner, "kind": "instructions"} for owner in ("unchanged", failed)],
                "shards": [{"id": "managed"}]}
        self.put("validation-plan-windows", "plan.json", plan)
        detail = f"{case['test']}: expected {case['expected']}, observed {case['actual']}; TRX duration {case['trxDuration']}"
        evidence = {"results": [{"id": failed, "status": "Failed", "detail": detail}],
                    "failures": [{"stage": "test-execution", "detail": f"Shared tests exited with {case['exit']}"}]}
        for missing in (True, False):
            if not missing:
                evidence["results"].append({"id": "unchanged", "status": "Passed"})
            self.put("validation-result-windows-managed", "results.json", evidence)
            self.summary.unlink(missing_ok=True)
            select = validation.artifact
            with patch.object(validation, "artifact", side_effect=lambda name: select(name, self.root)):
                validation.summarize()
            report = self.summary.read_text(encoding="utf-8")
            for expected in (detail, "Shared tests exited with 1",
                             "Missing planned checks: unchanged" if missing else "All 2 planned checks returned results."):
                self.assertIn(expected, report)

    def test_current_plan_requires_its_own_shard_and_certification(self):
        platform = "windows"
        commit = "a" * 40
        plan = self.put("validation-plan-windows", "plan.json", {"commit": commit, "shards": [{"id": "managed"}]})
        self.put("validation-plan-windows", "entry.json", {"sha256": "B" * 64})
        result = self.put("validation-joined-windows", "results.json", {})
        shard = self.put("validation-result-windows-managed", "results.json", {}, attempt=1)
        proof = {"attempt": 2, "run": "123", "platform": platform, "commit": commit, "executor": "B" * 64,
                 "planIdentity": plan, "resultsIdentity": result, "shards": [{"id": "managed", "sha256": shard}],
                 "passed": 1, "reused": 0, "notApplicable": 0}
        self.put("validation-joined-windows", "receipt.json", proof)
        certify = lambda: validation.certified_host(platform, commit, self.root)
        with self.assertRaisesRegex(ValueError, "differs"):
            certify()
        self.put("validation-result-windows-managed", "results.json", {})
        certify()
        for key, value in (("run", 124), ("attempt", 1), ("executor", "C" * 64), ("resultsIdentity", "0" * 64)):
            self.put("validation-joined-windows", "receipt.json", {**proof, key: value})
            with self.assertRaises(ValueError):
                certify()
        self.put("validation-joined-windows", "receipt.json", proof)
        (self.root / "validation-result-windows-managed-attempt-2/results.json").unlink()
        with self.assertRaises(FileNotFoundError):
            certify()

    def test_pinned_sources_are_prepared_for_the_exact_clean_candidate(self):
        def git(repo, *args):
            return subprocess.check_output(["git", "-C", repo, *args], text=True).strip()
        source, engine = self.root / "native", self.root / "engine"
        with patch.dict(os.environ, {"GIT_ALLOW_PROTOCOL": "file", "GIT_CONFIG_COUNT": "0", "GIT_CONFIG_NOSYSTEM": "1",
                                     "GIT_CONFIG_GLOBAL": str(self.root / "gitconfig")}):
            for repo in (source, engine):
                repo.mkdir()
                git(repo, "init", "-q")
                git(repo, "config", "user.name", "Source fixture")
                git(repo, "config", "user.email", "fixture@example.invalid")
            (source / "header").write_text("pinned")
            git(source, "add", "header")
            git(source, "commit", "-qm", "source")
            git(engine, "submodule", "add", "-q", str(source), "extern/native")
            git(engine, "commit", "-qm", "pin")
            pin, revision = git(source, "rev-parse", "HEAD"), git(engine, "rev-parse", "HEAD")
            git(engine, "submodule", "deinit", "-f", "--", "extern/native")
            for commit, paths in ((revision, {"extern/native": "f" * 40}), (revision, {"other": pin}),
                                  ("f" * 40, {"extern/native": pin})):
                with self.assertRaises(ValueError):
                    validation.prepare_sources(engine, commit, paths)
            validation.prepare_sources(engine, revision, {"extern/native": pin})
            self.assertEqual("pinned", (engine / "extern/native/header").read_text())
            (engine / "changed").write_text("uncommitted")
            with self.assertRaisesRegex(ValueError, "unchanged candidate"):
                validation.prepare_sources(engine, revision, {"extern/native": pin})

    def test_backend_test_reuse_requires_the_successful_default_branch_attempt(self):
        commit, identity = "a" * 40, "b" * 64
        event = self.root / "event.json"
        event.write_text('{"repository":{"default_branch":"master"}}')
        brief = dict(id=3, head_branch="master", head_sha=commit, repository_id=2, head_repository_id=2)
        asset = dict(id=4, expired=False, name=f"backend-tests-fixture-{identity}", size_in_bytes=384,
                     digest="sha256:" + identity, workflow_run=brief)
        run = dict(status="completed", conclusion="success", head_sha=commit, head_branch="master",
                   head_repository={"id": 2}, event="push", path=".github/workflows/engine-ci.yml", run_attempt=1)
        proof = dict(version=2, commit=commit, environment=identity, hostTarget="fixture", attempt=1, tests=[{"id": "test:fixture"}])
        def download(_, destination, expected):
            with zipfile.ZipFile(destination, "w") as package:
                package.writestr("backend-tests.json", json.dumps(proof))
            self.assertEqual(identity, expected)
        with (patch.dict(os.environ, {"GITHUB_REPOSITORY_ID": "2", "GITHUB_EVENT_PATH": str(event)}),
              patch.object(backends, "repository_pages", return_value=[asset]), patch.object(backends, "repository_json", return_value=run),
              patch.object(backends, "download", side_effect=download)):
            read = lambda: list(backends.test_evidence({"environment": identity, "backendHost": "fixture"}))
            self.assertEqual(proof["tests"], read())
            for target, field, value in ((run, "event", "pull_request"), (run, "run_attempt", 2),
                                         (brief, "head_repository_id", 7), (asset, "expired", True)):
                with patch.dict(target, {field: value}):
                    self.assertEqual([], read())

    def test_annotated_catalog_must_bind_the_release_and_asset(self):
        revision, digest = "a" * 40, "b" * 64
        platform = {"target": "fixture", "asset": "fixture-prebuilt-backends.zip", "sha256": digest}
        body = {"version": 1, "revision": revision, "platforms": {"fixture": platform}}
        tag = "backend-prebuilts-2026.01.01.1"
        message = "InnoEngine backend prebuilt catalog v1\n\n" + base64.b64encode(gzip.compress(json.dumps(body).encode())).decode()
        obj = {"tag": tag, "message": message, "object": {"type": "commit", "sha": revision}}
        ref = {"object": {"type": "tag", "sha": revision}}
        asset = {"name": platform["asset"], "state": "uploaded", "digest": "sha256:" + digest}
        release = {"tag_name": tag, "prerelease": True, "draft": False, "assets": [asset]}
        with patch.object(backends, "repository_json", side_effect=lambda path: ref if path.startswith("git/ref/") else obj):
            self.assertEqual(body["platforms"], backends.catalog(release))
            for target, field, value in ((ref["object"], "type", "commit"), (obj["object"], "sha", "c" * 40),
                                         (asset, "digest", "sha256:" + "c" * 64), (release, "assets", [asset, asset])):
                with patch.dict(target, {field: value}), self.assertRaises(ValueError):
                    backends.catalog(release)

    def test_only_the_current_release_can_skip_publication(self):
        hashes = {item["asset"]: "a" * 64 for item in backends.registrations().values()}
        revision = "b" * 40
        def release(index, digit):
            return dict(id=index, tag_name=f"backend-prebuilts-2026.01.01.{index}", draft=False, prerelease=True,
                        published_at=f"2026-01-0{index}", assets=[dict(name=name, digest="sha256:" + digit * 64, state="uploaded")
                                                                 for name in hashes])
        old, current = release(1, "a"), release(2, "b")
        self.assertEqual("reuse", backends.release_plan([old], hashes, revision)[0])
        operation, tag, _ = backends.release_plan([old, current], hashes, revision)
        self.assertEqual("create", operation)
        draft = dict(id=3, tag_name=tag, draft=True, prerelease=True, target_commitish=revision, assets=[])
        self.assertEqual("resume", backends.release_plan([old, current, draft], hashes, revision)[0])
        with self.assertRaisesRegex(ValueError, "Another producer"):
            backends.release_plan([old, current, {**draft, "target_commitish": "c" * 40}], hashes, revision)

    def test_legacy_release_rotation_survives_an_interrupted_tag_deletion(self):
        releases, refs, objects = {}, {}, {}
        for i in range(1, 5):
            tag = f"backend-prebuilts-2026.01.01.{i}"
            revision, obj = str(min(i, 3)) * 40, str(i + 4) * 40
            name, digest = "fixture-prebuilt-backends.zip", str(i) * 64
            metadata = {"version": 1, "revision": revision, "platforms": {"fixture": dict(target="fixture", asset=name, sha256=digest)}}
            encoded = base64.b64encode(gzip.compress(json.dumps(metadata).encode())).decode()
            objects[obj] = {"tag": tag, "object": {"type": "commit", "sha": revision},
                            "message": "InnoEngine backend prebuilt catalog v1\n\n" + encoded}
            refs[tag] = {"object": {"type": "tag", "sha": obj}}
            releases[i] = dict(id=i, name=f"Backend Prebuilts {i}", tag_name=tag, target_commitish=revision,
                               published_at=str(i), prerelease=True, draft=False, body="Certified.", assets=[
                                   dict(name=name, state="uploaded", digest="sha256:" + digest)])
        def api(path, *, method="GET", data=None, optional=False):
            if path == "git/ref/heads/master":
                return {"object": {"sha": "f" * 40}}
            kind, key = path.rsplit("/", 1)
            store = {"releases": releases, "git/ref/tags": refs, "git/refs/tags": refs, "git/tags": objects}[kind]
            key = int(key) if kind == "releases" else key
            if method == "PATCH":
                store[key].update(data)
            if method == "DELETE":
                if kind == "git/refs/tags" and interrupt:
                    interrupt.pop()
                    raise ConnectionError("interrupted retirement")
                del store[key]
                return
            return store.get(key) if optional else store[key]
        current, backup = releases[4], releases[2]
        def rotate(active):
            inventory = backends.published_backends(releases.values())
            backends.rotate_releases(inventory[0], inventory[1:], active)
        interrupt = [True]
        environment = {"DEFAULT_BRANCH": "master", "GITHUB_EVENT_NAME": "push", "GITHUB_REF": "refs/heads/master", "GITHUB_SHA": "f" * 40}
        with (patch.object(backends, "repository_json", side_effect=api), patch.object(backends, "require_idle_readers"),
              patch.object(backends, "repository_pages", side_effect=lambda _: list(releases.values())), patch.dict(os.environ, environment)):
            with patch.dict(os.environ, {"GITHUB_EVENT_NAME": "pull_request"}), self.assertRaisesRegex(ValueError, "trusted"):
                backends.retain_releases()
            with self.assertRaisesRegex(ValueError, "pinned"):
                rotate({releases[1]["tag_name"]})
            self.assertEqual(4, len(releases))
            with self.assertRaises(ConnectionError):
                backends.retain_releases()
            backends.retain_releases()
            backends.retain_releases()
        self.assertEqual({2, 4}, releases.keys())
        self.assertEqual({current["tag_name"], backup["tag_name"]}, refs.keys())
        self.assertEqual("Backend Prebuilts 4", current["name"])
        self.assertEqual("Backend Prebuilts 2 (Backup)", backup["name"])
        self.assertEqual("Certified.", current["body"])

    def test_download_negotiates_the_media_type_for_each_github_endpoint(self):
        payload = b"certified archive"
        digest = hashlib.sha256(payload).hexdigest()
        with (patch.dict(os.environ, {"GITHUB_API_URL": "https://api.github.com", "GITHUB_REPOSITORY": "owner/repo"}),
              patch.object(github_artifacts, "build_opener") as transport):
            for resource, accept in (("actions/artifacts/1/zip", "application/vnd.github+json"),
                                     ("releases/assets/2", "application/octet-stream")):
                transport.return_value.open.return_value = io.BytesIO(payload)
                destination = self.root / resource.split("/")[-1]
                github_artifacts.download(resource, destination, digest)
                request = transport.return_value.open.call_args.args[0]
                self.assertEqual(accept, request.get_header("Accept"))
                self.assertEqual(payload, destination.read_bytes())

    def test_interrupted_draft_reuses_only_the_exact_placeholder(self):
        revision = "a" * 40
        registrations = backends.registrations()
        platforms = {name: {"target": name, "asset": value["asset"], "sha256": "b" * 64}
                     for name, value in registrations.items()}
        archives = {p["asset"]: self.root / p["asset"] for p in platforms.values()}
        name = sorted(archives)[0]
        placeholder = {"name": name, "id": 123, "size": 0, "state": "starter"}
        draft = {"tag_name": "backend-prebuilts-2026.01.01.1", "draft": True, "prerelease": True,
                 "id": 1, "target_commitish": revision, "body": "", "assets": [placeholder]}
        current = copy.deepcopy(draft)
        current["assets"][0]["id"] = 124
        def publish():
            backends.publish_release([draft], platforms, archives, revision, set())
        with patch.object(backends, "repository_json", return_value=current) as api:
            with self.assertRaisesRegex(ValueError, "changed before recovery"):
                publish()
            api.assert_called_once_with("releases/1")
        current["assets"][0]["id"] = 123
        uploaded = []
        def api(resource, *, method="GET", **kwargs):
            if (resource, method) == ("releases/assets/123", "DELETE"):
                current["assets"].clear()
            elif resource == "releases/1" and method == "GET":
                return copy.deepcopy(current)
            elif resource.startswith("git/ref/tags/") and method == "GET":
                return None
            elif (resource, method) == ("git/tags", "POST"):
                return {"sha": "c" * 40, "object": {"sha": revision}}
            elif (resource, method) == ("git/refs", "POST"):
                self.assertCountEqual(uploaded, archives.keys())
            elif (resource, method) == ("releases/1", "PATCH"):
                return {**current, "draft": False}
            else:
                self.fail(f"Unexpected release request {method} {resource}")
        def upload(_, path):
            asset = {"name": path.name, "state": "uploaded", "digest": "sha256:" + "b" * 64}
            current["assets"].append(asset)
            uploaded.append(path.name)
            return asset
        with patch.object(backends, "repository_json", side_effect=api), patch.object(backends, "upload_release_asset", side_effect=upload):
            publish()
            self.assertCountEqual(uploaded, archives.keys())

    def test_cached_archive_requires_the_certified_bytes(self):
        data = b"certified platform"
        digest = hashlib.sha256(data).hexdigest()
        with patch.object(backends, "ROOT", self.root), patch.object(backends, "download") as download:
            download.side_effect = lambda _, path, __: path.write_bytes(data)
            package = {"assetId": 17, "sha256": digest}
            archive = backends.release_archive(package)
            self.assertEqual(archive, backends.release_archive(package))
            download.assert_called_once()
            self.assertIn(f"{len(data)} bytes cached", self.summary.read_text())
            archive.write_bytes(b"corrupt")
            with self.assertRaisesRegex(ValueError, "digest"):
                backends.release_archive(package)


if __name__ == "__main__":
    unittest.main()
