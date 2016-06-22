using SkulApp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkulApp.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10));

            saveButton.Clicked += SaveButton_Clicked;

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userNameEntry.Text))
            {
                await DisplayAlert("Error", "You must enter an e-mail", "Acept");
                userNameEntry.Focus();
                return;
            }

            if (!Utilities.IsValidEmail(userNameEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a valid email", "Acept");
                userNameEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(firstNameEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a first name", "Acept");
                firstNameEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(lastNameEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a last name", "Acept");
                lastNameEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phoneEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a phone", "Acept");
                phoneEntry.Focus();
                return;
            }

            if (phoneEntry.Text.Length < 7 || phoneEntry.Text.Length > 20)
            {
                await DisplayAlert("Error", "The phone must be between 7 and 20 characters", "Acept");
                phoneEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(addressEntry.Text))
            {
                await DisplayAlert("Error", "You must enter an address", "Acept");
                addressEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a password", "Acept");
                passwordEntry.Focus();
                return;
            }

            if (!Utilities.IsValidPassword(passwordEntry.Text))
            {
                await DisplayAlert("Error", "The password must be contain minimum 8 " +
                    "characters at least 1 uppercase alphabet, 1 lowercase Alphabet, " +
                    "1 number and 1 special character", "Acept");
                passwordEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a confirm password", "Acept");
                confirmPasswordEntry.Focus();
                return;
            }

            if (!passwordEntry.Text.Equals(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "The password and confirm does not match", "Acept");
                confirmPasswordEntry.Focus();
                return;
            }

            this.RegisterUser();

        }

        private async void RegisterUser()
        {
            waitActivityIndicator.IsRunning = true;

            var userPasswordConfirmation = new UserPasswordConfirmation
            {
                ConfirmPassword = confirmPasswordEntry.Text,
                Password = passwordEntry.Text,
                Adress = addressEntry.Text,
                FirstName = firstNameEntry.Text,
                Grade = gradeEntry.Text,
                Group = groupEntry.Text,
                LastName = lastNameEntry.Text,
                Phone = phoneEntry.Text,
                UserName = userNameEntry.Text,
            };

            var result = string.Empty;

            try
            {
                var jsonRequest = JsonConvert.SerializeObject(userPasswordConfirmation);
                var httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Utilities.localhost);
                var url = "/api/Users";

                var response = await client.PostAsync(url, httpContent);
                result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    await DisplayAlert("Error", result, "Acept");
                    return;
                }

            }
            catch (Exception ex)
            {
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
