using HBProductsSupport.Models;
using HBProductsSupport.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HBProductsSupport.ViewModels
{
    class ChatListPageViewModel : BaseViewModel
    {
        private ObservableCollection<Session> unansweredSessions;
        private ChatManager manager;
        private INotifyView view;

        public ChatListPageViewModel(INotifyView view)
        {
            this.view = view;
            manager = new ChatManager();
            unansweredSessions = new ObservableCollection<Session>();
            updateChatList();
        }

        private async void updateChatList()
        {
            string sessionsString = await manager.GetUnansweredSessions();

            if (sessionsString.Contains("Error:"))
            {
                view.notify("Error", sessionsString.Substring(6));
                return;
            }
            UnansweredSessions = JsonConvert.DeserializeObject<ObservableCollection<Session>>(sessionsString);
        }

        public ObservableCollection<Session> UnansweredSessions
        {
            get { return unansweredSessions; }
            set { SetProperty(ref unansweredSessions, value); OnPropertyChanged("Sessions"); }
        }




    }
}
