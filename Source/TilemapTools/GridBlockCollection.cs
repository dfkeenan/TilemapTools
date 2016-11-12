using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools
{
    public class GridBlockCollection<TGridBlock> : KeyedCollection<ShortPoint, TGridBlock>
        where TGridBlock : class, IGridBlock
    {
        public bool TryGetItem(ShortPoint key, out TGridBlock block)
        {
            if(Dictionary != null)
                return Dictionary.TryGetValue(key, out block);

            block = null;
            return false;
        }

        protected override ShortPoint GetKeyForItem(TGridBlock item) => item.Location;
    }
}
