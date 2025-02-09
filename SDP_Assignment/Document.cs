using SDP_Assignment.Jason;
using SDP_Assignment.MingQi;
using SDP_Assignment.RAEANN;
using SDP_Assignment;

public abstract class Document : ISubject
{
    private IDocumentState currentState;
    private List<NotifyObserver> observers = new List<NotifyObserver>();

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

        AttachObserver(owner);  // ✅ Automatically add the owner as an observer
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
            if (observer != excludeUser)  // ✅ Avoid notifying excluded user
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
        NotifyObservers(NotificationType.DocumentApproved, $"Document '{Title}' has been approved by {Approver.Name}.");
    }

    public void PushBack(string comments)
    {
        currentState.PushBack(this, comments, observers);
        NotifyObservers(NotificationType.DocumentPushedBack, $"Document '{Title}' has been pushed back with comments: {comments}");
    }

    public void Reject(string feedback)
    {
        currentState.Reject(this, feedback, observers);
        NotifyObservers(NotificationType.DocumentRejected, $"Document '{Title}' has been rejected with reason: {feedback}");
    }

    public void ResumeEditing()
    {
        currentState.ResumeEditing(this, observers);
    }

    public void EditContent(string newContent)
    {
        Content = newContent;
        NotifyObservers(NotificationType.DocumentEdited, $"Document '{Title}' has been edited.");
    }

    public bool IsUnderReview
    {
        get { return currentState is UnderReviewState; }
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

        if (collaborator != null && collaborator != Owner && !Collaborators.Contains(collaborator))
        {
            Collaborators.Add(collaborator);
            AttachObserver(collaborator); // ✅ Ensure the collaborator is added as an observer

            collaborator.StoreNotification(NotificationType.CollaboratorAdded,
                $"You have been added as a collaborator to document '{Title}'.");

            Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{Title}'.");

            NotifyObservers(NotificationType.CollaboratorAdded,
                $"Collaborator '{collaborator.Name}' added to document '{Title}'.", excludeUser: collaborator);
        }
        else
        {
            Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
        }
    }
}
