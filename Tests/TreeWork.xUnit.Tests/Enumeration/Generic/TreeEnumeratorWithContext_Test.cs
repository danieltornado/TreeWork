using System.Collections.Generic;
using System.Linq;
using TreeWork.Enumeration;
using TreeWork.Enumeration.Generic;
using Xunit;

namespace TreeWork.xUnit.Tests.Enumeration.Generic
{
    public sealed class TreeEnumeratorWithContext_Test
    {
        #region private:

        /// <summary>
        /// Сравнить два дерева.
        /// </summary>
        /// <param name="firstTree"></param>
        /// <param name="secondTree"></param>
        /// <param name="level"></param>
        private void CompareTrees(List<TreeItem> firstTree, List<TreeItem> secondTree, int level = 0)
        {
            Assert.Equal(firstTree.Count, secondTree.Count);

            for (int i = 0; i < firstTree.Count; i++)
            {
                var first = firstTree[i];
                var second = secondTree[i];

                //Assert.Equal(first.Id, second.Id, $"Не совпадают элементы на уровне {level}"); // NUnit
                Assert.Equal(first.Id, second.Id); // xUnit

                System.Diagnostics.Debug.WriteLine(first.ToString(level));

                CompareTrees(first.Childs, second.Childs, level + 1);
            }
        }

        /// <summary>
        /// Собирает подчинённые узлы для родителя <paramref name="parentId"/> и подчинённые узлы для каждого найденного узла (рекурсивно). 
        /// Таким образом, для родителя <paramref name="parentId"/> будет получена вся низлежащая иерархия.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="originalData"></param>
        /// <returns></returns>
        private List<TreeItem> GetSubLevel(int parentId, List<TreeItem> originalData)
        {
            var result = new List<TreeItem>();
            foreach (var treeItem in originalData.Where(a => a.ParentId == parentId).OrderBy(a => a.Id))
            {
                var newItem = (TreeItem)treeItem.Clone();
                result.Add(newItem);
                newItem.Childs = GetSubLevel(treeItem.Id, originalData);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Тест создания дерева из линейной таблицы через контекстный итератор.
        /// Итератор обходит таблицу, создавая видимость рекурсивного обхода, при этом добавляя подчинённых в каждый текущий элемент.
        /// </summary>
        /// <remarks>
        /// MethodName_WhatsBeingTested_ExpectedResult
        /// </remarks>
        [Fact]
        public void ContextEnumeratorClass_PropertyParent_MustWork()
        {
            // Создать линейный список
            // Сформировать из него дерево посредством тестируемого класса
            // Сфомировать дерево рекурсивно
            // Сравнить два дерева
            var items = TreeItemHelper.Generate_ForParentCheck();

            // classic method
            var recursiveTree = GetSubLevel(0, items);

            // no recursive method
            var noRecursiveTree = new List<TreeItem>();

            // Шаблон формирования дерева с использованием итератора с контекстом
            using (var enumerator = new TreeEnumeratorWithContext<TreeItem>(items.Where(a => a.ParentId == 0), item => items.Where(a => a.ParentId == item.Id).OrderBy(a => a.Id)))
            {
                while (enumerator.MoveNext())
                {
                    var treeItem = enumerator.Current.Value;

                    if (enumerator.Current.Parent != null)
                    {
                        // Если у текущего элемента есть родитель - добавим его к этому родителю
                        enumerator.Current.Parent.Childs.Add(treeItem);
                    }
                    else
                    {
                        // Если у текущего элемента нет родителя, значит, это корневой элемент
                        noRecursiveTree.Add(treeItem);
                    }
                }
            }

            CompareTrees(recursiveTree, noRecursiveTree);
        }

        /// <summary>
        /// Тест создания дерева из линейной таблицы через контекстный итератор.
        /// Итератор обходит таблицу, создавая видимость рекурсивного обхода, при этом добавляя подчинённых в каждый текущий элемент.
        /// </summary>
        /// <remarks>
        /// MethodName_WhatsBeingTested_ExpectedResult
        /// </remarks>
        [Fact]
        public void ContextEnumeratorClassWithAsTreeExtension_PropertyParent_MustWork()
        {
            // Создать линейный список
            // Сформировать из него дерево посредством тестируемого класса
            // Сфомировать дерево рекурсивно
            // Сравнить два дерева
            var items = TreeItemHelper.Generate_ForParentCheck();

            // classic method
            var recursiveTree = GetSubLevel(0, items);

            // no recursive method
            var noRecursiveTree = new List<TreeItem>();

            // Шаблон формирования дерева с использованием итератора с контекстом
            foreach (TreeNodeContext<TreeItem> node in items.Where(a => a.ParentId == 0).AsTreeWithContext(item => items.Where(a => a.ParentId == item.Id).OrderBy(a => a.Id)))
            {
                var treeItem = node.Value;

                if (node.Parent != null)
                {
                    // Если у текущего элемента есть родитель - добавим его к этому родителю
                    node.Parent.Childs.Add(treeItem);
                }
                else
                {
                    // Если у текущего элемента нет родителя, значит, это корневой элемент
                    noRecursiveTree.Add(treeItem);
                }
            }

            CompareTrees(recursiveTree, noRecursiveTree);
        }
    }
}
