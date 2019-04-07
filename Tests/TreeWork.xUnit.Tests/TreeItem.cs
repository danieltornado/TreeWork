using System;
using System.Collections.Generic;
using System.Text;

namespace TreeWork.xUnit.Tests
{
    internal class TreeItem : ICloneable
    {
        public int Id;
        public int ParentId;
        public string Name;
        public int Level;

        public List<TreeItem> Childs = new List<TreeItem>();

        /// <summary>
        /// Возвращает строку, представляющую текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return $"{{{Id}: {Name}}}";
        }

        /// <summary>
        /// Создает новый объект, являющийся копией текущего экземпляра.
        /// </summary>
        /// <returns>
        /// Новый объект, являющийся копией этого экземпляра.
        /// </returns>
        public object Clone()
        {
            return new TreeItem() { Id = Id, ParentId = Id, Name = Name };
        }

        public string ToString(int level)
        {
            var str = ToString();
            return str.PadLeft(str.Length + level, ' ');
        }
    }
}
