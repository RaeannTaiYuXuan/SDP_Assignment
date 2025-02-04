using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public class AddCollaboratorCommand : ICommand
    {
        private Document document;
        private User collaborator;

        public AddCollaboratorCommand(Document document, User collaborator)
        {
            this.document = document;        // Correct assignment
            this.collaborator = collaborator; // Correct assignment
        }

        public void Execute()
        {
            if (document == null || collaborator == null)
            {
                Console.WriteLine("Document or collaborator is null. Cannot execute command.");
                return;
            }

            document.Collaborators.Add(collaborator);
            Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");
        }

        public void Undo()
        {
            if (document == null || collaborator == null)
            {
                Console.WriteLine("Document or collaborator is null. Cannot undo command.");
                return;
            }

            document.Collaborators.Remove(collaborator);
            Console.WriteLine($"Undo: Collaborator '{collaborator.Name}' removed from document '{document.Title}'.");
        }
    }
}
