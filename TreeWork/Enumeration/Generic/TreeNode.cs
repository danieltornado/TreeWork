namespace TreeWork.Enumeration.Generic
{
    public struct TreeNode<T>
    {
        /// <summary>
        /// Возвращает текущий уровень вложенности.
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// Возвращает значение.
        /// </summary>
        public T Value { get; internal set; }
    }
}
