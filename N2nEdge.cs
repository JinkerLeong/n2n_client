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
        private static Process targetn2n;
        public string communityName { get; set; }
        public string communityPassword { get; set; }
        public string virtualIp { get; set; }
        public bool showConsole { get; set; }
        public string serverIp { get; set; }

        public string n2nEdgePath { get; private set; }

        private bool Running { get; set; }
        public bool havePassword { get; internal set; }

        /// <summary>
        /// get empty N2nEdge
        /// </summary>
        /// <returns></returns>
        public static N2nEdge getInstance(string edgeFilePath)
        {
            var edge = new N2nEdge();
            edge.n2nEdgePath = edgeFilePath;
            return edge;
        }

        private N2nEdge()
        {
            this.showConsole = this.havePassword = this.Running = false;
            this.virtualIp = this.serverIp = this.communityName = this.communityPassword = this.n2nEdgePath = "";
        }

        public async Task<bool> start()
        {
            return await Task.Run(() =>
            {
                string args = havePassword? 
                string.Format("-c {0} -k {1} -a {2} -l {3}", this.communityName, this.communityPassword, this.virtualIp, this.serverIp) :
                string.Format("-c {0} -a {1} -l {2}", this.communityName, this.virtualIp, this.serverIp);

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = n2nEdgePath;
                processStartInfo.Arguments = args;
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(n2nEdgePath);
                processStartInfo.CreateNoWindow = !showConsole;
                targetn2n = new Process();
                targetn2n.StartInfo = processStartInfo;
                Running = true;
                targetn2n.Start();
                targetn2n.WaitForExit();

                return Running;
            });
        }

        public void stop()
        {
            if (Running)
            {
                this.Running = false;
                targetn2n.Kill();
            }
        }
    }
}
