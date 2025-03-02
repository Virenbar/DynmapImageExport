using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;

namespace DynmapImageExport.Options
{
    internal class TraceOption : Option<bool>
    {
        private static TraceListener TL;

        public TraceOption() : base(new[] { "--trace", "-t" }, "Write trace log") { }

        public static CommandLineBuilder AddToBuilder(CommandLineBuilder builder)
        {
            var Trace = new TraceOption();
            builder.Command.AddGlobalOption(Trace);
            builder.AddMiddleware((context, next) =>
            {
                var trace = context.ParseResult.FindResultFor(Trace) is not null;
                if (trace && !System.Diagnostics.Trace.Listeners.Contains(TL))
                {
                    TL ??= new LogListener();
                    System.Diagnostics.Trace.Listeners.Add(TL);
                }
                else
                {
                    System.Diagnostics.Trace.Listeners.Remove(TL);
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

            private static string TimeStamp => $"[{DateTime.Now:HH:mm:ss.fff}]";

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