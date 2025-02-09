using SDP_Assignment.Jason;
using SDP_Assignment.MingQi;
using SDP_Assignment.RAEANN;
using SDP_Assignment;
using SDP_Assignment.RAEANN.COMPOSITE;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;

public abstract class Document : ISubject
{
    private IDocumentState currentState;
    private List<NotifyObserver> observers = new List<NotifyObserver>();

    private string title;
    private string content;
    private IDocumentComponent header;
    private IDocumentComponent footer;
    private User owner;
    private List<User> collaborators;
    private User approver;
    private string feedback;

    // Track applied decorators======================================
    private List<string> appliedDecorators = new List<string>();
    public List<string> AppliedDecorators => appliedDecorators;
    //================================================================


    public string Title { get; set; }

    public string Content { get; set; }
    public IDocumentComponent Header { get; set; }
    public IDocumentComponent Footer { get; set; }
    public User Owner { get; set; }
    public List<User> Collaborators { get; set; }
    public User Approver { get; internal set; }
    public string Feedback { get; internal set; }
    public ConvertStrategy ConvertStrategy { get; set; }

    public Document(string title, IDocumentComponent header, IDocumentComponent footer, User owner)
    {
        Title = title;
        Header = header ?? new HeaderComponent("DEFAULT HEADER");
        Footer = footer ?? new FooterComponent("DEFAULT FOOTER");
        Owner = owner;
        Collaborators = new List<User>();
        Content = string.Empty;
        currentState = new DraftState(); // Initial state

        AttachObserver(owner);
    }

    public void SetState(IDocumentState newState)
    {
        currentState = newState;
    }


    public void AttachObserver(NotifyObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void DetachObserver(NotifyObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(NotificationType type, string message, User excludeUser = null)
    {
        foreach (var observer in observers)
        {
            if (observer != excludeUser)  
            {
                observer.Notify(type, message);
            }
        }
    }

    public void SubmitForApproval(User approver)
    {
        currentState.SubmitForApproval(this, approver, observers);
    }


    public void Approve()
    {
        currentState.Approve(this, observers);
        //NotifyObservers(NotificationType.DocumentApproved, $"Document '{Title}' has been approved by {Approver.Name}.");
    }

    public void PushBack(string comments)
    {
        currentState.PushBack(this, comments, observers);
        //NotifyObservers(NotificationType.DocumentPushedBack, $"Document '{Title}' has been pushed back with comments: {comments}");

    }

    public void Reject(string feedback)
    {
        currentState.Reject(this, feedback, observers);
        //NotifyObservers(NotificationType.DocumentRejected, $"Document '{Title}' has been rejected with reason: {feedback}");

    }

    public void ResumeEditing()
    {
        currentState.ResumeEditing(this, observers);
    }

    public void EditContent(string newContent)
    {
        Content = newContent;
        NotifyObservers(NotificationType.DocumentEdited, $"Document '{Title}' has been edited.");
        Display();
    }

    public bool IsUnderReview
    {
        get { return currentState is UnderReviewState; }
    }
    public bool IsApproved
    {
        get { return currentState is ApprovedState; }
    }
    public bool IsRejected
    {
        get { return currentState is RejectedState; }
    }

    public void ClearFeedback()
    {
        Feedback = null;
    }

    public virtual void Display()
    {

        Console.WriteLine(Header.Render()); 
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine("Content:");
        Console.WriteLine(Content);
        Console.WriteLine(Footer.Render());

        // Show applied decorators
        if (appliedDecorators.Count > 0)
        {
            Console.WriteLine($"Enhancements: {string.Join(", ", appliedDecorators)}");
        }
        else
        {
            Console.WriteLine("No enhancements applied.");
        }

        Console.WriteLine("======================");
    }

    public bool HasDecorator(string decorator)
    {
        return appliedDecorators.Contains(decorator);
    }

    public void AddDecorator(string decorator)
    {
        if (!appliedDecorators.Contains(decorator))
        {
            appliedDecorators.Add(decorator);
        }
    }



    public void AddCollaborator(User loggedInUser, User collaborator)
    {

        if (Owner != loggedInUser)
        {
            Console.WriteLine("Only the owner can add collaborators.");
            return;
        }

        if (collaborator != null && collaborator != Owner && !Collaborators.Contains(collaborator))
        {
            currentState.AddCollaborator(this, collaborator, observers);
        }
        else
        {
            Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
        }
    }

}