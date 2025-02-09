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

            document.NotifyObservers(NotificationType.DocumentApproved, $"Document '{document.Title}' has been approved by {document.Approver.Name}.", excludeUser: document.Approver);
   
        }


        public void PushBack(Document document, string comments, List<NotifyObserver> observers)
        {
            document.SetState(new DraftState());
            document.Feedback = comments;

            string message = $"Document '{document.Title}' has been pushed back with comments: {comments}";

            document.NotifyObservers(NotificationType.DocumentPushedBack, message, excludeUser: document.Approver);

        }

        public void Reject(Document document, string feedback, List<NotifyObserver> observers)
        {
            document.SetState(new DraftState());
            document.Approver = null;

            document.NotifyObservers(NotificationType.DocumentRejected, $"Document '{document.Title}' has been rejected with reason: {feedback}", excludeUser: document.Approver);
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
            document.AddCollaborator(document.Owner, collaborator);
        }

    }
}
