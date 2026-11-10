# 内置字体

字体源文件统一存放于此目录，由 ImGui SDL3 后端声明部署，不由 Core 或平台中立接口持有。

| 源文件 | 产品内位置 | 用途 |
| --- | --- | --- |
| `JetBrainsMono-*.ttf` | `BuiltIn/Fonts/` | 正常、粗体、斜体、粗斜体正文 |
| `Supplemental/NotoSansCJKsc-Regular.otf` | `BuiltIn/Fonts/Supplemental/` | 四种正文共用的中文补充字形 |
| `icons/FA-*.ttf` | `BuiltIn/Icons/` | Font Awesome 图标 |
| `icons/InnoEngine-Icons.ttf` | `BuiltIn/Icons/` | `ImGuiIcon.Inno` 字形 |

后端项目通过 MSBuild `None` 链接声明及 `CopyToOutputDirectory`、`CopyToPublishDirectory` 将同一份资源传递给构建与发布产物。运行时读取产物中的上述路径，不依赖源码目录或系统安装字体。迁移不改变字体字节、码点、度量或加载生命周期。

中文字体来源与许可见 [Supplemental](Supplemental/README.md)；Inno 字形的设计署名和制作说明见 [品牌资源](../branding/README.md)。加载与资源释放契约见 [ImGui SDL3 后端](../../docs/backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)。
