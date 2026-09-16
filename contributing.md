## Contributing

All development should be completed on a separate branch and merged into `main` using a Pull Request (PR).

**Do not commit directly to `main`.**

### 1. Clone the Repository

You only need to do this the first time you work on the project.

```bash
git clone https://github.com/xdaybreakerx/ifq584-assignment-3.git
cd ifq584-assignment-3
```

If Git asks you to configure your identity, use the name and email associated with your own GitHub account:

```bash
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"
```

This ensures commits are correctly attributed to the person who completed the work.

### 2. Start From the Latest `main`

Before beginning a new piece of work:

```bash
git checkout main
git pull
```

Create a new branch for the task:

```bash
git checkout -b feature/short-description
```

Examples:

```text
feature/numerical-rules
feature/notakto-board
feature/gomoku-rules
feature/save-load
fix/undo-turn-order
docs/update-readme
```

Use a separate branch for each reasonably self-contained task.

### 3. Make and Commit Changes

While working, you can see changed files with:

```bash
git status
```

Stage the files you want to commit:

```bash
git add .
```

Commit the changes with a short description of what was implemented - using [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/):

```bash
git commit -m "Implement Gomoku win detection"
```

Prefer several meaningful commits over one very large commit at the end of the assignment.

For example:

```text
Feat: Add Gomoku board configuration
Feat: Implement horizontal and vertical win detection
Fix: Resolved logic issue for diagonal Gomoku win detection
```

### 4. Push Your Branch to GitHub

The first time you push a new branch:

```bash
git push -u origin feature/short-description
```

After that, additional commits can be pushed with:

```bash
git push
```

### 5. Open a Pull Request

On GitHub:

1. Open the repository.
2. Select **Pull requests**.
3. Select **New pull request**.
4. Set:
    - Base branch: `main`
    - Compare branch: your branch

5. Briefly describe:
    - what you changed;
    - why you changed it;
    - how you tested it.

6. Request a review from another team member.
7. Address any review comments before merging.

Once the PR has been approved and all automated checks pass, merge it into `main`.

### 6. Continue After a Merge

After your branch has been merged:

```bash
git checkout main
git pull
```

You can then delete the old local branch:

```bash
git branch -d feature/short-description
```

Create a new branch from the updated `main` before starting another task.

### Keeping a Branch Up to Date

If another team member has changed `main` while you are working:

```bash
git checkout main
git pull
git checkout feature/short-description
git merge main
```

If Git reports a merge conflict, resolve the conflicting files before committing the merge.

If you are unsure how to resolve a conflict, ask another team member before deleting either version of the code.

### Contribution Guidelines

- Do not push directly to `main`.
- Do not force-push shared branches.
- Pull the latest `main` before starting new work.
- Keep branches focused on one task or feature.
- Write meaningful commit messages.
- Commit regularly rather than submitting one large final commit.
- Run `dotnet build` before opening a Pull Request.
- Use your own GitHub account when committing so contribution history remains attributable.
- Review another team member's Pull Request rather than approving your own work.

### Additional

- GitHub should be used for source code, code review, and implementation history. Team agreements, meeting records, and other required assignment coordination should also be recorded in the team's QUT/Ed discussion space where required by the assignment.
- Gen AI tooling is not allowed per the assignment requirements.
