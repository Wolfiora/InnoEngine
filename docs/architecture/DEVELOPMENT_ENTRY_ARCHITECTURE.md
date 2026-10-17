# 开发入口与普通工程构建

[架构索引](README.md) · [Wiki 首页](../README.md) · [入口项目](../entry/README.md)

## 日常流程

安装必要 SDK 与当前平台工具链，递归 clone，打开 InnoEngine.sln，选择 InnoEngine.Editor 运行。IDE 使用正常项目图构建，随后运行 Program；无游戏目录参数时打开项目选择窗口。

```text
InnoEngine.Editor (Exe)
└─ 当前 SDK 宿主的 Editor 产品 (Library)
   ├─ 共同 Editor、Runtime、Content 与 Services
   ├─ 所选 backend 与运行 integration
   └─ 统一 Native 支持准备与显式部署

InnoEngine.Build (Exe)
└─ Inno.Build.Cli (Library)
   └─ 标准发行组合与中立构建机制
```

## 宿主选择与项目图

entry 的 MSBuild 属性读取 SDK 的实际宿主 RID，明确映射 win-x64 到 windows-x64、osx-arm64 到 macos-arm64。条件 ProjectReference 与编译常量选择相同产品；其他宿主明确拒绝。不通过目录名拼接 RID，不向所有共同项目注入宿主 RID。

当前宿主 Editor 通过 entry 依赖构建，平台产品不重复作为 Solution 顶级构建入口。entry 只对自己的依赖解除 Solution 未勾选项目的引用屏蔽；其他平台产品不会因此被隐式选中。

默认 Debug/Release 正常选择共同源码、工具和适用测试项目。SourceDebug/SourceRelease 保留源码批量验证用途，不构建 Editor entry。项目未改变时由 SDK 增量跳过；没有选入配置与已经最新是不同状态。

标准构建发行组合保留三个发布目标的可移植规则。编译 macOS 打包规则只产生 C# 类库，不解析 macOS Native SDK；真正的 SDK 与宿主能力在请求该目标构建时验证。

## Native 与运行输出

平台产品是支持准备的唯一 owner。普通 Build 复用统一 Native plan、绑定选择、完整性检查和原子发布；Design-time build 不执行 Native。产品将完整 native 目录作为传递内容输出，SDK 部署至 entry，加载器继续消费显式部署索引。

Program 直接调用平台 Run，不执行源码扫描、子 MSBuild、工具 worker 或预构建产品发现。Editor 主线程、取消和产品读取 owner 保持明确；SDK 与工具权限问题在构建或显式命令中报告。

## Build 命令与清理

默认 Build 准备宿主 Editor 支持。support 显式选择产品与目标；editor 执行完整 entry 工程构建。产品 clean 只清理选定 entry 与平台产品的输出、中间目录，先取得对应产品独占 lease。缓存退休继续由现有 owner、依赖与读取保护机制负责。

## 扩展与验证

新增 Editor 宿主时先实现真实平台产品，再增加 entry 的项目引用与编译选择；共同领域、backend 和共享 Hosting 不增加宿主判断。新增发布目标仍通过标准发行组合注册。

本轮只检查项目图、入口编译、部署与帮助命令，不执行 GUI、完整产品矩阵或第三方绑定专项验收。独立引擎 Publish 与安装布局另行处理。
