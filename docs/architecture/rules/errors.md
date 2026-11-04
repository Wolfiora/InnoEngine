## 6. 错误处理
- 参数校验优先：`ArgumentNullException.ThrowIfNull`、清晰的 `ArgumentException`。
- 安全失败与异常失败分离。
- 对关键边界返回明确异常信息。
