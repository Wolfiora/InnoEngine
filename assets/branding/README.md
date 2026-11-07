# InnoEngine 品牌资源

Logo 设计：[xdzzyq](https://github.com/XDzzzzzZyq)。PNG、单色字形和图标 TTF 均由该 Logo 转换而来；转换不改变原设计的作者归属。

## 文件与使用

- `logo.svg`：保留原始灰阶的矢量原稿，中心主体以统一裁剪轮廓覆盖颜色，避免三个面之间透出抗锯齿细缝。
- `logo.png`：512×512 RGBA，透明背景，图形居中，上下最小边距 16px；引擎内嵌的是这同一份文件。
- `logo-icon.svg`：展开裁剪后的单色轮廓，保留原有镂空，中央三个面合为一个整体。
- [InnoEngine-Icons.ttf](../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/BuiltIn/Icons/InnoEngine-Icons.ttf)：默认字体合并使用的独立字形文件。

原版图片读取与释放见 [Inno.Engine.Default](../../docs/modules/runtime/Inno.Engine.Default.md#品牌图片)。字体使用见 [Editor ImGui](../../docs/modules/editor/Inno.Editor.ImGui.md#引擎-logo-图标)。

## 制作参数

本次采用 FontTools 4.68.0 制作 TrueType 字体，Sharp 0.35.5 / librsvg 2.63.2 导出 PNG。工具只在制作时使用，不进入引擎或日常构建。

1. 原图可见边界为 x=39.32…472.68、y=16.62…486.55。保持几何坐标，使用正方形 viewBox `5.370666667 0.955666667 501.258666667 501.258666667`，导出 512px PNG 时上下留白为 16px。
2. 单色版取主体 clipPath 的闭合轮廓和外围三个多边形；不取被裁剪的大矩形，也不额外描边。以这些轮廓构造四个互不相接的字形 contour。
3. 扫描全部默认正文、Font Awesome 和补充 CJK 字体的 Unicode cmap。首个共同空闲的 BMP 私用区码点为 **U+E000**，固定映射到 `innoengine`；后续更新保持这个码点并检查冲突。
4. 字体 family 为 `InnoEngine Icons`，style 为 `Regular`，units/em=512，advance width=512，ascender=459，descender=-75，line gap=0。纵向度量参考当前 Font Awesome Solid。
5. 对原轮廓应用 `s=480/469.93`、`x'=256+(x-256)*s`、`y'=432+(16.62-y)*s`。TrueType 取整后可见边界为 (35,-48)…(477,432)，左侧 bearing=35。运行时沿用 14px 图标加载尺寸。
6. 保留 `.notdef` 和 `innoengine` 两个 glyph。字体 designer 为 `xdzzyq`，designer URL 为上述 GitHub 地址；不添加未经提供的授权声明。

更新 SVG 后重新导出所需成品，并复核字形预览、PNG 内容测试与署名。无需维护自动生成任务。
