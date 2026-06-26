using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityPOE
{
    // ================================================================
    //  MemoryManager — embedded here to fix CS0246 reference errors
    // ================================================================
    public class MemoryManager
    {
        public string UserName { get; set; } = string.Empty;
        public string UserMood { get; set; } = string.Empty;
        public string FavouriteTopic { get; set; } = string.Empty;
        public int MessageCount { get; set; } = 0;
        public int TopicsExplored { get; set; } = 0;

        private readonly Dictionary<string, string> _memory = new();
        private readonly List<string> _topicHistory = new();

        public void Remember(string key, string value) => _memory[key.ToLower()] = value;
        public string Recall(string key) => _memory.TryGetValue(key.ToLower(), out var v) ? v : string.Empty;
        public bool Has(string key) => _memory.ContainsKey(key.ToLower());
        public bool IsNameKnown() => !string.IsNullOrWhiteSpace(UserName);

        public void AddTopicToHistory(string topic)
        {
            var t = topic.ToLower().Trim();
            if (!_topicHistory.Contains(t))
            {
                _topicHistory.Add(t);
                TopicsExplored = _topicHistory.Count;
                FavouriteTopic = topic;
            }
            Remember("last_topic", topic);
        }

        public IReadOnlyList<string> TopicHistory => _topicHistory.AsReadOnly();
        public void IncrementMessages() => MessageCount++;

        public void Reset()
        {
            UserName = string.Empty;
            UserMood = string.Empty;
            FavouriteTopic = string.Empty;
            MessageCount = 0;
            TopicsExplored = 0;
            _memory.Clear();
            _topicHistory.Clear();
        }

        public int AwarenessScore
        {
            get
            {
                int score = 0;
                score += System.Math.Min(TopicsExplored * 10, 50);
                score += System.Math.Min(MessageCount * 2, 30);
                if (!string.IsNullOrEmpty(UserName)) score += 10;
                if (!string.IsNullOrEmpty(UserMood)) score += 10;
                return System.Math.Min(score, 100);
            }
        }

        public string BuildPersonalisedGreeting()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FavouriteTopic))
                return $"Welcome back, {UserName}! Last time you were interested in {FavouriteTopic}. Want to continue from there?";
            if (!string.IsNullOrEmpty(UserName))
                return $"Hello again, {UserName}! What cybersecurity topic can I help you with today?";
            return "Hello! I'm your Cybersecurity POE Bot. What's your name?";
        }
    }

    // ================================================================
    //  SentimentAnalyzer — embedded here to fix CS0246 reference errors
    // ================================================================
    public class SentimentAnalyzer
    {
        private static readonly List<string> HappyKeywords = new() {
            "happy","great","awesome","excellent","wonderful","fantastic","amazing",
            "good","glad","cheerful","excited","love","enjoying","brilliant","perfect","thrilled","joyful"
        };
        private static readonly List<string> SadKeywords = new() {
            "sad","unhappy","depressed","down","miserable","upset","crying","terrible","awful",
            "horrible","gloomy","heartbroken","lonely","disappointed","hopeless","blue","low"
        };
        private static readonly List<string> AngryKeywords = new() {
            "angry","mad","furious","annoyed","frustrated","rage","hate","irritated","livid",
            "outraged","infuriated","enraged","fuming","irritating","aggravated","disgusted"
        };
        private static readonly List<string> StressedKeywords = new() {
            "stressed","overwhelmed","anxious","worried","nervous","panic","pressure","tense",
            "uneasy","apprehensive","distressed","frantic","swamped","overloaded","burnt out","burnout"
        };
        private static readonly List<string> TiredKeywords = new() {
            "tired","exhausted","sleepy","fatigued","drained","weary","worn out",
            "drowsy","groggy","lethargic","sluggish","burned out"
        };

        private static readonly Dictionary<string, List<string>> SentimentResponses = new()
        {
            ["happy"] = new() {
                "That's wonderful to hear! 😊 A positive mindset is your first line of defence in cybersecurity — happy people are more alert!",
                "Love the energy! 🎉 Good things happen when you're in a great headspace. Ready to level up your cybersecurity knowledge?",
                "So glad you're feeling good! 😄 Let's channel that positivity into making your digital life safer too!"
            },
            ["sad"] = new() {
                "I'm sorry to hear you're not feeling great. 💙 Cybercriminals often target people when they're emotionally vulnerable — let me help keep you safe.",
                "That sounds tough. 😔 Take a breath — I'm here to help. Let's focus on learning how to stay safe online.",
                "I hear you. 💙 Learning new things can be a great distraction — what cybersecurity topic interests you?"
            },
            ["angry"] = new() {
                "I understand your frustration! 😤 Cybercriminals exploit angry emotions to get people to act without thinking. Stay sharp — I've got your back.",
                "Take it easy! 🔥 Social engineers actually love it when people are upset. You're smarter than that.",
                "I hear your frustration. 💢 Let's redirect that energy into protecting yourself online!"
            },
            ["stressed"] = new() {
                "I can tell you're under pressure. 😰 Stressed users are a top target for phishing — stress impairs decision-making. Let me help you stay alert.",
                "Take a deep breath. 🌬 Cybercriminals love creating urgency to stress you out — now you know their trick.",
                "You're not alone! 😟 Slowing down before clicking anything suspicious is your superpower right now."
            },
            ["tired"] = new() {
                "Get some rest when you can! 😴 Fatigue is a major risk factor for falling for scams — tired brains miss red flags.",
                "Being tired can make you more susceptible to cyber tricks. ☕ Never make important security decisions when exhausted!",
                "I hear you — rest is important! 💤 Let me give you quick, easy cybersecurity tips that don't require much brainpower right now."
            }
        };

        private static readonly Random _rand = new();

        public string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";
            var lower = input.ToLower();
            if (ContainsAny(lower, HappyKeywords))   return "happy";
            if (ContainsAny(lower, AngryKeywords))   return "angry";
            if (ContainsAny(lower, StressedKeywords))return "stressed";
            if (ContainsAny(lower, TiredKeywords))   return "tired";
            if (ContainsAny(lower, SadKeywords))     return "sad";
            return "neutral";
        }

        public string GetSentimentResponse(string sentiment)
        {
            if (sentiment == "neutral") return string.Empty;
            if (SentimentResponses.TryGetValue(sentiment, out var responses) && responses.Count > 0)
                return responses[_rand.Next(responses.Count)];
            return string.Empty;
        }

        public string SentimentToEmoji(string sentiment) => sentiment switch
        {
            "happy"   => "😊 Happy",
            "sad"     => "😔 Sad",
            "angry"   => "😤 Angry",
            "stressed"=> "😰 Stressed",
            "tired"   => "😴 Tired",
            _         => "😐 Neutral"
        };

        private static bool ContainsAny(string text, List<string> keywords)
        {
            foreach (var kw in keywords)
                if (text.Contains(kw, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }

    // ================================================================
    //  Chatbot — main logic with all 4 new features wired in
    // ================================================================
    public class Chatbot
    {
        public MemoryManager     Memory    { get; } = new MemoryManager();
        public SentimentAnalyzer Sentiment { get; } = new SentimentAnalyzer();
        public TaskManager       Tasks     { get; } = new TaskManager();
        public QuizEngine        Quiz      { get; } = new QuizEngine();
        public ActivityLog       Log       { get; } = new ActivityLog();

        public bool IsNameKnown => !string.IsNullOrEmpty(Memory.UserName);
        private bool _awaitingName        = true;
        private bool _awaitingTaskDate    = false;
        private bool _awaitingQuizAnswer  = false;
        private string _pendingTaskName   = string.Empty;

        private static readonly Dictionary<string, string> KeywordMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "phishing",           "phishing" },
            { "phish",              "phishing" },
            { "fake email",         "phishing" },
            { "spoofing",           "phishing" },
            { "smishing",           "phishing" },
            { "vishing",            "phishing" },
            { "spear phishing",     "phishing" },
            { "password",           "password safety" },
            { "passwords",          "password safety" },
            { "passphrase",         "password safety" },
            { "credential",         "password safety" },
            { "credentials",        "password safety" },
            { "login",              "password safety" },
            { "two-factor",         "two-factor authentication" },
            { "2fa",                "two-factor authentication" },
            { "mfa",                "two-factor authentication" },
            { "authenticator",      "two-factor authentication" },
            { "otp",                "two-factor authentication" },
            { "one-time",           "two-factor authentication" },
            { "ransomware",         "ransomware" },
            { "ransom",             "ransomware" },
            { "wannacry",           "ransomware" },
            { "encrypt",            "ransomware" },
            { "social engineering", "social engineering" },
            { "social engineer",    "social engineering" },
            { "pretexting",         "social engineering" },
            { "baiting",            "social engineering" },
            { "tailgating",         "social engineering" },
            { "manipulation",       "social engineering" },
            { "browsing",           "safe browsing" },
            { "browser",            "safe browsing" },
            { "https",              "safe browsing" },
            { "vpn",                "safe browsing" },
            { "website",            "safe browsing" },
            { "internet",           "safe browsing" },
            { "url",                "safe browsing" },
            { "malware",            "malware" },
            { "virus",              "malware" },
            { "trojan",             "malware" },
            { "spyware",            "malware" },
            { "worm",               "malware" },
            { "rootkit",            "malware" },
            { "keylogger",          "malware" },
            { "adware",             "malware" },
            { "antivirus",          "malware" },
            { "cyber hygiene",      "cyber hygiene" },
            { "hygiene",            "cyber hygiene" },
            { "digital hygiene",    "cyber hygiene" },
            { "habits",             "cyber hygiene" },
            { "backup",             "cyber hygiene" },
            { "update",             "cyber hygiene" },
            { "patch",              "cyber hygiene" },
            { "scam",               "scams" },
            { "scams",              "scams" },
            { "fraud",              "scams" },
            { "swindle",            "scams" },
            { "419",                "scams" },
            { "advance fee",        "scams" },
            { "romance scam",       "scams" },
            { "privacy",            "privacy" },
            { "personal data",      "privacy" },
            { "popia",              "privacy" },
            { "gdpr",               "privacy" },
            { "data protection",    "privacy" },
            { "tracking",           "privacy" },
            { "surveillance",       "privacy" },
        };

        private static readonly HashSet<string> GreetingWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "hi","hello","hey","howdy","greetings","good morning",
            "good afternoon","good evening","sup","what's up","yo"
        };

        private static readonly HashSet<string> FarewellWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "exit","quit","bye","goodbye","farewell","close","stop","leave"
        };

        public ChatResult Process(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return ChatResult.BotOnly("⚠ Please type something before sending. I'm here to help!", "warning");

                Memory.IncrementMessages();
                input = input.Trim();
                var lower = input.ToLower();

                // ── Farewell ───────────────────────────────────────
                if (FarewellWords.Any(f => lower == f || lower.StartsWith(f + " ")))
                    return ChatResult.Exit(BuildFarewellMessage());

                if (lower == "help" || lower == "?")
                    return ChatResult.BotOnly(Responses.HelpText, "info");

                if (lower == "clear")
                    return ChatResult.Special("clear");

                // ── Activity Log ───────────────────────────────────
                if (lower.Contains("show activity log") || lower.Contains("activity log") ||
                    lower.Contains("what have you done") || lower.Contains("show log") ||
                    lower.Contains("view log") || lower.Contains("history"))
                {
                    Log.LogActivityLogViewed();
                    return ChatResult.BotOnly(Log.GetFormattedLog(), "info");
                }

                if (lower.Contains("log summary") || lower.Contains("session summary"))
                    return ChatResult.BotOnly(Log.GetSummary(), "info");

                // ── Quiz: process answer ───────────────────────────
                if (_awaitingQuizAnswer && Quiz.IsActive)
                {
                    var qResult = Quiz.SubmitAnswer(input);
                    Log.LogQuizAnswered(qResult.IsCorrect, qResult.Score, qResult.Total);
                    string msg = $"{qResult.Feedback}\n\n{qResult.Explanation}";
                    if (qResult.IsFinished)
                    {
                        _awaitingQuizAnswer = false;
                        Log.LogQuizCompleted(qResult.Score, qResult.Total);
                        msg += "\n\n" + qResult.FinalSummary;
                    }
                    return ChatResult.BotOnly(msg, qResult.IsCorrect ? "success" : "error");
                }

                // ── Quiz: start ────────────────────────────────────
                if (lower.Contains("start quiz") || lower.Contains("begin quiz") ||
                    lower.Contains("take quiz")  || lower.Contains("play quiz") ||
                    lower == "quiz")
                {
                    Log.LogQuizStarted();
                    _awaitingQuizAnswer = true;
                    string quizIntro = Quiz.StartQuiz(5);
                    return ChatResult.BotOnly(
                        $"🎯 Welcome to the Cybersecurity Quiz! You'll get 5 questions.\n\n{quizIntro}", "info");
                }

                // ── Task: process reminder date ────────────────────
                if (_awaitingTaskDate)
                {
                    _awaitingTaskDate = false;
                    if (DateTime.TryParse(input, out DateTime reminderDate))
                    {
                        var (ok, msg, task) = Tasks.AddTask(_pendingTaskName, reminderDate);
                        if (ok && task != null)
                        {
                            Log.LogTaskAdded(task.TaskName);
                            Log.LogReminderCreated(task.TaskName, reminderDate);
                        }
                        _pendingTaskName = string.Empty;
                        return ChatResult.BotOnly(
                            msg + (ok && task != null
                                ? $"\n\n📌 Task: \"{task.TaskName}\"\n⏰ Reminder: {reminderDate:dd MMM yyyy HH:mm}"
                                : string.Empty),
                            ok ? "success" : "error");
                    }
                    else
                    {
                        var defaultDate = DateTime.Now.AddDays(1).Date;
                        var (ok, msg, task) = Tasks.AddTask(_pendingTaskName, defaultDate);
                        if (ok && task != null) Log.LogTaskAdded(task.TaskName);
                        _pendingTaskName = string.Empty;
                        return ChatResult.BotOnly(
                            $"⚠ Couldn't parse date. Task added with tomorrow as reminder.\n{msg}",
                            ok ? "warning" : "error");
                    }
                }

                // ── Task: show all ─────────────────────────────────
                if (lower.Contains("show tasks") || lower.Contains("view tasks") ||
                    lower.Contains("list tasks") || lower.Contains("my tasks") ||
                    lower.Contains("show reminders") || lower.Contains("view reminders"))
                {
                    Log.LogTaskViewed();
                    var (ok, _, taskList) = Tasks.GetAllTasks();
                    if (!ok || taskList.Count == 0)
                        return ChatResult.BotOnly("📭 No tasks found. Type 'add task' to create one!", "info");
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine($"📋 YOUR CYBERSECURITY TASKS ({taskList.Count} total)\n");
                    sb.AppendLine("ID  | Task                          | Due Date            | Status");
                    sb.AppendLine("─────────────────────────────────────────────────────────────");
                    foreach (var t in taskList)
                        sb.AppendLine($"{t.Id,-4}| {t.TaskName,-30} | {t.ReminderDate:dd MMM yyyy HH:mm} | {t.Status}");
                    sb.AppendLine("\nTip: Type 'delete task 3' or 'complete task 3' to manage tasks.");
                    return ChatResult.BotOnly(sb.ToString(), "info");
                }

                // ── Task: delete ───────────────────────────────────
                if (lower.Contains("delete task") || lower.Contains("remove task"))
                {
                    var idStr = System.Text.RegularExpressions.Regex.Match(input, @"\d+").Value;
                    if (int.TryParse(idStr, out int delId))
                    {
                        var (_, __, allT) = Tasks.GetAllTasks();
                        string tname = allT.Find(t => t.Id == delId)?.TaskName ?? $"ID {delId}";
                        var (ok, msg) = Tasks.DeleteTask(delId);
                        if (ok) Log.LogTaskDeleted(delId, tname);
                        return ChatResult.BotOnly(msg, ok ? "success" : "warning");
                    }
                    return ChatResult.BotOnly("⚠ Please specify a task ID. Example: 'delete task 3'", "warning");
                }

                // ── Task: complete ─────────────────────────────────
                if (lower.Contains("complete task") || lower.Contains("done task") ||
                    lower.Contains("mark complete")  || lower.Contains("finish task"))
                {
                    var idStr = System.Text.RegularExpressions.Regex.Match(input, @"\d+").Value;
                    if (int.TryParse(idStr, out int complId))
                    {
                        var (_, __, allT) = Tasks.GetAllTasks();
                        var existing = allT.Find(t => t.Id == complId);
                        if (existing != null)
                        {
                            var (ok, msg) = Tasks.UpdateTask(complId, existing.TaskName, existing.ReminderDate, "Completed");
                            if (ok) Log.LogTaskUpdated(complId, existing.TaskName);
                            return ChatResult.BotOnly(msg, ok ? "success" : "warning");
                        }
                    }
                    return ChatResult.BotOnly("⚠ Please specify a valid task ID. Example: 'complete task 3'", "warning");
                }

                // ── Task: NLP natural language ─────────────────────
                {
                    var (nlpFound, nlpTaskName) = TaskManager.ParseTaskFromMessage(input);
                    if (nlpFound)
                    {
                        _pendingTaskName  = nlpTaskName;
                        _awaitingTaskDate = true;
                        return ChatResult.BotOnly(
                            $"📌 I'll add \"{nlpTaskName}\" as a task.\n\n" +
                            "⏰ When would you like to be reminded? (e.g. 2025-12-31 09:00)",
                            "info");
                    }
                }

                // ── Task: explicit add ─────────────────────────────
                if (lower.Contains("add task") || lower.Contains("create task") ||
                    lower.Contains("new task")  || lower.Contains("set reminder"))
                {
                    string extracted = string.Empty;
                    string[] triggers = { "add task", "create task", "new task", "set reminder for", "set reminder to" };
                    foreach (var tr in triggers)
                    {
                        int idx = lower.IndexOf(tr, StringComparison.OrdinalIgnoreCase);
                        if (idx >= 0) { extracted = input.Substring(idx + tr.Length).Trim(); break; }
                    }
                    if (!string.IsNullOrWhiteSpace(extracted))
                    {
                        _pendingTaskName  = char.ToUpper(extracted[0]) + extracted.Substring(1);
                        _awaitingTaskDate = true;
                        return ChatResult.BotOnly(
                            $"📌 Adding task: \"{_pendingTaskName}\"\n\n⏰ What date/time? (e.g. 2025-12-31 09:00)",
                            "info");
                    }
                    return ChatResult.BotOnly(
                        "📌 What task would you like to add?\n\nExamples:\n• add task Enable Two-Factor Authentication\n• remind me to update my password",
                        "info");
                }

                // ── Name capture ───────────────────────────────────
                if (_awaitingName)
                {
                    if (lower.StartsWith("my name is "))
                        input = input.Substring("my name is ".Length).Trim();
                    return HandleNameCapture(input);
                }

                // ── Greeting ───────────────────────────────────────
                if (GreetingWords.Any(g => lower == g || lower.StartsWith(g + " ")))
                    return ChatResult.BotOnly(BuildGreetingResponse(), "bot");

                // ── Name shortcut ──────────────────────────────────
                if (lower.StartsWith("my name is "))
                {
                    var name = input.Substring("my name is ".Length).Trim();
                    return HandleNameCapture(name);
                }

                // ── Mood / sentiment ───────────────────────────────
                if (lower.StartsWith("i feel") || lower.StartsWith("i am feeling") ||
                    lower.StartsWith("i'm feeling") || lower.StartsWith("feeling"))
                {
                    var sentimentResult = HandleSentiment(input);
                    if (sentimentResult != null) return sentimentResult;
                }

                // ── Memory queries ─────────────────────────────────
                if (lower.Contains("my name") || lower.Contains("who am i"))
                    return ChatResult.BotOnly(
                        IsNameKnown
                            ? $"Your name is {Memory.UserName}! 🧠 I remembered it."
                            : "I don't know your name yet! Say 'My name is [your name]'.",
                        "info");

                if (lower.Contains("what do you know about me") || lower.Contains("what do you remember"))
                    return ChatResult.BotOnly(BuildMemoryRecall(), "info");

                // ── Topic detection ────────────────────────────────
                var detectedTopic = DetectTopic(lower);
                if (detectedTopic != null)
                {
                    Memory.AddTopicToHistory(detectedTopic);
                    Log.LogTopicExplored(detectedTopic);
                    return ChatResult.Topic(GetTopicResponse(detectedTopic), detectedTopic);
                }

                // ── Sentiment fallback ─────────────────────────────
                var sentiment = HandleSentiment(input);
                if (sentiment != null) return sentiment;

                return ChatResult.BotOnly(Responses.GetRandomDefault(), "warning");
            }
            catch (Exception ex)
            {
                return ChatResult.BotOnly($"⚠ Something unexpected happened. Please try again! (Error: {ex.Message})", "error");
            }
        }

        public ChatResult ProcessTopic(string topic)
        {
            try
            {
                Memory.IncrementMessages();
                Memory.AddTopicToHistory(topic);
                Log.LogTopicExplored(topic);
                return ChatResult.Topic(GetTopicResponse(topic), topic);
            }
            catch (Exception ex)
            {
                return ChatResult.BotOnly($"⚠ Could not load topic. {ex.Message}", "error");
            }
        }

        private ChatResult HandleNameCapture(string input)
        {
            if (input.Length < 2 || input.Length > 50 || input.All(char.IsDigit))
                return ChatResult.BotOnly(
                    "⚠ That doesn't look like a name. Please enter your first name!",
                    "warning");

            var name = char.ToUpper(input[0]) + input.Substring(1).ToLower();
            Memory.UserName = name;
            Memory.Remember("name", name);
            _awaitingName = false;

            return ChatResult.BotOnly(
                Responses.GetRandom(Responses.NameResponses(name)) +
                "\n\n💡 Tip: Type 'help' to see everything I can do, or click a topic on the left!",
                "success");
        }

        private ChatResult? HandleSentiment(string input)
        {
            var sentiment = Sentiment.DetectSentiment(input);
            if (sentiment == "neutral") return null;
            Memory.UserMood = Sentiment.SentimentToEmoji(sentiment);
            var reply = Sentiment.GetSentimentResponse(sentiment);
            return string.IsNullOrEmpty(reply) ? null : ChatResult.WithSentiment(reply, sentiment);
        }

        private string DetectTopic(string lower)
        {
            string[] directTopics = {
                "phishing","password safety","two-factor authentication",
                "ransomware","social engineering","safe browsing",
                "malware","cyber hygiene","scams","privacy"
            };
            foreach (var t in directTopics)
                if (lower.Contains(t)) return t;

            var orderedKeywords = KeywordMap.Keys.OrderByDescending(k => k.Length);
            foreach (var kw in orderedKeywords)
                if (lower.Contains(kw)) return KeywordMap[kw];

            return null!;
        }

        private string GetTopicResponse(string topic) => topic.ToLower() switch
        {
            "phishing"                 => Responses.GetRandom(Responses.PhishingResponses),
            "password safety"          => Responses.GetRandom(Responses.PasswordSafetyResponses),
            "two-factor authentication"=> Responses.GetRandom(Responses.TwoFactorResponses),
            "ransomware"               => Responses.GetRandom(Responses.RansomwareResponses),
            "social engineering"       => Responses.GetRandom(Responses.SocialEngineeringResponses),
            "safe browsing"            => Responses.GetRandom(Responses.SafeBrowsingResponses),
            "malware"                  => Responses.GetRandom(Responses.MalwareResponses),
            "cyber hygiene"            => Responses.GetRandom(Responses.CyberHygieneResponses),
            "scams"                    => Responses.GetRandom(Responses.ScamResponses),
            "privacy"                  => Responses.GetRandom(Responses.PrivacyResponses),
            _                          => Responses.GetRandomDefault()
        };

        private string BuildGreetingResponse()
        {
            if (IsNameKnown)
                return $"Hey {Memory.UserName}! 👋 Great to see you again. What cybersecurity topic can I help you with?";
            return Responses.GetRandom(Responses.GreetingResponses);
        }

        private string BuildFarewellMessage()
        {
            var name = IsNameKnown ? $", {Memory.UserName}" : string.Empty;
            return $"Goodbye{name}! 🛡 Stay safe online — remember:\n\n" +
                   "• Use strong, unique passwords\n• Enable 2FA everywhere\n• Think before you click!\n\n" +
                   $"You explored {Memory.TopicsExplored} topic(s) this session. Come back anytime!";
        }

        private string BuildMemoryRecall()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("🧠 Here's what I remember about you:\n");
            sb.AppendLine($"• Name:            {(IsNameKnown ? Memory.UserName : "Not told yet")}");
            sb.AppendLine($"• Mood:            {(string.IsNullOrEmpty(Memory.UserMood) ? "Not shared yet" : Memory.UserMood)}");
            sb.AppendLine($"• Favourite topic: {(string.IsNullOrEmpty(Memory.FavouriteTopic) ? "None yet" : Memory.FavouriteTopic)}");
            sb.AppendLine($"• Topics explored: {Memory.TopicsExplored}");
            sb.AppendLine($"• Messages sent:   {Memory.MessageCount}");
            if (Memory.TopicHistory.Count > 0)
            {
                sb.AppendLine("\nTopics covered this session:");
                foreach (var t in Memory.TopicHistory) sb.AppendLine($"  ✅ {t}");
            }
            return sb.ToString().TrimEnd();
        }

        public void ResetSession()
        {
            Memory.Reset();
            Log.LogSessionReset();
            _awaitingName       = true;
            _awaitingTaskDate   = false;
            _awaitingQuizAnswer = false;
            _pendingTaskName    = string.Empty;
        }
    }

    // ================================================================
    //  ChatResult
    // ================================================================
    public class ChatResult
    {
        public string BotMessage        { get; set; } = string.Empty;
        public string MessageType       { get; set; } = "bot";
        public string DetectedTopic     { get; set; } = string.Empty;
        public string DetectedSentiment { get; set; } = string.Empty;
        public bool   ShouldExit        { get; set; } = false;
        public bool   IsSpecialCommand  { get; set; } = false;
        public string SpecialCommand    { get; set; } = string.Empty;

        public static ChatResult BotOnly(string msg, string type = "bot") =>
            new() { BotMessage = msg, MessageType = type };
        public static ChatResult Topic(string msg, string topic) =>
            new() { BotMessage = msg, MessageType = "topic", DetectedTopic = topic };
        public static ChatResult WithSentiment(string msg, string sentiment) =>
            new() { BotMessage = msg, MessageType = "sentiment", DetectedSentiment = sentiment };
        public static ChatResult Exit(string msg) =>
            new() { BotMessage = msg, MessageType = "farewell", ShouldExit = true };
        public static ChatResult Special(string command) =>
            new() { IsSpecialCommand = true, SpecialCommand = command };
    }
}
