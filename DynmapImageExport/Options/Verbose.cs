using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;

namespace DynmapTools.Options
{
    internal class Verbose : Option<bool>
    {
        private static readonly TraceListener TL = new TextWriterTraceListener(Console.Out);

        public Verbose() : base(new[] { "--verbose", "-v" }, "Show trace log") { }

        public static CommandLineBuilder AddToBuilder(CommandLineBuilder builder)
        {
            var Verbose = new Verbose();
            builder.Command.AddGlobalOption(Verbose);
            builder.AddMiddleware((context, next) =>
            {
                var verbose = context.ParseResult.FindResultFor(Verbose) is not null;
                if (verbose) { Trace.Listeners.Add(TL); }
                else { Trace.Listeners.Remove(TL); }
                Trace.WriteLine($"Verbose: {verbose}");
                return next(context);
            });
            return builder;
        }
    }
}