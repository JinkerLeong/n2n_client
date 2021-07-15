using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace n2n_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerInformation serverInformation = ServerInformation.getInstance();
        Config config = Config.getInstance();
        N2nEdge defaultN2n = null;

        public MainWindow()
        {
            InitializeComponent();

            if (!CheckAll())
            {
                Environment.Exit(Environment.ExitCode);     
            }
        }

        private bool CheckAll()
        {
            if (!PublicHelper.N2nFile.Check())
            {
                MessageBox.Show("n2n files is missing", "Cant continue run");
                return false;
            }
            if (!PublicHelper.TapWindow.Check())
            {
                MessageBox.Show("tap-window files is missing!", "Cant continue run");
                return false;
            }

            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var source = serverInformation.getSource();
            foreach (var item in source)
            {
                savedInfos_ComboBox.Items.Add(item);
            }
            savedInfos_ComboBox.SelectedIndex = savedInfos_ComboBox.Items.Count - 1;

            virtualIp_TextBox.Text = config.virtualIp;
            communityName_TextBox.Text = config.communityName;
            communityPassword_TextBox.Text = config.communityPassword;
        }

        private void saveCustomIpBtn_Click(object sender, RoutedEventArgs e)
        {
            serverInformation.add(serverIp_TextBox.Text);
            serverInformation.save();
            savedInfos_ComboBox.Items.Add(serverIp_TextBox.Text);
            MessageBox.Show("Success add information.", "Result");
        }

        private void removeCustomIpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (savedInfos_ComboBox.SelectedIndex > 0)
            {
                int index = savedInfos_ComboBox.SelectedIndex;
                savedInfos_ComboBox.SelectedIndex = 0; // comboBox items changed will be error because target index = -1
                serverInformation.remove(index - 1);
                serverInformation.save();
                int asd = savedInfos_ComboBox.Items.Count;
                savedInfos_ComboBox.Items.RemoveAt(index);
                savedInfos_ComboBox.SelectedIndex = 0;
            }
        }

        private bool MatchAll()
        {
            Match result = result = Regex.Match(serverIp_TextBox.Text, @"([a-zA-Z0-9]|\.)+\:\d+");
            if (!result.Success || result.Length != serverIp_TextBox.Text.Length)
            {
                MessageBox.Show("<Server ip and port> contains illegal characters.", "Failed start");

                return false;
            }

            result = Regex.Match(virtualIp_TextBox.Text, @"\d+\.\d+\.\d+\.\d+");
            if (!result.Success || result.Length != virtualIp_TextBox.Text.Length)
            {
                MessageBox.Show("<Virtual local ip> contains illegal characters.", "Failed start");

                return false;
            }

            result = Regex.Match(communityName_TextBox.Text, @"\s");
            if (result.Success || string.IsNullOrWhiteSpace(communityName_TextBox.Text))
            {
                MessageBox.Show("<Community name> contains illegal characters.", "Failed start");

                return false;
            }

            result = Regex.Match(communityPassword_TextBox.Text, @"\s");
            if (result.Success)
            {
                MessageBox.Show("<Community password> contains illegal characters.", "Failed start");

                return false;
            }

            return true;
        }

        private void getedge()
        {
            if (defaultN2n == null)
            {
                defaultN2n = N2nEdge.getInstance(
                    virtualIp_TextBox.Text,
                    serverIp_TextBox.Text,
                    communityName_TextBox.Text,
                    communityPassword_TextBox.Text == "" ? "defaultpassword" : communityPassword_TextBox.Text,
                    PublicHelper.N2nFile.edgeFilePath);
                defaultN2n.showConsole = showConsole_CheckBox.IsChecked.Value;
            }
        }

        private async void startN2nBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!MatchAll()) return;

            getedge();

            if (startN2nBtn.Content.ToString() == "Start")
            {
                defaultN2n.virtualIp = virtualIp_TextBox.Text;
                defaultN2n.serverIp = serverIp_TextBox.Text;
                defaultN2n.communityName = communityName_TextBox.Text;
                defaultN2n.communityPassword = communityPassword_TextBox.Text == "" ? "defaultpassword" : communityPassword_TextBox.Text;
                defaultN2n.showConsole = showConsole_CheckBox.IsChecked.Value;

                config.save(virtualIp_TextBox.Text, communityName_TextBox.Text, communityPassword_TextBox.Text);
                startN2nBtn.Content = "Stop";

                bool end = await defaultN2n.start();
                if (end)
                    startN2nBtn.Content = "Start";
            }
            else
            {
                defaultN2n.stop();
                startN2nBtn.Content = "Start";
            }
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> info = PublicHelper.Default.GetAppInfo();
            if (MessageBox.Show(info.Key, "n2n client v1.1", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C start {info.Value}",
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };
                System.Diagnostics.Process.Start(psi);
            }
        }

        private void savedInfos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (savedInfos_ComboBox.SelectedIndex == 0)
            {
                serverIp_TextBox.Text = "";
                saveCustomIpBtn.IsEnabled = true;
                serverIp_TextBox.IsEnabled = true;
                removeCustomIpBtn.IsEnabled = false;
            }
            else
            {
                serverIp_TextBox.Text = savedInfos_ComboBox.Items[savedInfos_ComboBox.SelectedIndex].ToString();
                saveCustomIpBtn.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;
                removeCustomIpBtn.IsEnabled = true;
            }
        }

        private async void reinstallTapBtn_Click(object sender, RoutedEventArgs e)
        {
            reinstallTapBtn.Content = "Reinstalling......";
            reinstallTapBtn.IsEnabled = uninstallTapBtn.IsEnabled = false;

            bool result = await PublicHelper.TapWindow.reinstallTap();

            reinstallTapBtn.Content = "Reinstall tap driver";
            reinstallTapBtn.IsEnabled = uninstallTapBtn.IsEnabled = true;
        }

        private async void uninstallTapBtn_Click(object sender, RoutedEventArgs e)
        {
            uninstallTapBtn.Content = "Uninstalling......";
            reinstallTapBtn.IsEnabled = uninstallTapBtn.IsEnabled = false;

            bool result = await PublicHelper.TapWindow.uninstallTap();

            uninstallTapBtn.Content = "Uninstall tap driver";
            reinstallTapBtn.IsEnabled = uninstallTapBtn.IsEnabled = true;
        }
    }
}
