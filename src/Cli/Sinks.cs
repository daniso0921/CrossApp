using Core.Notifications;

namespace Cli;

// 1. Реалізація для виведення в консоль
public sealed class ConsoleSink : INotificationSink
{
    public void Notify(string message)
    {
        Console.WriteLine($"[ConsoleSink] {message}");
    }
}

// 2. Реалізація для запису у файловий лог
public sealed class FileSink : INotificationSink
{
    private readonly string _filePath;

    public FileSink(string filePath)
    {
        _filePath = filePath;
        // Автоматично створюємо папку для логів, якщо її немає
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
    }

    public void Notify(string message)
    {
        File.AppendAllText(_filePath, message + Environment.NewLine);
    }
}