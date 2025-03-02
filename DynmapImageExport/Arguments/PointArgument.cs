using DynmapImageExport.Models;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynmapImageExport.Arguments
{
    internal class PointArgument : Argument<Point[]>
    {
        /// <summary>
        /// Initializes a new instance of "point" argument
        /// </summary>
        public PointArgument() : this("point", "Point of map [x,y,z]") { }

        public PointArgument(string name, string description) : base(name, Parce, true, description)
        {
            Arity = ArgumentArity.ZeroOrMore;
        }

        private static Point[] Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return null; }
            try
            {
                return result.Tokens.Select(t => Point.Parse(t.Value)).ToArray();
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }
    }
}