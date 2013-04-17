using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Phone7.Fx.Controls;
using PhoneAppBarMvvm;


namespace PhoneAppBarMVVM.Helpers
{
    public class ApplicationBarService: IApplicationBarService
    {

        public void AddButton(string title, Uri imageUrl, Action OnClick)
        {
            ApplicationBarIconButton newButton = new ApplicationBarIconButton()
                {
                    Text = title, 
                    IconUri = imageUrl, 
                };
            newButton.Click += ((sender,e) => {OnClick.Invoke();}) ;

            ApplicationBar.Buttons.Add(newButton);
           
        }

        public IApplicationBar ApplicationBar
        {
            get
            {
                var currentPage = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
                var currentPage1 = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
                if (currentPage.ApplicationBar == null)
                {
                    currentPage.ApplicationBar = new ApplicationBar();
                }
                return currentPage.ApplicationBar;
            }
        }
    }
}
