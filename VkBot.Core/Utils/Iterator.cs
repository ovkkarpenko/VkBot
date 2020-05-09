using System.Collections.Generic;

namespace VkBot.Core.Utils
{
    public class Iterator<T>
    {
        private readonly List<T> _items;

        public Iterator(List<T> items)
        {
            _items = items;
        }

        public IEnumerator<T> GetItems()
        {
            foreach (T item in _items)
            {
                yield return item;
            }
        }
    }
}
