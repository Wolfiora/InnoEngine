# InnoEngine branding assets

Logo design: [xdzzyq](https://github.com/XDzzzzzZyq). The PNG, monochrome glyphs, ICO/ICNS and icon TTF derive from this logo. The current font uses `logo-fill-simple.svg`. Conversion does not change the original designer's attribution.

## License

The original logo, derived formats and `InnoEngine-Icons.ttf` use the [separate branding license](LICENSE.txt). Use and distribution are permitted only with InnoEngine source, binaries, interfaces and accompanying documentation, retaining attribution and the license. They cannot independently brand another product. Neither the engine's Apache-2.0 license nor third-party fonts' OFL applies to them. Other uses require the rights holder's permission. See the [font licenses](../fonts/README.md#licenses) for third-party assets.

## Files and consumers

| File | Role |
| --- | --- |
| `logo.svg` | Original designer source, preserving its 512×512 viewBox, grayscale and polygons. |
| `logo.png` | 512×512 RGBA export with transparent background, centered artwork and a minimum 16 px top/bottom margin. |
| `logo-fill-simple.svg` | Simplified solid monochrome source. Four open polylines use implicit closed fills; the current TTF's U+E000 glyph consumes this file. |
| `logo-fill.svg` | Detailed solid design reference, preserving four closed polygons and transparent gaps. Not the current font source. |
| `logo-edge.svg` | Outline design reference retaining the former glyph's eight inner/outer contours. Not the current font source. |
| `logo.ico` | Windows ApplicationIcon, containing 16, 20, 24, 32, 40, 48, 64, 128 and 256 px entries. |
| `logo.icns` | macOS application icon, containing 16, 32, 64, 128, 256 and 512 px plus corresponding @2x entries. |
| [InnoEngine-Icons.ttf](../fonts/icons/InnoEngine-Icons.ttf) | Separate glyph file merged into the default font. |

This directory owns design sources and build icons. Windows entries and the macOS Editor publish target consume `logo.ico`/`logo.icns` directly without a second build-source copy or ordinary runtime-file deployment. macOS places the icon in `.app/Contents/Resources`. The TTF is distributed with the ImGui presentation assembly. See [engine glyph usage](https://github.com/Wolfiora/InnoEngine.Docs/blob/docs-modernization/content/en/backends/imgui-context.md).

## Reproduction parameters

The recorded production used FontTools 4.68.0 for TrueType, Sharp 0.35.5 / librsvg 2.63.2 for PNG, and Pillow 11.1.0 for ICO/ICNS. These are asset-production tools, not engine or everyday build dependencies.

1. The original visible bounds are x=39.32…472.68 and y=21.04…490.96. Preserve the SVG source. PNG export used the in-memory square viewBox `5.376 5.376 501.248 501.248`, producing 16 px vertical margins at 512 px.
2. The outline design uses four approved outer contours (original y=16.62…486.55). Each is inset by 14 SVG units; adjacent offset lines meet at sharp corners, with reversed inner winding. The resulting eight contours use nonzero fill, leaving each piece hollow without adding separators from the original filled regions. Exact expanded coordinates remain in `logo-edge.svg`, now a design reference.
3. Scan the Unicode cmap of every default text and Font Awesome font. The first shared unused BMP private-use code point was **U+E000**, permanently mapped to `innoengine`. Preserve that code point and check conflicts on updates.
4. Font family: `InnoEngine Icons`; style: `Regular`; units/em: 512; advance width: 512; ascender: 459; descender: -75; line gap: 0. Vertical metrics follow the current Font Awesome Solid font.
5. The current glyph consumes the four implicitly closed polylines in `logo-fill-simple.svg`. Visible source bounds are x=39.48…472.84 and y=21.04…490.96. Apply `s=480/469.92`, `x'=256+(x-256)*s`, `y'=432+(21.04-y)*s`. TrueType rounding and duplicate-point removal produce four equally wound contours, 24 points, bounds (35,-48)…(477,432), left bearing 35 and advance width 512. The detailed solid reference uses y=20.88 and 60 points. The outline reference uses `s=480/469.93`, `y'=432+(16.62-y)*s` and eight contours. Runtime icon loading remains 14 px.
6. Keep the `.notdef` and `innoengine` glyphs. Font designer metadata identifies `xdzzyq` and the GitHub URL above. Its accompanying branding license deploys as `BuiltIn/Icons/LICENSE-InnoEngine.txt`.
7. Pillow generates the ICO from `logo.png`, preserving alpha and grayscale. Each size is resampled from the 512 px source and stored as a PNG entry, supported by Windows Vista and later:

   ```sh
   python -c "from PIL import Image; Image.open('assets/branding/logo.png').convert('RGBA').save('assets/branding/logo.ico', format='ICO', sizes=[(16,16),(20,20),(24,24),(32,32),(40,40),(48,48),(64,64),(128,128),(256,256)], bitmap_format='png')"
   ```

8. Pillow creates the ICNS from the same PNG with `icp4`, `icp5`, `icp6`, `ic07`, `ic08`, `ic09`, `ic10`, `ic11`, `ic12`, `ic13` and `ic14` entries, all containing PNG data. The platform-owned Editor publish target installs it. On macOS, the same size set can also form an `.iconset` for `iconutil -c icns`.

After a source update, regenerate applicable formats and inspect glyph previews, PNG alpha/bounds, ICO/ICNS size sets and attribution. Ordinary builds do not regenerate assets; no always-running generation task is required.
