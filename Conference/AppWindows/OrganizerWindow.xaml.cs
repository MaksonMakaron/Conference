using Conference.Classes;
using Conference.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conference.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для OrganizerWindow.xaml
    /// </summary>
    public partial class OrganizerWindow : Window
    {
        public OrganizerWindow()
        {
            InitializeComponent();
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная функция в разработке :(", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEvents_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная функция в разработке :(", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPlayers_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная функция в разработке :(", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnJuries_Click(object sender, RoutedEventArgs e)
        {
            RegestrationJuryAndModeratorWindow regestration = new RegestrationJuryAndModeratorWindow();
            regestration.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string fullName;
            if (Authorization.CurrentUser.Gender.Code == "M")
            {
                fullName = "Mrs " + Authorization.CurrentUser.FullName;
            }
            else
            {
                fullName = "Ms " + Authorization.CurrentUser.FullName;
            }

            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime >= TimeSpan.Parse("06:00") && currentTime <= TimeSpan.Parse("10:00"))
            {
               TxbHeader.Text = "Доброе утро!\r\n" + fullName;
            }
            else if (currentTime >= TimeSpan.Parse("10:01") && currentTime <= TimeSpan.Parse("18:00"))
            {
                TxbHeader.Text = "Добрый день!\r\n" + fullName;
            }
            else if (currentTime >= TimeSpan.Parse("18:01") && currentTime <= TimeSpan.Parse("23:59"))
            {
                TxbHeader.Text = "Добрый вечер!\r\n" + fullName;
            }
            else
            {
                MessageBox.Show("Вход не возможен. Время работы с 6:00 до 00:00", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                BaseWindow baseWindow = new BaseWindow();
                baseWindow.Show();
            }
        }
    }
}
