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
        private Document document;

        public DraftState(Document document)
        {
            this.document = document;
        }

        public void SubmitForApproval(User approver)
        {
            if (document.Approver != null && approver == null)
            {
                Console.WriteLine($"Document '{document.Title} resubmitted to {approver}'");
            }
            else if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = approver;
                approver.Update($"You have been set as the approver for document '{document.Title}'.");

                string message = $"Document '{document.Title}' submitted for review to {approver.Name}.";
                document.NotifyObservers(message, excludeUser: approver);
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
            document.SetState(document.UnderReviewState);

        }

        public void Approve()
        {
            Console.WriteLine("Cannot approve a document in Draft state.");
        }

        public void PushBack(string comments)
        {
            Console.WriteLine("Cannot push back a document in Draft state.");
        }

        public void Reject(string feedback)
        {
            Console.WriteLine("Cannot reject a document in Draft state.");
        }

        public void ResumeEditing()
        {
            Console.WriteLine("Document is already in Draft state.");
        }

        public void EditContent(string newContent)
        {
            document.Content = newContent;
            document.ClearFeedback();

            document.NotifyObservers(
               $"Document '{document.Title}' has been edited.");
        }

        public void AddCollaborator(User collaborator)
        {
            if (document.Owner == collaborator || document.Collaborators.Contains(collaborator))
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
                return;
            }

            document.Collaborators.Add(collaborator);
            document.RegisterObserver(collaborator);

            collaborator.StoreNotification(
                $"You have been added as a collaborator to document '{document.Title}'.");

            Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");

            document.NotifyObservers(
                            $"Collaborator '{collaborator.Name}' added to document '{document.Title}'.", excludeUser: document.Approver);
        }
    }
}