using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CybersecurityChatbot;

public class NLPProcessor
{
    private readonly Dictionary<string, List<string>> _intentPatterns;
    private readonly Dictionary<string, string> _intentResponses;

    public NLPProcessor()
    {
        _intentPatterns = new Dictionary<string, List<string>>
        {
            ["add_task"] = new List<string>
            {
                "add task", "create task", "new task", "add a task", "create a task",
                "i need to", "i should", "remind me to", "set a reminder",
                "add a reminder", "create a reminder"
            },
            ["quiz"] = new List<string>
            {
                "start quiz", "play quiz", "take quiz", "quiz me", "test me",
                "let's play", "i want to play", "do the quiz"
            },
            ["view_tasks"] = new List<string>
            {
                "view tasks", "show tasks", "my tasks", "list tasks",
                "what tasks", "show me tasks", "tasks list"
            },
            ["complete_task"] = new List<string>
            {
                "complete", "mark done", "finish task", "task done",
                "done", "finished", "mark complete"
            },
            ["delete_task"] = new List<string>
            {
                "delete", "remove task", "delete task", "clear task",
                "erase task", "remove"
            },
            ["activity_log"] = new List<string>
            {
                "show log", "activity log", "what have you done",
                "what did you do", "show activity", "recent actions"
            },
            ["help"] = new List<string>
            {
                "help", "commands", "what can you do", "what can i ask",
                "help me", "how to use"
            }
        };

        _intentResponses = new Dictionary<string, string>
        {
            ["add_task"] = "✅ I can help you add a task! Just tell me what you want to add.",
            ["quiz"] = "🎮 Ready for a cybersecurity quiz? Type 'start quiz' to begin!",
            ["view_tasks"] = "📋 Let me show you your tasks.",
            ["activity_log"] = "📊 Here's what I've been doing for you.",
            ["help"] = "🔧 I can help with:\n• Add tasks\n• View tasks\n• Complete/Delete tasks\n• Take a quiz\n• Show activity log",
            ["unknown"] = "I'm not sure what you mean. Try: 'help' for commands."
        };
    }

    public string DetectIntent(string input)
    {
        input = input.ToLower();

        foreach (var intent in _intentPatterns)
        {
            foreach (var pattern in intent.Value)
            {
                if (input.Contains(pattern))
                {
                    return intent.Key;
                }
            }
        }

        return "unknown";
    }

    public string ExtractTaskTitle(string input)
    {
        var prefixes = new[] {
            "add task", "create task", "new task", "add a task", "create a task",
            "i need to", "i should", "remind me to", "set a reminder",
            "add a reminder", "create a reminder"
        };

        var result = input;
        foreach (var prefix in prefixes)
        {
            if (result.ToLower().Contains(prefix))
            {
                result = result.Substring(result.ToLower().IndexOf(prefix) + prefix.Length);
                break;
            }
        }

        result = Regex.Replace(result, @"remind me in \d+ days?", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"in \d+ days?", "", RegexOptions.IgnoreCase);
        result = result.Replace("about", "").Replace("to", "").Trim();

        return string.IsNullOrEmpty(result) ? "Cybersecurity Task" : result;
    }

    public int ExtractDays(string input)
    {
        var match = Regex.Match(input, @"(\d+)\s*(day|days)");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        return 7;
    }

    public bool HasReminderRequest(string input)
    {
        input = input.ToLower();
        return input.Contains("remind me") || input.Contains("set reminder") ||
               input.Contains("in ") && input.Contains("day");
    }

    public string GetIntentSuggestion(string intent)
    {
        return _intentResponses.GetValueOrDefault(intent, _intentResponses["unknown"]);
    }

    public string GetNaturalResponse(string intent, string input)
    {
        return intent switch
        {
            "add_task" => $"I understand you want to add a task. What would you like the task to be called?",
            "quiz" => "🎮 Great! Type 'start quiz' to begin the cybersecurity quiz!",
            "view_tasks" => "📋 Let me show you all your cybersecurity tasks.",
            "complete_task" => "✅ I'll mark that task as completed. What's the task ID?",
            "delete_task" => "🗑️ I'll remove that task. What's the task ID?",
            "activity_log" => "📊 Here's a summary of recent activities.",
            "help" => _intentResponses["help"],
            _ => "I'm not sure what you mean. Type 'help' to see what I can do!"
        };
    }
}