using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyBuddy.Mobile.Models;
using StudyBuddy.Mobile.Services;
using static StudyBuddy.Mobile.Models.AuthModels;

namespace StudyBuddy.Mobile.ViewModels
{
    public partial class RegisterViewModel: ObservableObject
    {
        private readonly AuthService _authService = new();

        [ObservableProperty]
        private string firstName = string.Empty;

        [ObservableProperty]
        private string lastName = string.Empty;

        [ObservableProperty]
        private string nationality = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;


        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter your email and password.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            var register = new RegisterRequest
            {
                FirstName = FirstName,
                LastName = LastName,
                Nationality = Nationality,
                Age = age,
                Email = Email,
                Password = Password
            };

            var result = await _authService.RegisterAsync(register);

            IsBusy = false;

            if (result.Success)
                await Shell.Current.GoToAsync("//MainPage");
            else
                ErrorMessage = result.Message;
        }

        [RelayCommand]
        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
