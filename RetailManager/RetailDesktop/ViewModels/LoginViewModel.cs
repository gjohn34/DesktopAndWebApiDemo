using Caliburn.Micro;
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

            // Why doesn't this work?
        }

        public void LogIn()
        {
                bool t = true;
        }
    }
}
