using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using University.App.ViewModels;
using University.App.Views.Forms;
using University.DTOs;
using Xamarin.Forms;

namespace University.ViewModels.Forms
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Attributes
        private string _email;
        private string _password;
        #endregion

        #region Properties
        public string Email
        {
            get { return _email; }
            set { this.SetValue(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { this.SetValue(ref _password, value); }

        }
        #endregion

        #region Methods
        async void Register()
        {
            var data = new RegisterReqDTO { Email = this.Email, password = this.Password };
            var json = JsonConvert.SerializeObject(data);
            var req = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://reqres.in/api/register";
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, req);

                var statusCode = response.StatusCode;
                result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var loginRes = JsonConvert.DeserializeObject<RegisterResDTO>(result);
                    var token = loginRes.Token;
                    await Application.Current.MainPage.DisplayAlert("Notify Is Successful", token, "Cancel");
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    var registerResFail = JsonConvert.DeserializeObject<RegisterResFailDTO>(result);
                    var error = registerResFail.Error;
                    await Application.Current.MainPage.DisplayAlert("Notify Is Error", error, "Cancel");
                }
            }
        }
        #endregion

        #region Commands
        public Command RegisterCommand { get; set; }
        #endregion

        #region ViewsModels
        public RegisterViewModel()
        {
            this.RegisterCommand = new Command(this.Register);
        }
        #endregion
    }
}


namespace University.App.ViewModels.Forms
{
    internal class RegisterViewModels
    {
    }
}
