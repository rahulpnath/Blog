using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneAppBarMVVM.Helpers
{
   public interface IApplicationBarService
    {
        IApplicationBar ApplicationBar { get;} 

        void AddButton(string title, Uri imageUrl, Action OnClick);
    }
}
