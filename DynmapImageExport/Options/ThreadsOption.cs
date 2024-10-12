using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Options
{
    internal class ThreadsOption : Option<int?>
    {
        /// <summary>
        /// Initializes a new instance of "--threads" option
        /// </summary>
        public ThreadsOption() : base(new[] { "--threads", "-t" }, Parce, false, "Number of threads") { }

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