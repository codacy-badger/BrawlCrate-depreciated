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
        public static string tmpDirectory = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tmp";
        public static string stageDirectory = tmpDirectory + '\\' + "Stages";

        public static string pm36stgfinalDirectory = stageDirectory + '\\' + "STGPM36FINAL.pac";
        public static bool PM36STGFINAL()
        {
            Directory.CreateDirectory(tmpDirectory);
            Directory.CreateDirectory(stageDirectory);
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;

            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGFINAL.pac",
                                    @pm36stgfinalDirectory);
            }
            return File.Exists(pm36stgfinalDirectory);
        }
    }
}
