﻿using System.Windows;
using System.Windows.Input;
using t_project.Models;
using t_project.Views.Pages;

namespace t_project.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        #region Оформление окна
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }
        #endregion

        #region Навигация
        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            equipment.Content = new EquipmentPage();
        }

        private void Auditoriums_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new AuditoriumsPage());
        }

        private void DirectionsAndStatuses_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new DirectionStatusPage());
        }

        private void EquipmentType_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new TypePage());
        }

        private void EquipmentModels_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new ModelTypePage());
        }

        // НОВЫЙ ОБРАБОТЧИК ДЛЯ КНОПКИ "ПРОГРАММЫ"
        private void Programms_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new ProgrammsPage());
        }

        private void Programmers_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new ProgrammersPage());
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new InventoryPage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new UsersPage());
        }

        private void NetSettings_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new NetSettingsPage());
        }

        private void Materials_Click(object sender, RoutedEventArgs e)
        {
            equipment.Navigate(new MaterialsPage());
        }


        #endregion
    }
}