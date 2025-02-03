using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public class ConvertDocumentCommand : ICommand
    {
        private Document document;
        private IConverter converter;

        public ConvertDocumentCommand(Document document, IConverter converter)
        {
            this.document = document;   // Correct assignment
            this.converter = converter; // Correct assignment
        }

        public void Execute()
        {
            if (converter != null && document != null)
            {
                converter.Convert(document);
            }
            else
            {
                Console.WriteLine("Conversion failed: Document or converter is null.");
            }
        }

        public void Undo()
        {
            Console.WriteLine($"Undo: Conversion of document '{document.Title}' reverted.");
        }
    }
}

