using SkulApp.Classes;
using System;

using Xamarin.Forms;

namespace SkulApp.Pages
{
    public partial class HomePage : ContentPage
    {
        private UserPassword user;

        public HomePage(UserPassword user)
        {
            InitializeComponent();

            this.user = user;
            userNameLabel.Text = user.FullName;

            mySettingsButton.Clicked += MySettingsButton_Clicked;
            myHomeWorkButton.Clicked += myHomeWorkButton_Clicked;
        }

        private async void MySettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MySettingsPage(this.user));
        }

        private async void myHomeWorkButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyHomeWorkPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            userNameLabel.Text = user.FullName;
            
        }
    }

}
