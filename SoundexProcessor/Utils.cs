using System.Linq;

/// <summary>
/// Represents a Utility class
/// </summary>
public static class Utils
{
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