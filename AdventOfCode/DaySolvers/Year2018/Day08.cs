namespace AdventOfCode.Year2018
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var entries = lines[0].Split(' ').Select(int.Parse).ToList();
            var expectedResult = 47112;
            var result = GetMetadataCountEasy(entries, 0).totalMetadata;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var entries = lines[0].Split(' ').Select(int.Parse).ToList();
            var expectedResult = 0;
            var result = GetMetadataCountHard(entries, 0).totalMetadata;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public static (int totalMetadata, int endingIndex) GetMetadataCountEasy(List<int> entries, int startingIndex)
        {
            var childNodeCount = entries[startingIndex];
            var metadataCount = entries[startingIndex + 1];
            if (childNodeCount == 0) return (entries.Skip(startingIndex + 2).Take(metadataCount).Sum(), startingIndex + 2 + metadataCount);

            var nodeStartingIndex = startingIndex + 2;
            var totalMetadata = 0;
            for (var i = 0; i < childNodeCount; i++)
            {
                (var nodeMetadata, nodeStartingIndex) = GetMetadataCountEasy(entries, nodeStartingIndex);
                totalMetadata += nodeMetadata;
            }
            return (totalMetadata + entries.Skip(nodeStartingIndex).Take(metadataCount).Sum(), nodeStartingIndex + metadataCount);
        }

        public static (int totalMetadata, int endingIndex) GetMetadataCountHard(List<int> entries, int startingIndex)
        {
            var childNodeCount = entries[startingIndex];
            var metadataCount = entries[startingIndex + 1];
            if (childNodeCount == 0) return (entries.Skip(startingIndex + 2).Take(metadataCount).Sum(), startingIndex + 2 + metadataCount);

            var nodeStartingIndex = startingIndex + 2;
            var totalMetadata = 0;
            var nodeMetadatas = new List<int>();
            for (var i = 0; i < childNodeCount; i++)
            {
                (var nodeMetadata, nodeStartingIndex) = GetMetadataCountHard(entries, nodeStartingIndex);
                nodeMetadatas.Add(nodeMetadata);
            }

            foreach (var metadata in entries.Skip(nodeStartingIndex).Take(metadataCount))
            {
                if (nodeMetadatas.Count > metadata - 1)
                {
                    totalMetadata += nodeMetadatas[metadata - 1];
                }
            }

            return (totalMetadata, nodeStartingIndex + metadataCount);
        }
    }
}
