using Spectre.Console;
using System.CommandLine.Invocation;

namespace DynmapImageExport
{
    internal static class ExceptionHandler
    {
        internal static void Handle(Exception exception, InvocationContext context)
        {
            if (exception is ArgumentException AE)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{AE.Message}[/]");
            }
            else if (exception is HttpRequestException HRE)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{HRE.Message}[/]");
            }
            else if (exception is OperationCanceledException) { return; }
            else
            {
                AnsiConsole.MarkupInterpolated($"[red]{context.LocalizationResources.ExceptionHandlerHeader()}[/]");
                AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
            }
            context.ExitCode = 1;
        }
    }
}