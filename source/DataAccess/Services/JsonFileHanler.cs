using System.Text.Json;

namespace DataAccess.Services;

public class JsonFileHanler(string fileName)
{
    public string FileName { get; set; } = fileName;

    public async Task Add<T>(T item)
    {
        var list = await ReadFromFile<T>();
        if (list == null)
            list = [item];

        else
            list.Add(item);
        await WriteToFile(list);
    }

    public async Task<bool> WriteToFile<T>(List<T> list)
    {
        try
        {
            var writeData = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(FileName, writeData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return false;
        }
    }

    public async Task<List<T>?> ReadFromFile<T>()
    {

        if (File.Exists(FileName) == false)
        {
            // no file
            return default;
        }

        try
        {
            var rawData = await File.ReadAllTextAsync(FileName);
            return JsonSerializer.Deserialize<List<T>>(rawData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return null;
        }
    }

    public Task<bool> Clear()
    {
        try
        {
            File.Delete(FileName);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            return Task.FromResult(false);
        }
    }
}
