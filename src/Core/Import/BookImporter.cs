using Core.Dto;

namespace Core.Import;

public static class BookImporter
{
    public static ImportResult<BookDto> Load(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();

        return extension switch
        {
            ".csv" => BookCsvImporter.Load(path),
            ".json" => BookJsonImporter.Load(path),
            _ => new ImportResult<BookDto>([], [$"Невідомий формат файлу: {extension}"])
        };
    }
}