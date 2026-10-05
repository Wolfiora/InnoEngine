# Inno.Core.IO

[Core 索引](README.md) · [Settings](Inno.Core.Settings.md) · [Wiki 首页](../README.md)

`Inno.Core.IO` 是不含领域语义的文件系统安全基础层。它不认识 Asset、Plugin、Settings 或 Build 格式，只提供这些领域共同需要、并且必须只有一种正确实现的原子提交与路径边界能力。

## 原子提交

- `AtomicFile.WriteAllBytes` 在目标同目录写完整 staging 文件、强制刷新后再安装，读者不会观察到半个文件。
- `AtomicFile.Install` 要求候选文件与目标同目录，并以一次原子替换提交；失败时原目标保持不变。
- `AtomicDirectory.Publish` 将完整候选树移动到尚未占用的路径，不覆盖已存在的目录。
  它用于内容寻址 Artifact 和 Support Pack 的首次发布，支持在提交前取消。
  Windows 的临时访问/共享冲突最多等待两秒，成功后不再检查取消；永久失败保留候选及原目标。
- 原子文件和目录的 rename 共用同一个内部 IO 策略。Windows 临时未授予 delete sharing 的读者
  可能使目录移动报 `ERROR_ACCESS_DENIED`；只重试 rename，保留原异常，不吞掉权限或磁盘错误。
- `AtomicDirectory.Install` 要求候选和目标目录树互不包含，也不能为同一路径；通过两次目录移动替换完整树。
  调用方必须协调同一目标的读写，移动之间可能短暂没有目标目录，不能把它描述成文件式的单次原子替换。
  候选安装失败时恢复旧备份；恢复也失败时同时报告两项异常并保留两棵树，不删除其他写入者创建的目标强行回滚。
  候选移动成功即提交，随后清理旧备份。清理失败会抛出明确说明已安装目标和剩余备份位置的 `IOException`，
  保留完整新内容；不得恢复已被部分删除的旧备份。
  两次移动同样采用上述有界等待，备份删除失败仍按已提交失败语义报告。

Settings 文档、Build profile、Plugin package、runtime content pack、Asset Catalog、Artifact manifest 与单个 source metadata 写入都复用这些 primitive。Asset source + `.imeta` 的双文件事务仍由 Asset Pipeline 编排，因为它包含 watcher、source ownership 与 metadata 一致性语义；底层 IO 不伪装成领域事务。

## 路径边界

`PathBoundary.Resolve(root, relativePath)` 和 `RequireContained(root, path)` 在规范化绝对路径后验证目标仍位于 owner root 内，统一处理 `..`、绝对路径和平台大小写规则。根目录可以是卷根目录（例如 `C:\` 或 `/`），其尾部已有分隔符时不再重复追加。Asset source mount、Plugin package extraction 与 FileSystem Storage 使用该 API，因此这些路径不再各自维护一份 mount escape 判断。

`PathBoundary.EnumerateFiles(root)` 枚举 owner 的普通文件树，并在遇到文件符号链接、目录 junction
或其他 reparse point 时明确失败，避免产物哈希和打包跨出所有权边界。返回绝对路径，不保留文件句柄；
调用方仍需持有写入所有权，并决定排序和内容哈希。此入口用于已生成的产物，SDK 源树的链接策略由工具链另行管理。

## 进程间所有权

`FileLease.AcquireAsync(path, timeout, cancellationToken)` 接受显式绝对路径，以异步等待取得进程间独占文件所有权。
`timeout` 可以为零或 `Timeout.InfiniteTimeSpan`；超时抛出 `TimeoutException`，取消抛出 `OperationCanceledException`。
返回的 sealed `FileLease` 实现 `IDisposable`，重复释放安全。取消等待不会释放其他调用方的 lease。
lease 文件在释放后保留，避免 Unix 上删除文件后出现同一路径对应两个被分别锁定的 inode。
此契约只提供协作进程间的所有权；调用方决定受保护的产物、持有范围、校验与提交顺序。

## 设计边界

该程序集只接受显式路径，不保存全局 current directory，不提供 service locator，也不吞掉 IO 异常。调用方仍负责：

- 决定哪个 root/文件属于自己；
- 验证领域文档和命名；
- 组织多文件事务及并发策略；
- 处理用户可见 diagnostic。

普通读取、staging 目录内生成文件以及领域格式写入继续直接使用 `System.IO`；它们不需要为了“统一”而绕过一个 service。`Inno.Core.IO` 只收口跨领域且出错代价高的安全 primitive，而不是把各系统耦合到同一个“万能文件管理器”。

[上一页：Identity](Inno.Core.Identity.md) · [下一页：Input](Inno.Core.Input.md)
