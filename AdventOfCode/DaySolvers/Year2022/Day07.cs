namespace AdventOfCode.Year2022
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (rootDirectory, allDirectories) = ParseLines(lines);
            return allDirectories.Select(d => d.GetDirectorySize()).Where(d => d <= 100000).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var (rootDirectory, allDirectories) = ParseLines(lines);
            var totalSpace = 70000000;
            var usedSpace = rootDirectory.GetDirectorySize();
            var requiredSpace = 30000000;
            var spaceToFreeUp = requiredSpace - (totalSpace - usedSpace);
            return allDirectories.Select(d => d.GetDirectorySize()).Where(d => d >= spaceToFreeUp).OrderBy(d => d).First();
        }

        private static (ProblemDirectory, List<ProblemDirectory>) ParseLines(IList<string> lines)
        {
            var rootDirectory = new ProblemDirectory
            {
                ParentDirectory = null,
                DirectoryName = "/",
                SubDirectories = new Dictionary<string, ProblemDirectory>(),
                Files = new List<ProblemFile>(),
            };
            var allDirectories = new List<ProblemDirectory> {  rootDirectory };

            var currentDirectory = rootDirectory;
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("$ cd "))
                {
                    line = line.Replace("$ cd ", "");
                    if (line == "/")
                    {
                        currentDirectory = rootDirectory;
                    }
                    else if (line == "..")
                    {
                        if (currentDirectory.ParentDirectory != null)
                        {
                            currentDirectory = currentDirectory.ParentDirectory;
                        }
                        else throw new Exception("SOMETHING HAPPENED!");
                    }
                    else
                    {
                        if (!currentDirectory.SubDirectories.ContainsKey(line))
                        {
                            var newDirectory = new ProblemDirectory
                            {
                                DirectoryName = line,
                                SubDirectories = new Dictionary<string, ProblemDirectory>(),
                                ParentDirectory = currentDirectory,
                                Files = new List<ProblemFile>(),
                            };
                            currentDirectory.SubDirectories.Add(line, newDirectory);
                            allDirectories.Add(newDirectory);
                        }
                        currentDirectory = currentDirectory.SubDirectories[line];
                    }
                }
                else if (line == "$ ls")
                {
                    var j = i + 1;
                    while (j < lines.Count)
                    {
                        line = lines[j];
                        if (line.StartsWith("$"))
                        {
                            break;
                        }

                        if (line.StartsWith("dir"))
                        {
                            line = line.Replace("dir ", "");
                            if (!currentDirectory.SubDirectories.ContainsKey(line))
                            {
                                var newDirectory = new ProblemDirectory
                                {
                                    DirectoryName = line,
                                    SubDirectories = new Dictionary<string, ProblemDirectory>(),
                                    ParentDirectory = currentDirectory,
                                    Files = new List<ProblemFile>(),
                                };
                                currentDirectory.SubDirectories.Add(line, newDirectory);
                                allDirectories.Add(newDirectory);
                            }
                        }
                        else
                        {
                            var parts = line.Split(" ");
                            var fileSize = double.Parse(parts[0]);
                            var fileName = parts[1];
                            currentDirectory.Files.Add(new ProblemFile
                            {
                                FileName = fileName,
                                FileSize = fileSize,
                            });
                        }
                        j++;
                    }
                    i = j - 1;
                }
            }

            return (rootDirectory, allDirectories);
        }

        private class ProblemDirectory
        {
            public ProblemDirectory? ParentDirectory { get; set; }
            public string? DirectoryName { get; set; }
            public IDictionary<string, ProblemDirectory> SubDirectories { get; set; } = new Dictionary<string, ProblemDirectory>();
            public IList<ProblemFile> Files { get; set; } = new List<ProblemFile>();

            private double? _directorySize;

            public double GetDirectorySize()
            {
                if (!_directorySize.HasValue)
                {
                    _directorySize = Files.Select(f => f.FileSize ?? 0).Sum() + SubDirectories.Values.Select(sd => sd.GetDirectorySize()).Sum();
                }
                return _directorySize.Value;
            }
        }

        private class ProblemFile
        {
            public string? FileName { get; set; }
            public double? FileSize { get; set; }
        }
    }
}
