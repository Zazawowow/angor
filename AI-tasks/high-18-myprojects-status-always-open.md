# HIGH-18: MyProjectsViewModel — Project status always hardcoded "Open"

## Status: [x] COMPLETED — Derives status from FundingStartDate/FundingEndDate (Upcoming/Open/Closed)

## Section: MyProjects

## Problem
`MyProjectsViewModel.cs` line 131: When mapping SDK `ProjectDto` to `MyProjectItemViewModel`, the `Status` is hardcoded to `"Open"` regardless of the project's actual state.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/MyProjectsViewModel.cs` (line 131)

## What needs to happen
1. Check if `ProjectDto` or the SDK provides a status field.
2. Map real project status (Open, Funded, Closed, etc.) from SDK data.

## SDK availability
- `IProjectAppService.GetFounderProjects()` returns `ProjectDto` — check its properties for status information.

## Acceptance criteria
- Project cards show real status from SDK.
