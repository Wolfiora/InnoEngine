"""Replay the retained hosted validation and backend publication incidents offline."""
import hashlib
import json
import os
from pathlib import Path
import sys
import tempfile
import unittest
from unittest.mock import patch

sys.path.insert(0, str(Path(sys.argv.pop(1))))
import backend_artifacts as backends
import validation


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

    def test_release_rotation_resume_pins_and_rollback(self):
        revision = "a" * 40
        releases = []
        for generation in range(1, 6):
            hashes = {item["asset"]: str(generation) * 64 for item in backends.registrations().values()}
            decide = lambda active=(), partial=False: backends.release_plan(releases, hashes, revision, active, partial)
            operation, tag, _, retire = decide()
            self.assertEqual("create", operation)
            if len(releases) == 3:
                self.assertEqual(releases[0]["id"], retire["id"])
                self.assertEqual(releases[1]["id"], decide([retire["tag_name"]])[3]["id"])
                with self.assertRaisesRegex(ValueError, "pinned"):
                    decide([item["tag_name"] for item in releases])
            draft = dict(id=generation, tag_name=tag, draft=True, prerelease=True, target_commitish=revision, assets=[])
            releases.append(draft)
            self.assertEqual("resume", decide()[0])
            draft["target_commitish"] = "b" * 40
            with self.assertRaisesRegex(ValueError, "Another producer"):
                decide()
            draft.update(target_commitish=revision, draft=False, published_at=f"2026-01-0{generation}",
                         assets=[{"name": name, "digest": "sha256:" + digest, "state": "uploaded"} for name, digest in hashes.items()])
            if retire:
                releases.remove(retire)
            self.assertEqual(("reuse", tag), decide()[:2])
        oldest = {a["name"]: a["digest"][7:] for a in releases[0]["assets"]}
        self.assertEqual("create", backends.release_plan(releases, oldest, revision, [], True)[0])

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
