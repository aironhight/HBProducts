using HBProductsSupport.Models;
using HBProductsSupport.Services;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HBProductsSupport.ViewModels
{
    class ChatListPageViewModel : BaseViewModel
    {
        private readonly Color internetColor = Color.White; private readonly Color noInternetColor = Color.Red;
        private readonly string noInternetTitle = "No internet access..."; private readonly string internetAcessTitle = "Chat Sessions";

        private ObservableCollection<Session> unansweredSessions;
        private ObservableCollection<Session> employeeSessions;
        private ChatManager manager;
        private INotifyView view;
        private int empID;
        private string employeeName, title;
        private Timer timer, IDtimer;
        private bool IsUpdating;
        private Color backgroundColor;

        public ChatListPageViewModel(INotifyView view, string empName)
        {
            this.view = view;
            this.employeeName = empName;
            manager = new ChatManager();
            unansweredSessions = new ObservableCollection<Session>();
            employeeSessions = new ObservableCollection<Session>();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged; //Add an event handler for internet connectivity changes.
            PageTitle = "Getting Employee ID...";
            IsBusy = false;
            empID = -1;
            IsUpdating = false;
            backgroundColor = internetColor;
            GetEmployeeID();
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { SetProperty(ref backgroundColor, value); OnPropertyChanged("BackgroundColor"); }
        }

        private async void GetEmployeeID()
        {
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return;
            }

            int newID = await manager.GetEmpID(employeeName);

            if (empID != -1)//This will be executed if there are still pending requests after the Employee id has already been set...
            {
                if (IDtimer != null)
                    IDtimer.Dispose();
                return;
            }

            if (newID == -200 || newID == -12)
            {
                view.notify("id error");

                Task.Factory.StartNew(() =>
                {
                    if (IDtimer != null)
                        IDtimer.Dispose();
                    //Make the system check for new messages every 3 seconds.
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromSeconds(3);
                    IDtimer = new Timer((e) =>
                    {
                        GetEmployeeID();
                    }, null, startTimeSpan, periodTimeSpan);
                });
            } else
            {
                empID = newID;
                if (IDtimer != null)
                    IDtimer.Dispose();
                PageTitle = internetAcessTitle;
                StartUpdatingChatList();
            }      
        }

        public void StartUpdatingChatList()
        {
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return;
            }
            if (empID == -1) return;
            Task.Factory.StartNew(() =>
            {
                while (manager == null)
                {
                    Thread.Sleep(5000);
                }
                StopUpdatingChatList();
                //Make the system check for new messages every 3 seconds.
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromSeconds(3);
                timer = new Timer((e) =>
                {
                    UpdateSessionLists();
                }, null, startTimeSpan, periodTimeSpan);
            });
        }

        public void StopUpdatingChatList()
        {
            if (timer != null)
                timer.Dispose();
        }

        private async void UpdateSessionLists()
        {
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return;
            }
            if (IsUpdating) return; //If the previous request is not yet processed - don't make another one...
            IsUpdating = true;
            string sessionsString = await manager.GetUnansweredSessions();
            string empSessionsString = await manager.GetEmpSessions(empID);
            if (sessionsString.Contains("Error:"))
            {
                view.notify("session error", sessionsString.Substring(6));
                return;
            }
            if (empSessionsString.Contains("Error:"))
            {
                view.notify("session error", empSessionsString.Substring(6));
                return;
            }
            ObservableCollection<Session> newUnanswered = JsonConvert.DeserializeObject<ObservableCollection<Session>>(sessionsString);
            ObservableCollection<Session> newEmployeeSessions = JsonConvert.DeserializeObject<ObservableCollection<Session>>(empSessionsString);

            UpdateSessionList(UnansweredSessions, newUnanswered);
            UpdateSessionList(EmployeeSessions, newEmployeeSessions);

            IsUpdating = false;
        }

        private void UpdateSessionList(ObservableCollection<Session> oldList, ObservableCollection<Session> newList) {
            //Add the new sessions to the old list
            foreach (Session s in newList)
            {
                if (!sessionListContainsID(oldList, s.SessionID))
                    oldList.Add(s);
            }


            for(int i=0; i<oldList.Count; i++)
            {
                if (!sessionListContainsID(newList, oldList[i].SessionID))
                    oldList.Remove(oldList[i]);
            }

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
                return new Command(() => UpdateSessionLists());
            }
        }

        public async Task<bool> TakeSession(int sessionID, string customerName)
        {
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return false;
            }
            int takeResponse = await manager.TakeSession(empID, sessionID); //Take session

            switch(takeResponse)
            {
                case -5:
                    view.notify("session parsing error");
                    return false;
                case -2:
                    view.notify("session already taken");
                    return false;
                case -1:
                    view.notify("session nonexist");
                    return false;
            }
           
            await manager.sendMessage(sessionID, new Message(true, $"Hello, {customerName}! {Environment.NewLine}My name is {employeeName} and I will be assisting you today!", "", 0)); //Send greetings message :)
            return true;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                PageTitle = internetAcessTitle;
                BackgroundColor = internetColor;
                if (empID != -1 && empID != -200)
                    StartUpdatingChatList(); //Start Updating if there is network access...
                else
                    GetEmployeeID();
            }
            else
            {
                NoInternetAlert();
            }
        }

        public string PageTitle {
            get { return title;  }
            set { SetProperty(ref title, value); OnPropertyChanged("Title"); }
        }


        private bool sessionListContainsID(ObservableCollection<Session> sess, int id)
        {
            foreach(Session s in sess)
            {
                if(s.SessionID == id)
                    return true;
            }
            return false;
        }

        public bool ListsEnabled { get { return !IsBusy; } }

        private void NoInternetAlert()
        {
            BackgroundColor = noInternetColor;
            PageTitle = noInternetTitle;
            StopUpdatingChatList(); //Stop updating if there is no network access...
            view.notify("no internet");
        }

        public Boolean HasInternetConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
