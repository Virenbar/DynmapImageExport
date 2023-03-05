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
            //Program.Invoke("ls https://ebs.virenbar.ru/dynmap/");
            //Program.Invoke("i https://ebs.virenbar.ru/dynmap/ ebs surface");
            //Program.Invoke("m https://ebs.virenbar.ru/dynmap/ ebs surface");
            //Program.Invoke("ls https://map.minecrafting.ru");
            //Program.Invoke("i https://map.minecrafting.ru world flat");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [6,6,5,5] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [6,6,5,5] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [6,6,5,5] -z 2 -f jpeg");
            //Program.Invoke("m https://map.minecrafting.ru world flat [-110,100,110] [110,100,-110] -p [5] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world se_view [0,100,0] -p [5,11,5,10] -z 0");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [80]");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [40] -z 3");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [20] -z 4");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [10] -z 5");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [5] -z 6");
            //Program.Invoke("m https://map.minecrafting.ru world flat -f webp");
            //Program.Invoke("m https://map.minecrafting.ru world flat [2,2,2] -p [2,2]");
            // Semi-Valid
            //Program.Invoke("m https://map.minecrafting.ru/?worldname=world&mapname=flat world se_view [0, 100 ,0] -p [5, 11,5,10] -nc");
            // Invalid
            //Program.Invoke("m https://map.minecrafting.ru world flatt -p [2] 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [2,2,2] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [2] -z 10");
            Environment.Exit(0);
        }
    }
}