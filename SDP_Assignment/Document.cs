using SDP_Assignment.Jason;
using SDP_Assignment.SHIYING;
using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;

namespace SDP_Assignment
{
    public abstract class Document 
    {
        private string title;
        private string header;
        private string content;
        private string footer;
        private User owner;
        private List<User> collaborators;
        private User approver;
        private bool isUnderReview;
        public string Feedback { get; private set; }

        

        // Observer list
        private List<INotifiable> observers = new List<INotifiable>();

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyObservers($"Document '{Title}' has been edited.");
                ClearFeedback();
            }
        }

        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }

        public User Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                AttachObserver(owner);
            }
        }

        public List<User> Collaborators
        {
            get { return collaborators; }
            set
            {
                collaborators = value;
                foreach (var collaborator in collaborators)
                {
                    AttachObserver(collaborator);
                }
            }
        }

        public User Approver
        {
            get { return approver; }
            private set
            {
                approver = value;
                if (approver != null)
                    AttachObserver(approver);
            }
        }

        public bool IsUnderReview
        {
            get { return isUnderReview; }
            private set { isUnderReview = value; }
        }

        public ConvertStrategy ConvertStrategy { get; set; }

        public Document(string title, string header, string footer, User owner)
        {
            Title = title;
            Header = header;
            Footer = footer;
            Owner = owner;
            Collaborators = new List<User>();
            Content = string.Empty;
            IsUnderReview = false;

            AttachObserver(owner);  // Automatically add the owner as an observer
        }

        // Observer Methods
        public void AttachObserver(INotifiable observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void DetachObserver(INotifiable observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        private void NotifyObservers(string message)
        {
            foreach (var observer in observers)
            {
                observer.Notify(message);
            }
        }

        // Document State Management Methods
        public void SubmitForApproval(User approver)
        {
            if (approver != null && approver != Owner && !Collaborators.Contains(approver))
            {
                Approver = approver;
                IsUnderReview = true;
                NotifyObservers($"Document '{Title}' submitted for approval to {approver.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }

        public void Approve()
        {
            IsUnderReview = false;
            NotifyObservers($"Document '{Title}' has been approved by {Approver.Name}.");
        }

        public void PushBack(string comments)
        {
            IsUnderReview = false;
            Feedback = comments;
            NotifyObservers($"Document '{Title}' has been pushed back with comments: {comments}");
        }

        public void ClearFeedback()
        {
            Feedback = null;
        }

        public void Reject()
        {
            IsUnderReview = false;
            NotifyObservers($"Document '{Title}' has been rejected by {Approver.Name}.");
        }


        public void EditContent(string newContent)
        {
            if (IsUnderReview)
            {
                Console.WriteLine("Cannot edit the document while it's under review.");
                return;
            }

            Content = newContent;
            ClearFeedback();  // Clear any previous feedback
            NotifyObservers($"Document '{Title}' has been edited.");
        }


        public virtual void Display()
        {
            Console.WriteLine("====================================");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Header: {Header}");
            Console.WriteLine("Content:");
            Console.WriteLine(Content);
            Console.WriteLine($"Footer: {Footer}");
            Console.WriteLine("====================================");
        }

        public void AddCollaborator(User collaborator)
        {
            if (!Collaborators.Contains(collaborator))
            {
                Collaborators.Add(collaborator);
                AttachObserver(collaborator);
                NotifyObservers($"Collaborator '{collaborator.Name}' added to document '{Title}'.");
            }
        }
    }
}
