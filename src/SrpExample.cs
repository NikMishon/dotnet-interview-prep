using System;

// Нарушение SRP: Класс ReportGenerator отвечает и за генерацию отчета, и за его сохранение.
public class ReportGenerator
{
    public string GenerateReport(string data)
    {
        // Логика генерации отчета
        string report = $"Report based on: {data}";
        Console.WriteLine("Report generated.");
        return report;
    }

    public void SaveToFile(string report, string filePath)
    {
        // Логика сохранения в файл
        System.IO.File.WriteAllText(filePath, report);
        Console.WriteLine($"Report saved to {filePath}");
    }
}

// Рефакторинг с соблюдением SRP

// Класс, отвечающий только за генерацию отчета
public class ReportGeneratorSrp
{
    public string GenerateReport(string data)
    {
        string report = $"Report based on: {data}";
        Console.WriteLine("SRP Report generated.");
        return report;
    }
}

// Класс, отвечающий только за сохранение отчета
public class ReportSaver
{
    public void SaveToFile(string report, string filePath)
    {
        System.IO.File.WriteAllText(filePath, report);
        Console.WriteLine($"SRP Report saved to {filePath}");
    }
}

public class SrpExample
{
    public static void Run()
    {
        Console.WriteLine("--- Running SRP Example ---");
        
        // Использование классов, разделенных по ответственности
        var generator = new ReportGeneratorSrp();
        var saver = new ReportSaver();

        string report = generator.GenerateReport("Sample Data");
        saver.SaveToFile(report, "report.txt");
        
        Console.WriteLine("--------------------------");
    }
} 