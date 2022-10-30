using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynmapImageExport.Commands
{
    internal class AboutCommand : Command
    {
        public AboutCommand() : base("about", "Show info about application")
        {
            AddAlias("a");

            Handler = CommandHandler.Create(HandleCommand);
        }

        private int HandleCommand()
        {
            var Info = $"""
                [yellow]DYNMAP IMAGE EXPORT[/]
                Made by: [green]Virenbar[/]
                Source: [white]https://github.com/Virenbar/DynmapImageExport [/]
                Version: [yellow]{Assembly.GetExecutingAssembly().GetName().Version}[/]
                """;
            AnsiConsole.MarkupLine(Info);
            return 0;
        }
    }
}