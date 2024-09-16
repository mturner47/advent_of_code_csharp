using Helpers.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019
{
    internal partial class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var moons = lines.Select(ParseLine).ToList();
            var numTicks = 1000;
            for (var i = 0; i < numTicks; i++)
            {
                Tick(moons);
            }

            var expectedResult = 7471;
            var result = moons.Sum(GetTotalEnergy);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var moons = lines.Select(ParseLine).ToList();

            var initialState = moons.Select(m => new Moon { Position = m.Position, Velocity = m.Velocity }).ToList();
            var xTicks = 0;
            do
            {
                Tick(moons);
                xTicks++;
            } while (!AreEqual(moons, initialState, ((int x, int y, int z) a) => a.x));

            var yTicks = 0;
            moons = initialState.Select(m => new Moon { Position = m.Position, Velocity = m.Velocity }).ToList();
            do
            {
                Tick(moons);
                yTicks++;
            } while (!AreEqual(moons, initialState, ((int x, int y, int z) a) => a.y));

            var zTicks = 0;
            moons = initialState.Select(m => new Moon { Position = m.Position, Velocity = m.Velocity }).ToList();
            do
            {
                Tick(moons);
                zTicks++;
            } while (!AreEqual(moons, initialState, ((int x, int y, int z) a) => a.z));

            var expectedResult = 376243355967784L;
            var result = MathHelpers.LeastCommonMultiplier(MathHelpers.LeastCommonMultiplier(xTicks, yTicks), zTicks);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private class Moon
        {
            public (int x, int y, int z) Position { get; set; }
            public (int x, int y, int z) Velocity { get; set; }
        }

        private static Moon ParseLine(string line)
        {
            var regex = PlanetRegex();
            var matches = regex.Matches(line)[0];
            var x = int.Parse(matches.Groups[1].Value);
            var y = int.Parse(matches.Groups[2].Value);
            var z = int.Parse(matches.Groups[3].Value);
            return new Moon
            {
                Position = (x, y, z),
                Velocity = (0, 0, 0),
            };
        }

        private static void Tick(List<Moon> moons)
        {
            for (var i = 0; i < moons.Count; i++)
            {
                var xChange = 0;
                var yChange = 0;
                var zChange = 0;
                var moon = moons[i];
                for (var j = 0; j < moons.Count; j++)
                {
                    if (i == j) continue;
                    xChange += Diff(moon.Position.x, moons[j].Position.x);
                    yChange += Diff(moon.Position.y, moons[j].Position.y);
                    zChange += Diff(moon.Position.z, moons[j].Position.z);
                }
                var (vx, vy, vz) = moon.Velocity;
                moon.Velocity = (vx + xChange, vy + yChange, vz + zChange);
            }

            foreach (var moon in moons)
            {
                var (px, py, pz) = moon.Position;
                var (vx, vy, vz) = moon.Velocity;
                moon.Position = (px + vx, py + vy, pz + vz);
            }
        }

        private static int Diff(int a, int b)
        {
            if (a > b) return -1;
            if (a < b) return 1;
            return 0;
        }

        private static bool AreEqual(List<Moon> state1, List<Moon> state2, Func<(int x, int y, int z), int> dimensionFunc)
        {
            for (var i = 0; i < state1.Count; i++)
            {
                var moon1 = state1[i];
                var moon2 = state2[i];
                if (dimensionFunc(moon1.Position) != dimensionFunc(moon2.Position)
                    || dimensionFunc(moon1.Velocity) != dimensionFunc(moon2.Velocity))
                {
                    return false;
                }
            }
            return true;
        }

        private static int GetTotalEnergy(Moon moon)
        {
            var (px, py, pz) = moon.Position;
            var (vx, vy, vz) = moon.Velocity;
            return (Math.Abs(px) + Math.Abs(py) + Math.Abs(pz)) * (Math.Abs(vx) + Math.Abs(vy) + Math.Abs(vz));
        }

        [GeneratedRegex(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>")]
        private static partial Regex PlanetRegex();
    }
}
