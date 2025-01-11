using System.Text.RegularExpressions;

namespace DynmapImageExport.Extensions
{
    internal static class StringExtensions
    {
        private static readonly Regex Brackets = new(@"\s+(?=[^[\]]*\])");

        internal static string[] FixBrackets(this string[] args)
        {
            var arguments = new List<string>();
            var E = args.AsEnumerable().GetEnumerator();
            while (E.MoveNext())
            {
                var arg = E.Current;
                if (arg.Contains('[') && !arg.Contains(']'))
                {
                    while (!arg.Contains(']') && E.MoveNext())
                    {
                        arg += E.Current;
                    }
                    arguments.Add(arg);
                }
                else { arguments.Add(arg); }
            }
            return arguments.ToArray();
        }

        internal static string FixBrackets(this string args)
        {
            return Brackets.Replace(args, "");
        }

        internal static string RemoveInvalidChars(this string str)
        {
            return string.Join("", str.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        }

        internal static string RemoveName(this string args)
        {
            return args.Replace("DynmapImageExport.exe", "")
                .Replace("DynmapImageExport", "")
                .Trim();
        }

        internal static string ReplaceInvalidChars(this string str) => ReplaceInvalidChars(str, "_");

        internal static string ReplaceInvalidChars(this string str, string replacment)
        {
            return string.Join(replacment, str.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}