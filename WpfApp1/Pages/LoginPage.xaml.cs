using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Entities;

namespace WpfApp1.Pages
{
    public partial class LoginPage : Page
    {
        private string _captchaCode;
        private Random _rnd = new Random();

        public LoginPage()
        {
            InitializeComponent();
            DoRefreshCaptcha();
        }

        private void DoRefreshCaptcha()
        {
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            _captchaCode = "";
            for (int i = 0; i < 5; i++)
            {
                _captchaCode += chars[_rnd.Next(chars.Length)];
            }
            CaptchaText.Text = _captchaCode;
        }

        private void GenerateCaptcha(object sender, RoutedEventArgs e)
        {
            DoRefreshCaptcha();
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            DoRefreshCaptcha();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginField.Text) || string.IsNullOrEmpty(PassField.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (CaptchaInput.Text.ToUpper() != _captchaCode)
            {
                MessageBox.Show("Неверная капча!");
                DoRefreshCaptcha();
                return;
            }

            try
            {
                using (var db = new BanIDBaseEntities())
                {
                    var user = db.Users.FirstOrDefault(u => u.Login == LoginField.Text && u.Password == PassField.Password);

                    if (user != null)
                    {
                        NavigationService.Navigate(new UsingCabinet(user.ID, user.Login, user.EncryptedData));
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!");
                        DoRefreshCaptcha(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка базы: {ex.Message}");
            }
        }

        private void ShowReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}