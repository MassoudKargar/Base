# AGENTS.md

## Scope
- These instructions apply to the entire repository unless a deeper `AGENTS.md` overrides them.

## Repository Layout
- `src/1.Utilities` contains shared utility code.
- `src/2.Core` contains core/domain, contracts, request/response, and application services.
- `src/3.Infra` contains infrastructure and SQL data access projects.
- `src/4.EndPoints` contains the web endpoint application.
- `samples` contains sample applications and reference implementations; do not change sample code unless the task explicitly requires it.

## Working Rules
- Keep changes minimal and scoped to the requested task.
- Preserve the existing layered architecture and project boundaries.
- Prefer fixing issues in the appropriate layer instead of adding cross-layer shortcuts.
- Follow the existing naming and folder conventions for new files and projects.
- Avoid broad refactors unless they are required to complete the task safely.

## Validation
- Prefer validating the smallest affected project first.
- Use solution-level validation only when changes span multiple projects.
- If build or test failures are unrelated to the task, report them and do not expand scope to fix them.

## Notes for Agents
- Check for deeper `AGENTS.md` files before editing files in subdirectories.
- When touching `src`, prefer production code changes there over `samples`.
- Document any new conventions in this file if they apply repository-wide.
