## 26. Solution 与项目文件整理

- Solution Folder 与实际架构职责对应，删除没有有效项目或 Solution Items 的空分组；小型项目集合避免无必要的单项目包装层。
- `entry`、`src`、`backends`、`platforms`、`build`、`tools` 与 `tests` 中的创作源码项目必须纳入 Solution，包括绑定生成扩展和测试 fixture；`obj`、`artifacts` 及 extern 的独立第三方源码项目不纳入。没有入站 ProjectReference 不代表项目无用，清理前必须核对宿主入口、MSBuild 工具调用与生成配置。
- `.csproj`、`.props`、`.targets` 使用两空格缩进，顶层职责块以空行分隔，长属性列表逐属性换行，ProjectReference 相对路径使用 `/`。
- 只合并条件和引用可见性一致的相邻引用分组。不得重排 Import、Property、Target 或改变条件、metadata 和求值顺序；Editor 公开与实现引用边界必须保留。
- 详细规则见 `docs/architecture/CSHARP_DEVELOPMENT_STANDARD.md`；整理后核对有效 XML、Solution 项目身份与配置，并运行受影响构建及架构验证。
- 长时间构建与验收应在阶段结束后清理本次操作所属的旧工具快照、私有加载目录和测试浏览器 profile，并检查可用磁盘空间。清理前核对明确 owner、解析后的绝对路径、符号链接和活跃进程；保留当前产物及验收日志，不清理用户或 SDK 的全局缓存。含 hardlink/压缩文件的逻辑大小不能当作实际释放空间。
