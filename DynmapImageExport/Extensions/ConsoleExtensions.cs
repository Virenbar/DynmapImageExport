using DynmapImageExport.Options;
using Spectre.Console;
using System.CommandLine.Builder;
using System.Text.RegularExpressions;

namespace DynmapImageExport.Extensions
{
    internal static class ConsoleExtensions
    {
        internal static IProgress<int> AsProgress(this ProgressTask task)
        {
            return new Progress<int>(i =>
            {
                task.Increment(i);
                task.Description = Regex.Replace(task.Description, @"\d+/\d+", $"{task.Value}/{task.MaxValue}");
            });
        }

        internal static CommandLineBuilder UseTrace(this CommandLineBuilder builder) => TraceOption.AddToBuilder(builder);
    }
}