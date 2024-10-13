using System.Text;

namespace Nsga2;

public static class ExtensionMethods
{
    public static string SubstituteString(this string original, int index, int length, string substitute)
    {
        return new StringBuilder(original).Remove(index, length).Insert(index, substitute).ToString();
    }
}


