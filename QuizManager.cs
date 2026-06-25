using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot;

public class QuizManager
{
    private readonly List<QuizQuestion> _questions;
    private int _currentQuestionIndex = 0;
    private int _score = 0;
    private bool _quizActive = false;
    private readonly DataStorage _storage;

    public QuizManager(DataStorage storage)
    {
        _storage = storage;
        _questions = new List<QuizQuestion>();
        InitializeQuestions();
    }

    private void InitializeQuestions()
    {
        _questions.Add(new QuizQuestion
        {
            Id = 1,
            Question = "What is phishing?",
            Options = new List<string> {
                "A type of computer virus",
                "A method of stealing personal information through fake emails/websites",
                "A secure way to send files",
                "A type of password protection"
            },
            CorrectIndex = 1,
            Explanation = "Phishing is a cyber attack where scammers pretend to be legitimate organizations to steal your personal information."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 2,
            Question = "Which is an example of a strong password?",
            Options = new List<string> {
                "password123",
                "12345678",
                "MyBirthday1990!",
                "Tr0ub4dor&3ss"
            },
            CorrectIndex = 3,
            Explanation = "A strong password includes uppercase, lowercase, numbers, and symbols. 'Tr0ub4dor&3ss' meets all these criteria."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 3,
            Question = "What should you do if you receive a suspicious email from your bank?",
            Options = new List<string> {
                "Reply with your account details",
                "Click on the link in the email",
                "Contact your bank directly using their official number",
                "Forward it to your friends"
            },
            CorrectIndex = 2,
            Explanation = "Never respond to suspicious emails. Always contact your bank through their official channels to verify the request."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 4,
            Question = "What is Two-Factor Authentication (2FA)?",
            Options = new List<string> {
                "A type of antivirus software",
                "An extra layer of security requiring two verification methods",
                "A password manager",
                "A type of firewall"
            },
            CorrectIndex = 1,
            Explanation = "2FA requires two different verification methods (like password + phone code) for added security."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 5,
            Question = "What is a sign of a scam website?",
            Options = new List<string> {
                "It uses HTTPS with a valid certificate",
                "It shows contact information",
                "It has spelling errors and uses urgent language",
                "It asks for a credit card"
            },
            CorrectIndex = 2,
            Explanation = "Scam websites often have spelling errors, grammatical mistakes, and create false urgency to pressure you."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 6,
            Question = "What is social engineering?",
            Options = new List<string> {
                "Building social networks online",
                "Manipulating people into revealing confidential information",
                "Creating social media accounts",
                "A type of computer program"
            },
            CorrectIndex = 1,
            Explanation = "Social engineering exploits human psychology to gain unauthorized access to systems or data."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 7,
            Question = "How often should you update your passwords?",
            Options = new List<string> {
                "Never - once set, they're fine forever",
                "Every 6-12 months or if a breach is suspected",
                "Every 5 years",
                "Only when forced by the system"
            },
            CorrectIndex = 1,
            Explanation = "Regular password updates (every 6-12 months) reduce risk. Change immediately if you suspect a breach."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 8,
            Question = "What is a VPN used for?",
            Options = new List<string> {
                "To speed up your internet connection",
                "To hide your IP address and encrypt your internet traffic",
                "To install software updates",
                "To delete viruses"
            },
            CorrectIndex = 1,
            Explanation = "A VPN (Virtual Private Network) protects your privacy by encrypting your connection and hiding your IP address."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 9,
            Question = "True or False: Public Wi-Fi is completely safe for online banking.",
            Options = new List<string> { "True", "False" },
            CorrectIndex = 1,
            Explanation = "False! Public Wi-Fi is often insecure. Use a VPN if you must use public networks for sensitive transactions."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 10,
            Question = "What is ransomware?",
            Options = new List<string> {
                "A type of antivirus software",
                "Malware that encrypts your files and demands payment for decryption",
                "A secure backup system",
                "A type of password manager"
            },
            CorrectIndex = 1,
            Explanation = "Ransomware encrypts your files and holds them hostage until you pay a ransom to get them back."
        });

        _questions.Add(new QuizQuestion
        {
            Id = 11,
            Question = "What should you do if you think your account has been hacked?",
            Options = new List<string> {
                "Wait and see if anything happens",
                "Immediately change your password and contact the service provider",
                "Delete your account",
                "Ignore it"
            },
            CorrectIndex = 1,
            Explanation = "Act quickly! Change your password, enable 2FA, and contact the service provider immediately."
        });
    }

