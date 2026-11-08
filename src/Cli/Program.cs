using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Reports;

namespace Cli;

class Program
{
    static void Main(string[] args)
    {
        var sampleBooks = SampleData.GetBooks();
        var command = args.FirstOrDefault(a => !a.StartsWith("--")) ?? "report-top";

        switch (command)
        {
            case "report-top":
                Console.WriteLine("ТОП-5 ВИДАНЬ ЗА КІЛЬКІСТЮ:");
                PrintLines(ReportService.TopByQuantity(sampleBooks, b => b.Isbn, b => b.Title, b => b.Quantity, b => b.Genre, 5));
                break;

            case "report-low":
                Console.WriteLine("ДЕФІЦИТНІ ВИДАННЯ (< 10):");
                PrintLines(ReportService.LowStock(sampleBooks, b => b.Isbn, b => b.Title, b => b.Quantity, b => b.Genre, 10));
                break;

            case "report-groups":
                Console.WriteLine("РОЗПОДІЛ ЗА ЖАНРАМИ:");
                foreach (var g in ReportService.ByUnit(sampleBooks, b => b.Genre, b => b.Quantity))
                {
                    Console.WriteLine($"  {g.Key,-16} позицій: {g.Count,3}   сумарно: {g.TotalQuantity,6}");
                }
                break;

            case "report-sum":
                Console.WriteLine("ПІДСУМКОВІ ПОКАЗНИКИ:");
                var t = ReportService.Totals(sampleBooks, b => b.Quantity);
                Console.WriteLine($"  позицій {t.Items}, всього {t.Total}, середнє {t.Average:F2}, макс {t.Max}");
                break;

            case "report-demo":
                RunDeferredExecutionDemo(sampleBooks);
                break;

            default:
                Console.WriteLine("Доступні команди: report-top | report-low | report-groups | report-sum | report-demo");
                break;
        }
    }

    private static void PrintLines(IReadOnlyList<StockLine> lines)
    {
        foreach (var l in lines)
        {
            Console.WriteLine($"  {l.Sku,-10} {l.Title,-28} {l.Quantity,6} {l.Genre}");
        }
    }

    private static void RunDeferredExecutionDemo(IEnumerable<Book> initialBooks)
    {
        Console.WriteLine("ДЕМО ВІДКЛАДЕНОГО ВИКОНАННЯ LINQ:");
        var source = initialBooks.ToList();
        var query = source.Where(b => b.Quantity < 10);

        Console.WriteLine($"Дефіцитних до змін:    {query.Count()}");

        source.Add(Book.Create("ISBN-999", "Тестова книга", "Програмування", 3));
        
        Console.WriteLine($"Дефіцитних після змін: {query.Count()}");

        var snapshot = query.ToList();
        source.Clear();
        
        Console.WriteLine($"У знімку лишилось:     {snapshot.Count}");
    }
}