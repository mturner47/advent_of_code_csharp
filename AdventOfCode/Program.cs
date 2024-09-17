using AdventOfCode;

TestThis();

static void TestThis()
{
    var today = DateTime.Today;
    new List<Problem>
    {
        //new (2018, 1, Difficulty.Easy),
        //new (2018, 1, Difficulty.Hard),
        //new (2018, 2, Difficulty.Easy),
        //new (2018, 2, Difficulty.Hard),
        //new (2018, 3, Difficulty.Easy),
        //new (2018, 3, Difficulty.Hard),
        //new (2018, 4, Difficulty.Easy),
        //new (2018, 4, Difficulty.Hard),
        //new (2018, 5, Difficulty.Easy),
        //new (2018, 5, Difficulty.Hard),
        //new (2018, 6, Difficulty.Easy),
        //new (2018, 6, Difficulty.Hard),
        //new (2018, 7, Difficulty.Easy),
        //new (2018, 7, Difficulty.Hard),
        //new (2018, 8, Difficulty.Easy),
        //new (2018, 8, Difficulty.Hard),
        //new (2018, 9, Difficulty.Easy),
        //new (2018, 9, Difficulty.Hard),
        //new (2018, 10, Difficulty.Easy),
        //new (2018, 10, Difficulty.Hard),
        //new (2018, 11, Difficulty.Easy),
        //new (2018, 11, Difficulty.Hard),
        //new (2018, 12, Difficulty.Easy),
        //new (2018, 12, Difficulty.Hard),
        //new (2018, 13, Difficulty.Easy),
        //new (2018, 13, Difficulty.Hard),
        //new (2018, 14, Difficulty.Easy),
        //new (2018, 14, Difficulty.Hard),
        //new (2018, 15, Difficulty.Easy),
        //new (2018, 15, Difficulty.Hard),
        //new (2018, 16, Difficulty.Easy),
        //new (2018, 16, Difficulty.Hard),
        new (2018, 17, Difficulty.Easy),
        new (2018, 17, Difficulty.Hard),
        //new (2018, 18, Difficulty.Easy),
        //new (2018, 18, Difficulty.Hard),
        //new (2018, 19, Difficulty.Easy),
        //new (2018, 19, Difficulty.Hard),
        //new (2018, 20, Difficulty.Easy),
        //new (2018, 20, Difficulty.Hard),
        //new (2018, 21, Difficulty.Easy),
        //new (2018, 21, Difficulty.Hard),
        //new (2018, 22, Difficulty.Easy),
        //new (2018, 22, Difficulty.Hard),
        //new (2018, 23, Difficulty.Easy),
        //new (2018, 23, Difficulty.Hard),
        //new (2018, 24, Difficulty.Easy),
        //new (2018, 24, Difficulty.Hard),
        //new (2018, 25, Difficulty.Easy),
        //new (2018, 25, Difficulty.Hard),
    }.Select(ProblemRunner.GetSolution)
    .ToList()
    .ForEach(Console.WriteLine);

    Console.ReadLine();
}
