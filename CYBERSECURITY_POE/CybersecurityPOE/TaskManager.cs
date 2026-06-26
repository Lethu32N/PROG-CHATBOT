using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CybersecurityPOE
{
    // ============================================================
    //  CyberTask  — data model
    // ============================================================
    public class CyberTask
    {
        public int    Id           { get; set; }
        public string TaskName     { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; }
        public string Status       { get; set; } = "Pending";
        public DateTime CreatedAt  { get; set; } = DateTime.Now;

        public override string ToString() =>
            $"[{Id}] {TaskName} — Due: {ReminderDate:dd MMM yyyy HH:mm} | {Status}";
    }

    // ============================================================
    //  TaskManager  — CRUD + MySQL persistence
    // ============================================================
    public class TaskManager
    {
        // ── Connection string (edit host/port/db/user/password) ──
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=graceai_db;" +
            "User ID=root;Password=;CharSet=utf8mb4;";

        private bool _dbAvailable = false;

        // In-memory fallback for when MySQL is not connected
        private readonly List<CyberTask> _inMemoryTasks = new();
        private int _nextId = 1;

        public TaskManager()
        {
            TryInitialiseDatabase();
        }

        // ── Database initialisation ───────────────────────────────
        private void TryInitialiseDatabase()
        {
            try
            {
                using var conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string createDb = "CREATE DATABASE IF NOT EXISTS graceai_db CHARACTER SET utf8mb4;";
                using (var cmd = new MySqlCommand(createDb, conn))
                    cmd.ExecuteNonQuery();

                string createTable = @"
                    CREATE TABLE IF NOT EXISTS cyber_tasks (
                        id            INT AUTO_INCREMENT PRIMARY KEY,
                        task_name     VARCHAR(255)  NOT NULL,
                        reminder_date DATETIME      NOT NULL,
                        status        VARCHAR(50)   NOT NULL DEFAULT 'Pending',
                        created_at    DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";

                using (var cmd = new MySqlCommand(createTable, conn))
                    cmd.ExecuteNonQuery();

                _dbAvailable = true;
            }
            catch
            {
                // MySQL not available — fall back to in-memory storage
                _dbAvailable = false;
            }
        }

        public bool IsDatabaseAvailable => _dbAvailable;

        // ── ADD ──────────────────────────────────────────────────
        public (bool success, string message, CyberTask? task) AddTask(string taskName, DateTime reminderDate)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                return (false, "Task name cannot be empty.", null);

            var task = new CyberTask
            {
                TaskName     = taskName.Trim(),
                ReminderDate = reminderDate,
                Status       = "Pending",
                CreatedAt    = DateTime.Now
            };

            if (_dbAvailable)
            {
                try
                {
                    using var conn = new MySqlConnection(ConnectionString);
                    conn.Open();
                    const string sql = @"INSERT INTO cyber_tasks (task_name, reminder_date, status, created_at)
                                        VALUES (@name, @date, @status, @created);
                                        SELECT LAST_INSERT_ID();";
                    using var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name",    task.TaskName);
                    cmd.Parameters.AddWithValue("@date",    task.ReminderDate);
                    cmd.Parameters.AddWithValue("@status",  task.Status);
                    cmd.Parameters.AddWithValue("@created", task.CreatedAt);
                    task.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    return (true, $"✅ Task saved to database! ID: {task.Id}", task);
                }
                catch (Exception ex)
                {
                    return (false, $"❌ Database error: {ex.Message}", null);
                }
            }
            else
            {
                task.Id = _nextId++;
                _inMemoryTasks.Add(task);
                return (true, $"✅ Task saved (in-memory, no DB). ID: {task.Id}", task);
            }
        }

        // ── GET ALL ──────────────────────────────────────────────
        public (bool success, string message, List<CyberTask> tasks) GetAllTasks()
        {
            if (_dbAvailable)
            {
                try
                {
                    var tasks = new List<CyberTask>();
                    using var conn = new MySqlConnection(ConnectionString);
                    conn.Open();
                    const string sql = "SELECT id, task_name, reminder_date, status, created_at FROM cyber_tasks ORDER BY reminder_date ASC;";
                    using var cmd = new MySqlCommand(sql, conn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tasks.Add(new CyberTask
                        {
                            Id           = reader.GetInt32("id"),
                            TaskName     = reader.GetString("task_name"),
                            ReminderDate = reader.GetDateTime("reminder_date"),
                            Status       = reader.GetString("status"),
                            CreatedAt    = reader.GetDateTime("created_at")
                        });
                    }
                    return (true, $"Found {tasks.Count} task(s).", tasks);
                }
                catch (Exception ex)
                {
                    return (false, $"❌ Database error: {ex.Message}", new List<CyberTask>());
                }
            }
            else
            {
                return (true, $"Found {_inMemoryTasks.Count} task(s) (in-memory).", new List<CyberTask>(_inMemoryTasks));
            }
        }

        // ── UPDATE ───────────────────────────────────────────────
        public (bool success, string message) UpdateTask(int id, string newName, DateTime newDate, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return (false, "Task name cannot be empty.");

            if (_dbAvailable)
            {
                try
                {
                    using var conn = new MySqlConnection(ConnectionString);
                    conn.Open();
                    const string sql = @"UPDATE cyber_tasks
                                        SET task_name=@name, reminder_date=@date, status=@status
                                        WHERE id=@id;";
                    using var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name",   newName.Trim());
                    cmd.Parameters.AddWithValue("@date",   newDate);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id",     id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0
                        ? (true,  $"✅ Task {id} updated successfully.")
                        : (false, $"⚠ No task found with ID {id}.");
                }
                catch (Exception ex)
                {
                    return (false, $"❌ Database error: {ex.Message}");
                }
            }
            else
            {
                var t = _inMemoryTasks.Find(x => x.Id == id);
                if (t == null) return (false, $"⚠ No task found with ID {id}.");
                t.TaskName     = newName.Trim();
                t.ReminderDate = newDate;
                t.Status       = newStatus;
                return (true, $"✅ Task {id} updated (in-memory).");
            }
        }

        // ── DELETE ───────────────────────────────────────────────
        public (bool success, string message) DeleteTask(int id)
        {
            if (_dbAvailable)
            {
                try
                {
                    using var conn = new MySqlConnection(ConnectionString);
                    conn.Open();
                    const string sql = "DELETE FROM cyber_tasks WHERE id=@id;";
                    using var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0
                        ? (true,  $"🗑 Task {id} deleted.")
                        : (false, $"⚠ No task found with ID {id}.");
                }
                catch (Exception ex)
                {
                    return (false, $"❌ Database error: {ex.Message}");
                }
            }
            else
            {
                int removed = _inMemoryTasks.RemoveAll(x => x.Id == id);
                return removed > 0
                    ? (true,  $"🗑 Task {id} deleted (in-memory).")
                    : (false, $"⚠ No task found with ID {id}.");
            }
        }

        // ── NLP: parse a task out of a natural-language message ──
        public static (bool found, string taskName) ParseTaskFromMessage(string message)
        {
            var lower = message.ToLower();

            string[] triggers = {
                "remind me to ", "add task ", "create task ", "set reminder for ",
                "set reminder to ", "new task ", "schedule task "
            };

            foreach (var trigger in triggers)
            {
                int idx = lower.IndexOf(trigger, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var extracted = message.Substring(idx + trigger.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(extracted))
                        return (true, CapitaliseFirst(extracted));
                }
            }

            // Common cybersecurity tasks mentioned without a trigger phrase
            var cyberTaskMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "enable 2fa",            "Enable Two-Factor Authentication" },
                { "enable two-factor",     "Enable Two-Factor Authentication" },
                { "update password",       "Update Password" },
                { "change password",       "Update Password" },
                { "review privacy",        "Review Privacy Settings" },
                { "check privacy",         "Review Privacy Settings" },
                { "install antivirus",     "Install Antivirus Software" },
                { "backup data",           "Back Up Important Data" },
                { "update software",       "Update Software & OS" },
            };

            foreach (var (kw, name) in cyberTaskMap)
                if (lower.Contains(kw))
                    return (true, name);

            return (false, string.Empty);
        }

        private static string CapitaliseFirst(string s) =>
            string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s.Substring(1);
    }
}
