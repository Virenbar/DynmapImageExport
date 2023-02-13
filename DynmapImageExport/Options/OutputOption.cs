using System.CommandLine;

namespace DynmapImageExport.Options
{
    /// <summary>
    /// "--output" option
    /// </summary>
    internal class OutputOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of "--output" option
        /// </summary>
        public OutputOption() : base(new[] { "--output", "-o" }, "Output path") { }
    }
}