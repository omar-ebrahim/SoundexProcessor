using System;

namespace SoundexProcessor.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Forenames { get; set; }

        public string Surname { get; set; }

        public string ForenamesSoundex { get; set; }

        public string SurnameSoundex { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
