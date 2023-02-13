using System.CommandLine;

namespace DynmapImageExport.Options
{
    /// <summary>
    /// "--no-cache" option
    /// </summary>
    internal class NoCacheOption : Option<bool>
    {
        /// <summary>
        /// Initializes a new instance of "--no-cache" option
        /// </summary>
        public NoCacheOption() : base(new[] { "--no-cache", "-nc" }, "Ignore cached tiles") { }
    }
}