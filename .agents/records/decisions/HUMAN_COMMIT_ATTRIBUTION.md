# Human commit attribution

[Decisions](README.md) · [Rule authority](../../rulebook/delivery.md)

On 2026-11-21 the user replaced the delivery clause that named one person as the required Git author and committer. The requirement is a principle, not an identity: a human vouches for every commit.

The active clause requires the Git author and committer to be the human contributor responsible for the commit, and forbids any agent, bot or application identity as author, committer, `Co-authored-by` or any other attribution. It applies to every contributor and every tool, so the rule no longer needs to change when contributors do.

This is a substantive approved amendment, not an English translation. The original source hash and translation mapping remain unchanged in the rule registry; one amendment hash binds the active wording to this decision. The requirement to create pull requests with the GitHub account authorized by the user, and all other delivery obligations, remain in force.

An agent whose environment adds its own attribution does not commit; it hands the change to an environment that commits solely in the responsible human's name.
