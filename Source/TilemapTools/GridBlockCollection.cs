using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools
{
    public class GridBlockCollection<TTileDefinition> : KeyedCollection<ShortPoint, GridBlock<TTileDefinition>>
        where TTileDefinition : class
    {
        public bool TryGetItem(ShortPoint key, out GridBlock<TTileDefinition> block) => Dictionary.TryGetValue(key, out block);

        protected override ShortPoint GetKeyForItem(GridBlock<TTileDefinition> item) => item.Key;
    }
}
