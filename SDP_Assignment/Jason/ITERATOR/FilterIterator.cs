using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{
    public class FilterIterator : IIterator
    {
        private IIterator baseIterator;

        // A predicate function that determines whether an item should be returned.
        private Func<object, bool> predicate;

        // Stores next item
        private object nextItem;

        // Checks if the next item has been computed.
        private bool computedNext;

        public FilterIterator(IIterator baseIterator, Func<object, bool> predicate)
        {
            this.baseIterator = baseIterator; 
            this.predicate = predicate;         
        }

        public bool HasNext()
        {
            if (!computedNext) 
            {

                while (baseIterator.HasNext())
                {
                    var item = baseIterator.Next(); 
                    if (predicate(item)) 
                    {
                        nextItem = item;    // Store the matching item
                        computedNext = true; // Mark that the next item has been computed
                        return true;         // A matching item is found
                    }
                }
                return false; 
            }
            return true; 
        }

        public object Next()
        {
            if (!computedNext && !HasNext()) 
            {
                throw new InvalidOperationException("No more items."); 
            }
            computedNext = false; 
            return nextItem;     
        }
    }
}
