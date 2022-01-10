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

        private Window CreateWindowInstance<VM>()
        {
            Type wndType = null;
            var vmType = typeof(VM);

            while (vmType != null && !ViewMapper.VmToWindowMapping.TryGetValue(vmType, out wndType))
                vmType = vmType.BaseType;

            if (wndType == null)
                throw new ArgumentException($"No mapped window type for argument type {vmType.FullName}");

            var wnd = (Window)Activator.CreateInstance(wndType);
            return wnd;
        }
        public void CloseViewModel<VM>()
        {
            var vm = typeof(VM);
            Window window;
            if (!openedWindows.TryGetValue(vm, out window))
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
            var window = CreateWindowInstance<VM>();
            openedWindows[vm] = window;
            window.Show();
        }
        public async Task ShowModalViewModel<VM>()
        {
            var window = CreateWindowInstance<VM>();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
    }
}
