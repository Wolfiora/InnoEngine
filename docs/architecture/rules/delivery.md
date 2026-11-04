## 31. Issue 与 Pull Request 交付规范

- 创建或更新仓库 issue 或 Pull Request 时，标题必须采用 `type(scope): description` 格式；标题和正文必须全部使用英文。
- Issue 必须区分已复现缺陷与待验证风险，说明职责 owner、复现条件、实际结果、预期结果和可执行验收条件；提交前检索已有 issue，避免重复。
- Pull Request 正文必须根据目标分支的完整差异说明问题、最终行为与实际验证，并明确未执行的检查；创建前检索已有 PR，避免重复，不得将部分验收描述为完整矩阵通过。
- Git 提交的 author 和 committer 均使用 `Aaron Liao <hsuankailiao@gmail.com>`；不得使用 Codex/bot 身份或添加任何 `Co-authored-by` 署名。Pull Request 使用用户授权的 GitHub 账号创建。
- BGCS 缺陷必须通过公开 API 独立复现并明确标记；不得修改 BGCS 源码、包、二进制或手工修改生成物来绕过缺陷。
