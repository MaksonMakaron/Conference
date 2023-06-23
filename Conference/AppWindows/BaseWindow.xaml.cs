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
    /// Логика взаимодействия для BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        public BaseWindow()
        {
            InitializeComponent();
            var directions = App.DataBase.Directions.ToList();
            directions.Insert(0, new DataModel.Direction
            {
                Name = "Все направления"
            });
            CmbDirection.ItemsSource = directions;
            CmbDirection.SelectedIndex= 0;
            UpdateListEvents();
        }

        private void UpdateListEvents()
        {
            var eventsList = App.DataBase.Events.ToList();
            
            if(CmbDirection.SelectedIndex > 0)
            {
                eventsList = eventsList.Where(p => p.DirectionId == (CmbDirection.SelectedItem as Direction).Id).ToList();
            }

            if (DpDate.SelectedDate != null)
            {
                eventsList = eventsList.Where(p => p.Date == ((DateTime)DpDate.SelectedDate).Date).ToList();
            }

            if (eventsList.Count == 0)
            {
                MessageBox.Show("Отсутствуют результаты поиска", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            LVEvents.ItemsSource = eventsList.ToList();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private void CmbDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListEvents();
        }

        private void DpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListEvents();
        }
    }
}
