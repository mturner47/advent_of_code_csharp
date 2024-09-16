namespace AdventOfCode.Year2017
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var state = new State
            {
                Commands = lines,
                LastSoundPlayed = 0,
                RecoveredFrequency = null,
                Index = 0,
                Registers = [],
            };

            while (!state.RecoveredFrequency.HasValue)
            {
                state = RunCommand(state);
            }

            var expectedResult = 2951;
            var result = state.RecoveredFrequency.Value;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var p0Registers = new Dictionary<string, long> { { "a", 0 }, { "b", 0 }, { "f", 0 }, { "i", 0 }, { "p", 0 } };
            var p1Registers = new Dictionary<string, long> { { "a", 0 }, { "b", 0 }, { "f", 0 }, { "i", 0 }, { "p", 1 } };
            var p0State = new State { ProgramID = 0, Registers = p0Registers, Commands = lines };
            var p1State = new State { ProgramID = 1, Registers = p1Registers, Commands = lines };
            p0State.Sender = p1State;
            p1State.Sender = p0State;

            while (p0State.CanContinue() || p1State.CanContinue())
            {
                while (p0State.CanContinue()) RunCommandHard(p0State);
                while (p1State.CanContinue()) RunCommandHard(p1State);
            }

            var expectedResult = 7366;
            var result = p1State.NumValuesSent;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static State RunCommand(State state)
        {
            var parts = state.Commands[state.Index].Split(' ');
            var command = parts[0];
            if (command == "snd")
            {
                if (long.TryParse(parts[1], out var value))
                {
                    state.LastSoundPlayed = value;
                }
                else
                {
                    if (!state.Registers.ContainsKey(parts[1])) state.Registers[parts[1]] = 0;
                    state.LastSoundPlayed = state.Registers[parts[1]];
                }
                state.Index++;
                return state;
            }

            if (command == "set")
            {
                var register = parts[1];
                if (int.TryParse(parts[2], out var value))
                {
                    state.Registers[register] = value;
                }
                else
                {
                    state.Registers[register] = state.Registers[parts[2]];
                }
                state.Index++;
                return state;
            }

            if (command == "add" || command == "mul" || command == "mod")
            {
                if (!long.TryParse(parts[2], out var value))
                {
                    if (!state.Registers.ContainsKey(parts[2])) state.Registers[parts[2]] = 0;
                    value = state.Registers[parts[2]];
                }

                if (!state.Registers.ContainsKey(parts[1])) state.Registers[parts[1]] = 0;

                if (command == "add") state.Registers[parts[1]] += value;
                if (command == "mul") state.Registers[parts[1]] *= value;
                if (command == "mod") state.Registers[parts[1]] %= value;

                state.Index++;
                return state;
            }

            if (command == "rcv")
            {
                if (!long.TryParse(parts[1], out var value))
                {
                    if (!state.Registers.ContainsKey(parts[1])) state.Registers[parts[1]] = 0;
                    value = state.Registers[parts[1]];
                }

                if (value != 0) state.RecoveredFrequency = state.LastSoundPlayed;
                state.Index++;
                return state;
            }

            if (command == "jgz")
            {
                if (!long.TryParse(parts[1], out var value))
                {
                    if (!state.Registers.ContainsKey(parts[1])) state.Registers[parts[1]] = 0;
                    value = state.Registers[parts[1]];
                }

                if (value > 0)
                {
                    if (!long.TryParse(parts[2], out var offset))
                    {
                        if (!state.Registers.ContainsKey(parts[2])) state.Registers[parts[2]] = 0;
                        offset = state.Registers[parts[2]];
                    }
                    if (offset > int.MaxValue) state.Index = int.MaxValue;
                    else state.Index += (int)offset;
                }
                else state.Index++;
                return state;
            }
            throw new NotImplementedException();
        }

        private static State RunCommandHard(State state)
        {
            var parts = state.Commands[state.Index].Split(' ');
            var command = parts[0];
            if (command == "snd")
            {
                state.SentValues.Enqueue(long.TryParse(parts[1], out var value) ? value : state.Registers[parts[1]]);
                state.Index++;
                state.NumValuesSent++;
                return state;
            }

            if (command == "set")
            {
                var register = parts[1];
                state.Registers[register] = long.TryParse(parts[2], out var value) ? value : state.Registers[parts[2]];
                state.Index++;
                return state;
            }

            if (command == "add" || command == "mul" || command == "mod")
            {
                if (!long.TryParse(parts[2], out var value))
                {
                    value = state.Registers[parts[2]];
                }

                if (command == "add") state.Registers[parts[1]] += value;
                if (command == "mul") state.Registers[parts[1]] *= value;
                if (command == "mod") state.Registers[parts[1]] %= value;

                state.Index++;
                return state;
            }

            if (command == "rcv")
            {
                if (state.Sender?.SentValues.Count > 0)
                {
                    var sentValue = state.Sender?.SentValues.Dequeue();
                    if (!sentValue.HasValue) throw new NotImplementedException();
                    state.Registers[parts[1]] = sentValue.Value;
                    state.IsWaiting = false;
                    state.Index++;
                }
                else
                {
                    state.IsWaiting = true;
                }
                return state;
            }

            if (command == "jgz")
            {
                if ((long.TryParse(parts[1], out var value) ? value : state.Registers[parts[1]]) > 0)
                {
                    if (!long.TryParse(parts[2], out var offset))
                    {
                        offset = state.Registers[parts[2]];
                    }
                    if (offset > int.MaxValue) state.Index = int.MaxValue;
                    else state.Index += (int)offset;
                }
                else state.Index++;
                return state;
            }
            throw new NotImplementedException();
        }

        private class State
        {
            public int ProgramID;
            public long LastSoundPlayed;
            public long? RecoveredFrequency;
            public Queue<long> SentValues = new();
            public Dictionary<string, long> Registers = [];
            public int Index = 0;
            public IList<string> Commands = [];
            public State? Sender = null;
            public long NumValuesSent = 0;
            public bool IsWaiting = false;

            public bool CanContinue()
            {
                if (Index < 0 || Index >= Commands.Count) return false;
                if (IsWaiting && Sender?.SentValues.Count == 0) return false;
                return true;
            }
        }
    }
}
