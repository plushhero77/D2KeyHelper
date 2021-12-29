using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D2KeyHelper.Services
{
    public class WindowManagmentService
    {
        private Dictionary<object, Window> openedWindows = new Dictionary<object, Window>();

        private Window CreateWindowInstance(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("ViewModel");

            Type wndType = null;
            var vmType = vm.GetType();

            while (vmType != null && !ViewMapper.VmToWindowMapping.TryGetValue(vmType, out wndType))
                vmType = vmType.BaseType;

            if (wndType == null)
                throw new ArgumentException($"No mapped window type for argument type {vmType.FullName}");

            var wnd = (Window)Activator.CreateInstance(wndType);

            return wnd;
        }
        public void ShowViewModel(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (openedWindows.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var window = CreateWindowInstance(vm);
            openedWindows[vm] = window;
            window.Show();
        }
        public void CloseViewModel(object vm)
        {
            Window window;
            if (!openedWindows.TryGetValue(vm, out window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            window.Close();
            openedWindows.Remove(vm);
        }
        public async Task ShowModalViewModel(object vm) 
        {
            var window = CreateWindowInstance(vm);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
    }
}
