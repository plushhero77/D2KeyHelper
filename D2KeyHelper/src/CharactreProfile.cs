using D2KeyHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class CharactreProfile
    {
        public event Action OnProfileChanged;
#warning Исправить проблему с конструктором
        private BindingKeyCollection<BindingKey> bindingKeysCollection = new BindingKeyCollection<BindingKey>();
        private string name = "default_profile";

        public string Name { get => name; set => name = value; }
        public BindingKeyCollection<BindingKey> BindingKeysCollection
        {
            get => bindingKeysCollection;
            set
            {
                bindingKeysCollection = value;
                bindingKeysCollection.CollectionChanged += OnCollectionChanged;
                ((INotifyPropertyChanged)bindingKeysCollection).PropertyChanged += OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => OnProfileChanged?.Invoke();
        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => OnProfileChanged?.Invoke();

    }
}
