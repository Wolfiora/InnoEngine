# Bundled fonts

This directory owns the font source files. The ImGui SDL3 backend declares their deployment; Core and platform-neutral interfaces do not own them.

| Source | Product location | Purpose |
| --- | --- | --- |
| `JetBrainsMono-*.ttf` | `BuiltIn/Fonts/` | Regular, bold, italic and bold italic text |
| `icons/FA-*.ttf` | `BuiltIn/Icons/` | Font Awesome icons |
| `icons/InnoEngine-Icons.ttf` | `BuiltIn/Icons/` | The `ImGuiIcon.Inno` glyph |

The backend's MSBuild `None` links, `CopyToOutputDirectory` and `CopyToPublishDirectory` propagate the same resources into build and publish outputs. Runtime loading uses these product-relative paths, not the source checkout or installed system fonts. Documentation migration changes no font bytes, code points, metrics or loading lifetimes.

The default fonts support the English interface. They do not bundle supplementary language fonts or automatically merge language glyphs. Supply and explicitly select project fonts through the existing loading API. See [branding provenance](../branding/README.md) for the Inno glyph and the [managed ImGui context guide](https://github.com/Wolfiora/InnoEngine.Docs/blob/docs-modernization/content/en/backends/imgui-context.md) for loading and release contracts.

## Licenses

| Files | Version and license | Authoritative source |
| --- | --- | --- |
| `JetBrainsMono-*.ttf` | 2.304, SIL OFL 1.1; [complete license and copyright](LICENSE-JetBrainsMono.txt) | [JetBrains v2.304](https://github.com/JetBrains/JetBrainsMono/blob/v2.304/OFL.txt) |
| `icons/FA-Regular-400.ttf`, `icons/FA-Solid-900.ttf` | Font Awesome Free 6.7.2, SIL OFL 1.1 for the fonts; [complete official license](icons/LICENSE-FontAwesome.txt) | [Font Awesome 6.7.2](https://github.com/FortAwesome/Font-Awesome/blob/6.7.2/LICENSE.txt) |
| `icons/InnoEngine-Icons.ttf` | Derived from xdzzyq's logo; [separate InnoEngine-only license](../branding/LICENSE.txt) | [Branding provenance](../branding/README.md) |

The two third-party license texts are preserved verbatim from their official versions. The engine's Apache-2.0 license does not replace them. Font Awesome's official file also describes SVG/JS and code licenses; the two bundled TTF files use its font terms.

Build and publish deploy all three notices beside their fonts as `BuiltIn/Fonts/LICENSE-JetBrainsMono.txt`, `BuiltIn/Icons/LICENSE-FontAwesome.txt` and `BuiltIn/Icons/LICENSE-InnoEngine.txt`. Redistributing fonts requires retaining their respective copyright and license notices.
