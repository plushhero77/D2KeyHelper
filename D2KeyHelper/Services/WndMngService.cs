using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using D2KeyHelper.src.Interfaces;

namespace D2KeyHelper.Services
{
    public class WndMngService : IWindowManagmentService
    {
        private Dictionary<Type, Window> openedWindows = new Dictionary<Type, Window>();

        private Window CreateWindowInstance(Type vmType)
        {
            Type wndType = null;

            while (vmType != null && !ViewMapper.VmToWindowMapping.TryGetValue(vmType, out wndType))
                vmType = vmType.BaseType;

            if (wndType == null)
                throw new ArgumentException($"No mapped window type for argument type {vmType.FullName}");

            var wnd = (Window)Activator.CreateInstance(wndType);
            openedWindows[vmType] = wnd;
            wnd.Closed += new((o, e) => { openedWindows.Remove(vmType); });
            return wnd;
        }
        public void CloseViewModel<VM>()
        {
            var vm = typeof(VM);
            if (!openedWindows.TryGetValue(vm, out Window window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            window.Close();
            openedWindows.Remove(vm);
        }
        public void ShowViewModel<VM>()
        {
            var vm = typeof(VM);
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (openedWindows.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var window = CreateWindowInstance(vm);
            window.Show();
        }
        public async Task ShowModalViewModel<VM>()
        {
            var window = CreateWindowInstance(typeof(VM));
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
        public async Task ShowModalViewModel(IViewModelBase viewModel)
        {
            var window = CreateWindowInstance(viewModel.GetType());
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.DataContext = viewModel;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
        public bool IsViewActive<VM>() => openedWindows.TryGetValue(typeof(VM), out _);

    }
}
