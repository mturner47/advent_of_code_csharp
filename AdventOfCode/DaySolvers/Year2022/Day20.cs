namespace AdventOfCode.Year2022
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var things = GetThings(lines, 1);
            MixThings(things);

            return GetValueFromThings(things);
        }

        public object HardSolution(IList<string> lines)
        {
            var things = GetThings(lines, 811589153);
            for (var i = 0; i < 10; i++) { MixThings(things); }
            return GetValueFromThings(things);
        }

        private static Dictionary<int, Thing> GetThings(IList<string> lines, double multiplier)
        {
            var count = lines.Count;
            var things = new Dictionary<int, Thing>();
            for (var i = 0; i < count; i++)
            {
                var previousThing = i > 0 ? things[i - 1] : null;
                var newThing = new Thing
                {
                    Value = double.Parse(lines[i])*multiplier,
                    PreviousThing = previousThing,
                };
                things.Add(i, newThing);
                if (newThing.PreviousThing != null) newThing.PreviousThing.NextThing = newThing;

                if (i == count - 1)
                {
                    things[0].PreviousThing = newThing;
                    newThing.NextThing = things[0];
                }
            }
            return things;
        }

        private static void MixThings(Dictionary<int, Thing> things)
        {
            var count = things.Values.Count;
            for (var i = 0; i < count; i++)
            {
                var thingToMove = things[i];
                var shouldMoveLeft = thingToMove.Value < 0;

                var val = Math.Abs(thingToMove.Value) % (count - 1);
                for (var j = 0; j < val; j++)
                {
                    if (shouldMoveLeft)
                    {
                        var thingTwoPrevious = thingToMove!.PreviousThing!.PreviousThing;
                        var thingPrevious = thingToMove.PreviousThing;
                        var thingNext = thingToMove.NextThing;
                        thingTwoPrevious!.NextThing = thingToMove;
                        thingToMove.PreviousThing = thingTwoPrevious;
                        thingToMove.NextThing = thingPrevious;
                        thingPrevious.PreviousThing = thingToMove;
                        thingPrevious.NextThing = thingNext;
                        thingNext!.PreviousThing = thingPrevious;
                    }
                    else
                    {
                        var thingTwoNext = thingToMove!.NextThing!.NextThing;
                        var thingNext = thingToMove.NextThing;
                        var thingPrevious = thingToMove.PreviousThing;
                        thingTwoNext!.PreviousThing = thingToMove;
                        thingToMove.NextThing = thingTwoNext;
                        thingToMove.PreviousThing = thingNext;
                        thingNext.NextThing = thingToMove;
                        thingNext.PreviousThing = thingPrevious;
                        thingPrevious!.NextThing = thingNext;
                    }
                }
            }
        }

        private static double GetValueFromThings(Dictionary<int, Thing> things)
        {
            var count = things.Values.Count;
            var zeroThing = things.Values.First(t => t.Value == 0);
            var checkCount = 1000 % count;
            var currentThing = zeroThing;
            var sum = 0d;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < checkCount; j++)
                {
                    currentThing = currentThing!.NextThing;
                }
                sum += currentThing!.Value;
            }
            return sum;
        }

        private class Thing
        {
            public double Value { get; set; }
            public Thing? PreviousThing { get; set; }
            public Thing? NextThing { get; set; }
        }
    }
}
