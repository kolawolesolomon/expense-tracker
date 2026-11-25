# Expense Tracker

[![Tests](https://github.com/kolawolesolomon/expense-tracker/actions/workflows/tests.yml/badge.svg)](https://github.com/kolawolesolomon/expense-tracker/actions/workflows/tests.yml)

A C# and .NET 8 command-line journal with local JSON persistence. Record expenses, list transactions and summarize spending by category.

## Setup and use

Install the .NET 8 SDK and run these commands from the repository root.

```sh
dotnet run -- add 2500 Transport "Bus fare"
dotnet run -- list
dotnet run -- summary
```

Amounts are positive decimal numbers in a single user-selected currency. Category totals group names case-insensitively. The program writes to `expenses.json` by default. Set `EXPENSE_FILE` to use another path.

## Build and test

```sh
dotnet build
bash smoke-test.sh
```

The smoke test runs separate CLI processes against synthetic expenses in a temporary directory. It verifies persistence, listing, totals, invalid amounts and the Git ignore rules for the default data and temporary files.

## Limitations

Use one process at a time. The JSON is stored locally in plain text. If you override `EXPENSE_FILE`, use a path outside the repository or add an ignore rule for that path.
