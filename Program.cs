using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ChangeBackground
{
    class backgroundChanger
    {
        public static string imageFolderPath = @"C:\Users\4042\Desktop\imgz\";

        // Used to set wallpaper
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINFILE = 1;
        public const int SPIF_SENDCHANGE = 2;


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);
        static void Main(String[] args)
        {
            int i = 0; 
             
             var imgs =  Directory.GetFiles(imageFolderPath);
            // Set wallpaper
            // SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imgs[i++], SPIF_UPDATEINFILE | SPIF_SENDCHANGE);
            Backup_PolicyKey();

            int countall = imgs.Length-1;
            while (true) {
                Thread.Sleep(3000);
                if (i > countall) i = 0;
                DisplayPicture(imgs[i++]);

            }
        }
        private static void DisplayPicture(string file_name)
        {
            uint flags = 0;

            string RootPolicyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
            RegistryKey Policykey = Registry.CurrentUser.OpenSubKey(RootPolicyPath, true);
            Policykey.SetValue("Wallpaper", file_name);

            if (!SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0, file_name, SPIF_UPDATEINFILE | SPIF_SENDCHANGE))
            {
                Console.WriteLine("Error");
            }
        }

        private static void Backup_PolicyKey()
        {
            string RootPolicyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
            RegistryKey Policykey = Registry.CurrentUser.OpenSubKey(RootPolicyPath, true);

           var currentComp =  Policykey.GetValue("Wallpaper").ToString();

            Policykey.SetValue("Wallpaper_backup", currentComp);
            //   \\fssrv02.bki.co.th\wallpaper\wallpaper.jpg
        }



    }
}