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
        void SubmitForApproval(Document document, User approver, List<INotifiable> observers);
        void Approve(Document document, List<INotifiable> observers);
        void PushBack(Document document, string comments, List<INotifiable> observers);
        void Reject(Document document, string feedbacks, List<INotifiable> observers);
        void ResumeEditing(Document document, List<INotifiable> observers);
        void EditContent(Document document, string newContent, List<INotifiable> observers);
        void AddCollaborator(Document document, User collaborator, List<INotifiable> observers);
    }
}
