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
    public partial class MySettingsPage : ContentPage
    {

        private UserPassword user;

        public MySettingsPage(UserPassword user)
        {
            InitializeComponent();

            this.user = user;

            this.Padding = Device.OnPlatform(
            new Thickness(10, 20, 10, 10),
            new Thickness(10),
            new Thickness(10));
            
            userNameEntry.Text = user.UserName;
            firstNameEntry.Text = user.FirstName;
            lastNameEntry.Text = user.LastName;
            phoneEntry.Text = user.Phone;
            addressEntry.Text = user.Adress;
            gradeEntry.Text = user.Grade;
            groupEntry.Text = user.Group;

            saveButton.Clicked += SaveButton_Clicked;
            changePasswordButton.Clicked += changePasswordButton_Clicked;

        }

        private async void changePasswordButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage(this.user));
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

            if (string.IsNullOrEmpty(addressEntry.Text))
            {
                await DisplayAlert("Error", "You must enter an address", "Acept");
                addressEntry.Focus();
                return;
            }

            this.UpdateUser();
        }

        private async void UpdateUser()
        {
            waitActivityIndicator.IsRunning = true;

            user.Adress = addressEntry.Text;
            user.FirstName = firstNameEntry.Text;
            user.Grade = gradeEntry.Text;
            user.Group = groupEntry.Text;
            user.LastName = lastNameEntry.Text;
            user.Phone = phoneEntry.Text;
            user.UserName = userNameEntry.Text;

            try
            {
                var jsonRequest = JsonConvert.SerializeObject(user);
                var httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Utilities.localhost);
                var url = string.Format("/api/Users/{0}", user.UserId);

                var response = await client.PutAsync(url, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    await DisplayAlert("Error", "Error updating user", "Acept");
                    return;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Acept");
                return;
            }

            waitActivityIndicator.IsRunning = false;
            await DisplayAlert("Ok", "User updated ok", "Acept");
            await Navigation.PopAsync();
        }

    }
}
