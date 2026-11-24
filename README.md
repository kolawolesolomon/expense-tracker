# Expense Tracker

A lightweight C#/.NET 8 command-line expense journal with local JSON persistence and category totals.

```sh
dotnet run -- add 2500 Transport "Bus fare"
dotnet run -- list
dotnet run -- summary
```

Amounts use a single user-selected currency (no conversion). Data is stored locally in `expenses.json`; set `EXPENSE_FILE` to choose another path. Intended for single-process use. Personal expense data is excluded from Git.

Build with `dotnet build`. Initial implementation; execution verification pending.
