//================================================================\\
//  Simple application containing most functions for interfacing  \\
//      with Github API, including Updater and BugSquish.         \\
//================================================================\\
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Net
{
    public static class Updater
    {
        static byte[] _rawData =
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };

        public static readonly string BaseURL = "https://github.com/soopercool101/BrawlCrate/releases/download/";
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task UpdateCheck() { await UpdateCheck(false); }
        public static async Task UpdateCheck(bool Overwrite, string openFile = null, bool Documentation = false, bool Automatic = false)
        {
            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                    Console.WriteLine(s.Send("www.github.com").Status);

                // Initiate the github client.
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                // get Release
                IReadOnlyList<Release> releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                if (!Documentation)
                    releases = releases.Where(r => !r.Prerelease).ToList();
                else
                    releases = releases.Where(r => r.Prerelease).ToList();
                // Get Release Assets
                Release release = releases[0];
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];
                if(Asset == null)
                    return;
                
                if (Overwrite && !Documentation)
                {
                    //Find and close the BrawlCrate application that will be overwritten
                    TRY_AGAIN:
                    Process[] px = Process.GetProcessesByName("BrawlCrate");
                    Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                    Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                    if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                    {
                        DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                            "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                            "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                            "Select \"Cancel\" if you would like to wait to update until another time", releases[0].Name + " Update", MessageBoxButtons.YesNoCancel);
                        if (continueUpdate == DialogResult.Yes)
                        {
                            foreach (Process pNext in pToClose)
                                p.Kill();
                            goto TRY_AGAIN;
                        }
                        else if (continueUpdate == DialogResult.No)
                        {
                            goto TRY_AGAIN;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (p != null && p != default(Process) && Automatic)
                    {
                        p.Kill();
                    }
                    else if (p != null && p != default(Process) && p.CloseMainWindow())
                    {
                        p.WaitForExit();
                        p.Close();
                    }
                }

                // Check if we were passed in the overwrite paramter, and if not create a new folder to extract in.
                if (!Overwrite)
                {
                    Directory.CreateDirectory(AppPath + "/" + release.TagName);
                    AppPath += "/" + release.TagName;
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, releases[0].Name, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe"))
                    {
                        MessageBox.Show("Error downloading update");
                        return;
                    }


                    // Case 1: Wine (Batch files won't work, use old methodology) or documentation update
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0 || Documentation || !Overwrite)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                            if (Documentation)
                            {
                                update.WaitForExit();
                                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                                MessageBox.Show("Documentation was successfully updated to " + ((releases[0].Name.StartsWith("BrawlCrate Documentation", StringComparison.OrdinalIgnoreCase) && releases[0].Name.Length > 26) ? releases[0].Name.Substring(25) : releases[0].Name) + (Automatic ? "\nThis documentation release:\n" + releases[0].Body : ""));
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                        File.Delete(AppPath + "/Update.bat");
                    using (var sw = new StreamWriter(AppPath + "/Update.bat"))
                    {
                        sw.WriteLine("CD /d " + AppPath);
                        sw.WriteLine("START /wait temp.exe -y");
                        sw.WriteLine("del temp.exe /s /f /q");
                        sw.Write("START BrawlCrate.exe");
                        if (openFile != null && openFile != "<null>")
                            sw.Write(" " + openFile);
                    }
                    Process updateBat = Process.Start(AppPath + "/Update.bat");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task CheckUpdates(string releaseTag, string openFile, bool manual = true, bool checkDocumentation = false, bool automatic = false)
        {
            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            string docVer = null;
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "InternalDocumentation"))
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "InternalDocumentation" + '\\' + "version.txt"))
                    {
                        docVer = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "InternalDocumentation" + '\\' + "version.txt")[0];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("ERROR: Documentation Version could not be found.");
                return;
            }
            try
            {
                var github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                IReadOnlyList<Release> AllReleases = null;
                AllReleases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                IReadOnlyList<Release> releases = null;
                try
                {
                    // Remove all pre-release versions from the list (Prerelease versions are exclusively documentation updates)
                    releases = AllReleases.Where(r => !r.Prerelease).ToList();
					
					if (releases[0].TagName == releaseTag && (!checkDocumentation || AllReleases[0].TagName == docVer))
                    {
                        if(manual)
                            MessageBox.Show("No updates found.");
                        return;
                    }
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    if(manual)
                        MessageBox.Show("Unable to connect to the internet.");
                    return;
                }

                if (releases != null &&
                    releases.Count > 0 &&
                    !String.Equals(releases[0].TagName, releaseTag, StringComparison.InvariantCulture) && //Make sure the most recent version is not this version
                    releases[0].Name.IndexOf("BrawlCrate v", StringComparison.InvariantCultureIgnoreCase) >= 0) //Make sure this is a BrawlCrate release
                {
                    int descriptionOffset = 0;
                    if (releases[0].Body.Length > 110 && releases[0].Body.Substring(releases[0].Body.Length - 109) == "\nAlso check out the Brawl Stage Compendium for info and research on Stage Modding: https://discord.gg/s7c8763")
                        descriptionOffset = 110;
                    if (automatic)
                    {
                        Task t = UpdateCheck(true, openFile, false, true);
                        t.Wait();
                        return;
                    }
                    DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\n\nThis release:\n\n" + releases[0].Body.Substring(0, releases[0].Body.Length - descriptionOffset) + "\n\nUpdate now?", "Update", MessageBoxButtons.YesNo);
                    if (UpdateResult == DialogResult.Yes)
                    {
                        DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "", MessageBoxButtons.YesNoCancel);
                        if (OverwriteResult != DialogResult.Cancel)
                        {
                            Task t = UpdateCheck(OverwriteResult == DialogResult.Yes, openFile);
                            t.Wait();
                        }
                    }
                    return;
                }
                else if (manual && !checkDocumentation)
                {
                    MessageBox.Show("No updates found.");
                    return;
                }
                if (checkDocumentation)
                {
                    if(docVer == null)
                        MessageBox.Show("Documentation Version could not be found. Will download the latest documentation.");
                    try
                    {
                        releases = AllReleases.ToList();

                        // Ensure that the latest update is, in fact, a documentation update
                        if (!releases[0].Prerelease && !releases[0].Name.Contains("Documentation"))
                        {
                            if(manual)
                                MessageBox.Show("No updates found.");
                            return;
                        }

                        // Only get pre-release versions, as they are the pipeline documentation updates will be sent with
                        releases = AllReleases.Where(r => r.Prerelease).ToList();
                    }
                    catch (System.Net.Http.HttpRequestException)
                    {
                        if (manual)
                            MessageBox.Show("Unable to connect to the internet.");
                        return;
                    }

                    if (releases != null &&
                        releases.Count > 0 &&
                        !String.Equals(releases[0].TagName, docVer, StringComparison.InvariantCulture) && //Make sure the most recent version is not this version
                        releases[0].Name.IndexOf("Documentation", StringComparison.InvariantCultureIgnoreCase) >= 0) //Make sure this is a Documentation release
                    {
                        int descriptionOffset = 0;
                        if (automatic)
                        {
                            Task t = UpdateCheck(true, openFile, true, true);
                            t.Wait();
                            return;
                        }
                        DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\n\nThis documentation release:\n\n" + releases[0].Body.Substring(0, releases[0].Body.Length - descriptionOffset) + "\n\nUpdate now?", "Update", MessageBoxButtons.YesNo);
                        if (UpdateResult == DialogResult.Yes)
                        {
                            Task t = UpdateCheck(true, openFile, true);
                            t.Wait();
                        }
                    }
                    else if (manual)
                        MessageBox.Show("No updates found.");
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                if (manual)
                    MessageBox.Show("Unable to connect to the internet.");
                return;
            }
            catch (Exception e)
            {
                if (manual)
                    MessageBox.Show(e.Message);
            }
        }

        public static async Task ForceDownloadRelease(string openFile)
        {
            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                    Console.WriteLine(s.Send("www.github.com").Status);

                // Initiate the github client.
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                // get Release
                IReadOnlyList<Release> releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                releases = releases.Where(r => !r.Prerelease).ToList();
                Release release = releases[0];
                // Get Release Assets
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];
                if (Asset == null)
                    return;

                //Find and close the BrawlCrate application that will be overwritten
                TRY_AGAIN:
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                {
                    DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                        "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                        "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                        "Select \"Cancel\" if you would like to wait to update until another time", releases[0].Name + " Update", MessageBoxButtons.YesNoCancel);
                    if (continueUpdate == DialogResult.Yes)
                    {
                        foreach (Process pNext in pToClose)
                            p.Kill();
                        goto TRY_AGAIN;
                    }
                    else if (continueUpdate == DialogResult.No)
                    {
                        goto TRY_AGAIN;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (p != null && p != default(Process))
                {
                    p.Kill();
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, releases[0].Name, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe"))
                    {
                        MessageBox.Show("Error downloading update");
                        return;
                    }
                    
                    // Case 1: Wine (Batch files won't work, use old methodology)
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                        File.Delete(AppPath + "/Update.bat");
                    using (var sw = new StreamWriter(AppPath + "/Update.bat"))
                    {
                        sw.WriteLine("CD /d " + AppPath);
                        sw.WriteLine("START /wait temp.exe -y");
                        sw.WriteLine("del temp.exe /s /f /q");
                        sw.Write("START BrawlCrate.exe " + openFile != null ? openFile : "<null>" + " -Stable");
                    }
                    Process updateBat = Process.Start(AppPath + "/Update.bat");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task CheckNightlyUpdate(bool manual, string openFile)
        {
            try
            {
                string oldDate = "";
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly"))
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly" + '\\' + "new"))
                    {
                        oldDate = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly" + '\\' + "new")[0];
                    }
                }

                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                var github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                var branch = await github.Repository.Branch.Get("soopercool101", "BrawlCrate", "brawlcrate-master");
                var result = await github.Repository.Commit.Get("soopercool101", "BrawlCrate", branch.Commit.Sha);
                var commitDate = result.Commit.Author.Date;
                string newDate = commitDate.ToUniversalTime().ToString("O");
                if (oldDate.Equals(newDate, StringComparison.OrdinalIgnoreCase))
                {
                    if (manual)
                        MessageBox.Show("No updates found.");
                    return;
                }
                await ForceDownloadNightly(openFile, result.Sha.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("ERROR: Current nightly version could not be found. Updating to the latest commit");
                await ForceDownloadNightly(openFile);
                return;
            }
        }

        public static async Task ForceDownloadNightly(string openFile, string commitID = null)
        {
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                    Console.WriteLine(s.Send("www.github.com").Status);

                //Find and close the BrawlCrate application that will be overwritten
                TRY_AGAIN:
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                {
                    DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                        "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                        "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                        "Select \"Cancel\" if you would like to wait to update until another time", "Nightly Update", MessageBoxButtons.YesNoCancel);
                    if (continueUpdate == DialogResult.Yes)
                    {
                        foreach (Process pNext in pToClose)
                            p.Kill();
                        goto TRY_AGAIN;
                    }
                    else if (continueUpdate == DialogResult.No)
                    {
                        goto TRY_AGAIN;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (p != null && p != default(Process))
                {
                    p.Kill();
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = "https://github.com/soopercool101/BrawlCrate/raw/brawlcrate-master/NightlyBuild/BrawlCrateNightly.exe";

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, commitID == null ? "BrawlCrate Nightly Build" : commitID, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe"))
                    {
                        MessageBox.Show("Error downloading update");
                        return;
                    }
                    DirectoryInfo nightlyDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly");
                    nightlyDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    string Filename = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly" + '\\' + "New";
                    string oldName = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly" + '\\' + "Old";
                    if (File.Exists(Filename))
                        File.Move(Filename, oldName);
                    await WriteNightlyTime();
                    // Case 1: Wine (Batch files won't work, use old methodology)
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                        File.Delete(AppPath + "/Update.bat");
                    using (var sw = new StreamWriter(AppPath + "/Update.bat"))
                    {
                        sw.WriteLine("CD /d " + AppPath);
                        sw.WriteLine("START /wait temp.exe -y");
                        sw.WriteLine("del temp.exe /s /f /q");
                        sw.Write("START BrawlCrate.exe " + openFile != null ? openFile : "<null>" + " -Nightly");
                    }
                    Process updateBat = Process.Start(AppPath + "/Update.bat");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        //public static async Task ForceDownloadDocumentation() { }

        // Used when building for releases
        public static async Task WriteNightlyTime()
        {
            try
            {
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                var github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                var branch = await github.Repository.Branch.Get("soopercool101", "BrawlCrate", "brawlcrate-master");
                var result = await github.Repository.Commit.Get("soopercool101", "BrawlCrate", branch.Commit.Sha);
                var commitDate = result.Commit.Author.Date;
                commitDate = commitDate.ToUniversalTime();
                DirectoryInfo nightlyDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly");
                nightlyDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                string Filename = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Nightly" + '\\' + "New";
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                using (var sw = new StreamWriter(Filename))
                {
                    sw.Write(commitDate.ToString("O"));
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }
    }

    public static class BugSquish
    {
        static byte[] _rawData = 
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };

        public static async Task CreateIssue(
            string TagName,
            string ExceptionMessage,
            string StackTrace,
            string Title,
            string Description)
        {
            try
            {
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                var github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                IReadOnlyList<Release> releases = null;
                IReadOnlyList<Issue> issues = null;
                try
                {
                    releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");

                    // Check if this is a known pre-release version
                    bool isPreRelease = releases.Any(r => r.Prerelease
                        && r.Name.IndexOf("BrawlCrate", StringComparison.InvariantCultureIgnoreCase) >= 0);

                    // If this is not a known pre-release version, remove all pre-release versions from the list
                    if (!isPreRelease)
                    {
                        releases = releases.Where(r => !r.Prerelease).ToList();
                    }

                    issues = await github.Issue.GetAllForRepository("BrawlCrate", "BrawlCrateIssues");
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                    return;
                }

                if (releases != null && releases.Count > 0 && releases[0].TagName != TagName)
                {
                    //This build's version tag does not match the latest release's tag on the repository.
                    //This bug may have been fixed by now. Tell the user to update to be allowed to submit bug reports.

                    DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\nYou cannot submit bug reports using an older version of the program.\nUpdate now?", "An update is available", MessageBoxButtons.YesNo);
                    if (UpdateResult == DialogResult.Yes)
                    {
                        DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "", MessageBoxButtons.YesNoCancel);
                        if (OverwriteResult != DialogResult.Cancel)
                        {
                            Task t = Updater.UpdateCheck(OverwriteResult == DialogResult.Yes);
                            t.Wait();
                        }
                    }
                }
                else
                {
                    bool found = false;
                    if (issues != null && !String.IsNullOrEmpty(StackTrace))
                        foreach (Issue i in issues)
                            if (i.State == ItemState.Open)
                            {
                                string desc = i.Body;
                                if (desc.Contains(StackTrace) && 
                                    desc.Contains(ExceptionMessage) && 
                                    desc.Contains(TagName))
                                {
                                    found = true;
                                    IssueUpdate update = i.ToUpdate();

                                    update.Body =
                                        Title +
                                        Environment.NewLine +
                                        Description +
                                        Environment.NewLine +
                                        Environment.NewLine +
                                        i.Body;

                                    Issue x = await github.Issue.Update("BrawlCrate", "BrawlCrateIssues", i.Number, update);
                                }
                            }
                    
                    if (!found)
                    {
                        NewIssue issue = new NewIssue(Title)
                        {
                            Body =
                            Description +
                            Environment.NewLine +
                            Environment.NewLine +
                            TagName +
                            Environment.NewLine +
                            ExceptionMessage +
                            Environment.NewLine +
                            StackTrace
                        };
                        Issue x = await github.Issue.Create("BrawlCrate", "BrawlCrateIssues", issue);
                    }
                }
            }
            catch
            {
                MessageBox.Show("The application was unable to retrieve permission to send this issue.");
            }
        }
    }

    class Program
    {
        const string Usage = @"Usage: -n = New Folder";

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            System.Windows.Forms.Application.EnableVisualStyles();
            
            //Prevent crash that occurs when this dll is not present
            if (!File.Exists(System.Windows.Forms.Application.StartupPath + "/Octokit.dll"))
            {
                MessageBox.Show("Unable to find Octokit.dll.");
                return;
            }

            bool somethingDone = false;

            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-r": //overwrite
                        somethingDone = true;
                        Task t = Updater.UpdateCheck(true);
                        t.Wait();
                        break;
                    case "-n": // Update in new folder
                        somethingDone = true;
                        Task t1 = Updater.UpdateCheck(false);
                        t1.Wait();
                        break;
                    case "-bu": //BrawlCrate update call
                        somethingDone = true;
                        Task t2 = Updater.CheckUpdates(args[1], args[5], args[2] != "0", args[3] != "0", args[4] != "0");
                        t2.Wait();
                        break;
                    case "-bi": //BrawlCrate issue call
                        somethingDone = true;
                        Task t3 = BugSquish.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
                        t3.Wait();
                        break;
                    case "-bcommitTime": //Called on build to ensure time is saved
                        somethingDone = true;
                        Task t4 = Updater.WriteNightlyTime();
                        t4.Wait();
                        break;
                }
            }
            else if (args.Length == 0)
            {
                somethingDone = true;
                Task t = Updater.UpdateCheck(true);
                t.Wait();
            }

            if (!somethingDone)
                Console.WriteLine(Usage);
        }
    }
}