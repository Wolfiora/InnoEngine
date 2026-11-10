# Delivery

Load complete rule units. The [registry](coverage.json) owns routing and provenance.

<!-- rule-unit: delivery -->
<a id="delivery"></a>
## 31. Issues and pull requests

- When creating or updating a repository issue or pull request, use a `type(scope): description` title. Both the title and body must be entirely in English.
- Issues must distinguish reproduced defects from risks awaiting verification, and state the responsible owner, reproduction conditions, actual result, expected result and executable acceptance criteria. Search existing issues before submission to avoid duplicates.
- Pull request bodies must explain the problem, final behavior and actual verification against the complete difference from the target branch, and identify checks that were not run. Search existing PRs before creation. Never describe partial acceptance as a passed complete matrix.
- Use `Aaron Liao <hsuankailiao@gmail.com>` for both Git author and committer. Do not use a Codex/bot identity or add any `Co-authored-by` attribution. Create pull requests with the GitHub account authorized by the user.
- Reproduce BGCS defects independently through public APIs and identify them explicitly. Do not modify BGCS source, packages or binaries, or manually edit generated output to bypass defects.
<!-- /rule-unit: delivery -->

<!-- rule-unit: notification -->
<a id="notification"></a>
## 16. Completion sound
- By default, play `/System/Library/Sounds/Glass.aiff` before waiting for a design decision that changes scope, after completing the final design, and after completing the requested code or file operations and their necessary validation. The user need not repeat this request for each task.
- If the environment cannot access audio, state in the final result that the sound could not be played. Do not play it when the user explicitly requests silence.
<!-- /rule-unit: notification -->
