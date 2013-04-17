using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneAppBarMVVM.ViewModel;
using Phone7.Fx.Controls;

namespace PhoneAppBarMvvm
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainViewModel viewModel
        {
            get
            {
                return this.DataContext as MainViewModel;
            }
        }

        private ApplicationBarIconButton settingsButton;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            settingsButton = new ApplicationBarIconButton()
            {
                Text = "Settings",
                IconUri = new Uri("Images/appbar.feature.settings.rest.png", UriKind.Relative)
            };
            settingsButton.Click += settingsButton_Click;
            // Register for the messenger 
            Messenger.Default.Register<NotificationMessage>(this, OnNotificationMessage);
        }

        void settingsButton_Click(object sender, EventArgs e)
        {
            viewModel.SettingsCommand.Execute(null);
        }

        private void OnNotificationMessage(NotificationMessage message)
        {
            // Check here for the notification
            // You can also build cutoms notification message here for this by inheriting from MessageBase
            if (message.Notification == "AddSettings")
            {
                if (ApplicationBar == null)
                {
                    ApplicationBar = new ApplicationBar();
                }
                ApplicationBar.Buttons.Add(settingsButton);
            }
        }
    }
}