using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Entities;

namespace WpfApp1.Pages
{
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RegLogin.Text) || string.IsNullOrEmpty(RegPass.Password) ||
                string.IsNullOrEmpty(RegFio.Text) || string.IsNullOrEmpty(RegPassport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            try
            {
                using (var db = new Entities.BanIDBaseEntities())
                {
                    if (db.Users.Any(u => u.Login == RegLogin.Text))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return;
                    }

                    string personalInfo = $"ФИО: {RegFio.Text}; Паспорт: {RegPassport.Text}";
                    string encryptedStr = SecurityHelper.Encrypt(personalInfo);

                    var newUser = new Users
                    {
                        Login = RegLogin.Text,
                        Password = RegPass.Password,
                        Role = "User",
                        EncryptedData = encryptedStr
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("Регистрация успешно завершена! Теперь вы можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.Message}");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new LoginPage());
        }
    }
}