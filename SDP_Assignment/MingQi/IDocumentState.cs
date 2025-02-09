using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.MingQi
{
    public interface IDocumentState
    {
        void SubmitForApproval(Document document, User approver, List<NotifyObserver> observers);
        void Approve(Document document, List<NotifyObserver> observers);
        void PushBack(Document document, string comments, List<NotifyObserver> observers);
        void Reject(Document document, string feedbacks, List<NotifyObserver> observers);
        void ResumeEditing(Document document, List<NotifyObserver> observers);
        void EditContent(Document document, string newContent, List<NotifyObserver> observers);
        void AddCollaborator(Document document, User collaborator, List<NotifyObserver> observers);
    }
}
