var yearToMake = 2018;

GenerateNewYearFiles(yearToMake);

static void GenerateNewYearFiles(int year)
{
    var basePath = "C:\\Users\\markr\\home\\code\\csharp\\AdventOfCode\\AdventOfCode";
    var codeFolderPath = Path.Combine(basePath, "DaySolvers", $"Year{year}");
    var inputFolderPath = Path.Combine(basePath, "Inputs", $"{year}");
    Directory.CreateDirectory(codeFolderPath);
    Directory.CreateDirectory(inputFolderPath);
    var templateContent = File.ReadAllText("DayTemplate.cs.template");

    for (var i = 1; i <= 25; i++)
    {
        var dayString = $"{i:D2}";
        var newCodeFilePath = Path.Combine(codeFolderPath, $"Day{dayString}.cs");
        var newInputFilePath = Path.Combine(inputFolderPath, $"Day{dayString}.txt");
        var codeContent = templateContent.Replace("{Year}", year.ToString()).Replace("{Day}", dayString);
        File.WriteAllText(newCodeFilePath, codeContent);
        File.Create(newInputFilePath);
    }
}
