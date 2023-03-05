using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Options
{
    internal class ZoomOption : Option<int?>
    {
        /// <summary>
        /// Initializes a new instance of "--zoom" option
        /// </summary>
        public ZoomOption() : base(new[] { "--zoom", "-z" }, Parce, false, "Zoom") { }

        private static int? Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return null; }
            try
            {
                return int.Parse(result.Tokens[0].Value);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }
    }
}