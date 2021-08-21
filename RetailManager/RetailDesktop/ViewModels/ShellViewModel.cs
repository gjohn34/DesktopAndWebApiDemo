using Caliburn.Micro;
using RetailDesktop.EventModels;
using RetailDesktop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetailDesktop.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private IEventAggregator _events;
        private SimpleContainer _container;
        private ILoggedInUserModel _user;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, SimpleContainer container, ILoggedInUserModel user)
        {
            _events = events;
            _events.SubscribeOnPublishedThread(this);
            _salesVM = salesVM;
            _container = container;
            _user = user;


            // 'restarts' the login view to wipe data
            ActivateItemAsync(IoC.Get<LoginViewModel>());

        }

        public bool IsUserLoggedIn 
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    return true;
                }
                return false;
            }
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            NotifyOfPropertyChange(() => IsUserLoggedIn);
            return ActivateItemAsync(_salesVM);
        }
        public void ExitApp()
        {
            this.TryCloseAsync();
        }

        public void Logout()
        {
            _user.Logout();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsUserLoggedIn);
        }
    }
}
