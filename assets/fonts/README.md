# 内置字体

字体源文件统一存放于此目录，由 ImGui SDL3 后端声明部署，不由 Core 或平台中立接口持有。

| 源文件 | 产品内位置 | 用途 |
| --- | --- | --- |
| `JetBrainsMono-*.ttf` | `BuiltIn/Fonts/` | 正常、粗体、斜体、粗斜体正文 |
| `icons/FA-*.ttf` | `BuiltIn/Icons/` | Font Awesome 图标 |
| `icons/InnoEngine-Icons.ttf` | `BuiltIn/Icons/` | `ImGuiIcon.Inno` 字形 |

后端项目通过 MSBuild `None` 链接声明及 `CopyToOutputDirectory`、`CopyToPublishDirectory` 将同一份资源传递给构建与发布产物。运行时读取产物中的上述路径，不依赖源码目录或系统安装字体。迁移不改变字体字节、码点、度量或加载生命周期。

默认字体用于英文界面，不内置其他语言的补充字体，也不自动合并语言字形。项目需要其他语言时，自行提供字体并通过已有加载接口显式选择。Inno 字形的设计署名和制作说明见 [品牌资源](../branding/README.md)。加载与资源释放契约见 [ImGui SDL3 后端](../../docs/backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)。

## 许可证

| 文件 | 版本与授权 | 权威来源 |
| --- | --- | --- |
| `JetBrainsMono-*.ttf` | 2.304，SIL OFL 1.1；[完整许可及版权声明](LICENSE-JetBrainsMono.txt) | [JetBrains v2.304](https://github.com/JetBrains/JetBrainsMono/blob/v2.304/OFL.txt) |
| `icons/FA-Regular-400.ttf`、`icons/FA-Solid-900.ttf` | Font Awesome Free 6.7.2，字体适用 SIL OFL 1.1；[完整官方许可](icons/LICENSE-FontAwesome.txt) | [Font Awesome 6.7.2](https://github.com/FortAwesome/Font-Awesome/blob/6.7.2/LICENSE.txt) |
| `icons/InnoEngine-Icons.ttf` | 由 xdzzyq 的 Logo 转换；[仅随 InnoEngine 使用的独立许可](../branding/LICENSE.txt) | [品牌资源](../branding/README.md) |

两份第三方许可按对应官方版本原样保存，不由引擎的 Apache-2.0 替代。Font Awesome 官方文件同时说明 SVG／JS 与代码的许可；本目录的两个 TTF 适用其中的字体条款。

构建和发布分别将三份许可部署为 `BuiltIn/Fonts/LICENSE-JetBrainsMono.txt`、`BuiltIn/Icons/LICENSE-FontAwesome.txt`、`BuiltIn/Icons/LICENSE-InnoEngine.txt`，与字体一起分发。重新分发字体时须保留对应版权声明和许可。
