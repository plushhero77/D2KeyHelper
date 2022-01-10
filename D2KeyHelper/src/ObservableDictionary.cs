using DevExpress.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged where TKey : notnull, IEquatable<TKey>
    {
        private readonly ObservableCollection<KeyValuePair<TKey, TValue>> _items = new();

        public TValue this[TKey key] { get => _items.ToDictionary(x => x.Key)[key].Value; set => Add(key, value); }
        public TValue this[int index] { get => _items[index].Value; }

        public ICollection<TKey> Keys => _items.Select(x => x.Key).ToArray();

        public ICollection<TValue> Values => _items.Select(x => x.Value).ToArray();

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(TKey key, TValue value)
        {
            if (Keys.Contains(key) || key == null)
            {
                throw new ArgumentException($"Key {key} is already exist");
            }
            _items.Add(new KeyValuePair<TKey, TValue>(key, value));
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            _items.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null, null));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _items.Contains(item);

        public bool ContainsKey(TKey key) => Keys.Contains(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < _items.Count || array.Length > _items.Count)
            {
                throw new ArgumentException($"Array length lower then this array!");
            }

            for (int i = 0; i < _items.Count; i++)
            {
                array[arrayIndex + i] = _items.ElementAt(i);
            }

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
                throw new KeyNotFoundException($"Key {key} not found!");
            }

            var res = _items.Remove(_items.ToDictionary(x => x.Key)[key]);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key, null));
            return res;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!ContainsKey(key))
            {
                value = default;
                return true;
            }
            else
            {
                value = this[key];
                return true;
            }
        }

        public int GetIndex(KeyValuePair<TKey, TValue> valuePair) => _items.IndexOf(valuePair);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
