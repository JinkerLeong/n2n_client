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
        private static string tapPath = Path.Combine(Environment.CurrentDirectory, "tap-window", "tapinstall.exe");

        private static Process getUninstallTapProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(tapPath);
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Path.GetDirectoryName(tapPath);
            startInfo.Arguments = "remove tap0901";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            return process;
        }
        private static Process getInstallTapProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(tapPath);
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Path.GetDirectoryName(tapPath);
            startInfo.Arguments = "install OemVista.inf tap0901";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            return process;
        }

        //private static Process test()
        //{
        //    Process process = new Process();
        //    process.StartInfo.FileName = tapPath;
        //    process.StartInfo.Arguments = "remove tap0901";
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
    }
}
