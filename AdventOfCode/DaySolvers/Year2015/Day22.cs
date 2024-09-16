namespace AdventOfCode.Year2015
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var bossHP = int.Parse(lines[0].Replace("Hit Points: ", ""));
            var bossAtk = int.Parse(lines[1].Replace("Damage: ", ""));

            var initialState = new CombatState
            {
                BossHP = bossHP,
                PlayerHP = 50,
                PlayerMana = 500,
                ManaSpent = 0,
                IsPlayerTurn = true,
                ShieldTurnsLeft = 0,
                PoisonTurnsLeft = 0,
                RechargeTurnsLeft = 0,
                HardMode = false,
            };

            var expectedResult = 1269;
            var result = GetMinimumManaSpent(initialState, bossAtk);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var bossHP = int.Parse(lines[0].Replace("Hit Points: ", ""));
            var bossAtk = int.Parse(lines[1].Replace("Damage: ", ""));

            var initialState = new CombatState
            {
                BossHP = bossHP,
                PlayerHP = 50,
                PlayerMana = 500,
                ManaSpent = 0,
                IsPlayerTurn = true,
                ShieldTurnsLeft = 0,
                PoisonTurnsLeft = 0,
                RechargeTurnsLeft = 0,
                HardMode = true,
            };

            var expectedResult = 1309;
            var result = GetMinimumManaSpent(initialState, bossAtk);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int GetMinimumManaSpent(CombatState initialState, int bossAtk)
        {
            var priorityQueue = new PriorityQueue<CombatState, int>();
            priorityQueue.Enqueue(initialState, 0);

            while (true)
            {
                var currentState = priorityQueue.Dequeue();
                if (currentState.BossHP < 0)
                {
                    return currentState.ManaSpent;
                }

                if (currentState.PlayerHP < 0) continue;
                if (currentState.PlayerMana < 53) continue;

                var newState = GetNewState(currentState);

                if (!currentState.IsPlayerTurn)
                {
                    var bossDamage = int.Max(bossAtk - (currentState.ShieldTurnsLeft > 0 ? 7 : 0), 1);
                    newState.PlayerHP -= bossDamage;
                    priorityQueue.Enqueue(newState, newState.ManaSpent);
                }
                else
                {
                    // Magic Missile
                    newState = GetNewState(currentState);
                    newState.BossHP -= 4;
                    newState.ManaSpent += 53;
                    newState.PlayerMana -= 53;
                    priorityQueue.Enqueue(newState, newState.ManaSpent);

                    // Drain
                    if (currentState.PlayerMana >= 73)
                    {
                        newState = GetNewState(currentState);
                        newState.BossHP -= 2;
                        newState.PlayerHP += 2;
                        newState.ManaSpent += 73;
                        newState.PlayerMana -= 73;
                        priorityQueue.Enqueue(newState, newState.ManaSpent);
                    }

                    // Shield
                    if (currentState.PlayerMana >= 113 && currentState.ShieldTurnsLeft <= 1)
                    {
                        newState = GetNewState(currentState);
                        newState.ShieldTurnsLeft = 6;
                        newState.ManaSpent += 113;
                        newState.PlayerMana -= 113;
                        priorityQueue.Enqueue(newState, newState.ManaSpent);
                    }

                    // Poison
                    if (currentState.PlayerMana >= 173 && currentState.PoisonTurnsLeft <= 1)
                    {
                        newState = GetNewState(currentState);
                        newState.PoisonTurnsLeft = 6;
                        newState.ManaSpent += 173;
                        newState.PlayerMana -= 173;
                        priorityQueue.Enqueue(newState, newState.ManaSpent);
                    }

                    // Recharge
                    if (currentState.PlayerMana >= 229 && currentState.RechargeTurnsLeft <= 1)
                    {
                        newState = GetNewState(currentState);
                        newState.RechargeTurnsLeft = 5;
                        newState.ManaSpent += 229;
                        newState.PlayerMana -= 229;
                        priorityQueue.Enqueue(newState, newState.ManaSpent);
                    }
                }
            }
        }

        private static CombatState GetNewState(CombatState currentState)
        {
            return new CombatState
            {
                BossHP = currentState.BossHP - (currentState.PoisonTurnsLeft > 0 ? 3 : 0),
                PlayerHP = currentState.PlayerHP - (currentState.HardMode ? 1 : 0),
                PlayerMana = currentState.PlayerMana + (currentState.RechargeTurnsLeft > 0 ? 101 : 0),
                ManaSpent = currentState.ManaSpent,
                IsPlayerTurn = !currentState.IsPlayerTurn,
                ShieldTurnsLeft = currentState.ShieldTurnsLeft > 0 ? currentState.ShieldTurnsLeft - 1 : 0,
                PoisonTurnsLeft = currentState.PoisonTurnsLeft > 0 ? currentState.PoisonTurnsLeft - 1 : 0,
                RechargeTurnsLeft = currentState.RechargeTurnsLeft > 0 ? currentState.RechargeTurnsLeft - 1 : 0,
                HardMode = currentState.HardMode,
            };
        }

        private class CombatState
        {
            public int PlayerHP { get; set; }
            public int PlayerMana { get; set; }
            public int BossHP { get; set; }
            public int ManaSpent { get; set; }
            public bool IsPlayerTurn { get; set; }
            public int ShieldTurnsLeft { get; set; }
            public int PoisonTurnsLeft { get; set; }
            public int RechargeTurnsLeft { get; set; }
            public bool HardMode { get; set; }
        }
    }
}
