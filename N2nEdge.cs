using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2n_client
{
    class N2nEdge
    {
        private static string edgePath = Path.Combine(Environment.CurrentDirectory, "n2n", "edge.exe");

        private static Process targetn2n = null;
        public string communityName { get; set; }
        public string communityPassword { get; set; }
        public string virtualIp { get; set; }
        public bool showConsole { get; set; }
        public string serverIp { get; set; }

        public static N2nEdge getInstance(string _virtualIp, string _serverIp, string _communityName, string password)
        {
            return new N2nEdge(_virtualIp, _serverIp, _communityName, password);
        }

        public N2nEdge(string _virtualIp, string _serverIp, string _communityName, string _password)
        {
            this.showConsole = false;
            this.virtualIp = _virtualIp;
            this.serverIp = _serverIp;
            this.communityName = _communityName;
            this.communityPassword = _password;
        }
        public void start()
        {
            string args = string.Format("-c {0} -k {1} -a {2} -l {3}", this.communityName, this.communityPassword, this.virtualIp, this.serverIp);
            ProcessStartInfo processStartInfo = new ProcessStartInfo(edgePath);
            processStartInfo.Arguments = args;
            processStartInfo.CreateNoWindow = !showConsole;
            targetn2n = Process.Start(processStartInfo);
        }

        public void stop()
        {
            targetn2n.Kill();
        }
    }
}
