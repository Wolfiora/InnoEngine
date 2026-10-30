# Editor UI production-candidate evidence

Production revision: `dd5979c613b0fe8ebd9f7026f25001ceee9f8cee`.

Read the [final report](../EDITOR_UI_FINAL_ACCEPTANCE_2026_10_30.md) first. `manifest.json` records exact content hashes for this portable subset. The directory-local Git attribute preserves evidence bytes on every host, including the original CSV/JSON line endings; it does not alter source/build configuration.

`test-cases.csv` contains overlapping profile executions, not a summed distinct-test claim. `raw-*-measurements.json` preserves every initial producer measurement and the predeclared graph-tail repeat. Initial flags and failed historical attempts are not reclassified as passing trials. Complete TRX/binlogs/runtime manifests and failed-attempt logs remain in the recorded external evidence owner.

The 307-row plan matrix includes ten interface journeys, seven status-vocabulary definitions and two future post-merge audit rules. At report authoring, fifteen delivery/CI conditions remain pending; their exact final disposition is signed in the [stable PR verification attachment](https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338), together with the containing commit SHA and exact-head CI. This prevents a self-referential report-commit loop without using historical CI for a new head.

Physical/manual limits are explicit in the report. These files are acceptance evidence, never a production registry, resolver, configuration source or cache identity input.
