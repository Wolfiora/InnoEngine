"""Check publication against two local bare repositories, including a real competing push."""
import hashlib
import json
import os
from pathlib import Path
import subprocess
import sys

publisher, assembly, root = map(Path, sys.argv[1:])
engine, docs = root / "engine", root / "docs"
engine_remote, docs_remote = root / "engine.git", root / "docs.git"
source = "https://github.com/Wolfiora/InnoEngine"
os.environ.update(GIT_CONFIG_GLOBAL=str(root / "gitconfig"), GIT_CONFIG_NOSYSTEM="1",
                  GIT_ALLOW_PROTOCOL="file", GIT_TERMINAL_PROMPT="0", DOCS_TOKEN="")
root.mkdir(parents=True, exist_ok=True)


def git(directory, *args):
    return subprocess.check_output(["git", "-C", directory, *args], text=True).strip()


def write(file, content):
    file.parent.mkdir(parents=True, exist_ok=True)
    file.write_text(content, encoding="utf-8", newline="\n")
    return hashlib.sha256(file.read_bytes()).hexdigest()


def record(repo, message):
    git(repo, "add", ".")
    git(repo, "commit", "-qm", message)


for repo, branch in ((engine, "master"), (docs, "main")):
    repo.mkdir()
    git(repo, "init", "-qb", branch)
git(root, "config", "--global", "user.name", "Documentation fixture")
git(root, "config", "--global", "user.email", "fixture@example.invalid")
write(engine / "input", "source")
record(engine, "initial engine")
revision, tree = git(engine, "rev-parse", "HEAD"), git(engine, "rev-parse", "HEAD^{tree}")
git(root, "clone", "--bare", engine, engine_remote)
git(engine, "remote", "add", "origin", source + ".git")
write(docs / ".gitattributes", "* text=auto eol=lf\n")
write(docs / ".gitignore", "artifacts/\n")
write(docs / "content/en/manual/task.md", "Independent manual.")
record(docs, "initial manual")
main = git(docs, "rev-parse", "HEAD")
git(docs, "checkout", "-qb", "draft")
git(root, "clone", "--bare", docs, docs_remote)
for remote, url in ((engine_remote, source + ".git"), (docs_remote, source + ".Docs.git")):
    git(root, "config", "--global", "url." + remote.as_posix() + ".insteadOf", url)

inputs, packet = root / "inputs", root / "api-package.json"
signature = "public class Probe.Value"
symbol = dict(project="Probe", id="T:Probe.Value", kind="NamedType", signature=signature,
              signatureHash=hashlib.sha256(signature.encode()).hexdigest(), source="src/Value.cs", line=1,
              containingType=None, baseType=None, interfaces=[])
facts = dict(model="source-clr", configuration="Debug", target=None, projects=[
    dict(name="Probe", project="src/Probe.csproj", symbols=[symbol])])
catalog = "reference/projects/catalog.json"
capture = "reference/api/probe.json"
identity = write(inputs / catalog, json.dumps([dict(name="Probe", path="src/Probe.csproj", isTest=False, surfaces=["source-clr"])]))


def package(description):
    symbol["documentation"] = f"<member><summary>{description}</summary></member>"
    digest = write(inputs / capture, json.dumps(facts))
    manifests = [dict(path=capture, sha256=digest, model="source-clr", configuration="Debug", target=None,
                      sourceRevision=revision, sourceTree=tree)]
    write(inputs / "reference/engine.lock.json", json.dumps(dict(repository=source, revision=revision,
          sourceTree=tree, toolingRevision=revision, projectManifest=catalog,
          projectManifestIdentity=identity, apiManifests=manifests)))
    subprocess.run(["dotnet", assembly, "docs", "api-package", "--docs-root", inputs, "--package", packet], check=True)


def publish(name, success=True):
    result = subprocess.run([sys.executable, "-B", publisher, "publish", "--engine-root", engine,
                             "--assembly", assembly, "--package", packet, "--workspace", root / name],
                            text=True, stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    if (result.returncode == 0) != success:
        raise AssertionError(result.stdout)
    return result.stdout


package("A source value.")
publish("first")
first = git(docs_remote, "rev-parse", "draft")
assert git(docs_remote, "rev-parse", "main") == main
assert git(docs_remote, "show", "draft:content/en/manual/task.md") == "Independent manual."
git(docs, "checkout", "main")
write(docs / "release-note.txt", "Main-only release metadata.")
record(docs, "release")
git(docs, "push", docs_remote, "main")
publish("again")
assert git(docs_remote, "rev-parse", "draft") == first
author = root / "author"
git(root, "clone", "--branch", "draft", docs_remote, author)
fragment = json.loads((author / "reference/api/content.json").read_bytes())["entries"][0]["path"]
manual = (author / fragment).read_text(encoding="utf-8").replace("source: xml", "source: Author") + "\nIndependent author prose.\n"
write(author / fragment, manual)
record(author, "take ownership")
package("Updated XML explanation.")
hook = docs_remote / "hooks/pre-receive"
write(hook, '#!/bin/sh\nset -eu\nrm -- "$0"\n'
      'unset GIT_DIR GIT_WORK_TREE GIT_INDEX_FILE GIT_QUARANTINE_PATH GIT_OBJECT_DIRECTORY GIT_ALTERNATE_OBJECT_DIRECTORIES\n'
      'git -C "$INNO_FIXTURE_AUTHOR" push origin HEAD:refs/heads/draft\n')
hook.chmod(0o755)
os.environ["INNO_FIXTURE_AUTHOR"] = author.as_posix()
assert "Recompute ownership" in publish("race")
published = git(docs_remote, "show", f"draft:{fragment}")
marker = "# Authoring metadata."
assert marker in published and published.split(marker)[1] == manual.rstrip().split(marker)[1]
assert "outdatedFor:" in published and "outdatedFor:" not in manual
assert "Updated XML explanation." in git(docs_remote, "show", "draft:" + capture)
last = git(docs_remote, "rev-parse", "draft")
write(engine / "input", "New engine revision.")
record(engine, "advance engine")
git(engine, "push", "origin", "master")
assert "superseded" in publish("stale", False)
assert git(docs_remote, "rev-parse", "draft") == last
