using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class BookJsonImporter
{
    public static ImportResult<BookDto> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new ImportResult<BookDto>([], [$"Файл не знайдено: {path}"]);
        }

        try
        {
            string json = File.ReadAllText(path, System.Text.Encoding.UTF8);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            List<BookDto> items = JsonSerializer.Deserialize<List<BookDto>>(json, options) ?? [];
            return new ImportResult<BookDto>(items, []);
        }
        catch (Exception ex)
        {
            return new ImportResult<BookDto>([], [$"Помилка читання JSON: {ex.Message}"]);
        }
    }
}