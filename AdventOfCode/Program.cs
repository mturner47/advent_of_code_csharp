using AdventOfCode;

TestThis();

static void TestThis()
{
    var today = DateTime.Today;
    new List<Problem>
    {
        //new (2024, 1, Difficulty.Easy),
        //new (2024, 1, Difficulty.Hard),
        //new (2024, 2, Difficulty.Easy),
        //new (2024, 2, Difficulty.Hard),
        new (2024, 3, Difficulty.Easy),
        new (2024, 3, Difficulty.Hard),
        //new (2024, 4, Difficulty.Easy),
        //new (2024, 4, Difficulty.Hard),
        //new (2024, 5, Difficulty.Easy),
        //new (2024, 5, Difficulty.Hard),
        //new (2024, 6, Difficulty.Easy),
        //new (2024, 6, Difficulty.Hard),
        //new (2024, 7, Difficulty.Easy),
        //new (2024, 7, Difficulty.Hard),
        //new (2024, 8, Difficulty.Easy),
        //new (2024, 8, Difficulty.Hard),
        //new (2024, 9, Difficulty.Easy),
        //new (2024, 9, Difficulty.Hard),
        //new (2024, 10, Difficulty.Easy),
        //new (2024, 10, Difficulty.Hard),
        //new (2024, 11, Difficulty.Easy),
        //new (2024, 11, Difficulty.Hard),
        //new (2024, 12, Difficulty.Easy),
        //new (2024, 12, Difficulty.Hard),
        //new (2024, 13, Difficulty.Easy),
        //new (2024, 13, Difficulty.Hard),
        //new (2024, 14, Difficulty.Easy),
        //new (2024, 14, Difficulty.Hard),
        //new (2024, 15, Difficulty.Easy),
        //new (2024, 15, Difficulty.Hard),
        //new (2024, 16, Difficulty.Easy),
        //new (2024, 16, Difficulty.Hard),
        //new (2024, 17, Difficulty.Easy),
        //new (2024, 17, Difficulty.Hard),
        //new (2024, 18, Difficulty.Easy),
        //new (2024, 18, Difficulty.Hard),
        //new (2024, 19, Difficulty.Easy),
        //new (2024, 19, Difficulty.Hard),
        //new (2024, 20, Difficulty.Easy),
        //new (2024, 20, Difficulty.Hard),
        //new (2024, 21, Difficulty.Easy),
        //new (2024, 21, Difficulty.Hard),
        //new (2024, 22, Difficulty.Easy),
        //new (2024, 22, Difficulty.Hard),
        //new (2024, 23, Difficulty.Easy),
        //new (2024, 23, Difficulty.Hard),
        //new (2024, 24, Difficulty.Easy),
        //new (2024, 24, Difficulty.Hard),
        //new (2024, 25, Difficulty.Easy),
    }.Select(ProblemRunner.GetSolution)
    .ToList()
    .ForEach(Console.WriteLine);

    Console.ReadLine();
}
