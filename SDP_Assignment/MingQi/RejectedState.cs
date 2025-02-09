using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.MingQi
{
    public class RejectedState : IDocumentState
    {
        public void SubmitForApproval(Document document, User approver, List<INotifiable> observers)
        {
            if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = approver;
                document.SetState(new UnderReviewState());
                document.NotifyObservers($"Document '{document.Title}' resubmitted for approval to {approver.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }

        public void Approve(Document document, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot approve a document in Rejected state.");
        }

        public void PushBack(Document document, string comments, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot push back a document in Rejected state.");
        }

        public void Reject(Document document, string feedbacks, List<INotifiable> observers)
        {
            Console.WriteLine("Document is already in Rejected state.");
        }

        public void ResumeEditing(Document document, List<INotifiable> observers)
        {
            document.SetState(new DraftState());
            document.NotifyObservers($"Document '{document.Title}' is back in Draft state for editing.");
        }

        public void EditContent(Document document, string newContent, List<INotifiable> observers)
        {
            document.Content = newContent;
            document.ClearFeedback();
            document.NotifyObservers($"Document '{document.Title}' has been updated.");
        }

        public void AddCollaborator(Document document, User collaborator, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot add collaborators in Rejected state.");
        }
    }
}