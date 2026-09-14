using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Entities; 

namespace WpfApp1.Pages
{
    public partial class UsingCabinet : Page
    {
        private int _currentUserId;

        public UsingCabinet(int userId, string login, string encryptedData)
        {
            InitializeComponent();
            _currentUserId = userId;

            WelcomeLabel.Text = $"Личный кабинет: {login}";

            try
            {
                UserDataLabel.Text = SecurityHelper.Decrypt(encryptedData);
            }
            catch { UserDataLabel.Text = "Ошибка при расшифровке данных."; }

            LoadAccountInfo();
        }
        private void LoadAccountInfo()
        {
            try
            {
                using (var db = new BanIDBaseEntities())
                {
                    var account = db.Accounts.FirstOrDefault(a => a.UserID == _currentUserId);

                    if (account != null)
                    {
                        AccNumberText.Text = $"Счет: №{account.AccountNumber}";
                        AccBalanceText.Text = $"Доступный остаток: {account.Balance:N2} {account.Currency}";
                    }
                    else
                    {
                        AccNumberText.Text = "Счет не найден";
                        AccBalanceText.Text = "0,00 руб.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки счета: " + ex.Message);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == true)
                {
                    pd.PrintVisual(PrintArea, "Выписка BanID");
                    MessageBox.Show("Документ отправлен на печать.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка печати: " + ex.Message);
            }
        }

        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите действие из списка!");
                return;
            }

            try
            {
                using (var db = new BanIDBaseEntities())
                {
                    string selectedAction = (RequestCombo.SelectedItem as ComboBoxItem).Content.ToString();

                    var newRequest = new Requests
                    {
                        UserID = _currentUserId,
                        RequestType = selectedAction,
                        Status = "Новая",
                        CreatedAt = DateTime.Now
                    };

                    db.Requests.Add(newRequest);
                    db.SaveChanges();

                    MessageBox.Show($"Заявка на '{selectedAction}' успешно создана!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании заявки: " + ex.Message);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}