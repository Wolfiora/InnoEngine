# delivery

按完整规则单元加载；路由与来源见 [规则登记](coverage.json)。

<!-- rule-unit: delivery -->
<a id="delivery"></a>
## 31. Issue 与 Pull Request 交付规范

- 创建或更新仓库 issue 或 Pull Request 时，标题必须采用 `type(scope): description` 格式；标题和正文必须全部使用英文。
- Issue 必须区分已复现缺陷与待验证风险，说明职责 owner、复现条件、实际结果、预期结果和可执行验收条件；提交前检索已有 issue，避免重复。
- Pull Request 正文必须根据目标分支的完整差异说明问题、最终行为与实际验证，并明确未执行的检查；创建前检索已有 PR，避免重复，不得将部分验收描述为完整矩阵通过。
- Git 提交的 author 和 committer 均使用 `Aaron Liao <hsuankailiao@gmail.com>`；不得使用 Codex/bot 身份或添加任何 `Co-authored-by` 署名。Pull Request 使用用户授权的 GitHub 账号创建。
- BGCS 缺陷必须通过公开 API 独立复现并明确标记；不得修改 BGCS 源码、包、二进制或手工修改生成物来绕过缺陷。
<!-- /rule-unit: delivery -->

<!-- rule-unit: notification -->
<a id="notification"></a>
## 16. 完成提示音
- 在设计阶段等待用户作出会改变方案边界的决策前、最终设计完成后，以及完成用户要求的代码或文件操作并通过必要验证后，默认播放 `/System/Library/Sounds/Glass.aiff` 作为提示音，无需用户在每次任务中重复要求。
- 如果当前环境无法访问音频设备，应在最终结果中明确说明提示音未能播放；用户明确要求静默时不播放。
<!-- /rule-unit: notification -->
