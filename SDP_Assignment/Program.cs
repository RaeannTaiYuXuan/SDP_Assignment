// =======================
// Main Program with Extended Document Management
// =======================

using SDP_Assignment.RAEANN;
using SDP_Assignment;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using SDP_Assignment.Jason;
using SDP_Assignment.SHIYING;

class Program
{
    static List<User> users = new List<User>();
    static List<Document> documents = new List<Document>();
    static User loggedInUser = null;

    static void Main(string[] args)
    {

        //====================================================================================== first display ==================================================================================
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n===== Document Workflow System =====");
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. Login as User");
            Console.WriteLine("3. List Users");
            Console.WriteLine("4. List Documents");
            Console.WriteLine("5. Exit");
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
                    ListDocuments();
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


    //====================================================================================== methods for users =======================================================================

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
            Console.WriteLine($"Welcome, {loggedInUser.Name}!");
        else
            Console.WriteLine("User not found. Please create a user first.");
    }

    static void ListUsers()
    {
        Console.WriteLine("\nRegistered Users:");
        foreach (var user in users)
            Console.WriteLine($"- {user.Name}");
    }

    //====================================================================================== methods for documents ===================================================================

    static void ListDocuments()
    {
        Console.WriteLine("\n===== List Documents =====");
        Console.WriteLine("1. List All Documents");
        Console.WriteLine("2. List My Owned Documents");
        Console.Write("Select an option: ");

        string choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                ListAllDocuments();
                break;
            case "2":
                ListOwnedDocuments();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    static void ListAllDocuments()
    {
        Console.WriteLine("\nAvailable Documents:");

        if (documents.Any())
        {
            foreach (var doc in documents)
            {
                string format = doc.Converter != null ? doc.Converter.GetType().Name.Replace("Converter", "") : "Not Set";
                Console.WriteLine($"- {doc.Title} (Owner: {doc.Owner.Name}, Format: {format})");
            }
        }
        else
        {
            Console.WriteLine("No documents available.");
        }
    }


    static void ListOwnedDocuments()
    {
        var ownedDocs = documents.Where(d => d.Owner == loggedInUser).ToList();

        Console.WriteLine("\nYour Owned Documents:");

        if (ownedDocs.Any())
        {
            foreach (var doc in ownedDocs)
            {
                string format = doc.Converter != null ? doc.Converter.GetType().Name.Replace("Converter", "") : "Not Set";
                Console.WriteLine($"- {doc.Title} (Format: {format})");
            }
        }
        else
        {
            Console.WriteLine("You do not own any documents.");
        }
    }

    static void ShowDocumentContents(Document document)
    {
        Console.WriteLine($"Title: {document.Title}");
        Console.WriteLine($"Header: {document.Header}");
        Console.WriteLine($"Content: {document.Content}");
        Console.WriteLine($"Footer: {document.Footer}");
    }

    static void ListUserDocuments()
    {
        var userDocs = documents.Where(d => d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser) || d.Approver == loggedInUser);
        Console.WriteLine("\nYour Documents:");

        foreach (var doc in userDocs)
        {
            Console.WriteLine($"- {doc.Title} (Owner: {doc.Owner.Name})");

            if (doc.Collaborators.Any())
            {
                Console.WriteLine("  Collaborators:");
                foreach (var collaborator in doc.Collaborators)
                {
                    Console.WriteLine($"    - {collaborator.Name}");
                }
            }
            else
            {
                Console.WriteLine("  No collaborators.");
            }

            if (doc.Approver != null)
            {
                Console.WriteLine($"  Approver: {doc.Approver.Name}");
            }
        }
    }

    static Document SelectUserDocument()
    {
        // Include documents where the logged-in user is the Owner, Collaborator, or Approver
        var userDocs = documents.Where(d => d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser) || d.Approver == loggedInUser).ToList();

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

        Document selectedDoc = userDocs.FirstOrDefault(d => d.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (selectedDoc == null)
        {
            Console.WriteLine("Document not found or you do not have access.");
        }

        return selectedDoc;

    }

    static void EditDocument(Document document, CommandManager commandManager)
    {
        if (document.IsUnderReview)
        {
            Console.WriteLine("This document is under review. No edits are allowed.");
        }
        else if (document.Owner == loggedInUser || document.Collaborators.Contains(loggedInUser))
        {
            if (!string.IsNullOrEmpty(document.Feedback))
            {
                Console.WriteLine($"Feedback from approver: {document.Feedback}");
            }

            Console.Write("Enter new content: ");
            string newContent = Console.ReadLine();
            ICommand editCommand = new EditDocumentCommand(document, newContent);
            commandManager.ExecuteCommand(editCommand);

            document.ClearFeedback();  // Clear feedback after editing
            Console.WriteLine("Document updated successfully, and feedback cleared.");
        }
        else
        {
            Console.WriteLine("You do not have permission to edit this document.");
        }
    }


    //====================================================================================== user menu when logged in================================================================

    static void UserMenu()
    {
        CommandManager commandManager = new CommandManager();
        NotifyPushedBackDocuments();  // Notify users about pushed-back documents

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
                    ManageDocument(commandManager);
                    break;
                case "3":
                    ListUserDocuments();
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

    //====================================================================================== nottify pushed back documents =================================================================

    static void NotifyPushedBackDocuments()
    {
        var pushedBackDocs = documents.Where(d => (d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser)) && !string.IsNullOrEmpty(d.Feedback));

        foreach (var doc in pushedBackDocs)
        {
            Console.WriteLine($"\nNotification: Your document '{doc.Title}' has been pushed back with comments: {doc.Feedback}");
        }
    }


    //====================================================================================== creating, managing and approval methods =======================================================

