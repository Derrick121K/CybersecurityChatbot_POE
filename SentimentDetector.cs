using System;
using System.Collections.Generic;

namespace CybersecurityChatbot;

public enum Sentiment
{
    Neutral,
    Worried,
    Curious,
    Frustrated,
    Happy
}

public class SentimentDetector
{
    private readonly Dictionary<Sentiment, List<string>> _sentimentTriggers;
    private readonly Dictionary<Sentiment, string> _sentimentResponses;

    public SentimentDetector()
    {
        _sentimentTriggers = new Dictionary<Sentiment, List<string>>();
        _sentimentResponses = new Dictionary<Sentiment, string>();
        InitializeTriggers();
        InitializeResponses();
    }

    private void InitializeTriggers()
    {
        // Worried triggers
        _sentimentTriggers[Sentiment.Worried] = new List<string>
        {
            "worried", "scared", "afraid", "anxious", "nervous", "unsafe", "concerned", "fear"
        };

        // Curious triggers
        _sentimentTriggers[Sentiment.Curious] = new List<string>
        {
            "curious", "wondering", "interested", "want to know", "how does", "tell me about", "explain"
        };

        // Frustrated triggers
        _sentimentTriggers[Sentiment.Frustrated] = new List<string>
        {
            "frustrated", "annoyed", "confused", "don't understand", "not working", "hard", "difficult"
        };

        // Happy triggers
        _sentimentTriggers[Sentiment.Happy] = new List<string>
        {
            "great", "thanks", "helpful", "awesome", "love it", "perfect", "excellent"
        };
    }

    private void InitializeResponses()
    {
        _sentimentResponses[Sentiment.Worried] = "😟 I understand your concern. It's completely normal to feel worried about cybersecurity. Let me help you stay safe...";
        _sentimentResponses[Sentiment.Curious] = "🤔 Great question! I'm glad you're curious about staying safe online. Here's what you should know...";
        _sentimentResponses[Sentiment.Frustrated] = "😅 I know cybersecurity can be frustrating sometimes. Let me break it down simply for you...";
        _sentimentResponses[Sentiment.Happy] = "😊 That's great to hear! Keep up the good security habits! Here's something helpful...";
        _sentimentResponses[Sentiment.Neutral] = "";
    }

    public Sentiment Detect(string input)
    {
        input = input.ToLower();

        foreach (var sentiment in _sentimentTriggers)
        {
            foreach (var trigger in sentiment.Value)
            {
                if (input.Contains(trigger))
                {
                    return sentiment.Key;
                }
            }
        }

        return Sentiment.Neutral;
    }

    public string GetSentimentResponse(Sentiment sentiment)
    {
        return _sentimentResponses.GetValueOrDefault(sentiment, "");
    }
}