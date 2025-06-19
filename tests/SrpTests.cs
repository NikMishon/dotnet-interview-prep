using Xunit;

// Для запуска этого теста требуется ссылка на сборку с кодом из src
// и установленный пакет xUnit.

public class SrpTests
{
    [Fact]
    public void ReportGeneratorSrp_GenerateReport_ShouldReturnCorrectString()
    {
        // Arrange
        var generator = new ReportGeneratorSrp();
        string testData = "Test Data";
        string expected = "Report based on: Test Data";

        // Act
        string result = generator.GenerateReport(testData);

        // Assert
        Assert.Equal(expected, result);
    }
} 