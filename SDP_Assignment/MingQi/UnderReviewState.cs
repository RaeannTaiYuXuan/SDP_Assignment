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
        private Document document;
        public UnderReviewState(Document document)
        {
            this.document = document;
        }

        public void SubmitForApproval(User approver)
        {
            Console.WriteLine("Document is already under review.");
        }

        public void Approve()
        {
            document.SetState(document.ApprovedState);

            document.NotifyObservers($"Document '{document.Title}' has been approved by {document.Approver.Name}.", excludeUser: document.Approver);
   
        }

        public void PushBack(string comments)
        {
            document.SetState(document.DraftState);
            document.Feedback = comments;

            string message = $"Document '{document.Title}' has been pushed back with comments: {comments}";

            document.NotifyObservers(message, excludeUser: document.Approver);

        }

        public void Reject(string feedback)
        {
            document.SetState(document.RejectedState);
            document.Approver = null;

            document.NotifyObservers($"Document '{document.Title}' has been rejected with reason: {feedback}", excludeUser: document.Approver);
        }


        public void ResumeEditing()
        {
            Console.WriteLine("Cannot resume editing a document under review.");
        }

        public void EditContent(string newContent)
        {
            Console.WriteLine("Cannot edit the document while it's under review.");
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
