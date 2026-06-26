using System;
using System.Collections.Generic;
using System.Text;

namespace CybersecurityPOE
{
    // ============================================================
    //  LogEntry  — single activity record
    // ============================================================
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string   Category  { get; set; } = string.Empty;  // e.g. "TASK", "QUIZ", "CHAT"
        public string   Action    { get; set; } = string.Empty;
        public string   Detail    { get; set; } = string.Empty;

        public string Emoji => Category switch
        {
            "TASK"    => "📋",
            "QUIZ"    => "🎯",
            "CHAT"    => "💬",
            "SYSTEM"  => "⚙",
            "REMINDER"=> "⏰",
            _         => "•"
        };

        public override string ToString() =>
            $"{Emoji} [{Timestamp:HH:mm:ss}] {Category} — {Action}" +
            (string.IsNullOrWhiteSpace(Detail) ? string.Empty : $": {Detail}");
    }

    // ============================================================
    //  ActivityLog  — records and retrieves log entries
    // ============================================================
    public class ActivityLog
    {
        private readonly List<LogEntry> _entries = new();

        // ── Log helpers ───────────────────────────────────────────
        public void LogTaskAdded(string taskName) =>
            Add("TASK", "Task added", taskName);

        public void LogTaskDeleted(int taskId, string taskName) =>
            Add("TASK", "Task deleted", $"ID {taskId}: {taskName}");

        public void LogTaskUpdated(int taskId, string taskName) =>
            Add("TASK", "Task updated", $"ID {taskId}: {taskName}");

        public void LogTaskViewed() =>
            Add("TASK", "Task list viewed");

        public void LogReminderCreated(string taskName, DateTime when) =>
            Add("REMINDER", "Reminder created", $"\"{taskName}\" at {when:dd MMM yyyy HH:mm}");

        public void LogQuizStarted() =>
            Add("QUIZ", "Quiz started");

        public void LogQuizAnswered(bool correct, int score, int total) =>
            Add("QUIZ", correct ? "Answer correct" : "Answer incorrect", $"Score: {score}/{total}");

        public void LogQuizCompleted(int score, int total) =>
            Add("QUIZ", "Quiz completed", $"Final score: {score}/{total}");

        public void LogTopicExplored(string topic) =>
            Add("CHAT", "Topic explored", topic);

        public void LogChatMessage(string userName) =>
            Add("CHAT", "Message sent", $"by {userName}");

        public void LogSessionStarted() =>
            Add("SYSTEM", "Session started");

        public void LogSessionReset() =>
            Add("SYSTEM", "Session reset");

        public void LogActivityLogViewed() =>
            Add("SYSTEM", "Activity log viewed");

        // ── Core add ─────────────────────────────────────────────
        private void Add(string category, string action, string detail = "")
        {
            _entries.Add(new LogEntry
            {
                Timestamp = DateTime.Now,
                Category  = category,
                Action    = action,
                Detail    = detail
            });
        }

        // ── Retrieve / format ─────────────────────────────────────
        public IReadOnlyList<LogEntry> AllEntries => _entries.AsReadOnly();

        public string GetFormattedLog(int maxEntries = 20)
        {
            if (_entries.Count == 0)
                return "📭 No activity recorded yet. Start chatting or run the quiz!";

            var sb = new StringBuilder();
            sb.AppendLine($"📜 ACTIVITY LOG  ({_entries.Count} total event(s))\n");
            sb.AppendLine("──────────────────────────────────────");

            int start = Math.Max(0, _entries.Count - maxEntries);
            for (int i = start; i < _entries.Count; i++)
                sb.AppendLine(_entries[i].ToString());

            if (_entries.Count > maxEntries)
                sb.AppendLine($"\n… and {_entries.Count - maxEntries} earlier event(s) not shown.");

            sb.AppendLine("──────────────────────────────────────");
            sb.Append($"Session duration: {GetSessionDuration()}");

            return sb.ToString();
        }

        public string GetSummary()
        {
            int tasks   = _entries.FindAll(e => e.Category == "TASK").Count;
            int quiz    = _entries.FindAll(e => e.Category == "QUIZ").Count;
            int chat    = _entries.FindAll(e => e.Category == "CHAT").Count;
            int reminders = _entries.FindAll(e => e.Category == "REMINDER").Count;

            return $"📊 Session Summary\n\n" +
                   $"📋 Task events:     {tasks}\n" +
                   $"🎯 Quiz events:     {quiz}\n" +
                   $"💬 Chat events:     {chat}\n" +
                   $"⏰ Reminders set:   {reminders}\n" +
                   $"🕒 Duration:        {GetSessionDuration()}";
        }

        public void Clear() => _entries.Clear();

        private string GetSessionDuration()
        {
            if (_entries.Count == 0) return "—";
            var elapsed = DateTime.Now - _entries[0].Timestamp;
            if (elapsed.TotalMinutes < 1) return "< 1 minute";
            if (elapsed.TotalHours    < 1) return $"{(int)elapsed.TotalMinutes} min";
            return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m";
        }
    }
}
