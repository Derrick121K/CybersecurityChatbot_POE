using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot;

public class TaskManager
{
    private readonly DataStorage _storage;

    public TaskManager(DataStorage storage)
    {
        _storage = storage;
    }

    public string AddTask(string input)
    {
        // Parse task from input
        string title = "Cybersecurity Task";
        string description = "";
        DateTime? reminderDate = null;

        // Check for reminder
        if (input.ToLower().Contains("remind me in"))
        {
            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int days) && i + 1 < words.Length)
                {
                    if (words[i + 1].ToLower().Contains("day"))
                    {
                        reminderDate = DateTime.Now.AddDays(days);
                        break;
                    }
                }
            }
        }

        // Extract title (remove command words)
        var prefixes = new[] { "add task", "create task", "new task", "add a task" };
        var cleaned = input;
        foreach (var prefix in prefixes)
        {
            if (input.ToLower().Contains(prefix))
            {
                cleaned = input.Substring(input.ToLower().IndexOf(prefix) + prefix.Length);
                break;
            }
        }

        // Remove reminder phrases
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"remind me in \d+ days?", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        cleaned = cleaned.Trim();

        if (!string.IsNullOrEmpty(cleaned))
            title = cleaned;

        int taskId = _storage.AddTask(title, description, reminderDate);

        // Log the action
        _storage.LogAction("Task added", $"'{title}' (ID: {taskId})");

        string result = $"✅ Task added: '{title}'\n";
        result += $"📋 ID: {taskId}\n";
        if (reminderDate.HasValue)
            result += $"📅 Reminder set for: {reminderDate.Value:yyyy-MM-dd}";
        else
            result += "No reminder set.";
        result += "\n\nCommands: 'view tasks', 'complete [ID]', 'delete [ID]'";
        return result;
    }

    public string ViewTasks()
    {
        var tasks = _storage.GetTasks(false);
        if (tasks.Count == 0)
            return "📋 You have no pending tasks.\n\nType 'add task - [your task]' to create one!";

        var result = "📋 YOUR TASKS:\n\n";
        foreach (var task in tasks)
        {
            result += $"📌 {task.Title}\n";
            if (!string.IsNullOrEmpty(task.Description))
                result += $"   📝 {task.Description}\n";
            if (task.ReminderDate.HasValue)
                result += $"   📅 Reminder: {task.ReminderDate.Value:yyyy-MM-dd}\n";
            result += $"   🆔 ID: {task.Id}\n\n";
        }

        result += "Commands:\n";
        result += "  • complete [ID] - Mark task as done\n";
        result += "  • delete [ID] - Remove task";
        return result;
    }

    public string CompleteTask(string input)
    {
        var id = ExtractId(input);
        if (id == null) return "❌ Please specify a task ID. Example: 'complete 1'";

        if (_storage.CompleteTask(id.Value))
        {
            _storage.LogAction("Task completed", $"Task ID: {id.Value}");
            return $"✅ Task {id.Value} marked as completed! Great job! 🎉";
        }
        return $"❌ Could not find task with ID {id.Value}. Type 'view tasks' to see your tasks.";
    }

    public string DeleteTask(string input)
    {
        var id = ExtractId(input);
        if (id == null) return "❌ Please specify a task ID. Example: 'delete 1'";

        if (_storage.DeleteTask(id.Value))
        {
            _storage.LogAction("Task deleted", $"Task ID: {id.Value}");
            return $"🗑️ Task {id.Value} deleted successfully!";
        }
        return $"❌ Could not find task with ID {id.Value}. Type 'view tasks' to see your tasks.";
    }

    private int? ExtractId(string input)
    {
        var words = input.Split(' ');
        foreach (var word in words)
        {
            if (int.TryParse(word, out int id))
                return id;
        }
        return null;
    }
}