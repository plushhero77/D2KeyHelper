using D2KeyHelper.Services;
using D2KeyHelper.src;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.Viewmodels
{
    public class EditProfileVM : BindableBase
    {
        public ProfileService ProfileService { get; }
        public string[] CB_itemsCollection { get; }

        public EditProfileVM(ProfileService profileService)
        {
            ProfileService = profileService;
            CB_itemsCollection = Enum.GetNames(typeof(NativeWin32.Enums.VirtualKeyShort));

        }


    }
}
