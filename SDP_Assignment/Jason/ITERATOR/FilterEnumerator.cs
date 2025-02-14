using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{
    public class FilterEnumerator : IEnumerator<Document>
    {
        private readonly IEnumerator<Document> baseEnumerator;
        private readonly Func<Document, bool> predicate;

        public FilterEnumerator(IEnumerator<Document> baseEnumerator, Func<Document, bool> predicate)
        {
            this.baseEnumerator = baseEnumerator ?? throw new ArgumentNullException(nameof(baseEnumerator));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public Document Current => baseEnumerator.Current;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (baseEnumerator.MoveNext())
            {
                if (predicate(baseEnumerator.Current))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            baseEnumerator.Reset();
        }

        public void Dispose()
        {
            baseEnumerator.Dispose();
        }
    }

}
