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
        public void SubmitForApproval(Document document, User approver, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot submit an approved document for approval.");
        }

        public void Approve(Document document, List<INotifiable> observers)
        {
            Console.WriteLine("Document is already approved.");
        }

        public void PushBack(Document document, string comments, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot push back an approved document.");
        }

        public void Reject(Document document, string feedbacks, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot reject an approved document.");
        }

        public void ResumeEditing(Document document, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot resume editing an approved document.");
        }

        public void EditContent(Document document, string newContent, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot edit an approved document.");
        }

        public void AddCollaborator(Document document, User collaborator, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot add collaborators in Approved state.");
        }
    }
}