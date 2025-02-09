using SDP_Assignment.Jason;
using SDP_Assignment.MingQi;
using SDP_Assignment.RAEANN;
using SDP_Assignment;

public abstract class Document
{
    private IDocumentState currentState;
    private List<INotifiable> observers = new List<INotifiable>();

    private string title;
    private string header;
    private string content;
    private string footer;
    private User owner;
    private List<User> collaborators;
    private User approver;
    private string feedback;

    public string Title { get; set; }
    public string Header { get; set; }
    public string Content { get; set; }
    public string Footer { get; set; }
    public User Owner { get; set; }
    public List<User> Collaborators { get; set; }
    public User Approver { get; internal set; }
    public string Feedback { get; internal set; }
    public ConvertStrategy ConvertStrategy { get; set; }

    public Document(string title, string header, string footer, User owner)
    {
        Title = title;
        Header = header;
        Footer = footer;
        Owner = owner;
        Collaborators = new List<User>();
        Content = string.Empty;
        currentState = new DraftState(); // Initial state

        AttachObserver(owner);  // Automatically add the owner as an observer
    }

    // State methods
    public void SetState(IDocumentState newState)
    {
        currentState = newState;

        if (newState is UnderReviewState)
        {
            Console.WriteLine($"Document '{Title}' is now under review.");
        }
        else if (newState is DraftState)
        {
            Console.WriteLine($"Document '{Title}' is back in draft state.");
        }
    }

    public void SubmitForApproval(User approver)
    {
        currentState.SubmitForApproval(this, approver, observers);
    }

    public void Approve()
    {
        currentState.Approve(this, observers);
    }

    public void PushBack(string comments)
    {
        currentState.PushBack(this, comments, observers);
    }

    public void Reject(string feedback)
    {
        currentState.Reject(this, feedback, observers);
    }

    public void ResumeEditing()
    {
        currentState.ResumeEditing(this, observers);
    }

    public void EditContent(string newContent)
    {
        currentState.EditContent(this, newContent, observers);
    }

    public bool IsUnderReview
    {
        get { return currentState is UnderReviewState; }
    }

    public bool IsRejected
    {
        get { return currentState is RejectedState; }
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

    internal void NotifyObservers(string message)
    {
        foreach (var observer in observers)
        {
            observer.Notify(message);
        }
    }

    public void ClearFeedback()
    {
        Feedback = null;
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

    public void AddCollaborator(User loggedInUser, User collaborator)
    {
        if (Owner != loggedInUser)
        {
            Console.WriteLine("Only the owner can add collaborators.");
            return;
        }

        currentState.AddCollaborator(this, collaborator, observers);
    }
}