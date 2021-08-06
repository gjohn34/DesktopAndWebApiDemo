﻿using Caliburn.Micro;
using RetailDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailDesktop.ViewModels
{
    public class LoginViewModel : PropertyChangedBase
    {
        string _userName;
        string _password;
        private IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
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

        private string _errorMessage;

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

                // if response.token
                // api call to /getUser
                // save currentUser to state
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}