using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeWork.Enumeration.Generic;
using Xunit;

namespace TreeWork.xUnit.Tests.Enumeration.Generic
{
    public sealed class TreeEnumerator_Test
    {
        #region private:

        private class CustomEnumerable : IEnumerable<TreeItem>
        {
            private readonly IEnumerable<TreeItem> _collection;
            private readonly Counter _counter;

            public CustomEnumerable(IEnumerable<TreeItem> collection, Counter counter)
            {
                _collection = collection;
                _counter = counter;
            }

            public IEnumerator<TreeItem> GetEnumerator()
            {
                return new Counter.CustomEnumerator(_collection, _counter);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class Counter
        {
            public class CustomEnumerator : IEnumerator<TreeItem>
            {
                private readonly IEnumerator<TreeItem> _externalEnumerator;
                private readonly Counter _counter;

                public CustomEnumerator(IEnumerable<TreeItem> externalCollection, Counter counter)
                {
                    _externalEnumerator = externalCollection.GetEnumerator();
                    _counter = counter;
                }

                public bool MoveNext()
                {
                    return _externalEnumerator.MoveNext();
                }

                public void Reset()
                {
                    _externalEnumerator.Reset();
                }

                public TreeItem Current => _externalEnumerator.Current;

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    _externalEnumerator.Dispose();
                    _counter.DisposeCount++;
                }
            }

            /// <summary>
            /// Возвращает количество вызванных Dispose
            /// </summary>
            public int DisposeCount { get; private set; }
        }

        #endregion

        /// <summary>
        /// Тестирует корректные вызовы методов Dispose у итераторов, возвращающих подчинённые и корневые узлы.
        /// </summary>
        [Fact]
        public void Dispose_MustBeExecuting_Ok()
        {
            // Данные для теста
            var items = TreeItemHelper.Generate_ForDisposeTest();

            // Подсчёт вызовов
            var counter = new Counter();

            // Тестовые итераторы
            var head = new CustomEnumerable(items.Where(a => a.ParentId == 0), counter); // 1 call = 1 disposing
            IEnumerable<TreeItem> GetChildren(TreeItem item) => new CustomEnumerable(items.Where(a => a.ParentId == item.Id).OrderBy(a => a.Id), counter); // 4 elements = 5 calls = 5 disposing (try get subelements at Id=5)

            // Шаблон формирования дерева с использованием итератора с контекстом
            using (var enumerator = new TreeEnumerator<TreeItem>(head, GetChildren))
            {
                while (enumerator.MoveNext()) { }
            }

            // total 6 calls
            Assert.Equal(6, counter.DisposeCount);
        }

        /// <summary>
        /// Тест возможности использования итератора повторно после вызова Reset (с проверкой освобождения ресурсов)
        /// </summary>
        [Fact]
        public void Reset_MustNotSupportedException_Ok()
        {
            // Данные для теста
            var items = TreeItemHelper.Generate_ForDisposeTest();

            // Подсчёт вызовов
            var counter = new Counter();

            // Тестовые итераторы
            var head = new CustomEnumerable(items.Where(a => a.ParentId == 0), counter); // 1 call = 1 disposing
            IEnumerable<TreeItem> GetChildren(TreeItem item) => new CustomEnumerable(items.Where(a => a.ParentId == item.Id).OrderBy(a => a.Id), counter); // 4 elements = 5 calls = 5 disposing (try get subelements at Id=5)

            // Шаблон формирования дерева с использованием итератора с контекстом
            using (var enumerator = new TreeEnumerator<TreeItem>(head, GetChildren))
            {
                int i = 0;
                while (enumerator.MoveNext())
                {
                    // i = 0 - current=head
                    // i = 1 - current=sub 1 head 1
                    // i = 2 - current=sub 1 head 2
                    // i = 3 - current=sub 1 head 3
                    // i = 4 - current=sub 1 head 4

                    if (i == 4)
                    {
                        // enumerator has stack with 5 elements
                        Assert.Throws<NotSupportedException>(() => enumerator.Reset());
                        break;
                    }

                    i++;
                }
            }
        }
    }
}
