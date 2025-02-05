using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    // abstract decorator class
    public abstract class DocumentDecorator : Document
    {
        protected Document decoratedDocument;

        public DocumentDecorator(Document document)
            : base(document.Title, document.Header, document.Footer, document.Owner)
        {
            this.decoratedDocument = document;
        }

        public override void Display()
        {
            decoratedDocument.Display();
        }
    }
}