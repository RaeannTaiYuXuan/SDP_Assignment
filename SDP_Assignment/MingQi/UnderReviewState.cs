using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.MingQi
{
    public class UnderReviewState : IDocumentState
    {
        public void SubmitForApproval(Document document, User approver, List<NotifyObserver> observers)
        {
            Console.WriteLine("Document is already under review.");
        }

        public void Approve(Document document, List<NotifyObserver> observers)
        {
            document.SetState(new ApprovedState());

            //Notify owner and collaborators
            document.Owner.StoreNotification(NotificationType.DocumentApproved,
                $"Your document '{document.Title}' has been approved.");

            foreach (var collaborator in document.Collaborators)
            {
                collaborator.StoreNotification(NotificationType.DocumentApproved,
                    $"Document '{document.Title}' has been approved.");
            }
        }


        public void PushBack(Document document, string comments, List<NotifyObserver> observers)
        {
            //Notify owner and collaborators
            document.Owner.StoreNotification(NotificationType.DocumentPushedBack,
                $"Your document '{document.Title}' has been pushed back. Comments: {comments}");

            foreach (var collaborator in document.Collaborators)
            {
                collaborator.StoreNotification(NotificationType.DocumentPushedBack,
                    $"Document '{document.Title}' has been pushed back. Comments: {comments}");
            }
        }


        public void Reject(Document document, string feedback, List<NotifyObserver> observers)
        {
            document.SetState(new DraftState());

            //Notify owner and collaborators
            document.Owner.StoreNotification(NotificationType.DocumentRejected,
                $"Your document '{document.Title}' has been rejected. Reason: {feedback}");

            foreach (var collaborator in document.Collaborators)
            {
                collaborator.StoreNotification(NotificationType.DocumentRejected,
                    $"Document '{document.Title}' has been rejected. Reason: {feedback}");
            }
        }


        public void ResumeEditing(Document document, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot resume editing a document under review.");
        }

        public void EditContent(Document document, string newContent, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot edit the document while it's under review.");
        }

        public void AddCollaborator(Document document, User collaborator, List<NotifyObserver> observers)
        {
            if (collaborator != null && collaborator != document.Owner && !document.Collaborators.Contains(collaborator))
            {
                document.Collaborators.Add(collaborator);
                document.AttachObserver(collaborator);
                document.NotifyObservers(NotificationType.DocumentSubmitted, $"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");
            }
            else
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
            }
        }
    }
}
