using DynmapImageExport.Models;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Arguments
{
    internal class PointArgument : Argument<Point>
    {
        public PointArgument(string name, string description) : base(name, Parce, true, description) { }

        private static Point Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return new Point(0, 100, 0); }
            try
            {
                return Point.Parse(result.Tokens[0].Value);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }
    }
}