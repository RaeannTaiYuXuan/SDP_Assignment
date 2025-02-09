using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.MingQi
{
    public class DraftState : IDocumentState
    {
        public void SubmitForApproval(Document document, User approver, List<NotifyObserver> observers)
        {
            if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = approver;
                document.SetState(new UnderReviewState());

                //Ensure approver is only attached once
                if (!observers.Contains(approver))
                {
                    document.AttachObserver(approver);
                }

                //Store notification for the approver instead of printing immediately
                approver.StoreNotification(NotificationType.DocumentSubmitted,
                    $"You have been set as the approver for document '{document.Title}'.");

                //Store notifications for collaborators
                foreach (var observer in observers)
                {
                    if (observer != approver) //Ensure the approver doesn't get the same notification again
                    {
                        observer.StoreNotification(NotificationType.DocumentSubmitted,
                            $"Document '{document.Title}' submitted for review to {approver.Name}.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }


        public void Approve(Document document, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot approve a document in Draft state.");
        }

        public void PushBack(Document document, string comments, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot push back a document in Draft state.");
        }

        public void Reject(Document document, string feedback, List<NotifyObserver> observers)
        {
            Console.WriteLine("Cannot reject a document in Draft state.");
        }

        public void ResumeEditing(Document document, List<NotifyObserver> observers)
        {
            Console.WriteLine("Document is already in Draft state.");
        }

        public void EditContent(Document document, string newContent, List<NotifyObserver> observers)
        {
            document.Content = newContent;
            document.ClearFeedback();

            document.NotifyObservers(NotificationType.DocumentEdited,
               $"Document '{document.Title}' has been edited.");
        }

        public void AddCollaborator(Document document, User collaborator, List<NotifyObserver> observers)
        {
            if (collaborator != null && collaborator != document.Owner && !document.Collaborators.Contains(collaborator))
            {
                document.Collaborators.Add(collaborator);
                document.AttachObserver(collaborator);

                document.NotifyObservers(NotificationType.CollaboratorAdded,
                     $"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");
            }
            else
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
            }
        }
    }
}