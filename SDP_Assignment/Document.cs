using SDP_Assignment.Jason;
using SDP_Assignment.SHIYING;
using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;

namespace SDP_Assignment
{
    public class Document : IDocument
    {
        private string title;
        private string header;
        private string content;
        private string footer;
        private User owner;
        private List<User> collaborators;
        private User approver;
        private bool isUnderReview;
        public string Feedback { get; private set; }  // New property to store feedback

        // Add the Converter Property
        public IConverter Converter { get; set; }  // This allows setting PDF or Word converter dynamically

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
            set { content = value; }
        }

        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }

        public User Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public List<User> Collaborators
        {
            get { return collaborators; }
            set { collaborators = value; }
        }

        public User Approver
        {
            get { return approver; }
            private set { approver = value; }
        }

        public bool IsUnderReview
        {
            get { return isUnderReview; }
            private set { isUnderReview = value; }
        }

        public IConversionStrategy ConversionStrategy { get; set; }

        public Document(string title, string header, string footer, User owner)
        {
            Title = title;
            Header = header;
            Footer = footer;
            Owner = owner;
            Collaborators = new List<User>();
            Content = string.Empty;
            IsUnderReview = false;
            Converter = null;  // Initialize Converter as null, to be set later
        }

        public void SubmitForApproval(User approver)
        {
            if (approver != null && approver != Owner && !Collaborators.Contains(approver))
            {
                Approver = approver;
                IsUnderReview = true;
                Console.WriteLine($"Document '{Title}' submitted for approval to {approver.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid approver. Approver cannot be the owner or a collaborator.");
            }
        }


        public void Approve()
        {
            IsUnderReview = false;
            Console.WriteLine($"Document '{Title}' has been approved.");
        }

        public void PushBack(string comments)
        {
            IsUnderReview = false;
            Feedback = comments;  // Store feedback when document is pushed back
            Console.WriteLine($"Document '{Title}' has been pushed back with comments: {comments}");
        }

        public void ClearFeedback()
        {
            Feedback = null;  // Clear feedback when the document is edited
        }

        public void Reject()
        {
            IsUnderReview = false;
            Approver = null;
            Console.WriteLine($"Document '{Title}' has been rejected.");
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
    }
}
