---
tags: [documentation, wiki, public-api]
---
# Documentation

[InnoEngine.Docs](https://github.com/Wolfiora/InnoEngine.Docs) owns the rules for documentation, API prose, examples and content ownership: the [documentation rules](https://github.com/Wolfiora/InnoEngine.Docs/blob/main/.agents/rulebook/documentation.md) on `main`, the published branch; [`draft`](https://github.com/Wolfiora/InnoEngine.Docs/blob/draft/.agents/rulebook/documentation.md) holds unpublished changes.

Run `rules select` again with `--docs-root <explicit-checkout>`. It then also selects from the rule files of that checkout and lists them with the `docs/` prefix. It never guesses a sibling path or downloads the rules.
