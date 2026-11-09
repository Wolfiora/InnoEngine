# 构建品牌资源

此目录是构建输入，不是文档展示资源。设计原稿、PNG、制作参数和使用说明见 [assets/branding](../../assets/branding/README.md)。

- `logo.ico`：Windows 入口可执行文件的 `<ApplicationIcon>`，同时供 SDL 窗口类读取。
- `logo.icns`：macOS Editor `.app` 的 `Contents/Resources/logo.icns`，由 `CFBundleIconFile` 引用。

入口项目只引用本目录。更新 Logo 时重新生成本目录的两个文件；不要把构建引用改回 `assets/branding`，也不要把它们作为普通运行时文件部署。
