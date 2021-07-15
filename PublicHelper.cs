using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2n_client
{
    public static class PublicHelper
    {
        public static class Default
        {
            private static bool filesCheck(string folder, string[] filesName)
            {
                foreach (string file in filesName)
                {
                    if (!File.Exists(Path.Combine(folder, file)))
                        return false;
                }
                return true;
            }

            /// <summary>
            /// Check target folder and sub files exist or not.
            /// </summary>
            /// <param name="folderName"></param>
            /// <param name="filesName"></param>
            /// <returns></returns>
            public static bool CheckFolderAndFiles(string folderName, string[] filesName)
            {
                string folder = Path.Combine(Environment.CurrentDirectory, folderName);

                if (!Directory.Exists(folder) || !filesCheck(folderName, filesName))
                {
                    return false;
                }
                return true;
            }

            internal static KeyValuePair<string, string> GetAppInfo()
            {
                return new KeyValuePair<string, string>(
                     "Author: Jinker" + Environment.NewLine +
                "Running at platform: " + (Environment.Is64BitProcess ? "x64" : "x32") + Environment.NewLine +
                "Simple gui for n2n." + Environment.NewLine +
                "Project link: https://github.com/JinkerLeong/n2n_client" + "\n\n" +
                "Click OK to open project link, otherwise close about message.",

                     "https://github.com/JinkerLeong/n2n_client");
            }
        }
        
        public static class TapWindow
        {
            private static string tapFolderPath = Path.Combine(Environment.CurrentDirectory, "tap-window", Environment.Is64BitProcess ? "x64_files" : "x32_files");
            private static string tapFilePath = Path.Combine(tapFolderPath, "tapinstall.exe");

            private static Process getUninstallTapProcess()
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(tapFilePath);
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = tapFolderPath;
                startInfo.Arguments = "remove tap0901";
                Process process = new Process();
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                return process;
            }
            private static Process getInstallTapProcess()
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(tapFilePath);
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = tapFolderPath;
                startInfo.Arguments = "install OemVista.inf tap0901";
                Process process = new Process();
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                return process;
            }

            //private static Process test()
            //{
            //    Process process = new Process();
            //    process.StartInfo.FileName = tapFilePath;
            //    process.StartInfo.WorkingDirectory = tapFolderPath;
            //    process.StartInfo.Arguments = "install OemVista.inf tap0901x";
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.Verb = "runas";
            //    process.StartInfo.RedirectStandardOutput = true;
            //    process.StartInfo.RedirectStandardError = true;
            //    process.Start();
            //    //* Read the output (or the error)
            //    string output = process.StandardOutput.ReadToEnd();
            //    File.WriteAllText("output.txt", output);
            //    string err = process.StandardError.ReadToEnd();
            //    File.WriteAllText("error.txt", err);
            //    process.WaitForExit();

            //    return process;
            //}

            public static async Task<bool> reinstallTap()
            {
                return await Task.Run(() =>
                {
                    var uninstallProcess = getUninstallTapProcess();
                    var installProcess = getInstallTapProcess();
                    uninstallProcess.Start();
                    uninstallProcess.WaitForExit();

                    installProcess.Start();
                    installProcess.WaitForExit();

                    return true;
                });
            }

            public static async Task<bool> uninstallTap()
            {
                return await Task.Run(() =>
                {
                    var uninstallProcess = getUninstallTapProcess();
                    uninstallProcess.Start();
                    uninstallProcess.WaitForExit();

                    return true;
                });
            }

            public static bool Check()
            {
                string[] filesName =
                {
                    "OemVista.inf",
                    "tap0901.cat",
                    "tap0901.sys",
                    "tapinstall.exe"
                };

                return PublicHelper.Default.CheckFolderAndFiles(tapFolderPath, filesName);
            }
        }

        public static class N2nFile
        {
            public static string edgeFolderPath = Path.Combine(Environment.CurrentDirectory, "n2n", Environment.Is64BitProcess ? "x64_files" : "x32_files");
            public static string edgeFilePath = Path.Combine(edgeFolderPath, "edge.exe");
            public static bool Check()
            {
                string[] filesName =
                {
                    "edge.exe",
                };

                return PublicHelper.Default.CheckFolderAndFiles(edgeFolderPath, filesName);
            }
        }
    }
}
