using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot;

public class KeywordResponder
{
    private readonly Dictionary<string, List<string>> _responses;
    private readonly Random _random;

    public KeywordResponder()
    {
        _responses = new Dictionary<string, List<string>>();
        _random = new Random();
        InitializeResponses();
    }

    private void InitializeResponses()
    {
        // Keyword 1: password
        _responses["password"] = new List<string>
        {
            "🔐 Use strong passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols!",
            "🔐 Never reuse passwords across different accounts. Use a password manager like Bitwarden!",
            "🔐 Enable two-factor authentication (2FA) whenever possible for extra security!",
            "🔐 Avoid using personal information like birthdays or pet names in your passwords!"
        };

        // Keyword 2: phishing
        _responses["phishing"] = new List<string>
        {
            "🎣 Never click suspicious links in emails. Always hover over links to see the actual URL first!",
            "🎣 Legitimate companies never ask for passwords via email. When in doubt, contact them directly!",
            "🎣 Look for spelling errors and urgent language like 'act now' - these are common phishing tactics!",
            "🎣 Check the sender's email address carefully - scammers use addresses that look almost real!"
        };

        // Keyword 3: privacy
        _responses["privacy"] = new List<string>
        {
            "🛡️ Review your privacy settings on social media regularly. Limit what you share publicly!",
            "🛡️ Use a VPN on public WiFi to protect your personal information from hackers!",
            "🛡️ Be careful what permissions you give to apps on your phone - they don't all need your location!",
            "🛡️ Cover your webcam when not in use and use privacy screens on your laptop!"
        };

        // Keyword 4: scam
        _responses["scam"] = new List<string>
        {
            "⚠️ If something sounds too good to be true, it probably is. Research before investing money!",
            "⚠️ Never send money to someone you've only met online. Romance scams are very common!",
            "⚠️ Fake tech support calls are scams. Real companies like Microsoft won't call you unexpectedly!",
            "⚠️ Lottery and prize scams: You can't win a contest you didn't enter!"
        };

        // Keyword 5: malware
        _responses["malware"] = new List<string>
        {
            "🦠 Keep your antivirus software updated and run regular full system scans!",
            "🦠 Don't download software from untrusted websites. Use official sources like Microsoft Store only!",
            "🦠 Be careful with email attachments - even from people you know (their account might be hacked)!",
            "🦠 Enable ransomware protection in Windows Security to protect your important files!"
        };

        // Keyword 6: 2fa
        _responses["2fa"] = new List<string>
        {
            "📱 Two-factor authentication adds an extra layer of security. Always enable it when available!",
            "📱 Use an authenticator app like Google Authenticator or Microsoft Authenticator instead of SMS!",
            "📱 Backup codes are important - store them safely in case you lose your phone!",
            "📱 Hardware security keys like YubiKey are the most secure form of 2FA!"
        };
    }

    public string? GetResponse(string input)
    {
        input = input.ToLower();

        foreach (var keyword in _responses.Keys)
        {
            if (input.Contains(keyword))
            {
                var responses = _responses[keyword];
                return responses[_random.Next(responses.Count)];
            }
        }

        return null; // No keyword found
    }
}