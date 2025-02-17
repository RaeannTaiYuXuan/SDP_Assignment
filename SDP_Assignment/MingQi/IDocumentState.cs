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
        void SubmitForApproval(User approver);
        void Approve();
        void PushBack(string comments);
        void Reject(string feedbacks);
        void ResumeEditing();
        void EditContent(string newContent);
        void AddCollaborator(User collaborator);
    }
}
