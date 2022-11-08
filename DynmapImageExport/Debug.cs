using System.Diagnostics;

namespace DynmapImageExport
{
    internal static class Debug
    {
        internal static void Invoke()
        {
            /*
            https://ebs.virenbar.ru/dynmap/
            https://map.minecrafting.ru
            */
            // Valid
            //Program.Invoke("a");
            //Program.Invoke("ls https://map.minecrafting.ru");
            //Program.Invoke("i https://map.minecrafting.ru world flat");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] [6,6,5,5] 2");
            //Program.Invoke("m https://map.minecrafting.ru/?worldname=world&mapname=flat world se_view [0, 100 ,0] [5, 11,5,10] -nc");
            //Program.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [80]");
            //Program.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [40] 3");
            //Program.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [20] 4");
            //Program.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [10] 5");
            //Program.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [5] 6");

            //Program.Invoke("m https://map.minecrafting.ru world flat [2,2,2] [2,2]");
            // Invalid
            //Program.Invoke("m https://map.minecrafting.ru world flatt [0,100,0] [2] 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] [2,2,2] 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] [2] 10");
            Environment.Exit(0);
        }
    }
}