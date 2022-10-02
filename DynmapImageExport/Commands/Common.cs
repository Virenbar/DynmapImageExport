using Dynmap;
using Spectre.Console;

namespace DynmapImageExport.Commands
{
    internal static class Common
    {
        public static async Task<DynMap> GetDynmap(string url)
        {
            var D = new DynMap(url);
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("[yellow]Fetching dynmap...[/]", async ctx =>
                {
                    await D.RefreshConfig();
                    await Task.Delay(5000);
                });
            return D;
        }
    }
}