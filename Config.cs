using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace n2n_client
{
    class Config
    {
        private static string configPath = Path.Combine(Environment.CurrentDirectory, "config.ini");
        private static Config INSTANCE = new Config();

        public static Config getInstance() => INSTANCE;

        public string virtualIp { get; set; }
        public string communityName { get; set; }
        public string communityPassword { get; set; }

        private Config()
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close();
                writeConfig();
                PublicHelper.reinstallTap();
            }
            using (StreamReader sr = new StreamReader(configPath))
            {
                this.virtualIp = sr.ReadLine().Replace("virtualIp=","");
                this.communityName = sr.ReadLine().Replace("communityName=", "");
                this.communityPassword = sr.ReadLine().Replace("communityPassword=", "");
            }
        }

        private void writeConfig(string vip = "192.168.66.1", string cn = "main", string cp = "")
        {
            using (StreamWriter sw = new StreamWriter(configPath, false))
            {
                sw.WriteLine(string.Format("virtualIp={0}", vip));
                sw.WriteLine(string.Format("communityName={0}", cn));
                sw.WriteLine(string.Format("communityPassword={0}", cp));
            }
        }

        public void save(string vip = "", string cn = "", string cp = "")
        {
            writeConfig(vip, cn, cp);
        }
    }
}
