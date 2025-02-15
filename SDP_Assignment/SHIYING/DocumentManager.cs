using System;
using System.Collections.Generic;
using System.Linq;
using SDP_Assignment.Jason.ITERATOR;
using SDP_Assignment.RAEANN.COMPOSITE;
using SDP_Assignment.SHIYING;

namespace SDP_Assignment
{
    public class DocumentManager
    {
        // Internal storage for documents.
        private readonly List<Document> documents = new List<Document>();

        // Expose documents as a read-only list.
        public IReadOnlyList<Document> Documents => documents.AsReadOnly();

        // Add an already created document.
        public void AddDocument(Document doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            documents.Add(doc);
        }

        // Remove a document.
        public bool RemoveDocument(Document doc)
        {
            return documents.Remove(doc);
        }

        // Create a document using the provided factory and add it to the manager.
        public Document CreateDocument(
            IDocumentFactory factory,
            string title,
            string content,
            User owner,
            IDocumentComponent header,
            IDocumentComponent footer)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            Document doc = factory.CreateDocument(title, content, owner, header, footer);
            documents.Add(doc);
            return doc;
        }

        // Get a DocumentAggregate for iterating over the documents.
        public DocumentAggregate GetDocumentAggregate()
        {
            return new DocumentAggregate(documents);
        }

        // Optional: Get a document by title.
        public Document GetDocumentByTitle(string title)
        {
            return documents.FirstOrDefault(d => d.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}