    public string StartQuiz()
    {
        _currentQuestionIndex = 0;
        _score = 0;
        _quizActive = true;
        _storage.LogAction("Quiz started", "User began cybersecurity quiz");
        return GetNextQuestion();
    }

    public string GetNextQuestion()
    {
        if (!_quizActive || _currentQuestionIndex >= _questions.Count)
        {
            _quizActive = false;
            return EndQuiz();
        }

        var q = _questions[_currentQuestionIndex];
        var result = $"📝 QUESTION {_currentQuestionIndex + 1}/{_questions.Count}\n\n";
        result += $"Q: {q.Question}\n\n";
        result += "Options:\n";
        for (int i = 0; i < q.Options.Count; i++)
        {
            result += $"  {Convert.ToChar(65 + i)}. {q.Options[i]}\n";
        }
        result += $"\nScore: {_score}/{_currentQuestionIndex}";
        result += "\n\nType A, B, C, D (or the full answer) to respond!";
        return result;
    }

    public string SubmitAnswer(string answer)
    {
        if (!_quizActive || _currentQuestionIndex >= _questions.Count)
        {
            return "The quiz has ended. Type 'start quiz' to play again!";
        }

        var q = _questions[_currentQuestionIndex];
        int selectedIndex = ParseAnswer(answer, q);

        if (selectedIndex == -1)
        {
            return "❌ Invalid answer. Please type A, B, C, or D.";
        }

        bool isCorrect = selectedIndex == q.CorrectIndex;
        if (isCorrect) _score++;

        var result = isCorrect ? "✅ Correct! 🎉" : "❌ Incorrect.";
        result += $"\n\n💡 {q.Explanation}\n";

        _currentQuestionIndex++;

        if (_currentQuestionIndex >= _questions.Count)
        {
            _storage.LogAction("Quiz completed", $"Score: {_score}/{_questions.Count}");
            result += "\n\n" + EndQuiz();
        }
        else
        {
            result += "\n\n" + GetNextQuestion();
        }

        return result;
    }

    private int ParseAnswer(string answer, QuizQuestion question)
    {
        answer = answer.ToUpper().Trim();

        if (answer.Length == 1 && answer[0] >= 'A' && answer[0] <= 'D')
        {
            return answer[0] - 'A';
        }

        for (int i = 0; i < question.Options.Count; i++)
        {
            if (question.Options[i].ToUpper().Contains(answer) || answer.Contains(question.Options[i].ToUpper()))
            {
                return i;
            }
        }

        return -1;
    }

    private string EndQuiz()
    {
        var total = _questions.Count;
        var percentage = (double)_score / total * 100;

        var result = "🏁 QUIZ COMPLETE!\n\n";
        result += $"Your Score: {_score}/{total}\n";
        result += $"Percentage: {percentage:F0}%\n\n";

        if (percentage >= 80)
            result += "🌟 EXCELLENT! You're a Cybersecurity Pro!";
        else if (percentage >= 60)
            result += "👍 Good job! Keep learning to become a cybersecurity expert!";
        else if (percentage >= 40)
            result += "📖 You're on the right track! Review the basics and try again.";
        else
            result += "🔐 Keep learning! Cybersecurity is important for everyone.";

        _quizActive = false;
        return result;
    }

    public bool IsQuizActive => _quizActive;
}