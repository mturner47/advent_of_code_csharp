namespace AdventOfCode.Year2022
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var packetPairs = ConvertLinesToPacketPairs(lines).ToList();

            var sum = 0;
            for (var i = 0; i < packetPairs.Count; i++)
            {
                if (IsInRightOrder(packetPairs[i]) == Result.Right) sum += i + 1;
            }

            return sum;
        }

        public object HardSolution(IList<string> lines)
        {
            var packets = lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Concat(new List<string> { "[[2]]", "[[6]]" })
                .Select(l => ConvertLineToPacket(l).Item1).ToList();

            var sortedPackets = packets.OrderBy(l => l, new PacketComparer()).ToList();
            var twoIndex = 0;
            var sixIndex = 0;
            for (var i = 0; i < sortedPackets.Count; i++)
            {
                var packet = sortedPackets[i];
                if (packet.RawString == "[[2]]") twoIndex = i + 1;
                if (packet.RawString == "[[6]]") sixIndex = i + 1;
            }
            return twoIndex * sixIndex;
        }

        private static IEnumerable<(Packet, Packet)> ConvertLinesToPacketPairs(IList<string> lines)
        {
            var i = 0;
            while (i < lines.Count)
            {
                var line1 = lines[i];
                var line2 = lines[i + 1];
                var (packet1, _) = ConvertLineToPacket(line1);
                var (packet2, _) = ConvertLineToPacket(line2);
                yield return (packet1, packet2);
                i += 3;
            }
        }

        private static (Packet, int) ConvertLineToPacket(string line)
        {
            var intString = string.Concat(line.TakeWhile(IsInt));
            if (intString?.Length > 0)
            {
                var intPacket = new Packet
                {
                    Num = int.Parse(intString),
                    RawString = line,
                };
                return (intPacket, intString.Length);
            }

            var i = 1;
            var item = new Packet
            {
                Packets = new List<Packet>(),
                RawString = line,
            };
            while (i < line.Length)
            {
                var c = line[i];
                if (c == '[' || IsInt(c))
                {
                    var (subItem, size) = ConvertLineToPacket(line[i..]);
                    item.Packets.Add(subItem);
                    i += size;
                }
                else if (c == ']')
                {
                    break;
                }
                else if (c == ',')
                {
                    i++;
                }
            }

            return (item, i+1);
        }

        private static bool IsInt(char c)
        {
            return "0123456789".Contains(c);
        }

        private static Result IsInRightOrder((Packet, Packet) packetPair)
        {
            var (packet1, packet2) = packetPair;
            if (packet1.Num.HasValue && packet2.Num.HasValue)
            {
                if (packet1.Num.Value == packet2.Num.Value) return Result.Continue;
                return packet1.Num.Value < packet2.Num.Value ? Result.Right : Result.Wrong;
            }

            if (packet1.Packets != null && packet2.Num.HasValue)
            {
                var newPacket2 = new Packet { Packets = new List<Packet> { packet2 } };
                return IsInRightOrder((packet1, newPacket2));
            }

            if (packet1.Num.HasValue && packet2.Packets != null)
            {
                var newPacket1 = new Packet { Packets = new List<Packet> { packet1 } };
                return IsInRightOrder((newPacket1, packet2));
            }

            if (packet1.Packets != null && packet2.Packets != null)
            {
                var packets1 = packet1.Packets;
                var packets2 = packet2.Packets;
                for (var i = 0; i < packets1.Count; i++)
                {
                    if (i < packets2.Count)
                    {
                        var result = IsInRightOrder((packets1[i], packets2[i]));
                        if (result == Result.Continue) continue;
                        return result;
                    }

                    if (i >= packets2.Count) return Result.Wrong;
                }
                if (packets1.Count < packets2.Count) return Result.Right;
            }

            return Result.Continue;
        }

        private class Packet
        {
            public int? Num { get; set; }
            public List<Packet>? Packets { get; set; }
            public string? RawString { get; set; }
        }

        private class PacketComparer : IComparer<Packet>
        {
            public int Compare(Packet? x, Packet? y)
            {
                if (x == null || y == null) return 0;
                if (x == y) return 0;
                var result = IsInRightOrder((x, y));
                if (result == Result.Right) return -1;
                return 1;
            }
        }

        private enum Result
        {
            Right,
            Wrong,
            Continue,
        };
    }
}
