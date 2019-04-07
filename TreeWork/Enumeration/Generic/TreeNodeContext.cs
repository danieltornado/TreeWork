namespace TreeWork.Enumeration.Generic
{
    public struct TreeNodeContext<T>
    {
        /// <summary>
        /// Возвращает текущий уровень вложенности.
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// Возвращает значение.
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// Возвращает родителя для текущего значения.
        /// </summary>
        public T Parent { get; internal set; }
    }
}
