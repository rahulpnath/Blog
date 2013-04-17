using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PhoneAppBarMVVM.Helpers;
using System;

namespace PhoneAppBarMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : MyModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IApplicationBarService appBar)
            : base(appBar)
        {
            AddSettingsCommand = new RelayCommand(AddSettingsButton);
            AddPlusCommand = new RelayCommand(AddPlusButton);
        }

        private void AddPlusButton()
        {
            ApplicationBar.AddButton("Add", new Uri("Images/appbar.add.rest.png", UriKind.Relative), OnPlusButtonClicked);
        }

        private void OnPlusButtonClicked()
        {
            
        }

        private void AddSettingsButton()
        {
            MessengerInstance.Send<NotificationMessage>(new NotificationMessage(this, "AddSettings"));
        }

        public RelayCommand AddSettingsCommand { get; set; }

        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand AddPlusCommand { get; set; }


    }
}