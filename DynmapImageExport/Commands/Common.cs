using Dynmap;
using Spectre.Console;

namespace DynmapImageExport.Commands
{
    internal static class Common
    {
        public static async Task<DynMap> GetDynmap(Uri url)
        {
            var D = new DynMap(new Uri(url.OriginalString.Split('?')[0]));
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("[yellow]Fetching dynmap...[/]", async ctx =>
                {
                    await D.RefreshConfig();
                });
            return D;
        }
    }
}