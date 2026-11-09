# InnoEngine 品牌资源

Logo 设计：[xdzzyq](https://github.com/XDzzzzzZyq)。PNG、单色字形和图标 TTF 由该 Logo 转换而来；ICO/ICNS 构建图标由该 Logo 生成，存放在 `build/branding`。转换不改变原设计的作者归属。

## 文件与使用

- `logo.svg`：作者提供的 `Logo 3D v2(1).svg` 原稿，原样保留 512×512 viewBox、灰阶与多边形。
- `logo.png`：由上述 SVG 导出的 512×512 RGBA，透明背景，图形居中，上下最小边距 16px；供品牌展示使用。
- `logo-icon.svg`：用户确认的空心单色图标。四块形状分别描边、内部透明，保留原有间隔；中央不画三面的分界线。描边已展开为可直接导入字体的闭合轮廓。
- [InnoEngine-Icons.ttf](../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/BuiltIn/Icons/InnoEngine-Icons.ttf)：默认字体合并使用的独立字形文件。

本目录只保存设计源和文档资源。Windows/macOS 构建图标位于 [build/branding](../../build/branding/README.md)：`logo.ico` 供入口 `<ApplicationIcon>` 使用，`logo.icns` 供 `InnoEngine.Editor.app` 使用。构建不读取本目录的派生图标，也不把它们作为普通运行时文件部署；TTF 随 ImGui 呈现程序集分发。字体使用见 [Editor ImGui](../../docs/modules/editor/Inno.Editor.ImGui.md#引擎-logo-图标)。

## 制作参数

本次采用 FontTools 4.68.0 制作 TrueType 字体，Sharp 0.35.5 / librsvg 2.63.2 导出 PNG，Pillow 11.1.0 生成 `build/branding` 中的 ICO 和 ICNS。工具只在制作时使用，不进入引擎或日常构建。

1. 当前原图可见边界为 x=39.32…472.68、y=21.04…490.96。SVG 原稿不改写；仅导出 PNG 时在内存中采用正方形 viewBox `5.376 5.376 501.248 501.248`，使 512px PNG 上下留白为 16px。
2. 空心版采用已确认预览的四块外轮廓（原坐标 y=16.62…486.55）。每块向内偏移 14 个 SVG 单位，相邻偏移直线相交形成尖角，内轮廓反向绕行；四对内外轮廓共八个 contour。轮廓使用 nonzero 填充规则，四块内部为空；不引入原稿填色面之间的分界线。`logo-icon.svg` 已保存展开后的精确坐标，重新制作字体时直接消费该源稿。
3. 扫描全部默认正文、Font Awesome 和补充 CJK 字体的 Unicode cmap。首个共同空闲的 BMP 私用区码点为 **U+E000**，固定映射到 `innoengine`；后续更新保持这个码点并检查冲突。
4. 字体 family 为 `InnoEngine Icons`，style 为 `Regular`，units/em=512，advance width=512，ascender=459，descender=-75，line gap=0。纵向度量参考当前 Font Awesome Solid。
5. 对单色源稿的全部内外轮廓应用 `s=480/469.93`、`x'=256+(x-256)*s`、`y'=432+(16.62-y)*s`。TrueType 取整后可见边界为 (35,-48)…(477,432)，左侧 bearing=35，线宽约 14.3 font units。运行时沿用 14px 图标加载尺寸。
6. 保留 `.notdef` 和 `innoengine` 两个 glyph。字体 designer 为 `xdzzyq`，designer URL 为上述 GitHub 地址；不添加未经提供的授权声明。
7. `build/branding/logo.ico` 使用 Pillow 11.1.0 从 `logo.png` 生成，保留透明背景与灰阶。每个尺寸由 512px 源图缩放，并以 PNG 条目写入；Windows Vista 及以上支持该格式。重新生成命令：

   ```sh
   python -c "from PIL import Image; Image.open('assets/branding/logo.png').convert('RGBA').save('build/branding/logo.ico', format='ICO', sizes=[(16,16),(20,20),(24,24),(32,32),(40,40),(48,48),(64,64),(128,128),(256,256)], bitmap_format='png')"
   ```

8. `build/branding/logo.icns` 使用 Pillow 11.1.0 从 `logo.png` 生成，包含 `icp4`、`icp5`、`icp6`、`ic07`、`ic08`、`ic09`、`ic10`、`ic11`、`ic12`、`ic13`、`ic14` 条目，全部以 PNG 数据写入 ICNS 容器。macOS 分发由 Editor 的 publish 目标放入 `.app/Contents/Resources`，也可以在 macOS 上用相同尺寸的 `.iconset` 执行 `iconutil -c icns` 重建。

更新 SVG 后重新导出所需成品，并复核字形预览、PNG 透明边界、ICO/ICNS 尺寸集合与署名。无需维护自动生成任务，构建也不执行生成命令。
