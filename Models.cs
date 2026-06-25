using System;
using System.Collections.Generic;

namespace CybersecurityChatbot;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime? ReminderDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class QuizQuestion
{
    public int Id { get; set; }
    public string Question { get; set; } = "";
    public List<string> Options { get; set; } = new();
    public int CorrectIndex { get; set; }
    public string Explanation { get; set; } = "";
}

public class ActivityEntry
{
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = "";
    public string Details { get; set; } = "";
}

public class AppData
{
    public List<TaskItem> Tasks { get; set; } = new();
    public List<ActivityEntry> ActivityLog { get; set; } = new();
    public int NextTaskId { get; set; } = 1;
}