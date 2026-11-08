using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Reports;

public static class ReportService
{
    // 1) Фільтр + сортування + проєкція (видання з кількістю нижче порогового значення)
    public static IReadOnlyList<StockLine> LowStock<T>(IEnumerable<T> source, Func<T, string> skuSelector, Func<T, string> titleSelector, Func<T, int> quantitySelector, Func<T, string> genreSelector, int threshold) =>
        source.Where(item => quantitySelector(item) < threshold)
              .OrderBy(item => quantitySelector(item))
              .ThenBy(item => skuSelector(item), StringComparer.Ordinal)
              .Select(item => new StockLine(skuSelector(item), titleSelector(item), quantitySelector(item), genreSelector(item)))
              .ToList();

    // 2) Топ-N елементів із детермінованим стабільним сортуванням за другим ключем
    public static IReadOnlyList<StockLine> TopByQuantity<T>(IEnumerable<T> source, Func<T, string> skuSelector, Func<T, string> titleSelector, Func<T, int> quantitySelector, Func<T, string> genreSelector, int n) =>
        source.OrderByDescending(item => quantitySelector(item))
              .ThenBy(item => skuSelector(item), StringComparer.Ordinal)
              .Take(n)
              .Select(item => new StockLine(skuSelector(item), titleSelector(item), quantitySelector(item), genreSelector(item)))
              .ToList();

    // 3) Групування за категорією/жанром з агрегацією (підрахунок кількості та суми)
    public static IReadOnlyList<GroupSummary> ByUnit<T>(IEnumerable<T> source, Func<T, string> genreSelector, Func<T, int> quantitySelector) =>
        source.GroupBy(item => genreSelector(item), StringComparer.OrdinalIgnoreCase)
              .Select(g => new GroupSummary(
                  g.Key,
                  g.Count(),
                  g.Sum(quantitySelector)))
              .OrderByDescending(s => s.TotalQuantity)
              .ThenBy(s => s.Key, StringComparer.OrdinalIgnoreCase)
              .ToList();

    // 4) Агрегатні показники з безпечною обробкою порожньої послідовності (іменований кортеж)
    public static (int Items, int Total, double Average, int Max) Totals<T>(IEnumerable<T> source, Func<T, int> quantitySelector)
    {
        var list = source.ToList();
        return list.Count == 0
            ? (0, 0, 0d, 0)
            : (list.Count,
               list.Sum(quantitySelector),
               list.Average(quantitySelector),
               list.Max(quantitySelector));
    }
}