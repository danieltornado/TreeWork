using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TreeWork.xUnit.Tests
{
    internal class TreeItemHelper
    {
        /// <summary>
        /// Сгенерировать данные для теста
        /// </summary>
        /// <returns></returns>
        public static List<TreeItem> Generate_ForParentCheck()
        {
            var items = new List<TreeItem>()
            {
                new TreeItem() { Id = 1, ParentId = 0, Name = "Head 1", Level = 0 },
                new TreeItem() { Id = 2, ParentId = 0, Name = "Head 2", Level = 0 },
                new TreeItem() { Id = 3, ParentId = 0, Name = "Head 3", Level = 0 },

                new TreeItem() { Id = 4, ParentId = 3, Name = "Sub 1 head 3", Level = 1 },
                new TreeItem() { Id = 5, ParentId = 3, Name = "Sub 2 head 3", Level = 1 },
                new TreeItem() { Id = 6, ParentId = 3, Name = "Sub 3 head 3", Level = 1 },

                new TreeItem() { Id = 7, ParentId = 6, Name = "Sub 1 sub 3", Level = 2 },
                new TreeItem() { Id = 8, ParentId = 6, Name = "Sub 2 sub 3", Level = 2 },
                new TreeItem() { Id = 9, ParentId = 6, Name = "Sub 3 sub 3", Level = 2 },

                new TreeItem() { Id = 10, ParentId = 9, Name = "Sub 1 sub 3 sub 3", Level = 3 },
                new TreeItem() { Id = 11, ParentId = 9, Name = "Sub 2 sub 3 sub 3", Level = 3 },
                new TreeItem() { Id = 12, ParentId = 9, Name = "Sub 3 sub 3 sub 3", Level = 3 },

                new TreeItem() { Id = 13, ParentId = 0, Name = "Head 4", Level = 0 },

                new TreeItem() { Id = 14, ParentId = 1, Name = "Sub 4 head 1", Level = 1 },
            };

            return items;
        }

        /// <summary>
        /// Генерация данных для теста метода Dispose
        /// </summary>
        /// <returns></returns>
        public static List<TreeItem> Generate_ForDisposeTest()
        {
            var items = new List<TreeItem>()
            {
                new TreeItem() { Id = 1, ParentId = 0, Name = "Head 1" },
                new TreeItem() { Id = 2, ParentId = 1, Name = "Sub 1 Head 1" },
                new TreeItem() { Id = 3, ParentId = 2, Name = "Sub 1 Head 2" },
                new TreeItem() { Id = 4, ParentId = 3, Name = "Sub 1 Head 3" },
                new TreeItem() { Id = 5, ParentId = 4, Name = "Sub 1 Head 4" },
            };

            return items;
        }
    }
}
