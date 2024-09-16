namespace AdventOfCode.Year2015
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var bossHP = int.Parse(lines[0].Replace("Hit Points: ", ""));
            var bossAtk = int.Parse(lines[1].Replace("Damage: ", ""));
            var bossDef = int.Parse(lines[2].Replace("Armor: ", ""));
            var boss = (bossHP, bossAtk, bossDef);

            var playerHP = 100;
            var weapons = GetWeapons();
            var armor = GetArmor();
            var rings = GetRings();

            var minCost = 100000;
            foreach (var (weaponName, weaponCost, weaponAtk) in weapons)
            {
                foreach (var (armorName, armorCost, armorDef) in armor)
                {
                    for (var r1 = 0; r1 < rings.Count - 1; r1++)
                    {
                        var (ring1Name, ring1Cost, ring1Atk, ring1Def) = rings[r1];
                        for (var r2 = r1 + 1; r2 < rings.Count; r2++)
                        {
                            var (ring2Name, ring2Cost, ring2Atk, ring2Def) = rings[r2];
                            var playerAtk = weaponAtk + ring1Atk + ring2Atk;
                            var playerDef = armorDef + ring1Def + ring2Def;
                            var cost = weaponCost + armorCost + ring1Cost + ring2Cost;
                            var playerWon = SimulateCombat((playerHP, playerAtk, playerDef), boss);
                            if (playerWon && cost < minCost)
                            {
                                minCost = cost;
                            }
                        }
                    }
                }
            }

            var expectedResult = 91;
            var result = minCost;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var bossHP = int.Parse(lines[0].Replace("Hit Points: ", ""));
            var bossAtk = int.Parse(lines[1].Replace("Damage: ", ""));
            var bossDef = int.Parse(lines[2].Replace("Armor: ", ""));
            var boss = (bossHP, bossAtk, bossDef);

            var playerHP = 100;
            var weapons = GetWeapons();
            var armor = GetArmor();
            var rings = GetRings();

            var maxCost = 0;
            foreach (var (weaponName, weaponCost, weaponAtk) in weapons)
            {
                foreach (var (armorName, armorCost, armorDef) in armor)
                {
                    for (var r1 = 0; r1 < rings.Count - 1; r1++)
                    {
                        var (ring1Name, ring1Cost, ring1Atk, ring1Def) = rings[r1];
                        for (var r2 = r1 + 1; r2 < rings.Count; r2++)
                        {
                            var (ring2Name, ring2Cost, ring2Atk, ring2Def) = rings[r2];
                            var playerAtk = weaponAtk + ring1Atk + ring2Atk;
                            var playerDef = armorDef + ring1Def + ring2Def;
                            var cost = weaponCost + armorCost + ring1Cost + ring2Cost;
                            var playerWon = SimulateCombat((playerHP, playerAtk, playerDef), boss);
                            if (!playerWon && cost > maxCost)
                            {
                                maxCost = cost;
                            }
                        }
                    }
                }
            }

            var expectedResult = 91;
            var result = maxCost;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public List<(string name, int cost, int atk)> GetWeapons()
        {
            return
            [
                ("Dagger", 8, 4),
                ("Shortsword", 10, 5),
                ("Warhammer", 25, 6),
                ("Longsword", 40, 7),
                ("Greataxe", 74, 8),
            ];
        }

        public List<(string name, int cost, int def)> GetArmor()
        {
            return
            [
                ("None", 0, 0),
                ("Leather", 13, 1),
                ("Chainmail", 31, 2),
                ("Splintmail", 53, 3),
                ("Bandedmail", 75, 4),
                ("Platemail", 102, 5),
            ];
        }

        public List<(string name, int cost, int atk, int def)> GetRings()
        {
            return
            [
                ("No Ring 1", 0, 0, 0),
                ("No Ring 2", 0, 0, 0),
                ("Damage +1", 25, 1, 0),
                ("Damage +2", 50, 2, 0),
                ("Damage +3", 100, 3, 0),
                ("Defense +1", 20, 0, 1),
                ("Defense +2", 40, 0, 2),
                ("Defense +3", 80, 0, 3),
            ];
        }

        private static bool SimulateCombat((int maxHP, int atk, int def) player, (int maxHP, int atk, int def) boss)
        {
            var (playerMaxHP, playerAtk, playerDef) = player;
            var (bossMaxHP, bossAtk, bossDef) = boss;
            var (playerCurrentHP, bossCurrentHP) = (playerMaxHP, bossMaxHP);

            var currentTurn = 1;
            while (playerCurrentHP > 0 && bossCurrentHP > 0)
            {
                if (currentTurn % 2 == 1)
                {
                    bossCurrentHP -= int.Max(1, playerAtk - bossDef);
                }
                else
                {
                    playerCurrentHP -= int.Max(1, bossAtk - playerDef);
                }
                currentTurn++;
            }
            return playerCurrentHP > bossCurrentHP;
        }
    }

}
