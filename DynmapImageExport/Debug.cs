namespace DynmapImageExport
{
    internal static class Debug
    {
        internal static void Invoke()
        {
            // Valid examples
            // EBS (Uses internal web-server)
            // https://ebs.virenbar.ru/dynmap/standalone/config.js
            //Program.Invoke("ls https://ebs.virenbar.ru/dynmap/");
            //Program.Invoke("i https://ebs.virenbar.ru/dynmap/ ebs surface");
            //Program.Invoke("m https://ebs.virenbar.ru/dynmap/ ebs surface");
            // Minecrafting.ru (Uses standalone web-server)
            // https://map.minecrafting.ru/standalone/config.js
            //Program.Invoke("a");
            //Program.Invoke("ls https://map.minecrafting.ru");
            //Program.Invoke("i https://map.minecrafting.ru world flat");
            Program.Invoke("m https://map.minecrafting.ru world flat");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [5] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [6] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat [0,100,0] -p [6,6,5,5] -z 2 -f jpeg");
            //Program.Invoke("m https://map.minecrafting.ru world flat [80,100,300] [300,100,80] -p [5] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat -f webp");
            // Isometric example
            //Program.Invoke("m https://map.minecrafting.ru world se_view -p [5,11,5,10] -z 0");
            //Program.Invoke("m https://map.minecrafting.ru world se_view [190,100,190] -p [41] -z 3");
            //Program.Invoke("m https://map.minecrafting.ru world se_view [190,100,190] -p [21] -z 4");
            //Program.Invoke("m https://map.minecrafting.ru world se_view [190,100,190] -p [11] -z 5");
            // Full map example
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [80]");
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [40] -z 3");
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [20] -z 4 -t 8 -nc");
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [10] -z 5");
            //Program.Invoke("m https://map.minecrafting.ru world flat [190,100,190] -p [5] -z 6");
            // Semi-Valid examples
            //Program.Invoke("m https://map.minecrafting.ru/?worldname=world&mapname=flat world se_view [ 0, 100 ,0] -p [5, 11,5,10] -nc");
            //Program.Invoke("m https://map.minecrafting.ru world flat [ 0, 100 ,0 ] -p [5, 11,5,10]");
            //Program.Invoke(@"i https://map.minecrafting.ru ""world 2"" flat");
            // Invalid examples
            //Program.Invoke("m https://map.minecrafting.ru world flatt -p [2] 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [2,2,2] -z 2");
            //Program.Invoke("m https://map.minecrafting.ru world flat -p [2] -z 10");
            Environment.Exit(0);
        }
    }
}