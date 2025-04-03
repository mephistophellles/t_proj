using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using t_project.DB.Context;
using t_project.Models;
using t_project.Views.Windows;
using System.Linq;

namespace t_project.Views.Pages
{
    public partial class UsersPage : Page
    {
        private Collection<User> UsersList;
        private ICollectionView UsersView;
        private readonly UsersContext _db;

        public UsersPage()
        {
            InitializeComponent();
            _db = new UsersContext();
            DataContext = this;
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            _db.SaveChanges();
            UsersList = new Collection<User>(_db.Users.ToList());
            UsersView = CollectionViewSource.GetDefaultView(UsersList);
            UsersView.Filter = UsersFilter;
            UsersGrid.ItemsSource = UsersView;
            UsersGrid.Items.Refresh();
        }

        private bool UsersFilter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            if (item is User user)
            {
                string query = SearchTextBox.Text.ToLower();
                return (user.Name + user.LastName + user.Surname + user.Phone + user.Address + user.Login).ToLower().Contains(query);
            }
            return false;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersView?.Refresh();
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

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                Login = "ivanov",
                Password = "",
                Role = 0,
                Email = "ivanov@mail.ru",
                Name = "Иван",
                Surname = "Иванов",
                LastName = "Иванович",
                Phone = "",
                Address = ""
            };

            var editWindow = new EditUserWindow(newUser);
            if (editWindow.ShowDialog() == true)
            {
                _db.Users.Add(editWindow.user);
                UpdateUsers();
            }
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                var editWindow = new EditUserWindow(selectedUser);
                if (editWindow.ShowDialog() == true)
                {
                    _db.Entry(selectedUser).State = EntityState.Modified;
                    UpdateUsers();
                }
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show($"Удалить пользователя {selectedUser.Login} ({selectedUser.Surname} {selectedUser.Name} {selectedUser.LastName})?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _db.Users.Remove(selectedUser);
                    UpdateUsers();
                }
            }
        }
    }
}