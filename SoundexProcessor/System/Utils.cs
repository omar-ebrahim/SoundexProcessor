using System.Linq;
using System.Text;

namespace SoundexProcessor.System
{
    /// <summary>
    /// Represents a Utility class
    /// </summary>
    public static class Utils
    {
        private const int RequiredSoundexLength = 4;

        /// <summary>
        /// Gets the soundex value for a word.
        /// </summary>
        public static string GetSoundexValue(string word)
        {
            word = word.ToUpper();
            StringBuilder soundex = new StringBuilder();

            foreach (char ch in word)
            {
                if (char.IsLetter(ch))
                {
                    AddCharacterToSoundex(soundex, ch);
                }
            }

            FixSoundexLength(soundex);

            return soundex.ToString();
        }

        /// <summary>
        /// Adds a character or character code to the soundex <see cref="StringBuilder"/>
        /// </summary>
        /// <param name="soundex">The soundex <see cref="StringBuilder"/></param>
        /// <param name="character">The character to add or get the character code for.</param>
        private static void AddCharacterToSoundex(StringBuilder soundex, char character)
        {
            if (soundex.Length == 0)
            {
                // Add the first letter to the soundex string
                soundex.Append(character.ToString());
            }
            else
            {
                string characterCode = Utils.GetCharacterCode(character);
                if (PreviousCharacterIsDifferentToCurrentCharacter(soundex, characterCode))
                {
                    soundex.Append(characterCode);
                }
            }

        }

        /// <summary>
        /// Fixes the length of the soundex <see cref="StringBuilder"/>.
        /// If it's longer than 4, 
        /// </summary>
        /// <param name="soundex"></param>
        private static void FixSoundexLength(StringBuilder soundex)
        {
            var length = soundex.Length;
            if (length < RequiredSoundexLength)
            {
                PadWithZeroes(soundex, length);
            }
            else
            {
                TruncateToFourCharacters(soundex);
            }
        }

        /// <summary>
        /// Truncates the soundex <see cref="StringBuilder"/> to the requires soundex length of 4 characters.
        /// </summary>
        private static void TruncateToFourCharacters(StringBuilder soundex)
        {
            soundex.Length = RequiredSoundexLength;
        }

        /// <summary>
        /// Pads out the soundex <see cref="StringBuilder"/> with zeroes if it is shorter than 4 characters.
        /// </summary>
        private static void PadWithZeroes(StringBuilder soundex, int length)
        {
            soundex.Append(new string('0', RequiredSoundexLength - length));
        }

        /// <summary>
        /// Returns whether the previous character in the soundex <see cref="StringBuilder"/> is the same character.
        /// </summary>
        private static bool PreviousCharacterIsDifferentToCurrentCharacter(StringBuilder soundex, string characterCode)
        {
            var previousCharacterIndex = soundex.Length - 1;
            return characterCode != soundex[previousCharacterIndex].ToString();
        }

        /// <summary>
        /// Returns the soundex character code for a character.
        /// </summary>
        /// <param name="letter">The vcharacter to get the character code for.</param>
        public static string GetCharacterCode(char letter)
        {
            if ("BFPV".Contains(letter))
            {
                return "1";
            }
            else if ("CGJKQSXZ".Contains(letter))
            {
                return "2";
            }
            else if ("DT".Contains(letter))
            {
                return "3";
            }
            else if ('L' == letter)
            {
                return "4";
            }
            else if ("MN".Contains(letter))
            {
                return "5";
            }
            else if ('R' == letter)
            {
                return "6";
            }
            else
            {
                return "";
            }
        }
    }
}
