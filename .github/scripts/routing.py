"""Select the hosted validation jobs for one GitHub event."""

import argparse
import json
import os
import re
import subprocess
from pathlib import Path

from github_artifacts import repository_pages


def benchmark_only(base, candidate):
    if base == "0" * 40:
        return False  # A new branch has no previous push to compare.
    if not re.fullmatch("[0-9a-f]{40}", base):
        raise ValueError("Expected an exact base commit")
    names = subprocess.check_output(
        ["git", "diff", "--name-only", "--no-renames", "-z", base, candidate, "--"]
    ).split(b"\0")
    paths = [name for name in names if name]
    return bool(paths) and all(name.startswith(b"benchmark/") for name in paths)


def matching_pull(commit, ref):
    for pull in repository_pages(f"commits/{commit}/pulls"):
        head = pull["head"]
        if (pull["state"] == "open" and head["sha"] == commit
                and (head["repo"] or {}).get("full_name") == os.environ["GITHUB_REPOSITORY"]
                and "refs/heads/" + head["ref"] == ref):
            return pull["number"]
    return None


def select(event, data, commit):
    if not re.fullmatch("[0-9a-f]{40}", commit):
        raise ValueError("Expected an exact candidate commit")
    profile = "pull-request"
    if event == "workflow_dispatch":
        if str(data.get("inputs", {}).get("full", "false")).lower() == "true":
            profile = "full"
        return profile, "validation", None
    if event == "push":
        profile = "master" if data["ref"] == "refs/heads/" + data["repository"]["default_branch"] else "fast"
        base = data["before"]
    elif event == "pull_request":
        base = data["pull_request"]["base"]["sha"]
    elif event == "merge_group":
        base = data["merge_group"]["base_sha"]
    else:
        raise ValueError(f"Unsupported event: {event}")
    if benchmark_only(base, commit):
        return profile, "benchmark-only", None
    if profile == "fast":
        pull = matching_pull(commit, data["ref"])
        if pull is not None:
            return profile, "open-pull-request", pull
    return profile, "validation", None


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("operation", choices=["select"])
    parser.parse_args()
    event = os.environ["GITHUB_EVENT_NAME"]
    data = json.loads(Path(os.environ["GITHUB_EVENT_PATH"]).read_text(encoding="utf-8"))
    commit = os.environ["GITHUB_SHA"]
    profile, reason, pull = select(event, data, commit)
    config = Path(__file__).resolve().parents[1]
    hosts = json.loads((config / "validation-hosts.json").read_text(encoding="utf-8"))
    hosts = [host for host in hosts if profile != "fast" or host["fast"]]
    if not hosts or len({host["runner"] for host in hosts}) != len(hosts):
        raise ValueError("Hosted execution registration is empty or duplicated")
    targets = json.loads((config / "native-targets.json").read_text(encoding="utf-8"))
    requested = [name.strip() for name in os.getenv("REBUILD_TARGETS", "").split(",") if name.strip()]
    if len(set(requested)) != len(requested) or set(requested) - {target["target"] for target in targets}:
        raise ValueError("Unknown or duplicate backend target")
    if reason == "validation" and profile == "fast":
        # PR creation can race the push event. Recheck before scheduling the worker.
        pull = matching_pull(commit, data["ref"])
        if pull is not None:
            reason = "open-pull-request"
    groups = []
    if reason == "validation":
        bootstrap = len(hosts) > 1 and not any(
            release["prerelease"] and not release["draft"]
            and re.fullmatch(r"backend-prebuilts-[0-9.]+", release["tag_name"])
            for release in repository_pages("releases")
        )
        if bootstrap:
            groups.append({"name": "Initial catalog", "hosts": {"include": hosts}, "rebuild": ",".join(requested)})
        else:
            for host in hosts:
                ids = {target["target"] for target in targets if target["runner"] == host["runner"]}
                groups.append({"name": host["runner"], "hosts": {"include": [host]},
                               "rebuild": ",".join(name for name in requested if name in ids)})
    outputs = {
        "run": str(reason == "validation").lower(), "profile": profile, "reason": reason,
        "pull": str(pull or ""), "groups": json.dumps({"include": groups}), "hosts": json.dumps({"include": hosts}),
    }
    with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
        for key, value in outputs.items():
            print(f"{key}={value}", file=stream)
    print(json.dumps(outputs))


if __name__ == "__main__":
    main()
