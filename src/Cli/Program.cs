using System.Runtime.InteropServices;
using System.Text.Json;

// Створюємо анонімний об'єкт із системними даними та доменом
var info = new
{
    OSDescription = RuntimeInformation.OSDescription,
    OSVersion = Environment.OSVersion.ToString(),
    ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
    FrameworkDescription = RuntimeInformation.FrameworkDescription,
    ClrVersion = Environment.Version.ToString(),
    BaseDirectory = AppContext.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    Domain = "Бібліотека",
    Entities = new[] { "Book", "BookCopy", "Reader", "Loan" }
};

// Перевіряємо наявність прапорця --json в аргументах запуску
if (args.Contains("--json"))
{
    var jsonOptions = new JsonSerializerOptions { WriteIndented = false };
    string jsonString = JsonSerializer.Serialize(info, jsonOptions);
    Console.WriteLine(jsonString);
}
else
{
    Console.WriteLine("========================================");
    Console.WriteLine("          СИСТЕМНА ІНФОРМАЦІЯ           ");
    Console.WriteLine("========================================");
    Console.WriteLine($"OSDescription:        {info.OSDescription}");
    Console.WriteLine($"OSVersion:            {info.OSVersion}");
    Console.WriteLine($"ProcessArchitecture:  {info.ProcessArchitecture}");
    Console.WriteLine($"FrameworkDescription: {info.FrameworkDescription}");
    Console.WriteLine($"CLR Version:          {info.ClrVersion}");
    Console.WriteLine($"BaseDirectory:        {info.BaseDirectory}");
    Console.WriteLine($"CurrentDirectory:     {info.CurrentDirectory}");
    Console.WriteLine("----------------------------------------");
    Console.WriteLine($"Предметна область:    {info.Domain}");
    Console.WriteLine($"Сутності:             {string.Join(", ", info.Entities)}");
    Console.WriteLine("========================================");
}