using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.BrawlCrate
{
    public class Generate1to1Stages
    {
        public static string tmpDirectory = TmpDirGen();

        public static string TmpDirGen()
        {
            string tmp = AppDomain.CurrentDomain.BaseDirectory;
            if (!tmp.EndsWith("\\"))
                tmp += '\\';
            tmp += "tmp";
            return tmp;
        }
        
        public static uint randnumgen = 0;

        public static string pm36stgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "STGPM36FINAL.pac";
        public static bool PM36STGFINAL()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "STGPM36FINAL.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGFINAL.pac",
                                    @pm36stgfinalDirectory);
            }
            return File.Exists(pm36stgfinalDirectory);
        }

        public static void clearTmpDir(string directory)
        {
            Directory.CreateDirectory(directory);
            DirectoryInfo tmpdir = new DirectoryInfo(directory);
            bool canDelete = true;
            foreach (DirectoryInfo dir in tmpdir.GetDirectories())
                clearTmpDir(dir.FullName);
            foreach (FileInfo file in tmpdir.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (System.IO.IOException x) { canDelete = false; }
            }

            try
            {
                tmpdir.Delete();
            }
            catch (System.IO.IOException x) { }
        }
    }
}
