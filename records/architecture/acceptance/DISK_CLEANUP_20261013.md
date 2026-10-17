# 2026-10-13 构建中间产物空间检查与清理

## 本次结果

本次暂停新增构建，只检查并清理执行过程中产生的中间产物。C 盘容量约 463.01 GiB；操作前可用 **34.47 GiB**，操作后可用 **46.01 GiB**。

| 项目 | 结果 |
|---|---|
| 正式 owner 缓存清理 | 成功，退出码 0 |
| 退休产物逻辑大小 | 14,378,546,870 字节，约 13.39 GiB |
| 实际卷可用空间增加 | 12,396,142,592 字节，约 11.54 GiB |
| 活动构建 | 0；未停止无关应用或编译服务 |
| 验收网页服务 | 核对进程命令后停止本轮端口 8765、8766 的服务 |
| 当前 Editor | Debug/Release readiness 文件均保留 |
| 额外历史中间目录 | 37 个，逻辑大小约 51.32 GiB；删除被自动审批拒绝，未执行 |

逻辑大小不能直接作为可回收卷空间：压缩文件、分配单元以及扫描期间的缓存退休都会造成差异。本报告以清理前后的卷可用字节报告实际回收。

## 占用原因

多轮冷构建、绑定生成、NuGet 迁移及验收产生了不同身份的冻结源码副本、CLI 编译输出和 Native 编译工作目录。部分历史目录早于当前 publisher 登记机制；正常缓存清理只处理具有明确登记和保护关系的目录，因此没有覆盖这些历史副本。这些中间产物是本轮及此前执行工作造成的空间负担。

剩余 37 个候选包括：3 个冻结 managed 输入工作目录、23 个 Native 冻结输入及编译工作目录、11 个 CLI bootstrap 输出。目录检查排除了当前登记、当前根、登记依赖以及 Git 跟踪文件；本次没有把仅凭目录年龄或文件扩展名的发现作为删除依据。

额外批量删除操作被自动审批以 `blocked by policy` 拒绝。工具未提供更详细原因。没有重试同一删除、改换删除工具、补写登记或绕过拒绝。因此这 51.32 GiB **仍然存在**，不计入本次回收。

## 已执行命令

```powershell
dotnet entry/InnoEngine.Build/bin/Debug/net9.0/InnoEngine.Build.dll clean --scope cache --dry-run --cache-time-seconds 600
dotnet entry/InnoEngine.Build/bin/Debug/net9.0/InnoEngine.Build.dll clean --scope cache --cache-time-seconds 600
```

预览和执行使用相同 owner 清理机制。清理保护当前产品、缓存依赖及活动读取，没有删除锁文件来解锁。

## 保护与证据

源码、当前有效产品、正式缓存保护根、验收报告、日志、截图均保留。NuGet、SDK、用户数据和浏览器全局缓存没有清理。用户 Temp 下 `InnoTools` 的 junction 是短路径别名，不能重复计为对应 Native 工作目录的另一份数据。

原始证据位于 `artifacts/acceptance/final-platform-reload-nuget/20261013-final-closure/`：

- `logs/stage18-cache-clean-preview.log`：正式清理预览。
- `logs/stage18-cache-clean.log`：正式清理执行结果。
- `results/stage18-cache-clean.json`：实际卷空间变化。
- `results/stage18-disk-inventory.json`：仓库物理文件扫描，跳过 reparse points；扫描期间正式清理仍在执行，不作为严格同一时刻快照。
- `results/stage18-workspace-inventory.json`：细分工作目录占用。
- `results/stage18-unowned-workspace-clean-plan.json`：未执行的 37 个历史目录清单、具体绝对路径及依据。

本次没有继续冷构建或恢复被取消的并发性能队列，避免在清理期间继续增长。既有性能日志保留；没有将被取消的并发验收写成通过。

当前环境不存在 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。

## 用户要求恢复约 200 GB 后的全盘复核

后续已扫描整个 C 盘可访问目录：1,846,640 个文件，跳过 670 个 reparse point；376 个目录/文件因访问限制未计入。统计是逻辑文件字节，不是严格的 NTFS 分配空间，不会因此误删系统文件。

| 位置 | 逻辑占用 | 归属和处理 |
| --- | --- | --- |
| `C:\Dev\GameEngineDev\InnoEngine` | 137.65 GiB | 最大相关占用；冻结输入、编译工作目录、工具副本和验收构建输出是主要负担。 |
| `C:\Dev\GameEngineDev\BindGen-CS` | 20.67 GiB | 独立仓库；约 3.79 GiB 在早期 refactor 验收目录，其他大项为工具与测试 bin。未删除独立源码、既有输出或历史证据。 |
| `C:\Users\23842\AppData\Local\NVIDIA` | 25.87 GiB | 驱动缓存；未将全部占用认定为本轮引擎生成，也未清理。 |
| `C:\pagefile.sys` | 23.73 GiB | 系统分页文件，未修改。 |
| `C:\Program Files\Tuanjie\Hub` | 19.51 GiB | 已安装工具，未修改。 |
| `C:\Users\23842\Downloads` | 16.28 GiB | 用户下载，未修改。 |
| `C:\Users\23842\AppData\Local\InnoWebDotnet` | 2.90 GiB | 已有 Wasm SDK，未清理 SDK。 |

仓库重点占用在首次细分扫描时包括：BGFX 组件目录约 31.68 GiB、CLI 工具目录约 21.66 GiB、managed workspaces 约 20.68 GiB、Task 工具目录约 9.89 GiB。它们均包含实际构建工作副本，不能把这些组件的手写源码误认成几十 GiB。

已有记录显示：10 月 10 日曾有约 150.32 GiB 可用；本轮开始约 100.12 GiB；第一次空间投诉检查为 34.47 GiB。没有操作前约 200 GB 的完整文件快照，无法精确反推出每个文件的新增时间和净增长，但现存多份构建副本已查明，确实是执行工作造成的主要额外占用。

用户再次要求清理后，重新授权范围内的一个具体历史 managed workspace 删除仍被自动审批以 `blocked by policy` 拒绝，未执行。该拒绝没有提供更具体原因。没有通过其他删除工具、补写 owner 登记或运行脚本绕过。

完成并发正确性检查后，再次运行正式 owner 清理：逻辑退休 **5,059,592,545 字节**，实际卷增加 **3,094,953,984 字节**，退出码 0。这次检查也产生并随后回收了可退休工作副本，所以两轮回收量不能直接累加为最终净空闲增长。

最终可用约 **45.99 GiB**；**恢复约 200 GB 的要求没有完成**。剩余 37 个历史目录具体路径见 `stage18-unowned-workspace-clean-plan.json`。最新全盘清单、历史空间记录和清理结果分别为 `stage19-volume-inventory.json`、`stage19-disk-history.json`、`stage19-cache-clean.json`。当前有效产物及验收原始证据继续保留。
