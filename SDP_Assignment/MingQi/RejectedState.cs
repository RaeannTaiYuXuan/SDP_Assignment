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
        public void SubmitForApproval(Document document, User approver, List<NotifyObserver> observers)
        {
            if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = approver;
                document.SetState(new UnderReviewState());
                document.NotifyObservers(NotificationType.DocumentSubmitted, $"Document '{document.Title}' submitted for approval to {approver.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }

        public void Approve(Document document, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot approve a document in Rejected state.");
        }

        public void PushBack(Document document, string comments, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot push back a document in Rejected state.");
        }

        public void Reject(Document document, string feedbacks, List<NotifyObserver> observers)
        {
            Console.WriteLine("Document is already in Rejected state.");
        }

        public void ResumeEditing(Document document, List<NotifyObserver> observers)
        {
            document.SetState(new DraftState());
            document.NotifyObservers(NotificationType.DocumentSubmitted, $"Document '{document.Title}' is back in Draft state for editing.");
        }

        public void EditContent(Document document, string newContent, List<NotifyObserver> observers)
        {
            document.Content = newContent;
            document.ClearFeedback();
            document.NotifyObservers(NotificationType.DocumentSubmitted, $"Document '{document.Title}' has been updated.");
        }

        public void AddCollaborator(Document document, User collaborator, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot add collaborators in Rejected state.");
        }
    }
}