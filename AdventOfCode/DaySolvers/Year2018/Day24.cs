using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018
{
    internal partial class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var units = Parse(lines);
            var expectedResult = 15165;
            var (_, result) = RunSimulation(units);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var unitsLeftForImmuneSystem = 0;
            for (var i = 0; i < 100_000_000; i++)
            {
                var units = Parse(lines);
                foreach (var unit in units.Where(u => u.Type == "Immune System"))
                {
                    unit.Attack.damage += i;
                }

                var (winner, unitsLeft) = RunSimulation(units);
                if (winner == "Immune System")
                {
                    unitsLeftForImmuneSystem = unitsLeft;
                    break;
                }
            }
            var expectedResult = 4037;
            var result = unitsLeftForImmuneSystem;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (string winner, int unitsLeft) RunSimulation(List<Unit> units)
        {
            var unitDict = units.ToDictionary(u => u.UnitID, u => u);
            while (units.Select(u => u.Type).Distinct().Count() == 2)
            {
                var targets = new HashSet<(int attacker, int defender, int initiative)> ();
                foreach (var attacker in units.OrderByDescending(u => u.EffectivePower).ThenByDescending(u => u.Initiative))
                {
                    var opposingUnitType = attacker.Type == "Infection" ? "Immune System" : "Infection";
                    var defender = units
                        .Where(d => d.Type == opposingUnitType)
                        .Where(d => !d.Immunities.Contains(attacker.Attack.type))
                        .Where(d => !targets.Any(t => t.defender == d.UnitID))
                        .OrderByDescending(d => d.Weaknesses.Contains(attacker.Attack.type) ? attacker.EffectivePower*2 : attacker.EffectivePower)
                        .ThenByDescending(d => d.EffectivePower)
                        .ThenByDescending(d => d.Initiative)
                        .FirstOrDefault();

                    if (defender != null)
                    {
                        targets.Add((attacker.UnitID, defender.UnitID, attacker.Initiative));
                    }
                }

                if (targets.Count == 0)
                {
                    units = units.Where(u => u.Type == "Infection").ToList();
                    break;
                }

                var preCombatTotalUnits = units.Sum(u => u.UnitCount);
                foreach (var (attackerID, defenderID, _) in targets.OrderByDescending(t => t.initiative))
                {
                    if (unitDict.ContainsKey(attackerID))
                    {
                        var attacker = unitDict[attackerID];
                        var defender = unitDict[defenderID];
                        var damageDealt = attacker.EffectivePower;
                        if (defender.Weaknesses.Contains(attacker.Attack.type)) damageDealt *= 2;
                        defender.UnitCount -= damageDealt / defender.HealthPerUnit;
                        if (defender.UnitCount <= 0)
                        {
                            unitDict.Remove(defenderID);
                            units.Remove(defender);
                        }
                    }
                }
                if (units.Sum(u => u.UnitCount) == preCombatTotalUnits)
                {
                    units = units.Where(u => u.Type == "Infection").ToList();
                    break;
                }
            }

            return (units[0].Type, units.Sum(u => u.UnitCount));
        }

        private static List<Unit> Parse(IList<string> lines)
        {
            var units = new List<Unit>();
            var type = "Immune System";
            var regex = UnitRegex();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line == "Immune System:" || line == "") continue;
                if (line == "Infection:")
                {
                    type = "Infection";
                    continue;
                }
                var unit = new Unit { Type = type, UnitID = i };

                var groups = regex.Matches(line)[0].Groups;
                unit.UnitCount = int.Parse(groups[1].Value);
                unit.HealthPerUnit = int.Parse(groups[2].Value);
                unit.Attack = (int.Parse(groups[4].Value), groups[5].Value);
                unit.Initiative = int.Parse(groups[6].Value);
                var weaknessesAndImmunities = groups[3].Value.Replace(") ", "").Replace("(", "");
                foreach (var wai in weaknessesAndImmunities.Split("; ", StringSplitOptions.RemoveEmptyEntries))
                {
                    var parts = wai.Split(" to ");
                    var types = parts[1].Split(", ").ToHashSet();
                    if (parts[0] == "weak") unit.Weaknesses = types;
                    else unit.Immunities = types;
                }
                units.Add(unit);
            }
            return units;
        }

        private class Unit
        {
            public string Type = "";
            public int UnitID;
            public HashSet<string> Weaknesses = [];
            public HashSet<string> Immunities = [];
            public int UnitCount;
            public int HealthPerUnit;
            public (int damage, string type) Attack;
            public int Initiative;
            public int EffectivePower => UnitCount * Attack.damage;
        }

        [GeneratedRegex(@"^(\d+) units each with (\d+) hit points (\([^)]+\) )?with an attack that does (\d+) ([^ ]+) damage at initiative (\d+)$")]
        private static partial Regex UnitRegex();
    }
}
