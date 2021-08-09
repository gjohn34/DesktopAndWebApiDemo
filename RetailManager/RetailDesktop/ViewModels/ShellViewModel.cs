using Caliburn.Micro;
using RetailDesktop.EventModels;
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
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, SimpleContainer container)
        {
            _events = events;
            _events.SubscribeOnPublishedThread(this);
            _salesVM = salesVM;
            _container = container;

            // 'restarts' the login view to wipe data
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());

        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(_salesVM);
        }
    }
}
