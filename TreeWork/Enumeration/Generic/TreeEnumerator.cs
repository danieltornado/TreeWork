using System;
using System.Collections;
using System.Collections.Generic;

namespace TreeWork.Enumeration.Generic
{
    /// <summary>
    /// Итератор нерекурсивного обхода
    /// </summary>
    internal sealed class TreeEnumerator<T> : IEnumerator<TreeNode<T>>
    {
        private readonly Func<T, IEnumerable<T>> _getChildren;
        private readonly Stack<IEnumerator<T>> _callstack;
        private TreeNode<T> _current;

        /// <summary>
        /// State of enumerator: 0 - no code after yield, 1 - execute code after yield
        /// </summary>
        private byte _state;

        public TreeEnumerator(IEnumerable<T> collection, Func<T, IEnumerable<T>> getChildren)
        {
            _callstack = new Stack<IEnumerator<T>>();
            _getChildren = getChildren;

            var head = collection.GetEnumerator();
            _callstack.Push(head);

            _state = 0;
        }

        public bool MoveNext()
        {
            switch (_state)
            {
                // При первом выполнении пропускается

                // Code after yield
                case 1:
                    // Get childs and push enumerator to stack
                    var current = _callstack.Peek();
                    var collection = _getChildren(current.Current);
                    if (collection != null)
                    {
                        var child = collection.GetEnumerator();
                        _callstack.Push(child);
                    }
                    else
                    {
                        _callstack.Push(null);
                    }
                    // Set no code after yield
                    _state = 0;
                    break;
            }

            while (_callstack.Count > 0)
            {
                // All magic here

                // Continue
                var current = _callstack.Pop();
                if (current?.MoveNext() == true)
                {
                    // Return enumerator back to stack
                    _callstack.Push(current);

                    _state = 1;
                    _current.Level = _callstack.Count - 1;
                    _current.Value = current.Current;
                    return true;

                    // Code after yield
                }

                // Set no code after yield
                _state = 0;
                // End of collection
                current?.Dispose();
            }

            _current.Level = 0;
            _current.Value = default(T);
            return false;
        }

        public TreeNode<T> Current => _current;

        object IEnumerator.Current => Current;

        public void Reset()
        {
            // C# 6.0. Справочник. Полное описание языка. 6-е издание. Джозеф Албахари, Бен Албахари.
            // Раздел 7. Коллекции, глава Перечисление:
            //  Метод Reset существует главным образом для взаимодействия с
            //  СОМ: вызова его напрямую в общем случае избегают, т.к.он не является универсально
            //  поддерживаемым (и он необязателен, потому что обычно просто создает новый экземпляр перечислителя).
            throw new NotSupportedException();
        }

        void IDisposable.Dispose()
        {
            // Просто очищаем (без сброса)
            while (_callstack.Count > 0)
            {
                var current = _callstack.Pop();
                current?.Dispose();
            }
        }
    }
}
