using D2KeyHelper.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;

namespace D2KeyHelper
{
    public static class ViewMapper
    {
        private static Dictionary<Type, Type> _vmToWindowMapping = new();
        public static Dictionary<Type, Type> VmToWindowMapping => _vmToWindowMapping;

        public static void MapWindowType<VM, WND>() where WND : Window, new() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot map interfaces");
            if (_vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException($"Type {vmType.FullName}  is already mapped");
            _vmToWindowMapping[vmType] = typeof(WND);
        }
        public static void UnmapWindowType<VM>() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot map interfaces");
            if (!_vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException($"Type {vmType.FullName}  is not mapped");
            _vmToWindowMapping.Remove(vmType);
        }
       
    }
}
