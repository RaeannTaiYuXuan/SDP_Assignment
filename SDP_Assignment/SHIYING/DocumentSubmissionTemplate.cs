using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public abstract class DocumentSubmissionTemplate
    {
        public void SubmitDocument()
        {
            Validate();
            AssignApprover();
            NotifyCollaborators();
            LockEditing();
        }

        protected abstract void Validate();
        protected abstract void AssignApprover();

        private void NotifyCollaborators()
        {
            Console.WriteLine("Notifying all collaborators about submission.");
        }

        private void LockEditing()
        {
            Console.WriteLine("Editing has been locked for this document.");
        }
    }
}