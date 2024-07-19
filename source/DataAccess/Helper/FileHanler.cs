using System.Text.Json;

namespace DataAccess.Helper;

public class FileHanler
{
    public async Task Add<T>(T item, string fileName)
    {
        var list = await ReadFromFile<T>(fileName);
        if (list == null)
            list = [item];

        else
            list.Add(item);
        await WriteToFile<T>(list, fileName);
    }

    public async Task<bool> WriteToFile<T>(List<T> list, string fileName)
    {
        try
        {
            var writeData = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(fileName, writeData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return false;
        }
    }

    public async Task<List<T>?> ReadFromFile<T>(string filename)
    {

        if (File.Exists(filename) == false)
        {
            // no file
            return default;
        }

        try
        {
            var rawData = await File.ReadAllTextAsync(filename);
            return JsonSerializer.Deserialize<List<T>>(rawData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return null;
        }
    }

    public Task<bool> Clear(string fileName)
    {
        try
        {
            File.Delete(fileName);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            return Task.FromResult(false);
        }
    }
}
