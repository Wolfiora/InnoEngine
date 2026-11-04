# Inno.UI.Tests

[UI 索引](README.md) · [Wiki 首页](../README.md) · [RmlUi 实现验收](Inno.Adapter.UI.RmlUi.Tests.md)

验证中立 `UiRuntime` 的帧作用域、输入与脚本 facade 绑定，通过公开可替换 backend 边界运行；不引用 RmlUi，不要求窗口、GPU 或 native 部署。原 RmlUi 导入、字体、DOM、Context 与原生生命周期断言全部属于独立 backend 测试项目。

测试入口不属于产品 API，没有 protected 扩展契约。fixture 在结束时释放自己创建的 Runtime 和借用边界。

```powershell
dotnet test tests/ui/Inno.UI.Tests/Inno.UI.Tests.csproj
```
