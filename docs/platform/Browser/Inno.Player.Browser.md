# Inno.Player.Browser

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser 产品入口、JS 生命周期与 HTTP 内容来源。共享 Player 运行逻辑只有一份。

## 组合、生命周期与扩展

Program 与 BrowserPlayerComposition 只组合明确浏览器能力。main.js 提供页面启动和系统回调；BrowserBridge 将生命周期/帧调度接到共同 Shell。输入仍由平台 backend 转为 Core Events，再由同一 EventInput 实现产生快照。

HttpPlayerContentSource 先读取 owned manifest/catalog metadata，再取得并验证 Pack，使用共同 Reader 构造 owned store。Pack 身份只来自 catalog；不写 /Content 再解压为第二份目录。浏览器存储实现独立归本平台包。

创建的 stream/store、任务和 callback 由产品/共享 Player 明确接管。退出停止新工作、取消下载、注销回调、提交存储并释放 Session 和设备。Browser Editor 没有实现或注册。

本项目的 `OutputType=Exe` 表示托管入口，实际由网页宿主启动。直接 Build 需要冻结的 Native binding 选择与指纹；发布还需要原生 archive 闭包。通过统一 `game` 导出，再使用 HTTP 服务打开输出的 `index.html`，不能直接运行 Windows `.exe` 或以 `file://` 加载。详见[产品启动指南](../PRODUCT_STARTUP.md)。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

本项目没有公开入口，通过组合或扩展发现使用内部实现。

## 项目依赖

- [Inno.Integration.Browser.Sdl3](Inno.Integration.Browser.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Player.Runtime](../../runtime/Inno.Player.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Default](../../runtime/Inno.Adapter.Default.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Storage.Browser](Inno.Adapter.Storage.Browser.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
