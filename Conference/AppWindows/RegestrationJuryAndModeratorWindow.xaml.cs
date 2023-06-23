using Conference.Classes;
using Conference.DataModel;
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
using System.Windows.Shapes;

namespace Conference.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для RegestrationJuryAndModeratorWindow.xaml
    /// </summary>
    public partial class RegestrationJuryAndModeratorWindow : Window
    {
        public RegestrationJuryAndModeratorWindow()
        {
            InitializeComponent();
            TbId.Text = (App.DataBase.Users.Max(p => p.Id) + 1).ToString();
            CmbGender.ItemsSource = App.DataBase.Genders.ToList();
            CmbRole.ItemsSource = App.DataBase.Roles.Where(p=>p.Name == "Жюри" || p.Name == "Модератор").ToList();
            CmbDirection.ItemsSource = App.DataBase.Directions.ToList();
            CmbActivities.ItemsSource = App.DataBase.Activities.ToList();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrganizerWindow organizerWindow = new OrganizerWindow();
            organizerWindow.Show();
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TbFullName.Text))
            {
                errors.AppendLine("Введите ФИО");
            }
            if (CmbGender.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите пол");
            }
            if (CmbRole.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите роль");
            }
            if (string.IsNullOrWhiteSpace(TbEmail.Text))
            {
                errors.AppendLine("Введите email");
            }
            else
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(TbEmail.Text))
                {
                    errors.AppendLine("Некорректный email");
                }
            }
            if (string.IsNullOrWhiteSpace(TbPhone.Text))
            {
                errors.AppendLine("Введите телефон");
            }
            if (CmbDirection.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите направление");
            }
            if (CbIsEvent.IsChecked == true)
            {
                if (CmbActivities.SelectedIndex == -1)
                {
                    errors.AppendLine("Выберите мероприятие");
                }
            }
            string password = PbPassword.Password;
            if (CbIsVisibilityPassword.IsChecked == true)
            {
                password = TbPassword.Text;
            }

            string passwordRepeat = PbPasswordRepeat.Password;
            if (CbIsVisibilityPassword.IsChecked == true)
            {
                passwordRepeat = TbPasswordRepeat.Text;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.AppendLine("Заполните поля для паролей");
            }
            else
            {
                if (!Authorization.ValidatePassword(password, passwordRepeat))
                {
                    errors.AppendLine("Проверьте требования к паролю");
                }
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<Activity> activities = new List<Activity>();
            activities.Add(CmbActivities.SelectedItem as Activity);
            MessageBox.Show(activities.Count.ToString());

            var newUser = new User
            {
                FullName = TbFullName.Text,
                GenderId = (CmbGender.SelectedItem as Gender).Code,
                RoleId = (CmbRole.SelectedItem as Role).Id,
                Email = TbEmail.Text,
                Phone = TbPhone.Text,
                Password = password,
                Activities = activities,
            };

            try
            {
                App.DataBase.Users.Add(newUser);
                App.DataBase.SaveChanges();
                MessageBox.Show("Регистрация успешна!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                OrganizerWindow organizerWindow = new OrganizerWindow();
                organizerWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void CbIsEvent_Checked(object sender, RoutedEventArgs e)
        {
            CmbActivities.IsEnabled = true;
        }

        private void CbIsEvent_Unchecked(object sender, RoutedEventArgs e)
        {
            CmbActivities.IsEnabled = false;
        }

        private void CbIsVisibilityPassword_Checked(object sender, RoutedEventArgs e)
        {
            TbPassword.Visibility = Visibility.Visible;
            TbPassword.Text = PbPassword.Password;
            PbPassword.Visibility = Visibility.Collapsed;

            TbPasswordRepeat.Visibility = Visibility.Visible;
            TbPasswordRepeat.Text = PbPasswordRepeat.Password;
            PbPasswordRepeat.Visibility = Visibility.Collapsed;
        }

        private void CbIsVisibilityPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            TbPassword.Visibility = Visibility.Collapsed;
            PbPassword.Password = TbPassword.Text;
            PbPassword.Visibility = Visibility.Visible;

            TbPasswordRepeat.Visibility = Visibility.Collapsed;
            PbPasswordRepeat.Password = TbPasswordRepeat.Text;
            PbPasswordRepeat.Visibility = Visibility.Visible;
        }

        private void TbFullName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0))
            {
                e.Handled = true; 
            }
        }

        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        
    }
}
