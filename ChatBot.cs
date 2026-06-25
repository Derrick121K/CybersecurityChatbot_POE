using System;
using System.Diagnostics;
using System.Numerics;

namespace CybersecurityChatbot;

public class ChatBot
{
    // ===== PART 2: EXISTING FIELDS =====
    private readonly KeywordResponder _keywordResponder;
    private readonly SentimentDetector _sentimentDetector;
    private readonly MemoryStore _memoryStore;
    private bool _awaitingName = true;
    private string _lastTopic = "";
    private readonly Random _random = new();

    private readonly string[] _fallbackResponses = new string[]
    {
        "I'm not sure I understand. Can you try rephrasing?",
        "Could you ask me about cybersecurity topics like passwords, phishing, or privacy?",
        "I specialize in cybersecurity. Try asking about passwords, scams, or malware!",
        "Hmm, I didn't quite get that. Type 'help' to see what I can do!"
    };

    // ===== PART 3: NEW FIELDS =====
    private readonly DataStorage _storage;
    private readonly TaskManager _taskManager;
    private readonly QuizManager _quizManager;
    private readonly NLPProcessor _nlpProcessor;

    // ===== CONSTRUCTOR =====
    public ChatBot()
    {
        // Part 2: Initialize existing components
        _keywordResponder = new KeywordResponder();
        _sentimentDetector = new SentimentDetector();
        _memoryStore = new MemoryStore();

        // Part 3: Initialize new components
        _storage = new DataStorage();
        _taskManager = new TaskManager(_storage);
        _quizManager = new QuizManager(_storage);
        _nlpProcessor = new NLPProcessor();

        // Log the startup
        _storage.LogAction("Chatbot initialized", "Application started with Part 3 features");
    }

    // ===== PART 2: EXISTING METHODS =====
    public string GetGreeting()
    {
        return "Hello! Welcome to the Cybersecurity Chatbot! 🤖\n\nWhat's your name?";
    }

    public string GetAsciiArt()
    {
        return @"
    ╔═══════════════════════════════════════════════════════════════╗
    ║     ██████╗██╗   ██╗██████╗ ███████╗██████╗ ██████╗  ██████╗ ████████╗║
    ║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔═══██╗╚══██╔══╝║
    ║    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██████╔╝██║   ██║   ██║   ║
    ║    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██╔══██╗██║   ██║   ██║   ║
    ║    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║██████╔╝╚██████╔╝   ██║   ║
    ║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝    ╚═╝   ║
    ║                                                                       ║
    ║              C Y B E R S E C U R I T Y   O U T B O X                 ║
    ║                        v3.0 - Part 3 Ready                            ║
    ╚═══════════════════════════════════════════════════════════════════════╝";
    }

    private string ExtractTopic(string input)
    {
        string lower = input.ToLower();
        if (lower.Contains("password")) return "passwords";
        if (lower.Contains("phishing")) return "phishing";
        if (lower.Contains("privacy")) return "privacy";
        if (lower.Contains("scam")) return "scams";
        if (lower.Contains("malware")) return "malware";
        if (lower.Contains("2fa") || lower.Contains("two factor")) return "2FA";
        return "cybersecurity";
    }

    // ===== PART 3: NEW METHODS =====
    private string ProcessPart3Input(string input)
    {
        // 1. Detect intent using NLP
        string intent = _nlpProcessor.DetectIntent(input);

        // 2. Log the interaction
        _storage.LogAction($"Intent detected: {intent}", $"Input: {input}");

        // 3. Handle based on intent
        switch (intent)
        {
            case "add_task":
                return _taskManager.AddTask(input);

            case "view_tasks":
                return _taskManager.ViewTasks();

            case "complete_task":
                return _taskManager.CompleteTask(input);

            case "delete_task":
                return _taskManager.DeleteTask(input);

            case "quiz":
                // Check if user wants to start or is in the middle
                if (input.ToLower().Contains("start") || input.ToLower().Contains("begin"))
                {
                    return _quizManager.StartQuiz();
                }
                else if (_quizManager.IsQuizActive)
                {
                    return _quizManager.SubmitAnswer(input);
                }
                return "🎮 Type 'start quiz' to begin the cybersecurity quiz!";

            case "activity_log":
                return _storage.GetActivitySummary(10);

            case "help":
                return GetHelpMessage();

            default:
                return null; // Let Part 2 handle it
        }
    }

