"""GitHub API operations used by repository workflows."""

import hashlib
import json
import os
import shutil
from urllib.error import HTTPError
from urllib.parse import urlencode, urlsplit
from urllib.request import HTTPRedirectHandler, Request, build_opener


class _ReleaseRedirect(HTTPRedirectHandler):
    """GitHub storage redirects use a signed URL with its own credentials."""

    def redirect_request(self, request, fp, code, message, headers, new_url):
        if urlsplit(new_url).scheme != "https":
            raise ValueError("GitHub artifact redirects must use HTTPS")
        redirected = super().redirect_request(request, fp, code, message, headers, new_url)
        if redirected and urlsplit(new_url).netloc != urlsplit(request.full_url).netloc:
            redirected.remove_header("Authorization")
        return redirected


def download(resource, destination, expected):
    url = f"{os.environ['GITHUB_API_URL']}/repos/{os.environ['GITHUB_REPOSITORY']}/{resource}"
    accept = "application/octet-stream" if resource.startswith("releases/assets/") else "application/vnd.github+json"
    request = Request(url, headers={"Accept": accept, "Authorization": f"Bearer {os.environ['GH_TOKEN']}"})
    with build_opener(_ReleaseRedirect()).open(request, timeout=90) as response, destination.open("xb") as target:
        shutil.copyfileobj(response, target)
    with destination.open("rb") as target:
        if hashlib.file_digest(target, "sha256").hexdigest() != expected:
            raise ValueError(f"Downloaded artifact does not match its certified digest: {resource}")


def repository_json(resource, *, method="GET", data=None, optional=False):
    url = f"{os.environ['GITHUB_API_URL']}/repos/{os.environ['GITHUB_REPOSITORY']}/{resource}"
    body = json.dumps(data).encode("utf-8") if data is not None else None
    request = Request(url, method=method, data=body, headers={
        "Accept": "application/vnd.github+json", "Content-Type": "application/json",
        "Authorization": f"Bearer {os.environ['GH_TOKEN']}", "X-GitHub-Api-Version": "2022-11-28",
    })
    try:
        with build_opener(_ReleaseRedirect()).open(request, timeout=60) as response:
            return None if response.status == 204 else json.load(response)
    except HTTPError as error:
        if optional and error.code == 404:
            return None
        raise


def upload_release_asset(release, path):
    endpoint = release["upload_url"].partition("{")[0]
    host = urlsplit(os.environ["GITHUB_API_URL"])
    upload = urlsplit(endpoint)
    allowed = {"uploads.github.com"} if host.hostname == "api.github.com" else {host.netloc}
    repo = os.environ["GITHUB_REPOSITORY"]
    if (upload.scheme != "https" or upload.netloc not in allowed or upload.query or upload.fragment
            or not upload.path.endswith(f"/repos/{repo}/releases/{release['id']}/assets")):
        raise ValueError("Unexpected release upload endpoint")
    with path.open("rb") as payload:
        request = Request(endpoint + "?" + urlencode({"name": path.name}), method="POST", data=payload, headers={
            "Content-Length": str(path.stat().st_size), "Content-Type": "application/octet-stream",
            "Authorization": f"Bearer {os.environ['GH_TOKEN']}", "Accept": "application/vnd.github+json",
        })
        with build_opener(_ReleaseRedirect()).open(request, timeout=600) as response:
            return json.load(response)


def repository_pages(resource, *, key=None, filters=None):
    """Yield records from a paginated endpoint for the workflow repository."""
    base = os.environ["GITHUB_API_URL"]
    repository = os.environ["GITHUB_REPOSITORY"]
    for page in range(1, 2**31):
        query = urlencode({**(filters or {}), "per_page": 100, "page": page})
        request = Request(
            f"{base}/repos/{repository}/{resource}?{query}",
            headers={
                "Accept": "application/vnd.github+json",
                "Authorization": f"Bearer {os.environ['GH_TOKEN']}",
                "X-GitHub-Api-Version": "2022-11-28",
            },
        )
        with build_opener(_ReleaseRedirect()).open(request, timeout=30) as response:
            records = json.load(response)
        if key is not None:
            records = records[key]
        if not isinstance(records, list):
            raise ValueError(f"Expected an array from {resource}")
        yield from records
        if len(records) < 100:
            return
