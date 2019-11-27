using HBProductsSupport.Models;
using HBProductsSupport.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProductsSupport.ViewModels
{
    class ChatListPageViewModel : BaseViewModel
    {
        private ObservableCollection<Session> unansweredSessions;
        private ObservableCollection<Session> employeeSessions;
        private ChatManager manager;
        private INotifyView view;
        private int empID;

        public ChatListPageViewModel(INotifyView view, int empID)
        {
            this.view = view;
            this.empID = empID;
            manager = new ChatManager();
            unansweredSessions = new ObservableCollection<Session>();
            employeeSessions = new ObservableCollection<Session>();
            updateChatList();
        }

        private async void updateChatList()
        {
            IsBusy = true;
            string sessionsString = await manager.GetUnansweredSessions();
            string empSessionsString = await manager.GetEmpSessions(empID);
            if (sessionsString.Contains("Error:"))
            {
                view.notify("Error", sessionsString.Substring(6));
                IsBusy = false;
                return;
            }
            if (empSessionsString.Contains("Error:"))
            {
                view.notify("Error", empSessionsString.Substring(6));
                IsBusy = false;
                return;
            }
            UnansweredSessions = JsonConvert.DeserializeObject<ObservableCollection<Session>>(sessionsString);
            EmployeeSessions = JsonConvert.DeserializeObject<ObservableCollection<Session>>(empSessionsString);
            IsBusy = false;
        }

        public ObservableCollection<Session> UnansweredSessions
        {
            get { return unansweredSessions; }
            set { SetProperty(ref unansweredSessions, value); OnPropertyChanged("UnansweredSessions"); }
        }

        public ObservableCollection<Session> EmployeeSessions
        {
            get { return employeeSessions; }
            set { SetProperty(ref employeeSessions, value); OnPropertyChanged("EmployeeSessions"); }
        }

        public ICommand CommandUpdate
        {
            get
            {
                return new Command(() => updateChatList());
            }
        }




    }
}
