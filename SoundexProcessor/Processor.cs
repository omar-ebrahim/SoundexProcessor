using System;
using System.Collections.Generic;
using System.Linq;

public class Processor
{
    private readonly MatchSensitivity sensitivity;

    /// <summary>
    /// Instantiates the Processor class.
    /// </summary>
    /// <param name="sensitivity">How sensitive the processor is.</param>
    public Processor(MatchSensitivity sensitivity)
    {
        this.sensitivity = sensitivity;
    }

    /// <summary>
    /// Processes a list of people records, and returns a list of grouped person records. Intended to be records from a database.
    /// </summary>
    /// <param name="people">
    /// The records to process. Item1: ID, Item2: Forenames, Item3: Surname, Item4: Date of Birth.
    /// </param>
    public Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>> Process(List<Tuple<int, string, string, DateTime>> people)
    {
        var masterSoundexList = CreateSoundexList(people);
        return CreateGroupedSoundexListFromMasterList(masterSoundexList);
    }

    /// <summary>
    /// Creates a grouped list of records that may possibly be duplicates.
    /// </summary>
    private Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>> CreateGroupedSoundexListFromMasterList(List<Tuple<int, string, string, string, string, DateTime>> masterSoundexList)
    {
        var groupedSoundexList = new Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>>();
        foreach (var person in masterSoundexList)
        {
            var id = person.Item1;
            var forenames = person.Item2;
            var surname = person.Item3;
            var forenamesSoundex = person.Item4;
            var surnameSoundex = person.Item5;
            var dob = person.Item6;

            var forenamesSoundexInitial = forenamesSoundex.First();
            var surnameSoundexInitial = surnameSoundex.First();

            var forenamesCode = int.Parse(forenamesSoundex.Remove(0, 1));
            var surnameCode = int.Parse(surnameSoundex.Remove(0, 1));

            if (sensitivity != MatchSensitivity.VeryHigh)
            {
                ApplySentitivity(ref forenamesCode, ref surnameCode);
            }

            var key = $"{forenamesSoundexInitial}{forenamesCode} {surnameSoundexInitial}{surnameCode} {dob.ToString("dd/MM/yyyy")}";

            if (groupedSoundexList.Count == 0)
            {
                var list = new List<Tuple<int, string, string, string, string, DateTime>>();
                list.Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
                groupedSoundexList.Add(key, list);
            }
            else if (!groupedSoundexList.ContainsKey(key))
            {
                var list = new List<Tuple<int, string, string, string, string, DateTime>>();
                list.Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
                groupedSoundexList.Add(key, list);
            }
            else
            {
                groupedSoundexList[key].Add(new Tuple<int, string, string, string, string, DateTime>(id, forenames, surname, forenamesSoundex, surnameSoundex, dob));
            }
        }

        return groupedSoundexList;
    }

    /// <summary>
    /// Reads in the list of people and returns the same list with the forenames and surnames soundex codes - Item4 and Item5 respectively.
    /// </summary>
    private static List<Tuple<int, string, string, string, string, DateTime>> CreateSoundexList(List<Tuple<int, string, string, DateTime>> people)
    {
        var soundexList = new List<Tuple<int, string, string, string, string, DateTime>>();
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

        return soundexList;
    }

    /// <summary>
    /// Applies sensitivity by rounding down the numeric part of the whole soundex code 
    /// to the nearest whole value of the sensitivity passed in the constructor. 
    /// e.g. A210 rounded down to A200
    /// </summary>
    /// <param name="forenamesCode">The forenames soundex code without the preceeding letter.</param>
    /// <param name="surnameCode">The forenames soundex code without the preceeding letter.</param>
    private void ApplySentitivity(ref int forenamesCode, ref int surnameCode)
    {
        forenamesCode = (int)Math.Floor(forenamesCode / (double)sensitivity) * (int)sensitivity;
        surnameCode = (int)Math.Floor(surnameCode / (double)sensitivity) * (int)sensitivity;
    }
}