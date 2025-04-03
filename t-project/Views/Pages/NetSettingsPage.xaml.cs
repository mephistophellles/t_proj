using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using t_project.DB.Context;
using t_project.Models;
using t_project.Views.Windows;
using Microsoft.EntityFrameworkCore;

namespace t_project.Views
{
    public partial class NetSettingsPage : Page
    {
        private Collection<NetSettings> NetSettingsList;
        private ICollectionView NetSettingsView;
        private readonly NetSettingsContext db;

        public NetSettingsPage()
        {
            InitializeComponent();
            db = new NetSettingsContext();
            DataContext = this;
            UpdateNetSettings();
        }

        private void UpdateNetSettings()
        {
            db.SaveChanges();
            NetSettingsList = new Collection<NetSettings>(db.NetSettings.ToList());
            NetSettingsView = CollectionViewSource.GetDefaultView(NetSettingsList);
            NetSettingsView.Filter = NetSettingsFilter;
            NetSettingsGrid.ItemsSource = NetSettingsView;
            NetSettingsGrid.Items.Refresh();
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            var newSettings = new NetSettings
            {
                IpAddress = "192.168.0.1",
                Mask = 24,
                BaseGate = "192.168.0.254",
                DnsServers = "8.8.8.8;8.8.4.4"
            };

            if (ValidateSettings(newSettings))
            {
                db.NetSettings.Add(newSettings);
                UpdateNetSettings();
            }
        }

        private bool ValidateSettings(NetSettings settings)
        {
            var ipRegex = new Regex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

            if (!ipRegex.IsMatch(settings.IpAddress))
            {
                MessageBox.Show("Некорректный IP-адрес!");
                return false;
            }

            if (settings.Mask < 0 || settings.Mask > 32)
            {
                MessageBox.Show("Маска должна быть в диапазоне 0-32!");
                return false;
            }

            return true;
        }

        private bool NetSettingsFilter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            if (item is NetSettings setting)
            {
                string query = SearchTextBox.Text.ToLower();
                return (setting.IpAddress + setting.Mask + setting.BaseGate + setting.DnsServers).ToLower().Contains(query);
            }
            return false;
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (NetSettingsGrid.SelectedItem is NetSettings selected)
            {
                var editWindow = new EditNetSettingsWindow(selected);
                if (editWindow.ShowDialog() == true)
                {
                    db.Entry(selected).State = EntityState.Modified;
                    UpdateNetSettings();
                }
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (NetSettingsGrid.SelectedItem is NetSettings selected)
            {
                db.NetSettings.Remove(selected);
                UpdateNetSettings();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NetSettingsView?.Refresh();
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                SearchPlaceholder.Visibility = Visibility.Hidden;
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                SearchPlaceholder.Visibility = Visibility.Visible;
        }
    }
}