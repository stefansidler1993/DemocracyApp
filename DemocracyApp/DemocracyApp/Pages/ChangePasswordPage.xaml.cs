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
    public partial class ChangePasswordPage : ContentPage
    {
        private UserPassword user;

        public ChangePasswordPage(UserPassword user)
        {
            InitializeComponent();
            this.user = user;

            saveButton.Clicked += SaveButton_Clicked;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentPasswordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a current password", "Acept");
                currentPasswordEntry.Focus();
                return;
            }

            if (!currentPasswordEntry.Text.Equals(this.user.CurrentPassword))
            {
                await DisplayAlert("Error", "The currect password is wrong", "Acept");
                confirmPasswordEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(newPasswordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a new current password", "Acept");
                newPasswordEntry.Focus();
                return;
            }

            if (!Utilities.IsValidPassword(newPasswordEntry.Text))
            {
                await DisplayAlert("Error", "The password must be contain minimum 8 " +
                    "characters at least 1 uppercase alphabet, 1 lowercase Alphabet, " +
                    "1 number and 1 special character", "Acept");
                newPasswordEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "You must enter a confirm password", "Acept");
                confirmPasswordEntry.Focus();
                return;
            }

            if (!newPasswordEntry.Text.Equals(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "The password and confirm does not match", "Acept");
                confirmPasswordEntry.Focus();
                return;
            }

            this.ChangePassword();
        }

        private async void ChangePassword()
        {
            waitActivityIndicator.IsRunning = true;

            var changePasswordPage = new ChangePasswordRequest
            {
                OldPassword = user.CurrentPassword,
                NewPassword = newPasswordEntry.Text,
            };

            try
            {
                var jsonRequest = JsonConvert.SerializeObject(changePasswordPage);
                var httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Utilities.localhost);
                var url = string.Format("/api/Users/ChangePassword/{0}", user.UserId);

                var response = await client.PutAsync(url, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    await DisplayAlert("Error", "Error changing password", "Acept");
                    return;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Acept");
                return;
            }

            waitActivityIndicator.IsRunning = false;
            await DisplayAlert("Ok", "Password changed ok", "Acept");
            await Navigation.PopAsync();
        }

    }
}
