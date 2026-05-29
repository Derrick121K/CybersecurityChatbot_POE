using System;

namespace CybersecurityChatbot;

public class ChatBot
{
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

    public ChatBot()
    {
        _keywordResponder = new KeywordResponder();
        _sentimentDetector = new SentimentDetector();
        _memoryStore = new MemoryStore();
    }

    public string GetGreeting()
    {
        return "Hello! Welcome to the Cybersecurity Chatbot! 🤖\n\nWhat's your name?";
    }

    public string ProcessInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Please type something. I'm here to help you with cybersecurity!";
        }

        // STEP 1: Get user's name first (Memory requirement)
        if (_awaitingName)
        {
            _memoryStore.UserName = input.Trim();
            _awaitingName = false;
            return $"Nice to meet you, {_memoryStore.UserName}! 🎉\n\nI can help you with:\n" +
                   $"• Passwords 🔐\n• Phishing 🎣\n• Privacy 🛡️\n• Scams ⚠️\n• Malware 🦠\n• 2FA 📱\n\n" +
                   $"What would you like to learn about today? (Type 'help' for commands)";
        }

        string lowerInput = input.ToLower();

        // STEP 2: Handle follow-up requests (Conversation Flow requirement - 10 marks)
        if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") ||
            lowerInput.Contains("another tip") || lowerInput.Contains("continue"))
        {
            if (!string.IsNullOrEmpty(_lastTopic))
            {
                string personalizedOpener = _memoryStore.GetPersonalizedOpener();
                string? tip = _keywordResponder.GetResponse(_lastTopic);
                return $"{personalizedOpener}Here's another tip about {_lastTopic}:\n\n{tip}";
            }
            return "What topic would you like me to tell you more about? Try asking about passwords, phishing, or privacy!";
        }

        // STEP 3: Handle special commands
        if (lowerInput == "help" || lowerInput == "what can you do" || lowerInput == "commands")
        {
            return $"🔧 COMMANDS:\n\n" +
                   $"• Ask about: passwords, phishing, privacy, scams, malware, 2FA\n" +
                   $"• 'tell me more' - Get another tip on the same topic\n" +
                   $"• 'I'm interested in [topic]' - I'll remember your favorite topic\n" +
                   $"• 'how are you' - Chat with me\n" +
                   $"• 'exit' or 'bye' - End the conversation\n\n" +
                   $"What would you like to learn about, {_memoryStore.UserName}?";
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
            return $"Goodbye {_memoryStore.UserName}! Stay safe online! 🛡️\n\nRemember: Think before you click!";
        }

        // STEP 4: Store favorite topic (Memory requirement)
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
                    return $"Great! I'll remember that you're interested in {topic}. " +
                           $"That's an important cybersecurity topic!\n\n" +
                           $"{response ?? "What specific aspect would you like to know about it?"}";
                }
            }
        }

        // STEP 5: Sentiment Detection + Keyword Recognition combined
        Sentiment detectedSentiment = _sentimentDetector.Detect(input);
        string sentimentResponse = _sentimentDetector.GetSentimentResponse(detectedSentiment);

        string? keywordResponse = _keywordResponder.GetResponse(input);

        if (keywordResponse != null)
        {
            // Store topic for follow-up questions
            _lastTopic = ExtractTopic(input);

            string personalizedOpener = _memoryStore.GetPersonalizedOpener();

            // Combine sentiment response + personalized opener + keyword response
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                return sentimentResponse + "\n\n" + personalizedOpener + keywordResponse;
            }
            return personalizedOpener + keywordResponse;
        }

        // STEP 6: Fallback response (Error Handling requirement)
        return _fallbackResponses[_random.Next(_fallbackResponses.Length)];
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
    ║              C Y B E R S E C U R I T Y   C H A T B O T               ║
    ║                          v2.0 - .NET 10.0                             ║
    ╚═══════════════════════════════════════════════════════════════════════╝";
    }
}