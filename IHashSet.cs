using System;
using System.Collections.Generic;

namespace HashSet.Lib
{
    public interface IHashSet<T> where T : SPSStudent, IEquatable<T>
    {
        T Add(T value);
        bool IsPresent(T value);
        void Rebalance();
    }

    public interface SPSStudent : IEquatable<SPSStudent>
    {
        string Name { get; }
        string Year { get; }
        string Tutor { get; }
    }

    public class Student : SPSStudent
    {
        public string Name { get; }
        public string Year { get; }
        public string Tutor { get; }

        public Student(string name, string year, string tutor)
        {
            Name = name;
            Year = year;
            Tutor = tutor;
        }
        
        public override string ToString()
        {
            return $"{Name}{Year}{Tutor}";
        }

        public override int GetHashCode()
        {
            return Math.Abs(ToString().GetHashCode());
        }

        public bool Equals(SPSStudent other)
        {
            if (other == null) return false;
            return this.ToString() == other.ToString();
        }
    }

    public class CustomHashSet<T> : IHashSet<T> where T : SPSStudent, IEquatable<T>
    {
        private int _size;
        private int _itemCount;
        private bool _useChaining;
        private List<List<T>> _chainingList;
        private List<T> _probingList;

        public CustomHashSet(int size, bool useChaining)
        {
            _size = size;
            _useChaining = useChaining;
            _itemCount = 0;
            Init();
        }

        private void Init()
        {
            if (_useChaining)
            {
                _chainingList = new List<List<T>>(_size);
                for (int i = 0; i < _size; i++) _chainingList.Add(new List<T>());
            }
            else
            {
                _probingList = new List<T>(_size);
                for (int i = 0; i < _size; i++) _probingList.Add(default(T));
            }
        }

        public T Add(T item)
        {
            if ((double)_itemCount / _size >= 0.75) Rebalance();

            int index = item.GetHashCode() % _size;

            if (_useChaining)
            {
                _chainingList[index].Add(item);
                _itemCount++;
            }
            else
            {
                while (_probingList[index] != null)
                {
                    index = (index + 1) % _size;
                }
                _probingList[index] = item;
                _itemCount++;
            }

            return item;
        }

        public bool IsPresent(T item)
        {
            int index = item.GetHashCode() % _size;

            if (_useChaining)
            {
                foreach (var stored in _chainingList[index])
                {
                    if (stored.Equals(item)) return true;
                }
            }
            else
            {
                int start = index;
                while (_probingList[index] != null)
                {
                    if (_probingList[index].Equals(item)) return true;
                    index = (index + 1) % _size;
                    if (index == start) break;
                }
            }

            return false;
        }

        public void Rebalance()
        {
            var oldChaining = _chainingList;
            var oldProbing = _probingList;

            _size *= 2;
            _itemCount = 0;
            Init();

            if (_useChaining)
            {
                foreach (var list in oldChaining)
                {
                    foreach (var item in list) Add(item);
                }
            }
            else
            {
                foreach (var item in oldProbing)
                {
                    if (item != null) Add(item);
                }
            }
        }
    }
}