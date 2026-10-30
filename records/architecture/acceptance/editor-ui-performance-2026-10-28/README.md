# Editor UI controlled performance evidence

[Acceptance report](../EDITOR_UI_CLOSURE_ACCEPTANCE_2026_10_28.md) · [E11](https://github.com/Wolfiora/InnoEngine/issues/38)

Production baseline: `b303d9f10cbaf5a905f7cb529e52cf8293b0d8bd`. Corrected source: `f7dce89c0d91ecb0f2333b8ca3117fefc71db96a`. The local SDK-built binaries were frozen from that source working tree before commit metadata publication; compiled-source/PDB provenance and the runtime content hashes are retained in the evidence owner.

- [Frame measurements](frames.json): five alternating original/current process pairs per workload; five warmup and ten measured frames per process.
- [Menu/statistics measurements](menus.json): five alternating process pairs, five samples per workload/process, 2,000 operations per sample.
- [Summary](summary.json): aggregated medians and ratios; frame allocation and CPU normalized per measured frame.
- [Identity receipt](identity.json): every workload input and frozen runtime dependency matched before and after sampling.

All functional assertions pass. Shader P95 is +0.65%, CPU is unchanged and allocation decreases. Relative CPU/P95 regression is below 10% in all three recorded workloads.

This is native command validation and public headless contract measurement. It does not establish GPU, complete FlappyBird, cold-start, scroll/search/property-drag or physical-display performance. The remaining large-scene dirty-state cost is tracked in [#46](https://github.com/Wolfiora/InnoEngine/issues/46); full original/current FlappyBird comparison remains blocked by the documented API/input mismatch.

Raw TRX, per-process logs, full SHA-256 manifests, SDK/device identity and invalidated prior trials remain under `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27/e11`. Invalid or overlapping runs are not included in these final measurements.
