﻿using SDP_Assignment.RAEANN;
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
            if (document.Approver != null && approver == null)
            {
                Console.WriteLine($"Document '{document.Title} resubmitted to {approver}'");
            }
            else if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = approver;
                approver.Notify(NotificationType.DocumentSubmitted, $"You have been set as the approver for document '{document.Title}'.");

                string message = $"Document '{document.Title}' submitted for review to {approver.Name}.";
                document.NotifyObservers(NotificationType.DocumentSubmitted, message, excludeUser: approver);
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
            document.SetState(new UnderReviewState());

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
            if (document.Owner == collaborator || document.Collaborators.Contains(collaborator))
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
                return;
            }

            document.Collaborators.Add(collaborator);
            document.AttachObserver(collaborator);

            collaborator.StoreNotification(NotificationType.CollaboratorAdded,
                $"You have been added as a collaborator to document '{document.Title}'.");

            Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");

            document.NotifyObservers(NotificationType.CollaboratorAdded,
                            $"Collaborator '{collaborator.Name}' added to document '{document.Title}'.", excludeUser: document.Approver);
        }
    }
}