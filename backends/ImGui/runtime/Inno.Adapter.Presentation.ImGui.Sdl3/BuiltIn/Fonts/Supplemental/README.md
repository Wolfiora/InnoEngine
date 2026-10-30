# Supplemental text font

This unmodified Noto Sans CJK SC Regular 2.004 face supplies CJK glyphs missing from the existing text faces. Existing text and icon sources retain priority. It is loaded once into one context-owned pinned buffer, shared by that context's native font sources and released after native context destruction. Glyphs are baked on demand; the complete glyph set is not pre-rendered.

- Upstream: https://github.com/notofonts/noto-cjk
- Font: https://raw.githubusercontent.com/notofonts/noto-cjk/Sans2.004/Sans/OTF/SimplifiedChinese/NotoSansCJKsc-Regular.otf
- Font SHA-256: `2c76254f6fc379fddfce0a7e84fb5385bb135d3e399294f6eeb6680d0365b74b`
- Font bytes: 16,437,364
- License: unmodified SIL Open Font License text in [LICENSE](LICENSE), obtained from https://raw.githubusercontent.com/notofonts/noto-cjk/main/Sans/LICENSE
- License SHA-256: `6a73f9541c2de74158c0e7cf6b0a58ef774f5a780bf191f2d7ec9cc53efe2bf2`

The shared presentation adapter owns this content. No operating-system font lookup, platform table, separate font manager or generated-binding change is needed.
