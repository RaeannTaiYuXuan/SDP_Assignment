using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.MingQi
{
    public class ApprovedState : IDocumentState
    {
        private Document document;
        public ApprovedState(Document document)
        {
            this.document = document;
        }

        public void SubmitForApproval(User approver)
        {
            Console.WriteLine("Cannot submit an approved document for approval.");
        }

        public void Approve()
        {
            Console.WriteLine("Document is already approved.");
        }

        public void PushBack(string comments)
        {
            Console.WriteLine("Cannot push back an approved document.");
        }

        public void Reject(string feedbacks)
        {
            Console.WriteLine("Cannot reject an approved document.");
        }

        public void ResumeEditing()
        {
            Console.WriteLine("Cannot resume editing an approved document.");
        }

        public void EditContent(string newContent)
        {
            Console.WriteLine("Cannot edit an approved document.");
        }

        public void AddCollaborator(User collaborator)
        {
            Console.WriteLine("Cannot add collaborators in once approved.");
        }
    }
}