# InnoEngine agent instructions

架构、职责归属、依赖方向、低耦合和资源所有权优先于功能便利与速度。复用 Core IO、Events、Identity、Serialization；禁止平行管理器、测试后门、旧版兼容层和隐式 fallback。源码注释与公开 XML 使用英文，Wiki 正文使用中文。

## 开始任务

1. 确定当前分支、工作区差异、任务 owner、行为变化与验收边界。保护其他会话的改动；相邻仓库只读，除非用户明确授权。
2. 从根目录启动也必须读取下表相关规则；嵌套 AGENTS 的自动发现不能代替此步骤。用 `tests/ci/Test-AgentRules.ps1 -Paths <changed paths> -ShowInstructions` 获取候选规则、字节数与完整正文；按实际行为补选 `-Rules`。超过 16 KiB 时按领域分阶段阅读并报告，禁止截断。
3. 规则唯一权威位置见 [规则索引](docs/architecture/rules/README.md)；[覆盖映射](docs/architecture/rules/coverage.json) 保留原规则来源、自动检查与人工责任。

| 涉及范围 | 必读规则（docs/architecture/rules/ 下） |
|---|---|
| 手写 C# | naming、members、xml、errors、formatting；保持周边 API 风格 |
| 新系统、边界或 API | architecture、directories、encapsulation、completion、core-reuse、no-legacy |
| 测试、验收 | tests、compilation、review；依据实际公开契约与传递依赖 |
| Wiki/XML/示例 | wiki、xml |
| 脚本导出/编译 | scripting、identity-reload |
| Identity、Missing、Asset、Scene、插件、热重载 | identity-reload，以及 docs/architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md |
| Plugin、结构化内容 | plugins、no-legacy |
| Editor | editor-projects；涉及状态读 editor-state，呈现读 editor-layout、editor-acceptance |
| Rendering/内容/帧 | rendering、content-frame |
| Build/Native/平台/CI | build-host、platform-products、backend-build；只读取本次职责相关部分，不能漏掉适用约束 |
| 项目/解决方案 | solution、directories |
| 交付 | delivery、review、notification |

## 有界实施与验收

- “彻底完成”指本任务全部适用行为、调用方、项目引用、公开 XML、Wiki、回归及验证均完成。无关改进另记 issue，不无限扩大任务。
- 允许修改任务必需的回归、契约与验证工具测试；禁止无关测试重写、弱化断言、扩大生产可见性或反射穿透。
- 默认一个 owner 一个实现者。并行须有独立文件责任、工作区和输出 owner，禁止重复全仓扫描、构建、验收。
- 使用 `.agents/skills/` 中的 inno-change、inno-verify、inno-api-docs、inno-delivery；技能不替代领域规则。
- 接续只记录范围、决策、当前改动身份、有效检查、阻塞和下一步到已有 issue/records。验证证据必须匹配当前输入；缺失、跳过、不可用环境均不等于通过。
- 构建输出按工作区/配置/目标稳定复用，日志单独按运行归档。本机使用 C 盘明确 owner；清理必须经现有 lease/登记协议，不猜测目录归属。
- 最终核对实际获批计划，报告执行证据、未达目标与外部阻塞；可继续的工作应自动继续。提交和远程交付遵循 delivery 规则。
