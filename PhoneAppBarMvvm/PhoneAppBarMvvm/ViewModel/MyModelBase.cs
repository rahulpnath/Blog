using GalaSoft.MvvmLight;
using PhoneAppBarMVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneAppBarMVVM.ViewModel
{
    public class MyModelBase: ViewModelBase
    {
        public IApplicationBarService ApplicationBar { get; set; }

        public MyModelBase()
        {

        }
        public MyModelBase(IApplicationBarService appBar)
        {
            ApplicationBar = appBar;
        }
    }
}
