using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2n_client
{
    class ServerInformation
    {
        private static string configPath = Path.Combine(Environment.CurrentDirectory, "server.list");
        private static ServerInformation INSTANCE = new ServerInformation();

        public static ServerInformation getInstance() => INSTANCE;

        private List<string> serverips = new List<string>();

        private ServerInformation()
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close();
            }

            serverips = new List<string>(File.ReadAllLines(configPath));
        }

        private void writeConfig(List<string> sips)
        {
            File.Delete(configPath);
            File.WriteAllLines(configPath, serverips);
        }

        public void add(string ip)
        {
            serverips.Add(ip);
        }

        public void remove(int index)
        {
            serverips.RemoveAt(index);
        }

        public List<string> getSource()
        {
            var source = new List<string>(serverips);
            source.Insert(0, "Custom information");
            return source;
        }

        public void save()
        {
            writeConfig(serverips);
        }
    }
}
