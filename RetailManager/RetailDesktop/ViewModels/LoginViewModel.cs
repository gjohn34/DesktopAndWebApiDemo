using Caliburn.Micro;
using RetailDesktop.EventModels;
using RetailDesktop.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailDesktop.ViewModels
{
    public class LoginViewModel : Screen
    {
        string _userName;
        string _password;
        private string _errorMessage;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);

            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }


        public bool IsErrorVisible
        {
            get { return !string.IsNullOrEmpty(ErrorMessage); }
        }


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { 
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);

            }
        }



        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Password))
                {
                    output = true;
                }

                return output;

            }
        }

        public async Task LogIn()
        {
            ErrorMessage = "Sending...";
            try
            {
                var response = await _apiHelper.Authenticate(UserName, Password);
                if (!string.IsNullOrEmpty(response.Access_Token))
                {
                    await _apiHelper.GetLoggedInUserInfo(response.Access_Token);
                    ErrorMessage = "Login Successful";
                    await _events.PublishOnUIThreadAsync(new LogOnEvent());
                }
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}
