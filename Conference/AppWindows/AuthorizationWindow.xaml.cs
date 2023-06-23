using Conference.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private int countSignIn = 0;

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BaseWindow baseWindow = new BaseWindow();
            baseWindow.Show();
            this.Close();
        }

        async private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TbId.Text))
            {
                errors.AppendLine("Введите ID");
            }

            if (string.IsNullOrWhiteSpace(PbPassword.Password))
            {
                errors.AppendLine("Введите пароль");
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            countSignIn++;
            try
            {
                var user = Authorization.SignIn(Convert.ToInt32(TbId.Text), PbPassword.Password);
                if (user != null)
                {
                    Authorization.CurrentUser = user;
                    switch (user.Role.Name)
                    {
                        case "Жюри":
                            JuryWindow juryWindow = new JuryWindow();
                            juryWindow.Show();
                            this.Close();
                            break;
                        case "Модератор":
                            ModeratorWindow moderatorWindow = new ModeratorWindow();
                            moderatorWindow.Show();
                            this.Close();
                            break;
                        case "Организатор":
                            OrganizerWindow organizerWindow = new OrganizerWindow();
                            organizerWindow.Show();
                            this.Close();
                            break;
                        case "Участник":
                            PlayerWindow playerWindow = new PlayerWindow();
                            playerWindow.Show();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show("Произошла ошибка, обратитесь к администратору", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);


            if (countSignIn >= 3)
            {
                MessageBox.Show("Вы привысили количество попыток для входа! Повторите позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnSignIn.IsEnabled = false;
                await Task.Delay(5000);
                BtnSignIn.IsEnabled = true;
                countSignIn = 0;
            }
        }

        private void TbId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
