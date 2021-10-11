using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;
using D2KeyHelper.Pages;

namespace D2KeyHelper.Viewmodels
{
    public class MainVM : BindableBase
    {
        private readonly PageNavigationService navigationService;

        public MainVM(HookService _hookService, PageNavigationService _navigationService,ProfileService _profileService)
        {
            HookService = _hookService;
            navigationService = _navigationService;
            ProfileService = _profileService;
            _navigationService.OnPageChanged += NavigationService_OnPageChanged;
            CurrentPage = new KeyBindingPage();
        }
        public HookService HookService { get; }
        public ProfileService ProfileService { get; }
        public Page CurrentPage { get; set; }

        private void NavigationService_OnPageChanged(Page page) => CurrentPage = page;

        public ICommand SetHook => new DelegateCommand(() =>
        {
            var d2Proc = Process.GetProcessesByName("D2R");
            HookService.SetHook(d2Proc[0].Id);
        });
        public ICommand OpenKeyBindingPage => new DelegateCommand(() =>
        {
            navigationService.Navigate(new KeyBindingPage());
        });

    }
}
