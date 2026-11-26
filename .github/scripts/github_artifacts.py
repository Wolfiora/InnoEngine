"""GitHub API operations used by repository workflows."""

import hashlib
import json
import shutil
import os
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
    request = Request(url, headers={"Accept": "application/octet-stream", "Authorization": f"Bearer {os.environ['GH_TOKEN']}"})
    with build_opener(_ReleaseRedirect()).open(request, timeout=90) as response, destination.open("xb") as target:
        shutil.copyfileobj(response, target)
    with destination.open("rb") as target:
        if hashlib.file_digest(target, "sha256").hexdigest() != expected:
            raise ValueError(f"Downloaded artifact does not match its certified digest: {resource}")


def repository_json(resource):
    url = f"{os.environ['GITHUB_API_URL']}/repos/{os.environ['GITHUB_REPOSITORY']}/{resource}"
    request = Request(url, headers={"Accept": "application/vnd.github+json",
                                    "Authorization": f"Bearer {os.environ['GH_TOKEN']}"})
    with build_opener(_ReleaseRedirect()).open(request, timeout=30) as response:
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
