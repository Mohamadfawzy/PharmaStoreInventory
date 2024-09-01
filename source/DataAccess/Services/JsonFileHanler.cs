using System.Text.Json;

namespace DataAccess.Services;

public class JsonFileHanler(string fileName)
{
    public string FullPath { get; set; } = fileName;

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
            await File.WriteAllTextAsync(FullPath, writeData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> WriteToFile<T>(T item)
    {
        try
        {
            var writeData = JsonSerializer.Serialize(item);
            await File.WriteAllTextAsync(FullPath, writeData);
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

        if (File.Exists(FullPath) == false)
        {
            // no file
            return default;
        }

        try
        {
            var rawData = await File.ReadAllTextAsync(FullPath);
            return JsonSerializer.Deserialize<List<T>>(rawData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return null;
        }
    }
    
    public async Task<T?> SingleObject<T>()
    {

        if (File.Exists(FullPath) == false)
        {
            // no file
            return default;
        }

        try
        {
            var rawData = await File.ReadAllTextAsync(FullPath);
            return JsonSerializer.Deserialize<T>(rawData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return default;
        }
    }

    public Task<bool> Clear()
    {
        try
        {
            File.Delete(FullPath);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            return Task.FromResult(false);
        }
    }
}
