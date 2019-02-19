using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WpfCaliburn.Toramp;

namespace WpfCaliburn.ViewModels
{
    public class LoginViewModel : Screen
    {
        public string Login { get; set; }
        #if DEBUG
            = "demosute"; // sute@getnada.com : https://getnada.com/
        #endif

        public string Password { get; set; }
        #if DEBUG
            = "asdasd123";
        #endif
        

        public bool InProgress { get; private set; }

        private readonly TorampClient _client = new TorampClient();

        public async Task Authenticate(string login, string password)
        {
            try
            {
                await _client.Login(login, password);
                string series = await _client.GetChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось авторизоваться\r\n:{ex.Message}", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanAuthenticate(string login, string password)
        {
            return !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && !InProgress;
        }
    }
}