# Independent API element pages

The approved presentation change replaces aggregate type/member pages with one published page per exact API symbol. Type pages retain their own contract and use description tables for immediate members and nested types. Declaring chains come from captured containment, not filename or namespace guesses. Overloads keep their exact identities and separate pages.

Project categories use recursive tables with descriptions from authoritative project guides. Script directories retain their populated alphabetical sections. A symbol page carries its own source facts, editorial ownership, source link and required declaring-type contracts in the shared website/query model.

Registered member prose moves from hidden `_members` files to dotted element filenames. The synchronization transaction repairs Markdown link destinations and former aggregate-page symbol anchors using parser source spans. Manual explanation is preserved; source facts remain generated. Independent guides are never regenerated. Hidden-fragment query forwarding is removed.

## Local functional evidence

- All 128 documentation-tool regressions passed (`artifacts/acceptance/api-draft-pipeline/api-elements-05.trx`). Coverage includes nested generic structs/enums, overload links, independently queryable symbols, inherited obligations, old-page link migration, manual prose preservation, and idempotence.
- Earlier failed attempts are retained. Existing aggregate-page assertions were updated for the new approved unit; the migration regression identified missing precise parser URL spans before any real content migration.
- Rule coverage/loading passed in `api-elements-rule-check.log`.
- No product, Native, full-engine, stress or performance matrix was run.

Website migration and browser evidence belong to the companion Docs change. This record does not claim deployment. The unrelated existing Editor Assets `ScriptingApi.cs` working-tree edit was preserved and excluded.
