namespace AdventOfCode.Year2017
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var numSteps = int.Parse(lines[0]);

            var nodes = new List<Node>();
            var currentNode = new Node { Value = 0, NextNode = 0, PriorNode = 0 };
            nodes.Add(currentNode);

            for (var i = 1; i <= 2017; i++)
            {
                for (var j = 0; j < numSteps; j++)
                {
                    currentNode = nodes[currentNode.NextNode];
                }
                var nextNode = new Node { Value = i, NextNode = currentNode.NextNode, PriorNode = currentNode.Value };
                nodes[currentNode.NextNode].PriorNode = nextNode.Value;
                currentNode.NextNode = nextNode.Value;
                nodes.Add(nextNode);
                currentNode = nextNode;
            }

            var expectedResult = 996;
            var result = nodes[2017].NextNode;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var numSteps = int.Parse(lines[0]);

            var currentIndex = 0;
            var zeroIndex = 0;
            var valueAfterZero = 0;
            for (var i = 1; i < 50_000_000; i++)
            {
                currentIndex = (currentIndex + numSteps)%i;
                if (currentIndex == zeroIndex) valueAfterZero = i;
                if (currentIndex < zeroIndex) zeroIndex++;
                currentIndex++;
            }

            var expectedResult = 1898341;
            var result = valueAfterZero;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private class Node
        {
            public int PriorNode;
            public int NextNode;
            public int Value;
        }
    }
}
