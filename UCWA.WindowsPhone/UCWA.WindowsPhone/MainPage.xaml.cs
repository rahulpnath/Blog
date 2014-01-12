using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using UCWA.WindowsPhone.Resources;
using UCWA.WindowsPhone.Model;

namespace UCWA.WindowsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private UCWAHelper helper;

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(password.Password))
            {
                // User name and password is mandatory
                MessageBox.Show("Please enter username and password");
            }
            else
            {
                // try to connect to ucwa service
                progressBar.IsIndeterminate = true;
                progressBar.Visibility = System.Windows.Visibility.Visible;
                login.IsEnabled = false;

                helper = new UCWAHelper(userName.Text, password.Password, this.LoginCompleted);
            }

        }

        private void LoginCompleted()
        {
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            loginUCWA.Visibility = System.Windows.Visibility.Collapsed;
            userDetails.Visibility = System.Windows.Visibility.Visible;

            name.Text = helper.UserFullName;
            title.Text = helper.Title;
            department.Text = helper.Department;
        }

       
    }
}