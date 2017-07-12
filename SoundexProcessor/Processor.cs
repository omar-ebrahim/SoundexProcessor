using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundexProcessor
{
    public class Processor
    {
        public Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>> Process(List<Tuple<int, string, string, DateTime>> people)
        {
            var soundexList = new List<Tuple<int, string, string, string, string, DateTime>>();

            var groupedList = new Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>>();

            foreach (var person in people)
            {
                var id = person.Item1;
                var forenames = person.Item2;
                var surname = person.Item3;
                var dob = person.Item4;

                var forenamesSoundex = Soundex.GetSoundexValue(forenames);
                var surnameSoundex = Soundex.GetSoundexValue(surname);

                soundexList.Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
            }

            foreach (var person in soundexList)
            {
                var id = person.Item1;
                var forenames = person.Item2;
                var surname = person.Item3;
                var forenamesSoundex = person.Item4;
                var surnameSoundex = person.Item5;
                var dob = person.Item6;

                // round down to nearest 50
                var forenamesSoundexInitial = forenamesSoundex.First();
                var forenamesCode = int.Parse(forenamesSoundex.Remove(0, 1));
                forenamesCode = (int)Math.Floor(forenamesCode / 50.0) * 50;
                
                var surnameSoundexInitial = surnameSoundex.First();
                var surnameCode = int.Parse(surnameSoundex.Remove(0, 1));
                surnameCode = (int)Math.Floor(surnameCode / 50.0) * 50;

                var key = $"{forenamesSoundexInitial}{forenamesCode} {surnameSoundexInitial}{surnameCode} {dob.ToString("dd/MM/yyyy")}";

                if (groupedList.Count == 0)
                {
                    var list = new List<Tuple<int, string, string, string, string, DateTime>>();
                    list.Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
                    groupedList.Add(key, list);
                }
                else if (!groupedList.ContainsKey(key))
                {
                    var list = new List<Tuple<int, string, string, string, string, DateTime>>();
                    list.Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
                    groupedList.Add(key, list);
                }
                else
                {
                    groupedList[key].Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
                }
            }

            return groupedList;
        }
    }
}
