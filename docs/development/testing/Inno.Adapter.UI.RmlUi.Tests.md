# Inno.Adapter.UI.RmlUi.Tests

[测试与 fixture 索引](README.md)

[UI 索引](../../modules/ui/README.md) · [领域测试](Inno.UI.Tests.md) · [Wiki 首页](../../README.md)

此 backend 项目承接原 `Inno.UI.Tests/UiServiceTests.cs` 的全部实现断言，验证真实 RML 前端、导入器、字体 family/weight、DOM 事件、命中、Context/Document/Texture 释放及陈旧 native handle 拒绝。

依赖 RmlUi runtime/authoring、Assets 导入和 DotNet 类型/序列化发现。字体来自第三方只读样本。执行要求匹配目标与配置的 `inno-ui` 和依赖动态库；缺库、错误 ABI 或生命周期失败不能记为通过。

测试程序集不进入产品。`UiServiceTests` 的 public 构造、测试方法与 `Dispose` 仅属于 xUnit fixture，没有产品扩展 API 或 protected 契约。每次测试拥有临时 Project、Identity scope、ModuleHost、TypeCatalog 和 SerializationRegistry，按依赖逆序释放。

```powershell
dotnet test backends/RmlUi/tests/Inno.Adapter.UI.RmlUi.Tests/Inno.Adapter.UI.RmlUi.Tests.csproj --configuration Debug
```

正式验收由统一计划准备目标与部署闭包；上面的直接调用要求这些前提已经准备完成。
