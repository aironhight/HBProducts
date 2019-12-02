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
            //Check if the requested ID does not contain an error response.
            if (newID == -200 || newID == -12)
            {
                view.notify("id error");

                Task.Factory.StartNew(() =>
                {
                    if (IDtimer != null) //Destroy the timer if it already exists.
                        IDtimer.Dispose();
                    //Make the system check for new messages every 3 seconds.
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromSeconds(3);
                    IDtimer = new Timer((e) =>
                    {
                        GetEmployeeID();
                    }, null, startTimeSpan, periodTimeSpan);
                });
            }
            else
            {
                //The request for employee ID was successful.
                empID = newID;
                if (IDtimer != null)
                    IDtimer.Dispose();
                PageTitle = internetAcessTitle;
                StartUpdatingChatList();
            }      
        }

        //Starts updating the chat list
        public void StartUpdatingChatList()
        {
            //Check for internet connectivity
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return;
            }
            //If the employee ID is still not received
            if (empID == -1)
                return;

            //Start a new thread to make requests for updates every 3 seconds.
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

        //Stops the updating of the chat list
        public void StopUpdatingChatList()
        {
            if (timer != null)
                timer.Dispose();
        }

        //Updates the session list.
        private async void UpdateSessionLists()
        {
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return;
            }
            if (IsUpdating) return; //If the previous request is not yet processed - don't make another one...
            IsUpdating = true;
            string unansweredSessionsString = await manager.GetUnansweredSessions(); //JSON string of the unanswered sessions
            string employeeSessionsString = await manager.GetEmpSessions(empID); //JSON string of the answeered sessions for the current employee
            
            //Check if the strings contain error
            if (unansweredSessionsString.Contains("Error:"))
            {
                view.notify("session error", unansweredSessionsString.Substring(6));
                return;
            }
            if (employeeSessionsString.Contains("Error:"))
            {
                view.notify("session error", employeeSessionsString.Substring(6));
                return;
            }
            //Deserialize the strings
            ObservableCollection<Session> newUnanswered = JsonConvert.DeserializeObject<ObservableCollection<Session>>(unansweredSessionsString);
            ObservableCollection<Session> newEmployeeSessions = JsonConvert.DeserializeObject<ObservableCollection<Session>>(employeeSessionsString);

            //Update the sessions list.
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

        /**
         * Take a unanswered chat session with a given id
         * @params sessionID - The ID of the session to be taken
         * @params customerName - The name of the customer in the chat.
         */
        public async Task<bool> TakeSession(int sessionID, string customerName)
        {
            //Check for internet connectivity
            if (!HasInternetConnection())
            {
                NoInternetAlert();
                return false;
            }
            //Make a take session request.
            int takeResponse = await manager.TakeSession(empID, sessionID);

            //Check  the response for errors
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
            //Send a greetings message if the response is successful
            await manager.sendMessage(sessionID, new Message(true, $"Hello, {customerName}! {Environment.NewLine}My name is {employeeName} and I will be assisting you today!", "", 0)); //Send greetings message :)
            return true;
        }

        //Connectivity changed event handler.
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet) //Internet access
            {
                PageTitle = internetAcessTitle; 
                BackgroundColor = internetColor;
                if (empID != -1 && empID != -200)
                    StartUpdatingChatList(); //Start Updating if there is network access and the employee ID is already taken from the API...
                else
                    GetEmployeeID(); //Request a employee ID...
            }
            else
            {
                NoInternetAlert(); //Alert the user for no internet access
            }
        }

        public string PageTitle {
            get { return title;  }
            set { SetProperty(ref title, value); OnPropertyChanged("Title"); }
        }

        //Cheks if a list of sessions contains a session with a given ID
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

        //Stops the updating of the chat lists.
        private void NoInternetAlert()
        {
            BackgroundColor = noInternetColor; //Change the background color to indicate no internet connection
            PageTitle = noInternetTitle; //Change the title to indicate no internet connection
            StopUpdatingChatList(); //Stops the updating of the session lists.
            view.notify("no internet"); //Notify the view for no internet
        }

        //Checks if the device has wifi or mobile data connected.
        public Boolean HasInternetConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
