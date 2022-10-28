using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;
using System.Text;

namespace DynmapImageExport.Options
{
    internal class Verbose : Option<bool>
    {
        private static TraceListener TL;

        public Verbose() : base(new[] { "--verbose", "-v" }, "Write trace log") { }

        public static CommandLineBuilder AddToBuilder(CommandLineBuilder builder)
        {
            var Verbose = new Verbose();
            builder.Command.AddGlobalOption(Verbose);
            builder.AddMiddleware((context, next) =>
            {
                var verbose = context.ParseResult.FindResultFor(Verbose) is not null;
                if (verbose && !Trace.Listeners.Contains(TL))
                {
                    TL ??= new LogListener();
                    Trace.Listeners.Add(TL);
                }
                else
                {
                    Trace.Listeners.Remove(TL);
                }
                return next(context);
            });
            return builder;
        }

        private class AnsiListener : TraceListener
        {
            public override void Write(string message) => AnsiConsole.Write(message);

            public override void WriteLine(string message) => AnsiConsole.WriteLine(message);
        }

        private class LogListener : TraceListener
        {
            private readonly TextWriter Writer;

            public LogListener()
            {
                Writer = new StreamWriter("trace.log", true, Encoding.UTF8) { AutoFlush = true };
            }

            private string TimeStamp => $"[{DateTime.Now:HH:mm:ss.fff}]";

            public override void Write(string message) => Writer.Write($"{TimeStamp} {message}");

            public override void WriteLine(string message) => Writer.WriteLine($"{TimeStamp} {message}");

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Writer.Dispose();
            }
        }
    }
}