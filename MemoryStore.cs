using System.Collections.Generic;

namespace CybersecurityChatbot;

public class MemoryStore
{
    public string UserName { get; set; } = "";
    public string FavoriteTopic { get; set; } = "";
    private readonly Dictionary<string, string> _memories = new();

    public void Store(string key, string value)
    {
        if (_memories.ContainsKey(key))
            _memories[key] = value;
        else
            _memories.Add(key, value);
    }

    public string? Recall(string key)
    {
        return _memories.GetValueOrDefault(key);
    }

    public string GetPersonalizedOpener()
    {
        if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FavoriteTopic))
        {
            return $"As someone interested in {FavoriteTopic}, {UserName}, ";
        }
        else if (!string.IsNullOrEmpty(UserName))
        {
            return $"{UserName}, ";
        }
        else if (!string.IsNullOrEmpty(FavoriteTopic))
        {
            return $"Since you're interested in {FavoriteTopic}, ";
        }
        return "";
    }
}