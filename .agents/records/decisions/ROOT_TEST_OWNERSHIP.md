# Root test ownership

The user explicitly approved replacing backend-local test ownership with a single root tests boundary in the CI/CD, platform ownership and testing refactor plan (sections 4.1 and 4.8).

Directory clause 7 now assigns backend production implementation to backends/<component> and its tests to tests/backends/<component>. Clause 11 requires all engine-owned tests, fixtures and test support in root tests and rejects production references to those projects. Upstream extern tests remain excluded.

This is a substantive approved amendment, not an English translation. The original source hashes and translation mappings remain unchanged in the rule registry. Two separately identified amendment hashes bind the active wording to this decision. All unrelated directory obligations remain in force.

Implementation evidence belongs to ../acceptance/cicd-test-migration.md; completion is not claimed by this decision.
