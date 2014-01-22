using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IncrementalLoading.Phone.Resources;
using IncrementalLoading.PCL.ViewModels;
using IncrementalLoading.PCL.Models;

namespace IncrementalLoading.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainViewModel;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel.OnPageNavigated();
        }

        private void Photo_Loaded(object sender, ItemRealizationEventArgs e)
        {
            LongListSelector longList = sender as LongListSelector;
            PhotoCollectionViewModel vm = longList.DataContext as PhotoCollectionViewModel;

            vm.LoadMorePhotos(e.Container.Content as Photo);
        }
    }
}
