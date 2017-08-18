using SoundexProcessor.Enums;
using SoundexProcessor.Models;
using SoundexProcessor.System;
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
    public Dictionary<string, List<Person>> Process(List<Person> people)
    {
        var masterSoundexList = CreateSoundexList(people);
        return CreateGroupedSoundexListFromMasterList(masterSoundexList);
    }

    /// <summary>
    /// Creates a grouped list of records that may possibly be duplicates.
    /// </summary>
    private Dictionary<string, List<Person>> CreateGroupedSoundexListFromMasterList(List<Person> masterSoundexList)
    {
        var groupedSoundexList = new Dictionary<string, List<Person>>();
        foreach (var person in masterSoundexList)
        {
            var forenamesSoundexInitial = person.ForenamesSoundex.First();
            var surnameSoundexInitial = person.SurnameSoundex.First();

            var forenamesCode = int.Parse(person.ForenamesSoundex.Remove(0, 1));
            var surnameCode = int.Parse(person.SurnameSoundex.Remove(0, 1));

            if (sensitivity != MatchSensitivity.VeryHigh)
            {
                ApplySentitivity(ref forenamesCode, ref surnameCode);
            }

            var key = $"{forenamesSoundexInitial}{forenamesCode} {surnameSoundexInitial}{surnameCode} {person.DateOfBirth.ToString("dd/MM/yyyy")}";

            if (groupedSoundexList.Count == 0 || !groupedSoundexList.ContainsKey(key))
            {
                AddNewSoundexValue(groupedSoundexList, key, person.Id, person.Forenames, person.Surname, person.ForenamesSoundex, person.SurnameSoundex, person.DateOfBirth);
            }
            else
            {
                groupedSoundexList[key].Add(person);
            }
        }

        return groupedSoundexList;
    }

    /// <summary>
    /// Adds a new soundex value.
    /// </summary>
    private static void AddNewSoundexValue(Dictionary<string, List<Person>> groupedSoundexList, string key, int id, string forenames, string surname, string forenamesSoundex, string surnameSoundex, DateTime dob)
    {
        var list = new List<Person>
        {
            new Person()
            {
                Id = id,
                Forenames = forenames,
                Surname = surname,
                ForenamesSoundex = forenamesSoundex,
                SurnameSoundex = surnameSoundex,
                DateOfBirth = dob
            }
        };
        groupedSoundexList.Add(key, list);
    }

    /// <summary>
    /// Reads in the list of people and returns the same list with the forenames and surnames soundex codes - Item4 and Item5 respectively.
    /// </summary>
    private static List<Person> CreateSoundexList(List<Person> people)
    {
        var soundexList = new List<Person>();
        foreach (var person in people)
        {
            person.ForenamesSoundex = Utils.GetSoundexValue(person.Forenames);
            person.SurnameSoundex = Utils.GetSoundexValue(person.Surname);

            soundexList.Add(person);
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