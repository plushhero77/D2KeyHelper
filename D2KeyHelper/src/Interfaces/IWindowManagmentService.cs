using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D2KeyHelper.src.Interfaces
{
    public interface IWindowManagmentService
    {
        public void ShowViewModel<T>();
        public void CloseViewModel<T>();
        public Task ShowModalViewModel<T>(); 
        public Task ShowModalViewModel(IViewModelBase viewModel);
        public bool IsViewActive<T>();

    }
}
