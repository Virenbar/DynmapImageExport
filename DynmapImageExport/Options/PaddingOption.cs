using DynmapImageExport.Models;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Options
{
    internal class PaddingOption : Option<Padding>
    {
        /// <summary>
        /// Initializes a new instance of "--padding" option
        /// </summary>
        public PaddingOption() : base(new[] { "--padding", "-p" }, Parce, true, "Padding in tiles [all]|[vert,horz]|[top,right,bottom,left]") { }

        private static Padding Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return new Padding(2); }
            try
            {
                return Padding.Parse(result.Tokens[0].Value);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }
    }
}