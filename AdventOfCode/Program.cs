using AdventOfCode;

TestThis();

static void TestThis()
{
    var today = DateTime.Today;
    new List<Problem>
    {
        new (2020, 1, Difficulty.Easy),
        new (2020, 1, Difficulty.Hard),
        //new (2020, 2, Difficulty.Easy),
        //new (2020, 2, Difficulty.Hard),
        //new (2020, 3, Difficulty.Easy),
        //new (2020, 3, Difficulty.Hard),
        //new (2020, 4, Difficulty.Easy),
        //new (2020, 4, Difficulty.Hard),
        //new (2020, 5, Difficulty.Easy),
        //new (2020, 5, Difficulty.Hard),
        //new (2020, 6, Difficulty.Easy),
        //new (2020, 6, Difficulty.Hard),
        //new (2020, 7, Difficulty.Easy),
        //new (2020, 7, Difficulty.Hard),
        //new (2020, 8, Difficulty.Easy),
        //new (2020, 8, Difficulty.Hard),
        //new (2020, 9, Difficulty.Easy),
        //new (2020, 9, Difficulty.Hard),
        //new (2020, 10, Difficulty.Easy),
        //new (2020, 10, Difficulty.Hard),
        //new (2020, 11, Difficulty.Easy),
        //new (2020, 11, Difficulty.Hard),
        //new (2020, 12, Difficulty.Easy),
        //new (2020, 12, Difficulty.Hard),
        //new (2020, 13, Difficulty.Easy),
        //new (2020, 13, Difficulty.Hard),
        //new (2020, 14, Difficulty.Easy),
        //new (2020, 14, Difficulty.Hard),
        //new (2020, 15, Difficulty.Easy),
        //new (2020, 15, Difficulty.Hard),
        //new (2020, 16, Difficulty.Easy),
        //new (2020, 16, Difficulty.Hard),
        //new (2020, 17, Difficulty.Easy),
        //new (2020, 17, Difficulty.Hard),
        //new (2020, 18, Difficulty.Easy),
        //new (2020, 18, Difficulty.Hard),
        //new (2020, 19, Difficulty.Easy),
        //new (2020, 19, Difficulty.Hard),
        //new (2020, 20, Difficulty.Easy),
        //new (2020, 20, Difficulty.Hard),
        //new (2020, 21, Difficulty.Easy),
        //new (2020, 21, Difficulty.Hard),
        //new (2020, 22, Difficulty.Easy),
        //new (2020, 22, Difficulty.Hard),
        //new (2020, 23, Difficulty.Easy),
        //new (2020, 23, Difficulty.Hard),
        //new (2020, 24, Difficulty.Easy),
        //new (2020, 24, Difficulty.Hard),
        //new (2020, 25, Difficulty.Easy),
    }.Select(ProblemRunner.GetSolution)
    .ToList()
    .ForEach(Console.WriteLine);

    Console.ReadLine();
}
