using SkulApp.Classes;
using SkulApp.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkulApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            this.Padding = Device.OnPlatform(
                new Thickness(10, 20, 10, 10),
                new Thickness(10),
                new Thickness(10));

            loginButton.Clicked += this.LoginButton_Clicked;
            registerButton.Clicked += RegisterButton_Clicked;

        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }


        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailEntry.Text))
            {
                await DisplayAlert("Error", "You must enter an email", "Acept");
                emailEntry.Focus();
                return;
            }

            if (!Utilities.IsValidEmail(emailEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a valid email", "Acept");
                emailEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a password", "Acept");
                passwordEntry.Focus();
                return;
            }

            this.Login();
        }

        private async void Login()
        {
            waitActivityIndicator.IsRunning = true;

            var loginRequest = new LoginRequest
            {
                email = emailEntry.Text,
                password = passwordEntry.Text,
            };

            var result = string.Empty;

            try
            {
                var jsonRequest = JsonConvert.SerializeObject(loginRequest);
                var httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Utilities.localhost);
                var url = "/api/users/login";

                var response = await client.PostAsync(url, httpContent);
                result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    await DisplayAlert("Error", "Wrong user or password ", "Acept");
                   // await DisplayAlert("Error", result, "Acept");
                    //passwordEntry.Text = string.Empty;
                    //passwordEntry.Focus();
                    return;
                }


            }
            catch (Exception ex)
            {
                waitActivityIndicator.IsRunning = false;
                await DisplayAlert("Error", ex.Message, "Acept");
                return;
            }

            waitActivityIndicator.IsRunning = false;
            var user = JsonConvert.DeserializeObject<User>(result);
            var userPassword = new UserPassword
            {
                CurrentPassword = passwordEntry.Text,
                Adress = user.Adress,
                FirstName = user.FirstName,
                Grade = user.Grade,
                Group = user.Group,
                LastName = user.LastName,
                Phone = user.Phone,
                UserId = user.UserId,
                UserName = user.UserName,
            };

            await Navigation.PushAsync(new HomePage(userPassword));

        }
    }
}
