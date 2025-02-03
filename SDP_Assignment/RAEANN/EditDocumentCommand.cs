using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public class EditDocumentCommand : ICommand
    {
        private Document document;
        private string newContent;
        private string previousContent;

        public EditDocumentCommand(Document document, string newContent)
        {
            this.document = document;             // Properly assigning the constructor parameter to the class field
            this.newContent = newContent;         // Assigning the new content correctly
            this.previousContent = document.Content;  // Storing the original content for undo
        }

        public void Execute()
        {
            if (document == null)
            {
                Console.WriteLine("Document is null. Cannot execute command.");
                return;
            }

            document.Content = newContent;
            Console.WriteLine($"Document '{document.Title}' updated.");
        }

        public void Undo()
        {
            if (document == null)
            {
                Console.WriteLine("Document is null. Cannot undo command.");
                return;
            }

            document.Content = previousContent;
            Console.WriteLine($"Undo: Document '{document.Title}' reverted to previous content.");
        }
    }
}
