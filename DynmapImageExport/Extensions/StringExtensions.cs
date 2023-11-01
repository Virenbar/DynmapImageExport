namespace DynmapImageExport.Extensions
{
    internal static class StringExtensions
    {
        internal static string RemoveInvalidChars(this string str)
        {
            return string.Join("", str.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        }

        internal static string ReplaceInvalidChars(this string str) => ReplaceInvalidChars(str, "_");

        internal static string ReplaceInvalidChars(this string str, string replacment)
        {
            return string.Join(replacment, str.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}