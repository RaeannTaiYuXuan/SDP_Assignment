// =======================
// Main Program with Extended Document Management
// =======================

using SDP_Assignment;
using SDP_Assignment.Jason;
using SDP_Assignment.Jason.ITERATOR;
using SDP_Assignment.SHIYING;
using SDP_Assignment.SHIYING.DECORATOR;
using SDP_Assignment.MingQi;
using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;
using SDP_Assignment.RAEANN.COMPOSITE;
using System.ComponentModel.DataAnnotations;

class Program
{
    static List<User> users = new List<User>();
    static DocumentManager documentManager = new DocumentManager();
    static User loggedInUser = null;

    static void Main(string[] args)
    {
        // Preload some fake data
        SeedData();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n===== Document Workflow System =====");
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. Login as User");
            Console.WriteLine("3. List Users");
            Console.WriteLine("4. List Documents");
            Console.WriteLine("5. Exit");
            Console.WriteLine("======================================");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateUser();
                    break;
                case "2":
                    LoginUser();
                    if (loggedInUser != null)
                        UserMenu();
                    break;
                case "3":
                    ListUsers();
                    break;
                case "4":
                    ListAllDocuments();
                    break;
                case "5":
                    running = false;
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void CreateUser()
    {
        Console.Write("Enter user name: ");
        string name = Console.ReadLine();
        users.Add(new User(name));
        Console.WriteLine($"User {name} created successfully.");
    }

    static void LoginUser()
    {
        Console.Write("Enter user name to login: ");
        string name = Console.ReadLine();
        loggedInUser = users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (loggedInUser != null)
        {
            Console.WriteLine($"Welcome, {loggedInUser.Name}!");
            loggedInUser.ShowNotifications();
        }
        else
        {
            Console.WriteLine("User not found. Please create a user first.");
        }
    }

    static void ListUsers()
    {
        Console.WriteLine("\nRegistered Users:");
        foreach (var user in users)
            Console.WriteLine($"- {user.Name}");
    }

    // Jason's Iterators Stuff

    public static void ListAllDocuments()
    {
        Console.WriteLine("\nSelect Document Type to List:");
        Console.WriteLine("1. All Documents");
        Console.WriteLine("2. Technical Report");
        Console.WriteLine("3. Grant Proposal");
        Console.Write("Select an option: ");
        string choice = Console.ReadLine();
        Console.WriteLine();

        string typeFilter = choice switch
        {
            "1" => "All",
            "2" => "TechnicalReport",
            "3" => "GrantProposal",
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(typeFilter))
        {
            Console.WriteLine("Invalid option.");
            return;
        }

        var aggregate = documentManager.GetDocumentAggregate();

        using IEnumerator<Document> enumerator = typeFilter == "All"
            ? aggregate.GetEnumerator()
            : new FilterEnumerator(
                aggregate.GetEnumerator(),
                doc => doc.GetType().Name.Equals(typeFilter, StringComparison.OrdinalIgnoreCase)
            );

        Console.WriteLine($"\nDocuments ({(typeFilter == "All" ? "All" : typeFilter)}):");

        while (enumerator.MoveNext())
        {
            Document doc = enumerator.Current;
            string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";
            Console.WriteLine($"- {doc.Title} (Owner: {doc.Owner.Name}, Format: {format})");
        }
    }

    public static void ListOwnedDocuments()
    {
        if (loggedInUser == null)
        {
            Console.WriteLine("No user logged in.");
            return;
        }

        var aggregate = documentManager.GetDocumentAggregate();

        using var enumerator = new FilterEnumerator(
            aggregate.GetEnumerator(),
            doc => doc.Owner == loggedInUser
        );

        Console.WriteLine("\nYour Owned Documents:");

        while (enumerator.MoveNext())
        {
            Document doc = enumerator.Current;
            string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";
            Console.WriteLine($"- {doc.Title}");
            Console.WriteLine($"  Format: {format}");
            if (doc.Collaborators.Any())
            {
                Console.WriteLine($"  Collaborators: {string.Join(", ", doc.Collaborators.Select(c => c.Name))}");
            }
            if (doc.AppliedDecorators.Any())
            {
                Console.WriteLine($"  Enhancements: {string.Join(", ", doc.AppliedDecorators)}");
            }
        }
    }

    public static void ListCollabDocuments()
    {
        if (loggedInUser == null)
        {
            Console.WriteLine("No user logged in.");
            return;
        }

        var aggregate = documentManager.GetDocumentAggregate();

        using var enumerator = new FilterEnumerator(
            aggregate.GetEnumerator(),
            doc => (doc.Collaborators.Contains(loggedInUser) || doc.Approver == loggedInUser)
                   && doc.Owner != loggedInUser
        );

        Console.WriteLine("\nDocuments I'm In:");

        while (enumerator.MoveNext())
        {
            Document doc = enumerator.Current;
            string format = doc.ConvertStrategy?.GetType().Name.Replace("ConvertStrategy", "") ?? "Not Set";
            string role = doc.Approver == loggedInUser ? "Approver" : "Collaborator";
            Console.WriteLine($"- {doc.Title}");
            Console.WriteLine($"  Owner: {doc.Owner.Name}");
            Console.WriteLine($"  Your Role: {role}");
            Console.WriteLine($"  Format: {format}");
            if (doc.AppliedDecorators.Any())
            {
                Console.WriteLine($"  Enhancements: {string.Join(", ", doc.AppliedDecorators)}");
            }
        }
    }


    static void UserMenu()
    {
        NotifyPushedBackDocuments();

        bool userRunning = true;
        while (userRunning)
        {
            Console.WriteLine("\n===== User Menu =====");
            Console.WriteLine("1. Create New Document");
            Console.WriteLine("2. Manage Document");
            Console.WriteLine("3. List My Documents");
            Console.WriteLine("4. Logout");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateDocument();
                    break;
                case "2":
                    ManageDocument();
                    break;
                case "3":
                    // Reuse the same document options prompt as in ListDocuments()
                    Console.WriteLine("\nList My Documents Options:");
                    Console.WriteLine("1. List Owned Documents");
                    Console.WriteLine("2. List Documents I'm In");
                    Console.Write("Select an option: ");
                    string subChoice = Console.ReadLine();
                    Console.WriteLine();

                    if (subChoice == "1")
                        ListOwnedDocuments();
                    else if (subChoice == "2")
                        ListCollabDocuments();
                    else
                        Console.WriteLine("Invalid option.");
                    break;
                case "4":
                    userRunning = false;
                    loggedInUser = null;
                    Console.WriteLine("Logged out successfully.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void NotifyPushedBackDocuments()
    {
        var pushedBackDocs = documentManager.Documents
            .Where(d => (d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser))
                        && !string.IsNullOrEmpty(d.Feedback));
        // Uncomment to show notifications if needed.
        // foreach (var doc in pushedBackDocs)
        // {
        //     Console.WriteLine($"\nNotification: Your document '{doc.Title}' was pushed back: {doc.Feedback}");
        // }
    }

    static void CreateDocument()
    {
        Console.Write("Enter document title: ");
        string title = Console.ReadLine();

        Console.WriteLine("\nSelect document type:");
        Console.WriteLine("1. Technical Report");
        Console.WriteLine("2. Grant Proposal");
        Console.Write("Select an option: ");

        IDocumentFactory factory;
        CompositeComponent header = new CompositeComponent();
        IDocumentComponent footer = new FooterComponent("==== Confidential Footer ====");

        string docTypeChoice = Console.ReadLine();
        switch (docTypeChoice)
        {
            case "1":
                factory = new TechnicalReportFactory();
                header.Add(new HeaderComponent("===== Technical Report ====="));
                header.Add(new HeaderComponent($"Author: {loggedInUser.Name}"));
                break;
            case "2":
                factory = new GrantProposalFactory();
                header.Add(new HeaderComponent("===== Grant Proposal ====="));
                header.Add(new HeaderComponent($"Submitted by: {loggedInUser.Name}"));
                break;
            default:
                throw new ArgumentException("Invalid choice");
        }

        Console.Write("Enter content: ");
        string content = Console.ReadLine();

        // Use the DocumentManager's CreateDocument method.
        Document doc = documentManager.CreateDocument(factory, title, content, loggedInUser, header, footer);

        Console.WriteLine($"{doc.GetType().Name} '{title}' created successfully.");
        doc.Display();
    }

    static void ManageDocument()
    {
        Document doc = SelectUserDocument();
        if (doc == null)
            return;

        bool managing = true;
        while (managing)
        {
            Console.WriteLine("\n===== Manage Document =====");
            Console.WriteLine("1. Edit");
            Console.WriteLine("2. Submit for Review");
            Console.WriteLine("3. Push Back");
            Console.WriteLine("4. Approve");
            Console.WriteLine("5. Reject");
            Console.WriteLine("6. Add Collaborator");
            Console.WriteLine("7. Set File Conversion Type");
            Console.WriteLine("8. Convert File");
            Console.WriteLine("9. Apply Security & Branding");
            Console.WriteLine("10. Stop Managing Document");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    EditDocument(doc);
                    break;
                case "2":
                    SubmitForReview(doc);
                    break;
                case "3":
                    PushBackDocument(doc);
                    break;
                case "4":
                    ApproveDocument(doc);
                    break;
                case "5":
                    RejectDocument(doc);
                    break;
                case "6":
                    AddCollaborator(doc);
                    break;
                case "7":
                    SetFileConversionType(doc);
                    break;
                case "8":
                    ConvertFile(doc);
                    break;
                case "9":
                    doc = ApplyDecorators(doc);
                    break;
                case "10":
                    managing = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void EditDocument(Document document)
    {
        if (document.IsUnderReview)
        {
            Console.WriteLine("Cannot edit - document is under review.");
            return;
        }
        else if (document.IsApproved)
        {
            Console.WriteLine("Cannot edit - document has already been approved.");
            return;
        }
        else
        {
            if (document.Owner == loggedInUser || document.Collaborators.Contains(loggedInUser))
            {
                Console.Write("Enter new content: ");
                string newContent = Console.ReadLine();
                document.EditContent(loggedInUser,newContent);
            }
            else
            {
                Console.WriteLine("Cannot edit - you are not a collaborator.");
            }
        }
    }

    static void AddCollaborator(Document document)
    {
        if (document.Owner != loggedInUser)
        {
            Console.WriteLine("Only the owner can add collaborators.");
            return;
        }

        Console.Write("Enter collaborator name: ");
        string collaboratorName = Console.ReadLine();
        User collaborator = users.FirstOrDefault(u => u.Name.Equals(collaboratorName, StringComparison.OrdinalIgnoreCase));

        if (collaborator != null && collaborator != document.Owner && !document.Collaborators.Contains(collaborator))
        {
            document.AddCollaborator(loggedInUser, collaborator);
        }
        else
        {
            Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
        }
    }

    static void SubmitForReview(Document document)
    {
        if (!document.Collaborators.Contains(loggedInUser) && document.Owner != loggedInUser)
        {
            Console.WriteLine("Cannot submit - you are not a collaborator or owner.");
            return;
        }

        if (document.IsRejected || document.Approver == null)
        {
            Console.Write("Enter new approver name: ");
            string approverName = Console.ReadLine();
            User approver = users.FirstOrDefault(u => u.Name.Equals(approverName, StringComparison.OrdinalIgnoreCase));

            if (approver != null && approver != loggedInUser && approver != document.Owner && !document.Collaborators.Contains(approver))
            {
                document.SubmitForApproval(approver);
            }
            else
            {
                Console.WriteLine("Invalid approver. The approver cannot be the owner or a collaborator.");
            }
        }
        else
        {
            document.SubmitForApproval(document.Approver);
        }
    }

    static void PushBackDocument(Document document)
    {
        if (document.Approver == loggedInUser)
        {
            Console.Write("Enter comments for push back: ");
            string comments = Console.ReadLine();
            document.PushBack(comments);
        }
        else
        {
            Console.WriteLine("Cannot push back - you are not the approver.");
            return;
        }
    }

    static void ApproveDocument(Document document)
    {
        if (document.Approver == loggedInUser)
        {
            document.Approve();
        }
        else
        {
            Console.WriteLine("Cannot approve - you are not the approver.");
        }
    }

    static void RejectDocument(Document document)
    {
        if (document.Approver == loggedInUser)
        {
            Console.WriteLine("Enter rejection reason: ");
            string feedback = Console.ReadLine();
            Console.WriteLine($"Document '{document.Title}' has been rejected by {document.Approver.Name}: {feedback}");
            document.Reject(feedback);
        }
        else
        {
            Console.WriteLine("Cannot reject - you are not the approver.");
        }
    }

    static void SetFileConversionType(Document document)
    {
        Console.WriteLine("Select conversion strategy:");
        Console.WriteLine("1. PDF");
        Console.WriteLine("2. Word");
        string choice = Console.ReadLine();

        ConvertStrategy strategy = choice switch
        {
            "1" => new ConvertToPDF(),
            "2" => new ConvertToWord(),
            _ => throw new ArgumentException("Invalid choice")
        };

        document.ConvertStrategy = strategy;
        Console.WriteLine("Conversion strategy updated successfully.");
    }

    static void ConvertFile(Document document)
    {
        if (document.ConvertStrategy != null)
        {
            string result = document.ConvertStrategy.Convert(document);
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("No file conversion type set. Please set it first.");
        }
    }

    static Document SelectUserDocument()
    {
        var userDocs = documentManager.Documents
            .Where(d => d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser) || d.Approver == loggedInUser)
            .ToList();

        if (!userDocs.Any())
        {
            Console.WriteLine("You have no documents to manage.");
            return null;
        }

        Console.WriteLine("\nSelect a document by title:");
        foreach (var doc in userDocs)
        {
            Console.WriteLine($"- {doc.Title} (Owner: {doc.Owner.Name})");
        }

        Console.Write("Enter document title: ");
        string title = Console.ReadLine();

        return userDocs.FirstOrDefault(d => d.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    static Document ApplyDecorators(Document doc)
    {
        bool decorating = true;
        while (decorating)
        {
            Console.WriteLine("\n===== Apply Document Features =====");
            Console.WriteLine("1. Add Watermark");
            Console.WriteLine("2. Add Digital Signature");
            Console.WriteLine("3. Encrypt Document");
            Console.WriteLine("4. Show Document");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    if (doc.HasDecorator("Watermark"))
                    {
                        Console.WriteLine("Watermark is already applied.");
                    }
                    else
                    {
                        doc = new WatermarkDecorator(doc);
                    }
                    break;
                case "2":
                    if (doc.HasDecorator("Signature"))
                    {
                        Console.WriteLine("Digital signature is already applied.");
                    }
                    else
                    {
                        doc = new SignatureDecorator(doc);
                    }
                    break;
                case "3":
                    if (doc.HasDecorator("Encryption"))
                    {
                        Console.WriteLine("Document is already encrypted.");
                    }
                    else
                    {
                        doc = new EncryptionDecorator(doc);
                    }
                    break;
                case "4":
                    doc.Display();
                    break;
                case "0":
                    decorating = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        return doc; // Return the decorated document
    }

    static void SeedData()
    {
        Console.WriteLine("Loading Test Data\n==================================================");
        // Create three users: Bob, Jeff, and John.
        User bob = new User("Bob");
        User jeff = new User("Jeff");
        User john = new User("John");

        // Add users to the system.
        users.Add(bob);
        users.Add(jeff);
        users.Add(john);

        // Create a Technical Report for Bob.
        CompositeComponent headerBob = new CompositeComponent();
        headerBob.Add(new HeaderComponent("===== Technical Report ====="));
        headerBob.Add(new HeaderComponent("Author: Bob"));
        IDocumentComponent footerBob = new FooterComponent("==== Confidential Footer ====");
        IDocumentFactory techFactory = new TechnicalReportFactory();
        Document bobDoc = techFactory.CreateDocument("Bob Report", "Content for Bob's report.", bob, headerBob, footerBob);
        documentManager.AddDocument(bobDoc);

        // Create a Grant Proposal for Jeff.
        CompositeComponent headerJeff = new CompositeComponent();
        headerJeff.Add(new HeaderComponent("===== Grant Proposal ====="));
        headerJeff.Add(new HeaderComponent("Submitted by: Jeff"));
        IDocumentComponent footerJeff = new FooterComponent("==== Confidential Footer ====");
        IDocumentFactory grantFactory = new GrantProposalFactory();
        Document jeffDoc = grantFactory.CreateDocument("Jeff Grant", "Content for Jeff's proposal.", jeff, headerJeff, footerJeff);
        documentManager.AddDocument(jeffDoc);

        // Create a Technical Report for John.
        CompositeComponent headerJohn = new CompositeComponent();
        headerJohn.Add(new HeaderComponent("===== Technical Report ====="));
        headerJohn.Add(new HeaderComponent("Author: John"));
        IDocumentComponent footerJohn = new FooterComponent("==== Confidential Footer ====");
        Document johnDoc = techFactory.CreateDocument("John Report", "Content for John's report.", john, headerJohn, footerJohn);
        documentManager.AddDocument(johnDoc);

        // Add Jeff as a collaborator to John's document.
        johnDoc.AddCollaborator(john, jeff);

        Console.WriteLine("Users: Jeff, John, Bob");
        Console.WriteLine("Documents: Jeff Grant, John Report, Bob Report");
        Console.WriteLine("==================================================");
        Console.WriteLine("Test Data Created");
    }
}