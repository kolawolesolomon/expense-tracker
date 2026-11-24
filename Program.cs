using System.Globalization;
using System.Text.Json;

var path = Environment.GetEnvironmentVariable("EXPENSE_FILE") ?? "expenses.json";
try {
    var entries = File.Exists(path) ? JsonSerializer.Deserialize<List<Expense>>(File.ReadAllText(path)) ?? new() : new List<Expense>();
    switch (args.FirstOrDefault()) {
        case "add" when args.Length == 4:
            if (!decimal.TryParse(args[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) || amount <= 0 || string.IsNullOrWhiteSpace(args[2]) || string.IsNullOrWhiteSpace(args[3]))
                throw new ArgumentException("Provide a positive amount, category and description.");
            entries.Add(new Expense(DateOnly.FromDateTime(DateTime.Today), amount, args[2], args[3]));
            var temp = path + ".tmp";
            File.WriteAllText(temp, JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temp, path, true);
            Console.WriteLine("Expense recorded.");
            break;
        case "list":
            foreach (var e in entries) Console.WriteLine($"{e.Date:yyyy-MM-dd} | {e.Category} | {e.Amount.ToString("F2", CultureInfo.InvariantCulture)} | {e.Description}");
            break;
        case "summary":
            foreach (var group in entries.GroupBy(e => e.Category, StringComparer.OrdinalIgnoreCase))
                Console.WriteLine($"{group.Key}: {group.Sum(e => e.Amount).ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Total: {entries.Sum(e => e.Amount).ToString("F2", CultureInfo.InvariantCulture)}");
            break;
        default:
            Console.WriteLine("Usage: add <amount> <category> <description> | list | summary");
            return 1;
    }
    return 0;
} catch (Exception e) when (e is IOException or UnauthorizedAccessException or JsonException or ArgumentException or OverflowException) {
    Console.Error.WriteLine(e.Message);
    return 1;
}
record Expense(DateOnly Date, decimal Amount, string Category, string Description);
