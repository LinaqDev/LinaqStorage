using System;
using System.Collections.Generic;
using System.Reflection;

namespace LinaqStorage
{
    public static class Constants
    {
        public static void InitConstatns()
        {
            ApplicationName = "Linaq Storage";
            AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
            ApplicationNameWithVersion = $"{ApplicationName} (0.0.0) ";
#else
            ApplicationNameWithVersion = $"{ApplicationName} ({AssemblyVersion.Remove(AssemblyVersion.LastIndexOf('.'))}) ";
#endif 

            CopyRightString = $"© {DateTime.UtcNow.Year.ToString()}";

            InitTypesIconsList();
        }

        public static string AssemblyVersion { get; set; }
        public static string ApplicationName { get; set; }
        public static string ApplicationNameWithVersion { get; set; }
        public static string CopyRightString { get; set; }

        public static string localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static List<string> TypesIconsList = new List<string>();

        private static void InitTypesIconsList()
        {
            TypesIconsList.Add("_page");
            TypesIconsList.Add("aac");
            TypesIconsList.Add("ai");
            TypesIconsList.Add("aiff");
            TypesIconsList.Add("avi");
            TypesIconsList.Add("bmp");
            TypesIconsList.Add("c");
            TypesIconsList.Add("cpp");
            TypesIconsList.Add("css");
            TypesIconsList.Add("csv");
            TypesIconsList.Add("dat");
            TypesIconsList.Add("dmg");
            TypesIconsList.Add("doc");
            TypesIconsList.Add("dotx");
            TypesIconsList.Add("dwg");
            TypesIconsList.Add("dxf");
            TypesIconsList.Add("eps");
            TypesIconsList.Add("exe");
            TypesIconsList.Add("flv");
            TypesIconsList.Add("gif");
            TypesIconsList.Add("h");
            TypesIconsList.Add("hpp");
            TypesIconsList.Add("html");
            TypesIconsList.Add("icon");
            TypesIconsList.Add("ics");
            TypesIconsList.Add("iso");
            TypesIconsList.Add("java");
            TypesIconsList.Add("jpg");
            TypesIconsList.Add("js");
            TypesIconsList.Add("key");
            TypesIconsList.Add("less");
            TypesIconsList.Add("mid");
            TypesIconsList.Add("mp3");
            TypesIconsList.Add("mp4");
            TypesIconsList.Add("mpg");
            TypesIconsList.Add("odf");
            TypesIconsList.Add("ods");
            TypesIconsList.Add("odt");
            TypesIconsList.Add("otp");
            TypesIconsList.Add("ots");
            TypesIconsList.Add("ott");
            TypesIconsList.Add("pdf");
            TypesIconsList.Add("php");
            TypesIconsList.Add("png");
            TypesIconsList.Add("ppt");
            TypesIconsList.Add("psd");
            TypesIconsList.Add("py");
            TypesIconsList.Add("qt");
            TypesIconsList.Add("rar");
            TypesIconsList.Add("rb");
            TypesIconsList.Add("rtf");
            TypesIconsList.Add("sass");
            TypesIconsList.Add("screen");
            TypesIconsList.Add("scss");
            TypesIconsList.Add("sql");
            TypesIconsList.Add("tgs");
            TypesIconsList.Add("tgz");
            TypesIconsList.Add("tiff");
            TypesIconsList.Add("txt");
            TypesIconsList.Add("wav");
            TypesIconsList.Add("xls");
            TypesIconsList.Add("xlsx");
            TypesIconsList.Add("xml");
            TypesIconsList.Add("yml");
            TypesIconsList.Add("zip");
        }
    }
}