    static void CreateDocument()
    {
        Console.Write("Enter document title: ");
        string title = Console.ReadLine();
        Console.WriteLine("\nSelect document type:");
        Console.WriteLine("1. Technical Report");
        Console.WriteLine("2. Grant Proposal");
        Console.Write("Select an option: ");

        string type = Console.ReadLine() switch
        {
            "1" => "technical",
            "2" => "grant",
            _ => throw new ArgumentException("Invalid choice")
        };

        Console.Write("Enter content: ");
        string content = Console.ReadLine();

        var factory = DocumentCreator.GetFactory(type);
        Document doc = factory.CreateDocument(title, content, loggedInUser);
        documents.Add(doc);
        Console.WriteLine($"{type.ToUpper()} document '{title}' created successfully.");
    }

    static void ManageDocument(CommandManager commandManager)
    {
        Document docToManage = SelectUserDocument();
        if (docToManage != null)
        {
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
                Console.WriteLine("8. Produce Converted File");
                Console.WriteLine("9. Show Document Contents");
                Console.WriteLine("10. Stop Managing Document");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        EditDocument(docToManage, commandManager);  // Refactored to separate method
                        break;

                    case "2":
                        SubmitForReview(docToManage);
                        break;

                    case "3":
                        PushBackDocument(docToManage);
                        break;

                    case "4":
                        ApproveDocument(docToManage);
                        break;

                    case "5":
                        RejectDocument(docToManage);
                        break;

                    case "6":
                        AddCollaborator(docToManage, commandManager);  
                        break;

                    case "7":
                        SetFileConversionType(docToManage);
                        break;

                    case "8":
                        ProduceConvertedFile(docToManage, commandManager);
                        break;

                    case "9":
                        ShowDocumentContents(docToManage);
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
        else
        {
            Console.WriteLine("Document not found or you do not have access.");
        }
    }

    //====================================================================================== adding of collaborators =================================================================
    static void AddCollaborator(Document document, CommandManager commandManager)
    {
        if (document.Owner == loggedInUser)
        {
            Console.Write("Enter collaborator name: ");
            string collaboratorName = Console.ReadLine();
            User collaborator = users.FirstOrDefault(u => u.Name.Equals(collaboratorName, StringComparison.OrdinalIgnoreCase));

            if (collaborator != null && collaborator != document.Owner && !document.Collaborators.Contains(collaborator))
            {
                ICommand addCollaboratorCommand = new AddCollaboratorCommand(document, collaborator);
                commandManager.ExecuteCommand(addCollaboratorCommand);
                Console.WriteLine($"Collaborator '{collaborator.Name}' added to document '{document.Title}'.");
            }
            else
            {
                Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
            }
        }
        else
        {
            Console.WriteLine("Only the owner can add collaborators.");
        }
    }

    static void SubmitForReview(Document document)
    {
        if (document.IsUnderReview)
        {
            Console.WriteLine($"Document '{document.Title}' is already under review and cannot be resubmitted.");
            return;
        }

        Console.Write("Enter approver name: ");
        string approverName = Console.ReadLine();
        User approver = users.FirstOrDefault(u => u.Name.Equals(approverName, StringComparison.OrdinalIgnoreCase));

        if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
        {
            document.SubmitForApproval(approver);
            Console.WriteLine($"Document '{document.Title}' is now under review by {approver.Name}.");
        }
        else
        {
            Console.WriteLine("Invalid approver. The approver cannot be the owner or a collaborator.");
        }
    }

    //====================================================================================== only approver can do this ======================================================================

    static void PushBackDocument(Document document)
    {
        if (document.IsUnderReview)
        {
            if (document.Approver == loggedInUser)
            {
                Console.Write("Enter comments for push back: ");
                string comments = Console.ReadLine();
                document.PushBack(comments);
            }
            else
            {
                Console.WriteLine("Only the approver can push back this document.");
            }
        }
        else
        {
            Console.WriteLine("Document is not under review.");
        }
    }

    static void ApproveDocument(Document document)
    {
        if (document.IsUnderReview)
        {
            if (document.Approver == loggedInUser)
            {
                document.Approve();
            }
            else
            {
                Console.WriteLine("Only the approver can approve this document.");
            }
        }
        else
        {
            Console.WriteLine("Document is not under review.");
        }
    }

    static void RejectDocument(Document document)
    {
        if (document.IsUnderReview)
        {
            if (document.Approver == loggedInUser)
            {
                document.Reject();
            }
            else
            {
                Console.WriteLine("Only the approver can reject this document.");
            }
        }
        else
        {
            Console.WriteLine("Document is not under review.");
        }
    }

    //====================================================================================== file type ======================================================================================


    static void SetFileConversionType(Document document)
    {
        if (document.IsUnderReview)
        {
            Console.WriteLine("Cannot change conversion strategy while under review.");
            return;
        }

        Console.WriteLine("Select conversion strategy:");
        Console.WriteLine("1. PDF");
        Console.WriteLine("2. Word");
        string choice = Console.ReadLine();

        IConversionStrategy strategy = choice switch
        {
            "1" => new PDFConversionStrategy(),
            "2" => new WordConversionStrategy(),
            _ => throw new ArgumentException("Invalid choice")
        };

        document.ConversionStrategy = strategy;
        Console.WriteLine("Conversion strategy updated successfully.");
    }


    static void ProduceConvertedFile(Document document, CommandManager commandManager)
    {

        if (document.Converter != null)
        {
            ICommand convertCommand = new ConvertDocumentCommand(document, document.Converter);
            commandManager.ExecuteCommand(convertCommand);
        }
        else
        {
            Console.WriteLine("No file conversion type set. Please set it first.");
        }
    }



   


   

}
