# Inno.Core.IO

[Core 索引](README.md) · [Settings](Inno.Core.Settings.md) · [Wiki 首页](../README.md)

`Inno.Core.IO` 是不含领域语义的文件系统安全基础层。它不认识 Asset、Plugin、Settings 或 Build 格式，只提供这些领域共同需要、并且必须只有一种正确实现的原子提交与路径边界能力。

## 原子提交

- `AtomicFile.WriteAllBytes` 在目标同目录写完整 staging 文件、强制刷新后再安装，读者不会观察到半个文件。
- `AtomicFile.Install` 要求候选文件与目标同目录，并以一次原子替换提交；失败时原目标保持不变。
- `AtomicDirectory.Install` 要求候选和目标目录树互不包含，也不能为同一路径；通过两次目录移动替换完整树。
  调用方必须协调同一目标的读写，移动之间可能短暂没有目标目录，不能把它描述成文件式的单次原子替换。
  候选安装失败时恢复旧备份；恢复也失败时同时报告两项异常并保留两棵树，不删除其他写入者创建的目标强行回滚。
  候选移动成功即提交，随后清理旧备份。清理失败会抛出明确说明已安装目标和剩余备份位置的 `IOException`，
  保留完整新内容；不得恢复已被部分删除的旧备份。

Settings 文档、Build profile、Plugin package、runtime content pack、Asset Catalog、Artifact manifest 与单个 source metadata 写入都复用这些 primitive。Asset source + `.imeta` 的双文件事务仍由 Asset Pipeline 编排，因为它包含 watcher、source ownership 与 metadata 一致性语义；底层 IO 不伪装成领域事务。

## 路径边界

`PathBoundary.Resolve(root, relativePath)` 和 `RequireContained(root, path)` 在规范化绝对路径后验证目标仍位于 owner root 内，统一处理 `..`、绝对路径和平台大小写规则。根目录可以是卷根目录（例如 `C:\` 或 `/`），其尾部已有分隔符时不再重复追加。Asset source mount、Plugin package extraction 与 FileSystem Storage 使用该 API，因此这些路径不再各自维护一份 mount escape 判断。

## 设计边界

该程序集只接受显式路径，不保存全局 current directory，不提供 service locator，也不吞掉 IO 异常。调用方仍负责：

- 决定哪个 root/文件属于自己；
- 验证领域文档和命名；
- 组织多文件事务及并发策略；
- 处理用户可见 diagnostic。

普通读取、staging 目录内生成文件以及领域格式写入继续直接使用 `System.IO`；它们不需要为了“统一”而绕过一个 service。`Inno.Core.IO` 只收口跨领域且出错代价高的安全 primitive，而不是把各系统耦合到同一个“万能文件管理器”。

[上一页：Identity](Inno.Core.Identity.md) · [下一页：Input](Inno.Core.Input.md)
