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

        // ---------------------------
        // Iterator Pattern Methods
        // ---------------------------

        // List all documents with optional filtering by document type.
        // This method prompts the user to select the document type and prints the documents.
        public void ListAllDocuments()
        {
            // Prompt the user for an option.
            Console.WriteLine("\nSelect Document Type to List:");
            Console.WriteLine("1. All Documents");
            Console.WriteLine("2. Technical Report");
            Console.WriteLine("3. Grant Proposal");
            Console.Write("Select an option: ");
            string? choice = Console.ReadLine();
            Console.WriteLine();

            // Set filter type based on the choice.
            string typeFilter = choice switch
            {
                "1" => "All",
                "2" => "TechnicalReport",
                "3" => "GrantProposal",
                _ => string.Empty
            };

            // Display error if an invalid option was chosen.
            if (string.IsNullOrEmpty(typeFilter))
            {
                Console.WriteLine("Invalid option.");
                return;
            }

            // Create an aggregate from the documents list.
            var aggregate = new DocumentAggregate(documents);

            // Get an enumerator; if "All" is selected, use the base enumerator,
            // otherwise use the FilterEnumerator to filter documents by type.
            using IEnumerator<Document> enumerator = typeFilter == "All"
                ? aggregate.GetEnumerator()
                : new FilterEnumerator(
                    aggregate.GetEnumerator(),
                    doc => doc.GetType().Name.Equals(typeFilter, StringComparison.OrdinalIgnoreCase)
                );

            Console.WriteLine($"\nDocuments ({(typeFilter == "All" ? "All" : typeFilter)}):");

            // Iterate through the enumerator and print document details.
            while (enumerator.MoveNext())
            {
                Document doc = enumerator.Current;
                string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";
                Console.WriteLine($"- {doc.Title} (Owner: {doc.Owner.Name}, Format: {format})");
            }
        }

        // List documents owned by the specified user.
        public void ListOwnedDocuments(User user)
        {
            if (user == null)
            {
                Console.WriteLine("No user specified.");
                return;
            }

            var aggregate = new DocumentAggregate(documents);
            // Create a FilterEnumerator to select documents where the owner is the user.
            using var enumerator = new FilterEnumerator(
                aggregate.GetEnumerator(),
                doc => doc.Owner == user
            );

            Console.WriteLine("\nYour Owned Documents:");

            while (enumerator.MoveNext())
            {
                Document doc = enumerator.Current;
                string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";

                Console.WriteLine($"- {doc.Title}");
                Console.WriteLine($"  Format: {format}");
                if (doc.Collaborators.Any())
                {
                    Console.WriteLine($"  Collaborators: {string.Join(", ", doc.Collaborators.Select(c => c.Name))}");
                }
                if (doc.AppliedDecorators.Any())
                {
                    Console.WriteLine($"  Enhancements: {string.Join(", ", doc.AppliedDecorators)}");
                }
            }
        }

        // List documents where the specified user is a collaborator or approver (but not the owner).
        public void ListCollabDocuments(User user)
        {
            if (user == null)
            {
                Console.WriteLine("No user specified.");
                return;
            }

            var aggregate = new DocumentAggregate(documents);
            // Create a FilterEnumerator to select documents where the user is in Collaborators or is the Approver,
            // but not the owner.
            using var enumerator = new FilterEnumerator(
                aggregate.GetEnumerator(),
                doc => (doc.Collaborators.Contains(user) || doc.Approver == user)
                       && doc.Owner != user
            );

            Console.WriteLine("\nDocuments I'm In:");

            while (enumerator.MoveNext())
            {
                Document doc = enumerator.Current;
                string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";
                string role = doc.Approver == user ? "Approver" : "Collaborator";

                Console.WriteLine($"- {doc.Title}");
                Console.WriteLine($"  Owner: {doc.Owner.Name}");
                Console.WriteLine($"  Your Role: {role}");
                Console.WriteLine($"  Format: {format}");
                if (doc.AppliedDecorators.Any())
                {
                    Console.WriteLine($"  Enhancements: {string.Join(", ", doc.AppliedDecorators)}");
                }
            }
        }
    }
}
