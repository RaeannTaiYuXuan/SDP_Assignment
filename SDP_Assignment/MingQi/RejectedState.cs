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
        private Document document;
        private bool mustEditBeforeResubmitting = true; // Ensure edit before resubmission

        public RejectedState(Document document)
        {
            this.document = document;
            mustEditBeforeResubmitting = true;
        }

        public void SubmitForApproval(User approver)
        {
            if (mustEditBeforeResubmitting)
            {
                Console.WriteLine("You must edit the document before resubmitting for approval.");
                return;
            }

            if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.Approver = null;
                document.Approver = approver;
                document.SetState(document.UnderReviewState);

                approver.Update($"You have been set as the approver for document '{document.Title}'.");

                string message = $"Document '{document.Title}' submitted for review to {approver.Name}.";
                document.NotifyObservers( message, excludeUser: approver);

                Console.WriteLine($"Document '{document.Title}' has been submitted for review to {approver.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }

        public void Approve()
        {
            Console.WriteLine("Cannot approve a document in Rejected state.");
        }

        public void PushBack(string comments)
        {
            Console.WriteLine("Cannot push back a document in Rejected state.");
        }

        public void Reject(string feedback)
        {
            Console.WriteLine("Document is already in Rejected state.");
        }

        public void ResumeEditing()
        {
            document.SetState(document.DraftState);

            document.NotifyObservers($"Document '{document.Title}' is back in Draft state for editing.");

            Console.WriteLine($"Document '{document.Title}' is now in Draft state and can be edited.");

            mustEditBeforeResubmitting = false; // Reset flag to allow resubmission
        
        }

        public void EditContent(string newContent)
        {
            document.Content = newContent;
            document.ClearFeedback();

            mustEditBeforeResubmitting = false; //  Allow resubmission after editing

            document.NotifyObservers($"Document '{document.Title}' has been updated.");

            Console.WriteLine($"Document '{document.Title}' has been edited and updated.");
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

            Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");

            document.NotifyObservers(
                            $"Collaborator '{collaborator.Name}' added to document '{document.Title}'.", excludeUser: document.Approver);
        }
    }
}
