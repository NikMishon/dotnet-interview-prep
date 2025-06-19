using System;
using System.Threading.Tasks;

public class AsyncAwaitExample
{
    public static async Task Run()
    {
        Console.WriteLine("--- Running Async/Await Example ---");
        
        Console.WriteLine("Starting async operation...");
        string result = await LongRunningOperationAsync();
        Console.WriteLine($"Async operation completed with result: {result}");
        
        Console.WriteLine("---------------------------------");
    }

    private static async Task<string> LongRunningOperationAsync()
    {
        Console.WriteLine("LongRunningOperationAsync: Started.");
        await Task.Delay(2000); // Имитация долгой работы (2 секунды)
        Console.WriteLine("LongRunningOperationAsync: Finished.");
        return "Success";
    }
} 