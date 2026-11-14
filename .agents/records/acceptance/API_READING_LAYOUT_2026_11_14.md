# API reading layout

The user requested a compact member table, explicit XML-only tips and source links at the document footer. Canonical composition now produces one member table with kind, identity and description columns; configuration/target strings remain in structured provenance instead of declaration prose. Only an explicit `source: xml` renders the GitHub-style tip before required contracts. Manual and absent-source markers do not render that notice. Synchronization ownership and stable symbol hashes are unchanged.

Documentation-tool tests passed: 131, zero failures/skips (`artifacts/acceptance/api-draft-pipeline/api-reading-02.trx`). The first run retained one obsolete expectation for the removed body editing link; its assertion now verifies retained source provenance and absence of the retired link. No product, Native or performance matrix was run. The unrelated existing ScriptingApi.cs edit remains outside this commit.

Website presentation and Vercel transport preparation are owned by the Docs repository. Deployment needs the user's Vercel account configuration and a human publication merge; this record does not claim a live Vercel deployment.
