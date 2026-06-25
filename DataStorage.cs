using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CybersecurityChatbot;

public class DataStorage
{
    private readonly string _filePath;
    private AppData _data;

    public DataStorage()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_data.json");
        _data = LoadData();
    }

    private AppData LoadData()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<AppData>(json) ?? new AppData();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
        return new AppData();
    }

    private void SaveData()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    // ===== TASK METHODS =====
    public int AddTask(string title, string description = "", DateTime? reminderDate = null)
    {
        var task = new TaskItem
        {
            Id = _data.NextTaskId++,
            Title = title,
            Description = description,
            ReminderDate = reminderDate,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };
        _data.Tasks.Add(task);
        SaveData();
        return task.Id;
    }

    public List<TaskItem> GetTasks(bool includeCompleted = false)
    {
        return includeCompleted
            ? _data.Tasks.OrderByDescending(t => t.CreatedAt).ToList()
            : _data.Tasks.Where(t => !t.IsCompleted).OrderByDescending(t => t.CreatedAt).ToList();
    }

    public bool CompleteTask(int id)
    {
        var task = _data.Tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            task.IsCompleted = true;
            SaveData();
            return true;
        }
        return false;
    }

    public bool DeleteTask(int id)
    {
        var task = _data.Tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _data.Tasks.Remove(task);
            SaveData();
            return true;
        }
        return false;
    }

    // ===== ACTIVITY LOG METHODS =====
    public void LogAction(string action, string details = "")
    {
        _data.ActivityLog.Insert(0, new ActivityEntry
        {
            Timestamp = DateTime.Now,
            Action = action,
            Details = details
        });

        // Keep only last 50 entries
        if (_data.ActivityLog.Count > 50)
            _data.ActivityLog = _data.ActivityLog.Take(50).ToList();

        SaveData();
    }

    public List<ActivityEntry> GetActivityLog(int count = 10)
    {
        return _data.ActivityLog.Take(count).ToList();
    }

    public string GetActivitySummary(int count = 10)
    {
        var entries = GetActivityLog(count);
        if (entries.Count == 0)
            return "No activities logged yet.";

        var result = "📊 RECENT ACTIVITIES:\n\n";
        int i = 1;
        foreach (var entry in entries)
        {
            result += $"{i}. [{entry.Timestamp:HH:mm:ss}] {entry.Action}";
            if (!string.IsNullOrEmpty(entry.Details))
                result += $" - {entry.Details}";
            result += "\n";
            i++;
        }
        return result;
    }

    // ===== RESET METHODS (Optional) =====
    public void ClearAllData()
    {
        _data = new AppData();
        SaveData();
    }
}