# 平台/后端集成整改续跑清单

[验收报告](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md) · [批准计划](BACKEND_PLATFORM_INTEGRATION_PLAN.md)

## 从这里继续

源码重构已完成，最后 Debug/Release Solution 编译均通过。用户在额度仅剩 4% 时要求停止 Computer Use，并将未完成验收留至额度重置；不要重新执行 phase/edit 迁移脚本，不要覆盖既有未提交工作区。

1. 阅读 AGENTS、验收报告及本次 `results/handoff-source-identity.json`；确认工作区之后是否改变。当前源码没有 BGCS/extern/生成绑定手工修改。
2. 先重新执行 ImGui 最终 35 项；上一份 `imgui.trx` 34 项早于最终 DPI 调整，只作为历史证据。
3. `remaining-gates.py` 根据已有成功记录跳过 Debug/Release Solution；继续 Release 架构验证、Canvas 和 BGFX 原生测试。执行前若源码变动，撤销对应成功缓存并重验，不盲目跳过。
4. `products.py` 使用 owned CLI 副本、同一发行绑定及四条部署路径，已成功条目跳过；Native 输入被冻结后禁止边编译边改 recipe/配置源码。首次冷构建昂贵，待热缓存完成。CLI 副本需要与当前源码一致；若源码变动先重新 build/copy。
5. Rendering2D 使用其正式 Validate-Rendering2D.ps1，Windows Editor Debug/Release 单独产品 build/smoke；实际 UI 必须等用户再次允许 Computer Use。当前不要自动启动 UI。
6. 复核发布 closure、Native exports、光照方向、夜晚星光、真实音频和存储；热构建测量必须在其他构建结束后独立执行。
7. 重新运行 structure/doc links/有效项目图，刷新 file-map 和本验收报告；只清理本任务明确拥有且已不再使用的缓存。

## 命令

工作目录 `C:/Dev/GameEngineDev/InnoEngine`，Web SDK `C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe`，真实 Python `C:/Users/23842/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`。

```powershell
& 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe' test tests/editor/Inno.Editor.ImGui.Tests/Inno.Editor.ImGui.Tests.csproj -m:1 -nodeReuse:false --logger 'trx;LogFileName=imgui-final.trx' --results-directory artifacts/acceptance/backend-platform-integration/results
& 'C:/Users/23842/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' artifacts/acceptance/backend-platform-integration/remaining-gates.py
& 'C:/Users/23842/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' artifacts/acceptance/backend-platform-integration/products.py
```

以上产品脚本的浏览器验收使用隔离 headless Edge，不操作用户桌面；实际桌面 UI 仍需用户许可。未来实际执行过程中保持无关 SDK/用户全局缓存不动。

## 中断与缓存

`results/deferred-processes.json` 与 `results/deferred-imgui-processes.json` 记录本轮明确终止的验证进程；本次中断不算测试失败，也不算通过。Native 的发布候选/中间目录可能保留，重启后重新取得 lease、检查内容完整性，不能手工把它们标为完成。停止后尝试只移除可确认归属、无活跃进程的 Temp/InnoTools junction，自动审批审核以通用 blocked-by-policy 原因拒绝了该删除操作，未执行删除。真实 Native obj、当前产物、SDK/用户缓存与验收日志均保留，磁盘余量见 `results/cache-cleanup.json`。
