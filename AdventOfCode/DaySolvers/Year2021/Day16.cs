namespace AdventOfCode.Year2021
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var hexString = lines[0];
            var bitString = ConvertHexStringToBitString(hexString);

            var (packet, _) = ParseBitString(bitString, 0);
            return GetTotalVersion(packet);
        }

        public object HardSolution(IList<string> lines)
        {
            var hexString = lines[0];
            var bitString = ConvertHexStringToBitString(hexString);

            var (packet, _) = ParseBitString(bitString, 0);
            return packet.Value ?? 0;
        }

        private (Packet, int) ParseBitString(string bitString, int startingIndex)
        {
            var version = ConvertBitStringToDouble(bitString[startingIndex..(startingIndex + 3)]);
            var typeID = ConvertBitStringToDouble(bitString[(startingIndex + 3)..(startingIndex + 6)]);
            if (typeID == 4) // Literal
            {
                var resultString = "";
                var shouldContinue = true;
                var currentIndex = startingIndex + 6;
                while (shouldContinue)
                {
                    resultString += bitString[(currentIndex + 1)..(currentIndex + 5)];
                    shouldContinue = bitString[currentIndex] == '1';
                    currentIndex += 5;
                }

                var value = ConvertBitStringToDouble(resultString);

                return (new Packet(version, value), currentIndex);
            }
            else
            {
                var packets = new List<Packet>();
                var endingIndex = 0;
                var lengthTypeID = bitString[startingIndex + 6];
                if (lengthTypeID == '0') // by length
                {
                    var length = (int)ConvertBitStringToDouble(bitString[(startingIndex + 7)..(startingIndex + 7 + 15)]);
                    endingIndex = startingIndex + 7 + 15 + length;
                    var currentIndex = startingIndex + 7 + 15;
                    while (currentIndex < endingIndex)
                    {
                        (var subPacket, currentIndex) = ParseBitString(bitString, currentIndex);
                        packets.Add(subPacket);
                    }
                }
                else // by quantity
                {
                    var quantity = ConvertBitStringToDouble(bitString[(startingIndex + 7)..(startingIndex + 7 + 11)]);
                    var currentIndex = startingIndex + 7 + 11;
                    for (var i = 0; i < quantity; i++)
                    {
                        (var subPacket, currentIndex) = ParseBitString(bitString, currentIndex);
                        packets.Add(subPacket);
                    }
                    endingIndex = currentIndex;
                }

                var value = typeID switch
                {
                    0 => packets.Select(p => p.Value).Sum(),
                    1 => packets.Select(p => p.Value).Aggregate(1d, (total, v) => total * (v ?? 1)), //product
                    2 => packets.Select(p => p.Value).Min(),
                    3 => packets.Select(p => p.Value).Max(),
                    5 => packets[0].Value > packets[1].Value ? 1 : 0,
                    6 => packets[0].Value < packets[1].Value ? 1 : 0,
                    7 => packets[0].Value == packets[1].Value ? 1 : 0,
                    _ => 0d,
                };
                return (new Packet(version, value, packets), endingIndex);
            }
        }

        private double GetTotalVersion(Packet packet)
        {
            if (packet == null) return 0;
            if (packet.Packets != null)
            {
                return packet.Version + packet.Packets.Select(GetTotalVersion).Sum();
            }

            return packet.Version;
        }

        private record Packet(double Version, double? Value = null, List<Packet>? Packets = null);

        private static string ConvertHexStringToBitString(string hexString)
        {
            return string.Concat(hexString.SelectMany(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        private static double ConvertBitStringToDouble(string bitString)
        {
            var multiplier = 1d;
            var sum = 0d;
            for (var i = bitString.Length - 1; i >= 0; i--)
            {
                if (bitString[i] == '1')
                {
                    sum += multiplier;
                }
                multiplier *= 2;
            }
            return sum;
        }
    }
}
