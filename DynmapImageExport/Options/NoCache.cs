using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynmapImageExport.Options
{
    internal class NoCache : Option<bool>
    {
        public NoCache() : base(new[] { "--no-cache", "-nc" }, "Ignore cached tiles") { }

        public static CommandLineBuilder AddToBuilder(CommandLineBuilder builder)
        {
            var Verbose = new Verbose();
            builder.Command.AddGlobalOption(Verbose);
            builder.AddMiddleware((context, next) =>
            {
                var verbose = context.ParseResult.FindResultFor(Verbose) is not null;
                if (verbose) { }
                else { }
                Trace.WriteLine($"No Cache: {verbose}");
                return next(context);
            });
            return builder;
        }
    }
}