    private string GetHelpMessage()
    {
        return $@"🔧 {_memoryStore.UserName}, here are ALL my features:

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📋 TASK ASSISTANT (Part 3)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  • Add task: ""add task -Review privacy settings""
  • View tasks: ""view tasks""
  • Complete task: ""complete 1""
  • Delete task: ""delete 1""

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🎮 QUIZ GAME (Part 3)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  • Start quiz: ""start quiz""
  • Answer: ""A"", ""B"", ""C"", or ""D""

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 ACTIVITY LOG (Part 3)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  • Show log: ""show activity log""

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
💬 CHAT FEATURES (Part 2)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  • Ask about: passwords, phishing, privacy, scams, malware, 2FA
  • ""tell me more"" - Get another tip
  • ""I'm interested in [topic]"" - I'll remember it

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
❌ EXIT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  • ""exit"" or ""bye"" - End conversation

What would you like to do?";
    }

    // ===== MAIN PROCESSING METHOD =====
    public string ProcessInput(string input)
    {
        // Check for empty input
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Please type something. I'm here to help you with cybersecurity!";
        }

        // ===== STEP 1: Get user's name =====
        if (_awaitingName)
        {
            _memoryStore.UserName = input.Trim();
            _awaitingName = false;
            _storage.LogAction("User identified", $"Name: {_memoryStore.UserName}");
            return $"Nice to meet you, {_memoryStore.UserName}! 🎉\n\nI can help you with:\n" +
                   $"• Passwords 🔐\n• Phishing 🎣\n• Privacy 🛡️\n• Scams ⚠️\n• Malware 🦠\n• 2FA 📱\n\n" +
                   $"Try: 'add task', 'start quiz', or ask me a cybersecurity question!\n" +
                   $"Type 'help' for all commands.";
        }

        string lowerInput = input.ToLower();

        // ===== STEP 2: Check Part 3 features FIRST =====
        var part3Response = ProcessPart3Input(input);
        if (part3Response != null)
        {
            return part3Response;
        }

        // ===== STEP 3: Follow-up requests (Part 2) =====
        if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") ||
            lowerInput.Contains("another tip") || lowerInput.Contains("continue"))
        {
            if (!string.IsNullOrEmpty(_lastTopic))
            {
                string personalizedOpener = _memoryStore.GetPersonalizedOpener();
                string? tip = _keywordResponder.GetResponse(_lastTopic);
                _storage.LogAction("Follow-up requested", $"Topic: {_lastTopic}");
                return $"{personalizedOpener}Here's another tip about {_lastTopic}:\n\n{tip}";
            }
            return "What topic would you like me to tell you more about? Try asking about passwords, phishing, or privacy!";
        }

        // ===== STEP 4: Special commands =====
        if (lowerInput == "help" || lowerInput == "what can you do" || lowerInput == "commands")
        {
            return GetHelpMessage();
        }

        if (lowerInput.Contains("how are you"))
        {
            string[] howAreResponses = new string[]
            {
                "I'm just code, but I'm functioning perfectly! Ready to help you stay safe online! 🤖",
                "All systems operational! How can I help you with cybersecurity today?",
                "I'm great! Thanks for asking. What cybersecurity topic interests you?",
                "Running smoothly! Ask me anything about online safety!"
            };
            return howAreResponses[_random.Next(howAreResponses.Length)];
        }

        if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "bye")
        {
            _storage.LogAction("Chat ended", $"User: {_memoryStore.UserName}");
            return $"Goodbye {_memoryStore.UserName}! Stay safe online! 🛡️\n\nRemember: Think before you click!";
        }

        // ===== STEP 5: Store favorite topic =====
        if (lowerInput.Contains("interested in"))
        {
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].ToLower() == "in" && i + 1 < words.Length)
                {
                    string topic = words[i + 1].Trim('.', '!', '?', ',');
                    _memoryStore.FavoriteTopic = topic;
                    string? response = _keywordResponder.GetResponse(topic);
                    _storage.LogAction("Favorite topic stored", $"Topic: {topic}");
                    return $"Great! I'll remember that you're interested in {topic}. " +
                           $"That's an important cybersecurity topic!\n\n" +
                           $"{response ?? "What specific aspect would you like to know about it?"}";
                }
            }
        }

        // ===== STEP 6: Sentiment Detection + Keyword Recognition =====
        Sentiment detectedSentiment = _sentimentDetector.Detect(input);
        string sentimentResponse = _sentimentDetector.GetSentimentResponse(detectedSentiment);

        string? keywordResponse = _keywordResponder.GetResponse(input);

        if (keywordResponse != null)
        {
            _lastTopic = ExtractTopic(input);
            string personalizedOpener = _memoryStore.GetPersonalizedOpener();

            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                return sentimentResponse + "\n\n" + personalizedOpener + keywordResponse;
            }
            return personalizedOpener + keywordResponse;
        }

        // ===== STEP 7: Fallback response =====
        string fallback = _fallbackResponses[_random.Next(_fallbackResponses.Length)];
        _storage.LogAction("Fallback response used", $"Input: {input}");
        return fallback;
    }
}