using SDP_Assignment.Jason;
using SDP_Assignment.MingQi;
using SDP_Assignment.RAEANN;
using SDP_Assignment;
using SDP_Assignment.RAEANN.COMPOSITE;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using System.ComponentModel;

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
    public IDocumentState DraftState { get; private set; }
    public IDocumentState UnderReviewState { get; private set; }
    public IDocumentState ApprovedState { get; private set; }
    public IDocumentState RejectedState { get; private set; }

    public Document(string title, IDocumentComponent header, IDocumentComponent footer, User owner)
    {
        DraftState = new DraftState(this);
        UnderReviewState = new UnderReviewState(this);
        ApprovedState = new ApprovedState(this);
        RejectedState = new RejectedState(this);

        Title = title;
        Header = header ?? new HeaderComponent("DEFAULT HEADER");
        Footer = footer ?? new FooterComponent("DEFAULT FOOTER");
        Owner = owner;
        Collaborators = new List<User>();
        Content = string.Empty;

        currentState = DraftState; // Initial state

        RegisterObserver(owner);
    }

    public void SetState(IDocumentState newState)
    {
        currentState = newState;
    }


    public void RegisterObserver(NotifyObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(NotifyObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(string message, User excludeUser = null)
    {
        foreach (var observer in observers)
        {
            if (observer != excludeUser)  
            {
                observer.Update(message);
            }
        }
    }

    public void SubmitForApproval(User approver)
    {
        currentState.SubmitForApproval(approver);
    }


    public void Approve()
    {
        currentState.Approve();
    }

    public void PushBack(string comments)
    {
        currentState.PushBack(comments);
    }

    public void Reject(string feedback)
    {
        currentState.Reject(feedback);
    }

    public void ResumeEditing()
    {
        currentState.ResumeEditing();
    }

    public void EditContent(User loggedInUser, string newContent)
    {
        currentState.EditContent(newContent);
        Content = newContent;

        if (currentState is RejectedState rejectedState)
        {
            rejectedState.ResumeEditing(); // Reset the "must edit before resubmitting" flag
        }

        Console.WriteLine($"Document '{Title}' has been updated by {(loggedInUser == Owner ? "Owner" : "Collaborator")} {loggedInUser.Name}.");
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
            currentState.AddCollaborator(collaborator);
        }
        else
        {
            Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
        }
    }
}