﻿// =======================
// Main Program with Extended Document Management
// =======================

using SDP_Assignment;
using SDP_Assignment.Jason;
using SDP_Assignment.SHIYING;
using SDP_Assignment.RAEANN;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<User> users = new List<User>();
    static List<Document> documents = new List<Document>();
    static User loggedInUser = null;

    static void Main(string[] args)
    {
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
                string format = doc.ConversionStrategy != null ? doc.ConversionStrategy.GetType().Name.Replace("ConversionStrategy", "") : "Not Set";
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
                string format = doc.ConversionStrategy != null ? doc.ConversionStrategy.GetType().Name.Replace("ConversionStrategy", "") : "Not Set";
                Console.WriteLine($"- {doc.Title} (Format: {format})");
            }
        }
        else
        {
            Console.WriteLine("You do not own any documents.");
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
                    ListOwnedDocuments();
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
        var pushedBackDocs = documents.Where(d => (d.Owner == loggedInUser || d.Collaborators.Contains(loggedInUser)) && !string.IsNullOrEmpty(d.Feedback));

        foreach (var doc in pushedBackDocs)
        {
            Console.WriteLine($"\nNotification: Your document '{doc.Title}' has been pushed back with comments: {doc.Feedback}");
        }
    }

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
            Console.WriteLine("9. Stop Managing Document");
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
            Console.WriteLine("This document is under review. No edits are allowed.");
            return;
        }

        Console.Write("Enter new content: ");
        string newContent = Console.ReadLine();
        document.EditContent(newContent);
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
            document.AddCollaborator(collaborator);
        }
        else
        {
            Console.WriteLine("Invalid collaborator. Collaborator cannot be the owner or already added.");
        }
    }

    static void SubmitForReview(Document document)
    {
        if (document.IsUnderReview)
        {
            Console.WriteLine("Document is already under review.");
            return;
        }

        Console.Write("Enter approver name: ");
        string approverName = Console.ReadLine();
        User approver = users.FirstOrDefault(u => u.Name.Equals(approverName, StringComparison.OrdinalIgnoreCase));

        if (approver != null && approver != document.Owner && !document.Collaborators.Contains(approver))
        {
            document.SubmitForApproval(approver);
        }
        else
        {
            Console.WriteLine("Invalid approver. The approver cannot be the owner or a collaborator.");
        }
    }

    static void PushBackDocument(Document document)
    {
        if (document.IsUnderReview && document.Approver == loggedInUser)
        {
            Console.Write("Enter comments for push back: ");
            string comments = Console.ReadLine();
            document.PushBack(comments);
        }
        else
        {
            Console.WriteLine("Only the approver can push back this document or document is not under review.");
        }
    }

    static void ApproveDocument(Document document)
    {
        if (document.IsUnderReview && document.Approver == loggedInUser)
        {
            document.Approve();
        }
        else
        {
            Console.WriteLine("Only the approver can approve this document or document is not under review.");
        }
    }

    static void RejectDocument(Document document)
    {
        if (document.IsUnderReview && document.Approver == loggedInUser)
        {
            document.Reject();
        }
        else
        {
            Console.WriteLine("Only the approver can reject this document or document is not under review.");
        }
    }

    static void SetFileConversionType(Document document)
    {
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

    static void ConvertFile(Document document)
    {
        if (document.ConversionStrategy != null)
        {
            string result = document.ConversionStrategy.Convert(document);
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("No file conversion type set. Please set it first.");
        }
    }

    static Document SelectUserDocument()
    {
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

        return userDocs.FirstOrDefault(d => d.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
}
