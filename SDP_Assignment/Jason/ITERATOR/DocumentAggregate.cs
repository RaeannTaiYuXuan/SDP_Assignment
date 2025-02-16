using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{
    public class DocumentAggregate : IAggregate
    {
        private readonly List<Document> documents;
        public DocumentAggregate(List<Document> documents)
        {
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }

        public int Count => documents.Count;

        public Document GetDocument(int index)
        {
            if (index < 0 || index >= documents.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return documents[index];
        }

        public IEnumerator<Document> GetEnumerator()
        {
            return documents.GetEnumerator();
        }
    }
}
