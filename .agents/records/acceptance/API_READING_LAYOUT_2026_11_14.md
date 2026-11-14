# API reading layout

The XML renderer writes the source summary first and groups remarks, parameters, type parameters, returns, values, exceptions, examples and related references into actual Markdown sections. Independent API pages place the summary below their title and Project/API/Scope metadata immediately before inheritance. Declarations remain derived facts. Manual fragments retain their original bytes and existing prose ownership.

Exact symbol queries now return the complete independently registered symbol page, including its introduction and following contracts. Larger pages expose an explicit whole-page continuation. Multi-symbol narrative pages retain bounded symbol reads. The parser schema changes to invalidate older composed snapshots.

Functional validation: the documentation tooling suite passed 132 cases. The initial new regression exposed a missing summary in symbol reads; `api-layout-01.trx` preserves that failure. The corrected run and final run are recorded in `artifacts/acceptance/api-draft-pipeline/api-layout-02.trx` and `api-layout-final.trx`. Rule preservation checks pass all 241 identities and eight external documentation units. No engine product, Native, performance or cross-platform matrix is claimed by this change.

The Docs repository owns sidebar rendering and the regenerated XML fragments. Its matching tooling binding must select the implementation containing this change. No product signatures, runtime behavior or captured API source revision change.
