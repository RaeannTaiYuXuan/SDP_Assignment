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
        public void SubmitForApproval(Document document, User approver, List<INotifiable> observers)
        {
            Console.WriteLine("Document is already under review.");
        }

        public void Approve(Document document, List<INotifiable> observers)
        {
            document.SetState(new ApprovedState());
            document.NotifyObservers($"Document '{document.Title}' has been approved by {document.Approver.Name}.");
        }

        public void PushBack(Document document, string comments, List<INotifiable> observers)
        {
            document.SetState(new DraftState());
            document.Feedback = comments;
            document.NotifyObservers($"Document '{document.Title}' has been pushed back with comments: {comments}");
        }

        public void Reject(Document document, string feedbacks, List<INotifiable> observers)
        {
            document.SetState(new RejectedState());
            document.NotifyObservers($"Document '{document.Title}' has been rejected with feedback: {feedbacks}.");
        }

        public void ResumeEditing(Document document, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot resume editing a document under review.");
        }

        public void EditContent(Document document, string newContent, List<INotifiable> observers)
        {
            Console.WriteLine("Cannot edit the document while it's under review.");
        }

        public void AddCollaborator(Document document, User collaborator, List<INotifiable> observers)
        {
            if (collaborator != null && collaborator != document.Owner && !document.Collaborators.Contains(collaborator))
            {
                document.Collaborators.Add(collaborator);
                document.AttachObserver(collaborator);
                document.NotifyObservers($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");
            }
            else
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
            }
        }
    }
}
