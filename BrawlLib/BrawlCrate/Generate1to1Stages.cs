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
        public static uint randnumgen = 0;

        public static string TmpDirGen()
        {
            string tmp = AppDomain.CurrentDomain.BaseDirectory;
            if (!tmp.EndsWith("\\"))
                tmp += '\\';
            tmp += "tmp";
            return tmp;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                              vBrawl 1:1s                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string vbrawlstgbattlefieldDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbBF.pac";
        public static bool vBrawlSTGBATTLEFIELD()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            vbrawlstgbattlefieldDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbBF.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/vBrawl/STGBATTLEFIELD.pac",
                                    @vbrawlstgbattlefieldDirectory);
            }
            return File.Exists(vbrawlstgbattlefieldDirectory);
        }
        public static string vbrawlstgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbFD.pac";
        public static bool vBrawlSTGFINAL()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            vbrawlstgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbFD.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/vBrawl/STGFINAL.pac",
                                    @vbrawlstgfinalDirectory);
            }
            return File.Exists(vbrawlstgfinalDirectory);
        }
        public static string vbrawlstgvillageDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbSV.pac";
        public static bool vBrawlSTGVILLAGE()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            vbrawlstgvillageDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbSV.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/vBrawl/STGVILLAGE.pac",
                                    @vbrawlstgvillageDirectory);
            }
            return File.Exists(vbrawlstgvillageDirectory);
        }
        public static string vbrawlstgvillagenvDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbSVn.pac";
        public static bool vBrawlSTGVILLAGE_NV()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            vbrawlstgvillagenvDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "vbSVn.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/vBrawl/STGVILLAGE_nv.pac",
                                    @vbrawlstgvillagenvDirectory);
            }
            return File.Exists(vbrawlstgvillagenvDirectory);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                              PROJECT M 1:1s                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string pm36stgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36FD.pac";
        public static bool PM36STGFINAL()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgfinalDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36FD.pac";
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
        
        public static string pm36stgbattlefieldDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36BF.pac";
        public static bool PM36STGBATTLEFIELD()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgbattlefieldDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36BF.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGBATTLEFIELD.pac",
                                    @pm36stgbattlefieldDirectory);
            }
            return File.Exists(pm36stgbattlefieldDirectory);
        }

        public static string pm36stgvillageDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36SV.pac";
        public static bool PM36STGVILLAGE()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgvillageDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36SV.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGVILLAGE.pac",
                                    @pm36stgvillageDirectory);
            }
            return File.Exists(pm36stgvillageDirectory);
        }
        public static string pm36stgvillagenvDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36SVn.pac";
        public static bool PM36STGVILLAGE_NV()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgvillagenvDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36SVn.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGVILLAGE_nv.pac",
                                    @pm36stgvillagenvDirectory);
            }
            return File.Exists(pm36stgvillagenvDirectory);
        }
        
        public static string pm36stgdolpicDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36DS.pac";
        public static bool PM36STGDOLPIC()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgdolpicDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36DS.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGDOLPIC.pac",
                                    @pm36stgdolpicDirectory);
            }
            return File.Exists(pm36stgdolpicDirectory);
        }

        public static string pm36stgfamicomDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36FoD.pac";
        public static bool PM36STGFAMICOM()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgfamicomDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36FoD.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGFAMICOM.pac",
                                    @pm36stgfamicomDirectory);
            }
            return File.Exists(pm36stgfamicomDirectory);
        }

        public static string pm36stgmadeinDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36WL.pac";
        public static bool PM36STGMADEIN()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgmadeinDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36WL.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGMADEIN.pac",
                                    @pm36stgmadeinDirectory);
            }
            return File.Exists(pm36stgmadeinDirectory);
        }

        public static string pm36stggreenhillDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36GHZ.pac";
        public static bool PM36STGGREENHILL()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stggreenhillDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36GHZ.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGGREENHILL.pac",
                                    @pm36stggreenhillDirectory);
            }
            return File.Exists(pm36stggreenhillDirectory);
        }

        public static string pm36stgdxpstadiumDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36PS2.pac";
        public static bool PM36STGDXPSTADIUM()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            pm36stgdxpstadiumDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36PS2.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Project%20M/3.6/STGDXPSTADIUM.pac",
                                    @pm36stgdxpstadiumDirectory);
            }
            return File.Exists(pm36stgdxpstadiumDirectory);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                              CUSTOM 1:1s                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static string skySanctDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36Sky.pac";
        public static bool STGSKYSANCTUARY()
        {
            Random rand = new Random();
            byte[] buf = new byte[4];
            rand.NextBytes(buf);
            randnumgen = BitConverter.ToUInt32(buf, 0);
            skySanctDirectory = tmpDirectory + '\\' + randnumgen.ToString("X8").Substring(2, 6) + "M36Sky.pac";
            DirectoryInfo tmp = Directory.CreateDirectory(tmpDirectory);
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // Use TLS 1.2, used by GitHub
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/soopercool101/Stage-Templates-1to1s/raw/master/Custom/STGSKYSANCTUARY.pac",
                                    @skySanctDirectory);
            }
            return File.Exists(skySanctDirectory);
        }

        public static void clearTmpDir(string directory)
        {
            Directory.CreateDirectory(directory);
            DirectoryInfo tmpdir = new DirectoryInfo(directory);
            foreach (DirectoryInfo dir in tmpdir.GetDirectories())
                clearTmpDir(dir.FullName);
            foreach (FileInfo file in tmpdir.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (System.IO.IOException x) {  }
            }

            try
            {
                tmpdir.Delete();
            }
            catch (System.IO.IOException x) { }
        }
    }
}
