using EverybodyCodes;

TestThis();

static void TestThis()
{
    var today = DateTime.Today;
    new List<Problem>
    {
        //new (2024, 2, Part.One),
        //new (2024, 2, Part.Two),
        new (2024, 2, Part.Three),
    }.Select(ProblemRunner.GetSolution)
    .ToList()
    .ForEach(Console.WriteLine);

    Console.ReadLine();
}
