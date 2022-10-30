using DynmapImageExport.Models;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Arguments
{
    internal class PaddingArgument : Argument<Padding>
    {
        public PaddingArgument(string name, string description) : base(name, Parce, true, description) { }

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