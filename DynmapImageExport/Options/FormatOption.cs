using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Options
{
    internal class FormatOption : Option<ImageFormat?>
    {
        private static readonly HashSet<string> formats;

        static FormatOption()
        {
            formats = Enum.GetNames(typeof(ImageFormat)).ToHashSet(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Initializes a new instance of "--format" option
        /// </summary>
        public FormatOption() : base(new[] { "--format", "-f" }, Parce, false, "Format")
        {
            this.AddCompletions(formats.ToArray());
            AddValidator(Validator);
        }

        private static ImageFormat? Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return null; }
            try
            {
                return (ImageFormat)Enum.Parse(typeof(ImageFormat), result.Tokens[0].Value.ToUpper());
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }

        private static void Validator(OptionResult result)
        {
            if (result.Tokens.SingleOrDefault()?.Value is string str && !formats.Contains(str))
            {
                result.ErrorMessage = $"Invalid format: {str}";
            }
        }
    }
}