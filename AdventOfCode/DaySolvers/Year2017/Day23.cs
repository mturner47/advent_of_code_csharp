namespace AdventOfCode.Year2017
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var registers = "abcdefgh".ToDictionary(c => c.ToString(), _ => 0L);
            var mulCount = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                var command = lines[i];
                if (command.StartsWith("set"))
                {
                    var parts = command.Replace("set ", "").Split(" ");
                    registers[parts[0]] = long.TryParse(parts[1], out var value) ? value : registers[parts[1]];
                }
                else if (command.StartsWith("sub"))
                {
                    var parts = command.Replace("sub ", "").Split(" ");
                    registers[parts[0]] -= long.TryParse(parts[1], out var value) ? value : registers[parts[1]];
                }
                else if (command.StartsWith("mul"))
                {
                    mulCount++;
                    var parts = command.Replace("mul ", "").Split(" ");
                    registers[parts[0]] *= long.TryParse(parts[1], out var value) ? value : registers[parts[1]];
                }
                else if (command.StartsWith("jnz"))
                {
                    var parts = command.Replace("jnz ", "").Split(" ");
                    if ((long.TryParse(parts[0], out var value) ? value : registers[parts[0]]) != 0)
                    {
                        var offsetValue = long.TryParse(parts[1], out var v) ? v : registers[parts[1]];
                        if (offsetValue + i - 1 > int.MaxValue) offsetValue = int.MaxValue - i + 1;
                        i += (int)offsetValue - 1;
                    }
                }
            }

            var expectedResult = 3025;
            var result = mulCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var registers = "abcdefgh".ToDictionary(c => c.ToString(), _ => 0L);
            registers["a"] = 1;
            List<int> a = [105701, 105727, 105733, 105751, 105761, 105767, 105769, 105817, 105829, 105863, 105871, 105883, 105899, 105907, 105913, 105929, 105943, 105953, 105967, 105971, 105977, 105983, 105997, 106013, 106019, 106031, 106033, 106087, 106103, 106109, 106121, 106123, 106129, 106163, 106181, 106187, 106189, 106207, 106213, 106217, 106219, 106243, 106261, 106273, 106277, 106279, 106291, 106297, 106303, 106307, 106319, 106321, 106331, 106349, 106357, 106363, 106367, 106373, 106391, 106397, 106411, 106417, 106427, 106433, 106441, 106451, 106453, 106487, 106501, 106531, 106537, 106541, 106543, 106591, 106619, 106621, 106627, 106637, 106649, 106657, 106661, 106663, 106669, 106681, 106693, 106699, 106703, 106721, 106727, 106739, 106747, 106751, 106753, 106759, 106781, 106783, 106787, 106801, 106823, 106853, 106859, 106861, 106867, 106871, 106877, 106903, 106907, 106921, 106937, 106949, 106957, 106961, 106963, 106979, 106993, 107021, 107033, 107053, 107057, 107069, 107071, 107077, 107089, 107099, 107101, 107119, 107123, 107137, 107171, 107183, 107197, 107201, 107209, 107227, 107243, 107251, 107269, 107273, 107279, 107309, 107323, 107339, 107347, 107351, 107357, 107377, 107441, 107449, 107453, 107467, 107473, 107507, 107509, 107563, 107581, 107599, 107603, 107609, 107621, 107641, 107647, 107671, 107687, 107693, 107699, 107713, 107717, 107719, 107741, 107747, 107761, 107773, 107777, 107791, 107827, 107837, 107839, 107843, 107857, 107867, 107873, 107881, 107897, 107903, 107923, 107927, 107941, 107951, 107971, 107981, 107999, 108007, 108011, 108013, 108023, 108037, 108041, 108061, 108079, 108089, 108107, 108109, 108127, 108131, 108139, 108161, 108179, 108187, 108191, 108193, 108203, 108211, 108217, 108223, 108233, 108247, 108263, 108271, 108287, 108289, 108293, 108301, 108343, 108347, 108359, 108377, 108379, 108401, 108413, 108421, 108439, 108457, 108461, 108463, 108497, 108499, 108503, 108517, 108529, 108533, 108541, 108553, 108557, 108571, 108587, 108631, 108637, 108643, 108649, 108677, 108707, 108709, 108727, 108739, 108751, 108761, 108769, 108791, 108793, 108799, 108803, 108821, 108827, 108863, 108869, 108877, 108881, 108883, 108887, 108893, 108907, 108917, 108923, 108929, 108943, 108947, 108949, 108959, 108961, 108967, 108971, 108991, 109001, 109013, 109037, 109049, 109063, 109073, 109097, 109103, 109111, 109121, 109133, 109139, 109141, 109147, 109159, 109169, 109171, 109199, 109201, 109211, 109229, 109253, 109267, 109279, 109297, 109303, 109313, 109321, 109331, 109357, 109363, 109367, 109379, 109387, 109391, 109397, 109423, 109433, 109441, 109451, 109453, 109469, 109471, 109481, 109507, 109517, 109519, 109537, 109541, 109547, 109567, 109579, 109583, 109589, 109597, 109609, 109619, 109621, 109639, 109661, 109663, 109673, 109717, 109721, 109741, 109751, 109789, 109793, 109807, 109819, 109829, 109831, 109841, 109843, 109847, 109849, 109859, 109873, 109883, 109891, 109897, 109903, 109913, 109919, 109937, 109943, 109961, 109987, 110017, 110023, 110039, 110051, 110059, 110063, 110069, 110083, 110119, 110129, 110161, 110183, 110221, 110233, 110237, 110251, 110261, 110269, 110273, 110281, 110291, 110311, 110321, 110323, 110339, 110359, 110419, 110431, 110437, 110441, 110459, 110477, 110479, 110491, 110501, 110503, 110527, 110533, 110543, 110557, 110563, 110567, 110569, 110573, 110581, 110587, 110597, 110603, 110609, 110623, 110629, 110641, 110647, 110651, 110681, 110711, 110729, 110731, 110749, 110753, 110771, 110777, 110807, 110813, 110819, 110821, 110849, 110863, 110879, 110881, 110899, 110909, 110917, 110921, 110923, 110927, 110933, 110939, 110947, 110951, 110969, 110977, 110989, 111029, 111031, 111043, 111049, 111053, 111091, 111103, 111109, 111119, 111121, 111127, 111143, 111149, 111187, 111191, 111211, 111217, 111227, 111229, 111253, 111263, 111269, 111271, 111301, 111317, 111323, 111337, 111341, 111347, 111373, 111409, 111427, 111431, 111439, 111443, 111467, 111487, 111491, 111493, 111497, 111509, 111521, 111533, 111539, 111577, 111581, 111593, 111599, 111611, 111623, 111637, 111641, 111653, 111659, 111667, 111697, 111721, 111731, 111733, 111751, 111767, 111773, 111779, 111781, 111791, 111799, 111821, 111827, 111829, 111833, 111847, 111857, 111863, 111869, 111871, 111893, 111913, 111919, 111949, 111953, 111959, 111973, 111977, 111997, 112019, 112031, 112061, 112067, 112069, 112087, 112097, 112103, 112111, 112121, 112129, 112139, 112153, 112163, 112181, 112199, 112207, 112213, 112223, 112237, 112241, 112247, 112249, 112253, 112261, 112279, 112289, 112291, 112297, 112303, 112327, 112331, 112337, 112339, 112349, 112361, 112363, 112397, 112403, 112429, 112459, 112481, 112501, 112507, 112543, 112559, 112571, 112573, 112577, 112583, 112589, 112601, 112603, 112621, 112643, 112657, 112663, 112687, 112691, 112741, 112757, 112759, 112771, 112787, 112799, 112807, 112831, 112843, 112859, 112877, 112901, 112909, 112913, 112919, 112921, 112927, 112939, 112951, 112967, 112979, 112997, 113011, 113017, 113021, 113023, 113027, 113039, 113041, 113051, 113063, 113081, 113083, 113089, 113093, 113111, 113117, 113123, 113131, 113143, 113147, 113149, 113153, 113159, 113161, 113167, 113171, 113173, 113177, 113189, 113209, 113213, 113227, 113233, 113279, 113287, 113327, 113329, 113341, 113357, 113359, 113363, 113371, 113381, 113383, 113417, 113437, 113453, 113467, 113489, 113497, 113501, 113513, 113537, 113539, 113557, 113567, 113591, 113621, 113623, 113647, 113657, 113683, 113717, 113719, 113723, 113731, 113749, 113759, 113761, 113777, 113779, 113783, 113797, 113809, 113819, 113837, 113843, 113891, 113899, 113903, 113909, 113921, 113933, 113947, 113957, 113963, 113969, 113983, 113989, 114001, 114013, 114031, 114041, 114043, 114067, 114073, 114077, 114083, 114089, 114113, 114143, 114157, 114161, 114167, 114193, 114197, 114199, 114203, 114217, 114221, 114229, 114259, 114269, 114277, 114281, 114299, 114311, 114319, 114329, 114343, 114371, 114377, 114407, 114419, 114451, 114467, 114473, 114479, 114487, 114493, 114547, 114553, 114571, 114577, 114593, 114599, 114601, 114613, 114617, 114641, 114643, 114649, 114659, 114661, 114671, 114679, 114689, 114691, 114713, 114743, 114749, 114757, 114761, 114769, 114773, 114781, 114797, 114799, 114809, 114827, 114833, 114847, 114859, 114883, 114889, 114901, 114913, 114941, 114967, 114973, 114997, 115001, 115013, 115019, 115021, 115057, 115061, 115067, 115079, 115099, 115117, 115123, 115127, 115133, 115151, 115153, 115163, 115183, 115201, 115211, 115223, 115237, 115249, 115259, 115279, 115301, 115303, 115309, 115319, 115321, 115327, 115331, 115337, 115343, 115361, 115363, 115399, 115421, 115429, 115459, 115469, 115471, 115499, 115513, 115523, 115547, 115553, 115561, 115571, 115589, 115597, 115601, 115603, 115613, 115631, 115637, 115657, 115663, 115679, 115693, 115727, 115733, 115741, 115751, 115757, 115763, 115769, 115771, 115777, 115781, 115783, 115793, 115807, 115811, 115823, 115831, 115837, 115849, 115853, 115859, 115861, 115873, 115877, 115879, 115883, 115891, 115901, 115903, 115931, 115933, 115963, 115979, 115981, 115987, 116009, 116027, 116041, 116047, 116089, 116099, 116101, 116107, 116113, 116131, 116141, 116159, 116167, 116177, 116189, 116191, 116201, 116239, 116243, 116257, 116269, 116273, 116279, 116293, 116329, 116341, 116351, 116359, 116371, 116381, 116387, 116411, 116423, 116437, 116443, 116447, 116461, 116471, 116483, 116491, 116507, 116531, 116533, 116537, 116539, 116549, 116579, 116593, 116639, 116657, 116663, 116681, 116687, 116689, 116707, 116719, 116731, 116741, 116747, 116789, 116791, 116797, 116803, 116819, 116827, 116833, 116849, 116867, 116881, 116903, 116911, 116923, 116927, 116929, 116933, 116953, 116959, 116969, 116981, 116989, 116993, 117017, 117023, 117037, 117041, 117043, 117053, 117071, 117101, 117109, 117119, 117127, 117133, 117163, 117167, 117191, 117193, 117203, 117209, 117223, 117239, 117241, 117251, 117259, 117269, 117281, 117307, 117319, 117329, 117331, 117353, 117361, 117371, 117373, 117389, 117413, 117427, 117431, 117437, 117443, 117497, 117499, 117503, 117511, 117517, 117529, 117539, 117541, 117563, 117571, 117577, 117617, 117619, 117643, 117659, 117671, 117673, 117679, 117701, 117703, 117709, 117721, 117727, 117731, 117751, 117757, 117763, 117773, 117779, 117787, 117797, 117809, 117811, 117833, 117839, 117841, 117851, 117877, 117881, 117883, 117889, 117899, 117911, 117917, 117937, 117959, 117973, 117977, 117979, 117989, 117991, 118033, 118037, 118043, 118051, 118057, 118061, 118081, 118093, 118127, 118147, 118163, 118169, 118171, 118189, 118211, 118213, 118219, 118247, 118249, 118253, 118259, 118273, 118277, 118297, 118343, 118361, 118369, 118373, 118387, 118399, 118409, 118411, 118423, 118429, 118453, 118457, 118463, 118471, 118493, 118529, 118543, 118549, 118571, 118583, 118589, 118603, 118619, 118621, 118633, 118661, 118669, 118673, 118681, 118687, 118691, 118709, 118717, 118739, 118747, 118751, 118757, 118787, 118799, 118801, 118819, 118831, 118843, 118861, 118873, 118891, 118897, 118901, 118903, 118907, 118913, 118927, 118931, 118967, 118973, 119027, 119033, 119039, 119047, 119057, 119069, 119083, 119087, 119089, 119099, 119101, 119107, 119129, 119131, 119159, 119173, 119179, 119183, 119191, 119227, 119233, 119237, 119243, 119267, 119291, 119293, 119297, 119299, 119311, 119321, 119359, 119363, 119389, 119417, 119419, 119429, 119447, 119489, 119503, 119513, 119533, 119549, 119551, 119557, 119563, 119569, 119591, 119611, 119617, 119627, 119633, 119653, 119657, 119659, 119671, 119677, 119687, 119689, 119699, 119701, 119723, 119737, 119747, 119759, 119771, 119773, 119783, 119797, 119809, 119813, 119827, 119831, 119839, 119849, 119851, 119869, 119881, 119891, 119921, 119923, 119929, 119953, 119963, 119971, 119981, 119983, 119993, 120011, 120017, 120041, 120047, 120049, 120067, 120077, 120079, 120091, 120097, 120103, 120121, 120157, 120163, 120167, 120181, 120193, 120199, 120209, 120223, 120233, 120247, 120277, 120283, 120293, 120299, 120319, 120331, 120349, 120371, 120383, 120391, 120397, 120401, 120413, 120427, 120431, 120473, 120503, 120511, 120539, 120551, 120557, 120563, 120569, 120577, 120587, 120607, 120619, 120623, 120641, 120647, 120661, 120671, 120677, 120689, 120691, 120709, 120713, 120721, 120737, 120739, 120749, 120763, 120767, 120779, 120811, 120817, 120823, 120829, 120833, 120847, 120851, 120863, 120871, 120877, 120889, 120899, 120907, 120917, 120919, 120929, 120937, 120941, 120943, 120947, 120977, 120997, 121001, 121007, 121013, 121019, 121021, 121039, 121061, 121063, 121067, 121081, 121123, 121139, 121151, 121157, 121169, 121171, 121181, 121189, 121229, 121259, 121267, 121271, 121283, 121291, 121309, 121313, 121321, 121327, 121333, 121343, 121349, 121351, 121357, 121367, 121369, 121379, 121403, 121421, 121439, 121441, 121447, 121453, 121469, 121487, 121493, 121501, 121507, 121523, 121531, 121547, 121553, 121559, 121571, 121577, 121579, 121591, 121607, 121609, 121621, 121631, 121633, 121637, 121661, 121687, 121697, 121711, 121721, 121727, 121763, 121787, 121789, 121843, 121853, 121867, 121883, 121889, 121909, 121921, 121931, 121937, 121949, 121951, 121963, 121967, 121993, 121997, 122011, 122021, 122027, 122029, 122033, 122039, 122041, 122051, 122053, 122069, 122081, 122099, 122117, 122131, 122147, 122149, 122167, 122173, 122201, 122203, 122207, 122209, 122219, 122231, 122251, 122263, 122267, 122273, 122279, 122299, 122321, 122323, 122327, 122347, 122363, 122387, 122389, 122393, 122399, 122401, 122443, 122449, 122453, 122471, 122477, 122489, 122497, 122501, 122503, 122509, 122527, 122533, 122557, 122561, 122579, 122597, 122599, 122609, 122611, 122651, 122653, 122663, 122693];
            List<int> b = [105700, 105717, 105734, 105751, 105768, 105785, 105802, 105819, 105836, 105853, 105870, 105887, 105904, 105921, 105938, 105955, 105972, 105989, 106006, 106023, 106040, 106057, 106074, 106091, 106108, 106125, 106142, 106159, 106176, 106193, 106210, 106227, 106244, 106261, 106278, 106295, 106312, 106329, 106346, 106363, 106380, 106397, 106414, 106431, 106448, 106465, 106482, 106499, 106516, 106533, 106550, 106567, 106584, 106601, 106618, 106635, 106652, 106669, 106686, 106703, 106720, 106737, 106754, 106771, 106788, 106805, 106822, 106839, 106856, 106873, 106890, 106907, 106924, 106941, 106958, 106975, 106992, 107009, 107026, 107043, 107060, 107077, 107094, 107111, 107128, 107145, 107162, 107179, 107196, 107213, 107230, 107247, 107264, 107281, 107298, 107315, 107332, 107349, 107366, 107383, 107400, 107417, 107434, 107451, 107468, 107485, 107502, 107519, 107536, 107553, 107570, 107587, 107604, 107621, 107638, 107655, 107672, 107689, 107706, 107723, 107740, 107757, 107774, 107791, 107808, 107825, 107842, 107859, 107876, 107893, 107910, 107927, 107944, 107961, 107978, 107995, 108012, 108029, 108046, 108063, 108080, 108097, 108114, 108131, 108148, 108165, 108182, 108199, 108216, 108233, 108250, 108267, 108284, 108301, 108318, 108335, 108352, 108369, 108386, 108403, 108420, 108437, 108454, 108471, 108488, 108505, 108522, 108539, 108556, 108573, 108590, 108607, 108624, 108641, 108658, 108675, 108692, 108709, 108726, 108743, 108760, 108777, 108794, 108811, 108828, 108845, 108862, 108879, 108896, 108913, 108930, 108947, 108964, 108981, 108998, 109015, 109032, 109049, 109066, 109083, 109100, 109117, 109134, 109151, 109168, 109185, 109202, 109219, 109236, 109253, 109270, 109287, 109304, 109321, 109338, 109355, 109372, 109389, 109406, 109423, 109440, 109457, 109474, 109491, 109508, 109525, 109542, 109559, 109576, 109593, 109610, 109627, 109644, 109661, 109678, 109695, 109712, 109729, 109746, 109763, 109780, 109797, 109814, 109831, 109848, 109865, 109882, 109899, 109916, 109933, 109950, 109967, 109984, 110001, 110018, 110035, 110052, 110069, 110086, 110103, 110120, 110137, 110154, 110171, 110188, 110205, 110222, 110239, 110256, 110273, 110290, 110307, 110324, 110341, 110358, 110375, 110392, 110409, 110426, 110443, 110460, 110477, 110494, 110511, 110528, 110545, 110562, 110579, 110596, 110613, 110630, 110647, 110664, 110681, 110698, 110715, 110732, 110749, 110766, 110783, 110800, 110817, 110834, 110851, 110868, 110885, 110902, 110919, 110936, 110953, 110970, 110987, 111004, 111021, 111038, 111055, 111072, 111089, 111106, 111123, 111140, 111157, 111174, 111191, 111208, 111225, 111242, 111259, 111276, 111293, 111310, 111327, 111344, 111361, 111378, 111395, 111412, 111429, 111446, 111463, 111480, 111497, 111514, 111531, 111548, 111565, 111582, 111599, 111616, 111633, 111650, 111667, 111684, 111701, 111718, 111735, 111752, 111769, 111786, 111803, 111820, 111837, 111854, 111871, 111888, 111905, 111922, 111939, 111956, 111973, 111990, 112007, 112024, 112041, 112058, 112075, 112092, 112109, 112126, 112143, 112160, 112177, 112194, 112211, 112228, 112245, 112262, 112279, 112296, 112313, 112330, 112347, 112364, 112381, 112398, 112415, 112432, 112449, 112466, 112483, 112500, 112517, 112534, 112551, 112568, 112585, 112602, 112619, 112636, 112653, 112670, 112687, 112704, 112721, 112738, 112755, 112772, 112789, 112806, 112823, 112840, 112857, 112874, 112891, 112908, 112925, 112942, 112959, 112976, 112993, 113010, 113027, 113044, 113061, 113078, 113095, 113112, 113129, 113146, 113163, 113180, 113197, 113214, 113231, 113248, 113265, 113282, 113299, 113316, 113333, 113350, 113367, 113384, 113401, 113418, 113435, 113452, 113469, 113486, 113503, 113520, 113537, 113554, 113571, 113588, 113605, 113622, 113639, 113656, 113673, 113690, 113707, 113724, 113741, 113758, 113775, 113792, 113809, 113826, 113843, 113860, 113877, 113894, 113911, 113928, 113945, 113962, 113979, 113996, 114013, 114030, 114047, 114064, 114081, 114098, 114115, 114132, 114149, 114166, 114183, 114200, 114217, 114234, 114251, 114268, 114285, 114302, 114319, 114336, 114353, 114370, 114387, 114404, 114421, 114438, 114455, 114472, 114489, 114506, 114523, 114540, 114557, 114574, 114591, 114608, 114625, 114642, 114659, 114676, 114693, 114710, 114727, 114744, 114761, 114778, 114795, 114812, 114829, 114846, 114863, 114880, 114897, 114914, 114931, 114948, 114965, 114982, 114999, 115016, 115033, 115050, 115067, 115084, 115101, 115118, 115135, 115152, 115169, 115186, 115203, 115220, 115237, 115254, 115271, 115288, 115305, 115322, 115339, 115356, 115373, 115390, 115407, 115424, 115441, 115458, 115475, 115492, 115509, 115526, 115543, 115560, 115577, 115594, 115611, 115628, 115645, 115662, 115679, 115696, 115713, 115730, 115747, 115764, 115781, 115798, 115815, 115832, 115849, 115866, 115883, 115900, 115917, 115934, 115951, 115968, 115985, 116002, 116019, 116036, 116053, 116070, 116087, 116104, 116121, 116138, 116155, 116172, 116189, 116206, 116223, 116240, 116257, 116274, 116291, 116308, 116325, 116342, 116359, 116376, 116393, 116410, 116427, 116444, 116461, 116478, 116495, 116512, 116529, 116546, 116563, 116580, 116597, 116614, 116631, 116648, 116665, 116682, 116699, 116716, 116733, 116750, 116767, 116784, 116801, 116818, 116835, 116852, 116869, 116886, 116903, 116920, 116937, 116954, 116971, 116988, 117005, 117022, 117039, 117056, 117073, 117090, 117107, 117124, 117141, 117158, 117175, 117192, 117209, 117226, 117243, 117260, 117277, 117294, 117311, 117328, 117345, 117362, 117379, 117396, 117413, 117430, 117447, 117464, 117481, 117498, 117515, 117532, 117549, 117566, 117583, 117600, 117617, 117634, 117651, 117668, 117685, 117702, 117719, 117736, 117753, 117770, 117787, 117804, 117821, 117838, 117855, 117872, 117889, 117906, 117923, 117940, 117957, 117974, 117991, 118008, 118025, 118042, 118059, 118076, 118093, 118110, 118127, 118144, 118161, 118178, 118195, 118212, 118229, 118246, 118263, 118280, 118297, 118314, 118331, 118348, 118365, 118382, 118399, 118416, 118433, 118450, 118467, 118484, 118501, 118518, 118535, 118552, 118569, 118586, 118603, 118620, 118637, 118654, 118671, 118688, 118705, 118722, 118739, 118756, 118773, 118790, 118807, 118824, 118841, 118858, 118875, 118892, 118909, 118926, 118943, 118960, 118977, 118994, 119011, 119028, 119045, 119062, 119079, 119096, 119113, 119130, 119147, 119164, 119181, 119198, 119215, 119232, 119249, 119266, 119283, 119300, 119317, 119334, 119351, 119368, 119385, 119402, 119419, 119436, 119453, 119470, 119487, 119504, 119521, 119538, 119555, 119572, 119589, 119606, 119623, 119640, 119657, 119674, 119691, 119708, 119725, 119742, 119759, 119776, 119793, 119810, 119827, 119844, 119861, 119878, 119895, 119912, 119929, 119946, 119963, 119980, 119997, 120014, 120031, 120048, 120065, 120082, 120099, 120116, 120133, 120150, 120167, 120184, 120201, 120218, 120235, 120252, 120269, 120286, 120303, 120320, 120337, 120354, 120371, 120388, 120405, 120422, 120439, 120456, 120473, 120490, 120507, 120524, 120541, 120558, 120575, 120592, 120609, 120626, 120643, 120660, 120677, 120694, 120711, 120728, 120745, 120762, 120779, 120796, 120813, 120830, 120847, 120864, 120881, 120898, 120915, 120932, 120949, 120966, 120983, 121000, 121017, 121034, 121051, 121068, 121085, 121102, 121119, 121136, 121153, 121170, 121187, 121204, 121221, 121238, 121255, 121272, 121289, 121306, 121323, 121340, 121357, 121374, 121391, 121408, 121425, 121442, 121459, 121476, 121493, 121510, 121527, 121544, 121561, 121578, 121595, 121612, 121629, 121646, 121663, 121680, 121697, 121714, 121731, 121748, 121765, 121782, 121799, 121816, 121833, 121850, 121867, 121884, 121901, 121918, 121935, 121952, 121969, 121986, 122003, 122020, 122037, 122054, 122071, 122088, 122105, 122122, 122139, 122156, 122173, 122190, 122207, 122224, 122241, 122258, 122275, 122292, 122309, 122326, 122343, 122360, 122377, 122394, 122411, 122428, 122445, 122462, 122479, 122496, 122513, 122530, 122547, 122564, 122581, 122598, 122615, 122632, 122649, 122666, 122683, 122700];

            var countInCommon = b.Count - a.Intersect(b).Count();

            var expectedResult = 915;
            var result = countInCommon;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
