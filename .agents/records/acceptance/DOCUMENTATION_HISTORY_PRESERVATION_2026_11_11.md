# Historical evidence preservation during documentation migration

The 193 original record entries have explicit dispositions: 112 unchanged historical Markdown records, 68 byte-identical evidence payloads, six historical records with link-only repairs and seven English navigation/current-status entries. The original baseline remains available through immutable Git links for rewritten navigation/current summaries. Historical execution conclusions, identities, language and limitations are not promoted to current acceptance.

Seventeen original screenshots moved from docs/architecture/images to .agents/records/history/visual-evidence. Every source was checked against its baseline SHA-256 before movement and every destination afterward. Four historical reports now link to the new relative locations; the other two historical link repairs only follow translated current-ledger anchors. A bounded comparison permits only those exact link replacements and line-ending normalization for Markdown. Original JSON/CSV/image payloads are never normalized.

The existing agent-document structure/link validator passed using the compiled Build entry and the positional repository-root argument. An initial invocation incorrectly used --engine-root and failed before validation; its log is retained separately. No SDK graph, engine compile or Native preparation ran. Moving evidence changes ownership location, not disk usage or historical certification.

Full product-content review and validator cutover are still open. The migration inventory records actual remaining work rather than assuming every old file is obsolete because project entries now exist.
