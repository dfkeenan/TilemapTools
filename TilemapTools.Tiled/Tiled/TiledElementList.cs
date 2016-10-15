using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TilemapTools.Tiled
{
    public class TiledElementList<T> : KeyedCollection<string, T> where T : ITiledElement
    {
        private Dictionary<string, int> nameCount = new Dictionary<string, int>();

        protected override void InsertItem(int index, T item)
        {
            var name = item.Name ?? "";

            // count duplicate keys
            if (this.Contains(name))
                nameCount[name] += 1;
            else
                nameCount.Add(name, 0);

            base.InsertItem(index, item);
        }

        protected override string GetKeyForItem(T item)
        {
            var name = item.Name ?? "";
            var count = nameCount[name];

            var dupes = 0;

            // For duplicate keys, append a counter
            while (Contains(name))
            {
                name = name + String.Concat(Enumerable.Repeat("_", dupes + 1))
                            + count.ToString();
                dupes++;
            }

            return name;
        }
    }
}