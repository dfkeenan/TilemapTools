using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools
{
    /// <summary>
    /// A keyed collection of <see cref="IGridBlock"/> using <see cref="IGridBlock.Location"/> as the key.
    /// </summary>
    /// <typeparam name="TGridBlock"></typeparam>
    public class GridBlockCollection<TGridBlock> : KeyedCollection<ShortPoint, TGridBlock>
        where TGridBlock : class, IGridBlock
    {
        /// <summary>
        /// Gets the item associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="block">When this method returns, contains the item associated with the specified key, if the key is found; otherwise, the default value for the type of the item parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="GridBlockCollection{TGridBlock}"/> contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetItem(ShortPoint key, out TGridBlock block)
        {
            if(Dictionary != null)
                return Dictionary.TryGetValue(key, out block);

            block = this.FirstOrDefault( b => b.Location == key);
            return block != null;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate, optionally disposing them.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{TGridBlock}"/> delegate that defines the conditions of the elements to remove.</param>
        /// <param name="dispose">When <c>true</c> disposes grid block on removal./></param>
        /// <returns>The number of elements removed from the <see cref="GridBlockCollection{TGridBlock}"/>.</returns>
        /// <exception cref="ArgumentNullException">match is <c>null</c></exception>
        public int RemoveAll(Predicate<TGridBlock> match, bool dispose = true)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match)); 

            int count = 0;

            for (int i = this.Count - 1; i >= 0; i--)
            {
                if(match(this[i]))
                {
                    if(dispose)
                    {
                        this[i].Dispose();
                    }

                    RemoveAt(i);
                    count++;
                }

            }

            return count;
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="GridBlockCollection{TGridBlock}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{TGridBlock}"/> delegate to perform on each element of the <see cref="GridBlockCollection{TGridBlock}"/>.</param>
        /// <exception cref="ArgumentNullException">action is <c>null</c></exception>
        public void ForEach(Action<TGridBlock> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in this)
            {
                action(item);
            }
        }


        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override ShortPoint GetKeyForItem(TGridBlock item) => item.Location;
    }
}
