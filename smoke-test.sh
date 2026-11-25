#!/usr/bin/env bash
set -euo pipefail

test_dir=$(mktemp -d)
trap 'rm -rf "$test_dir"' EXIT
export EXPENSE_FILE="$test_dir/expenses.json"

dotnet run --no-build -- add 12.50 Food Lunch
dotnet run --no-build -- add 7.50 food Snack
dotnet run --no-build -- add 5 Transport Bus
test -s "$EXPENSE_FILE"
list=$(dotnet run --no-build -- list)
grep -F 'Food | 12.50 | Lunch' <<< "$list"
grep -F 'Transport | 5.00 | Bus' <<< "$list"
summary=$(dotnet run --no-build -- summary)
grep -Fx 'Food: 20.00' <<< "$summary"
grep -Fx 'Transport: 5.00' <<< "$summary"
grep -Fx 'Total: 25.00' <<< "$summary"

cp "$EXPENSE_FILE" "$test_dir/before.json"
for amount in -1 0 invalid; do
  if dotnet run --no-build -- add "$amount" Food Invalid; then
    echo "Unexpectedly accepted amount $amount" >&2
    exit 1
  fi
done
cmp "$EXPENSE_FILE" "$test_dir/before.json"
git check-ignore --quiet expenses.json
git check-ignore --quiet expenses.json.tmp
