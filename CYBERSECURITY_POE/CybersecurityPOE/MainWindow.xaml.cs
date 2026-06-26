using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CybersecurityPOE
{
    public partial class MainWindow : Window
    {
        // ── Core engine ───────────────────────────────────────────
        private readonly Chatbot _chatbot = new();
        private readonly SpeechSynthesizer _synth = new();
        private readonly DispatcherTimer _clockTimer = new();
        private readonly ObservableCollection<string> _detectedKeywords = new();
        private readonly ObservableCollection<string> _exploredTopics   = new();

        // ── State ─────────────────────────────────────────────────
        private bool _speechEnabled = true;
        private int  _tipIndex      = 0;

        // ── Terminal colour palette for message bubbles ───────────
        // Completely different from the original cyan/navy scheme.
        // Now uses green phosphor + amber on pure-black backgrounds.
        private static readonly Dictionary<string, (Color border, Color bg, Color text)> BubbleColors = new()
        {
            ["bot"]       = (Color.FromRgb(0, 120, 0),    Color.FromRgb(5, 14, 5),    Color.FromRgb(180, 255, 180)),
            ["user"]      = (Color.FromRgb(0, 80, 0),     Color.FromRgb(4, 10, 4),    Color.FromRgb(0, 255, 65)),
            ["success"]   = (Color.FromRgb(0, 200, 80),   Color.FromRgb(4, 14, 4),    Color.FromRgb(150, 255, 150)),
            ["warning"]   = (Color.FromRgb(200, 130, 0),  Color.FromRgb(14, 9, 0),    Color.FromRgb(255, 200, 80)),
            ["error"]     = (Color.FromRgb(200, 30, 30),  Color.FromRgb(14, 4, 4),    Color.FromRgb(255, 140, 140)),
            ["info"]      = (Color.FromRgb(0, 160, 100),  Color.FromRgb(4, 12, 9),    Color.FromRgb(140, 255, 200)),
            ["topic"]     = (Color.FromRgb(0, 200, 60),   Color.FromRgb(4, 14, 5),    Color.FromRgb(200, 255, 200)),
            ["sentiment"] = (Color.FromRgb(180, 120, 0),  Color.FromRgb(12, 8, 0),    Color.FromRgb(255, 210, 120)),
            ["farewell"]  = (Color.FromRgb(0, 140, 60),   Color.FromRgb(4, 12, 6),    Color.FromRgb(160, 255, 180)),
            ["system"]    = (Color.FromRgb(40, 80, 40),   Color.FromRgb(4, 8, 4),     Color.FromRgb(100, 160, 100)),
        };

        public MainWindow()
        {
            InitializeComponent();
            KeywordsList.ItemsSource        = _detectedKeywords;
            TopicsExploredList.ItemsSource  = _exploredTopics;

            ConfigureSpeech();
            ConfigureClock();
            StartSession();
        }

        // ── Speech setup ─────────────────────────────────────────
        private void ConfigureSpeech()
        {
            try
            {
                var voices    = _synth.GetInstalledVoices();
                var preferred = voices.FirstOrDefault(v =>
                    v.VoiceInfo.Name.Contains("David", StringComparison.OrdinalIgnoreCase));

                if (preferred != null) _synth.SelectVoice(preferred.VoiceInfo.Name);
                _synth.Rate   = 2;
                _synth.Volume = 80;
            }
            catch { _speechEnabled = false; }
        }

        // ── Clock ────────────────────────────────────────────────
        private void ConfigureClock()
        {
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick    += (_, _) => ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            _clockTimer.Start();
            ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        // ── Session start ────────────────────────────────────────
        private async void StartSession()
        {
            await Task.Delay(400);
            _chatbot.Log.LogSessionStarted();

            SpeakAsync("Hello! I am your Cybersecurity POE Bot. What is your name?");

            AppendBotMessage(
                "// SYSTEM BOOT COMPLETE\n" +
                "// CYBERSECURITY POE v2.0  —  Awareness Terminal\n\n" +
                "Hello, operator. I'm your Cybersecurity POE Bot.\n" +
                "I'm here to help you stay safe in the digital world.\n\n" +
                ">> What is your name?",
                "bot");

            UpdateSidePanels();
            InputBox.Focus();
        }

        // ── Topic button click ───────────────────────────────────
        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string topic)
            {
                AppendUserMessage($"> {topic}");
                var result = _chatbot.ProcessTopic(topic);
                HandleChatResult(result);

                if (!_exploredTopics.Contains(topic))
                    _exploredTopics.Add($"✔ {topic}");
                NoTopicsLabel.Visibility = _exploredTopics.Count == 0
                    ? Visibility.Visible : Visibility.Collapsed;

                SpeakTopicIntro(topic);
                UpdateSidePanels();
            }
        }

        // ── Action button handlers ───────────────────────────────
        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("> start quiz");
            var result = _chatbot.Process("start quiz");
            HandleChatResult(result);
            UpdateQuizScore();
        }

        private void BtnShowTasks_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("> show tasks");
            var result = _chatbot.Process("show tasks");
            HandleChatResult(result);
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("> add task");
            var result = _chatbot.Process("add task");
            HandleChatResult(result);
        }

        private void BtnActivityLog_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("> show activity log");
            var result = _chatbot.Process("show activity log");
            HandleChatResult(result);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _chatbot.ResetSession();
            ChatPanel.Children.Clear();
            _detectedKeywords.Clear();
            _exploredTopics.Clear();
            NoKeywordsLabel.Visibility = Visibility.Visible;
            NoTopicsLabel.Visibility   = Visibility.Visible;
            UpdateSidePanels();
            AppendSystemMessage("// SESSION RESET — all memory cleared");
            StartSession();
        }

        private void BtnSpeech_Click(object sender, RoutedEventArgs e)
        {
            _speechEnabled = !_speechEnabled;
            AppendSystemMessage(_speechEnabled
                ? "// VOICE OUTPUT: ENABLED"
                : "// VOICE OUTPUT: DISABLED");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            AppendSystemMessage("// TERMINAL CLEARED");
        }

        // ── Quick task from sidebar ──────────────────────────────
        private void BtnQuickAddTask_Click(object sender, RoutedEventArgs e)
        {
            var taskName = QuickTaskName.Text.Trim();
            if (string.IsNullOrWhiteSpace(taskName) || taskName == "Task name...")
            {
                AppendBotMessage("// ERROR: task name cannot be empty.", "warning");
                return;
            }

            var date = QuickTaskDate.SelectedDate ?? DateTime.Now.AddDays(1);
            var (ok, msg, task) = _chatbot.Tasks.AddTask(taskName, date);

            if (ok && task != null)
            {
                _chatbot.Log.LogTaskAdded(task.TaskName);
                _chatbot.Log.LogReminderCreated(task.TaskName, date);
                AppendBotMessage(
                    $"// TASK QUEUED\n>> \"{task.TaskName}\"\n>> Reminder: {date:dd MMM yyyy}",
                    "success");
                QuickTaskName.Text = string.Empty;
                UpdateTaskStatus();
            }
            else
            {
                AppendBotMessage($"// ERROR: {msg}", "error");
            }
            UpdateSidePanels();
        }

        private void QuickTaskName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (QuickTaskName.Text == "Task name...")
            {
                QuickTaskName.Text       = string.Empty;
                QuickTaskName.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 65));
            }
        }

        private void QuickTaskName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuickTaskName.Text))
            {
                QuickTaskName.Text       = "Task name...";
                QuickTaskName.Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 40));
            }
        }

        // ── Input handling ───────────────────────────────────────
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(InputBox.Text))
                SubmitInput();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e) => SubmitInput();

        private void SubmitInput()
        {
            var text = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            AppendUserMessage($"> {text}");
            InputBox.Clear();

            var result = _chatbot.Process(text);
            HandleChatResult(result);
            UpdateSidePanels();
        }

        // ── Route ChatResult ─────────────────────────────────────
        private void HandleChatResult(ChatResult result)
        {
            if (result.IsSpecialCommand)
            {
                if (result.SpecialCommand == "clear") ChatPanel.Children.Clear();
                return;
            }

            if (!string.IsNullOrEmpty(result.BotMessage))
                AppendBotMessage(result.BotMessage, result.MessageType);

            if (!string.IsNullOrEmpty(result.DetectedTopic))
            {
                if (!_detectedKeywords.Contains(result.DetectedTopic))
                {
                    _detectedKeywords.Add(result.DetectedTopic);
                    NoKeywordsLabel.Visibility = Visibility.Collapsed;
                }
                if (!_exploredTopics.Contains($"✔ {result.DetectedTopic}"))
                {
                    _exploredTopics.Add($"✔ {result.DetectedTopic}");
                    NoTopicsLabel.Visibility = Visibility.Collapsed;
                }
                SpeakTopicIntro(result.DetectedTopic);
            }

            if (result.ShouldExit)
            {
                Task.Delay(2000).ContinueWith(_ =>
                    Dispatcher.Invoke(Close));
            }

            UpdateQuizScore();
        }

        // ── Update right-panel & status bar ─────────────────────
        private void UpdateSidePanels()
        {
            MemUserName.Text  = _chatbot.Memory.IsNameKnown() ? _chatbot.Memory.UserName : "—";
            MemMood.Text      = !string.IsNullOrEmpty(_chatbot.Memory.UserMood)
                ? _chatbot.Memory.UserMood : "—";
            MemTopic.Text     = !string.IsNullOrEmpty(_chatbot.Memory.FavouriteTopic)
                ? _chatbot.Memory.FavouriteTopic : "—";
            MemMessages.Text  = _chatbot.Memory.MessageCount.ToString();

            UserStatusLabel.Text = _chatbot.Memory.IsNameKnown()
                ? $"USER: {_chatbot.Memory.UserName.ToUpper()}"
                : "NO SESSION";

            var score = _chatbot.Memory.AwarenessScore;
            AwarenessBar.Value   = score;
            AwarenessLabel.Text  = score switch
            {
                0       => "0%  — INIT",
                < 25    => $"{score}% — BEGINNER",
                < 50    => $"{score}% — DEVELOPING",
                < 75    => $"{score}% — INTERMEDIATE",
                < 90    => $"{score}% — ADVANCED",
                _       => $"{score}% — EXPERT ★"
            };

            _tipIndex           = (_tipIndex + 1) % Responses.SecurityTips.Count;
            SecurityTipLabel.Text = Responses.SecurityTips[_tipIndex];

            var topics = _chatbot.Memory.TopicsExplored;
            StatusLabel.Text = topics > 0
                ? $"CYBERSECURITY POE  //  {topics} topic(s) loaded  //  {_chatbot.Memory.MessageCount} msg(s) processed"
                : "CYBERSECURITY POE  //  system ready  //  awaiting input...";

            NoKeywordsLabel.Visibility = _detectedKeywords.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;

            UpdateTaskStatus();
            UpdateQuizScore();
        }

        private void UpdateTaskStatus()
        {
            bool dbOk = _chatbot.Tasks.IsDatabaseAvailable;
            TaskStatusLabel.Text      = dbOk ? "● MySQL connected" : "● in-memory mode";
            TaskStatusLabel.Foreground = new SolidColorBrush(
                dbOk ? Color.FromRgb(0, 200, 80) : Color.FromRgb(120, 120, 40));
        }

        private void UpdateQuizScore()
        {
            int  score  = _chatbot.Quiz.Score;
            int  total  = _chatbot.Quiz.TotalQuestions;
            bool active = _chatbot.Quiz.IsActive;

            QuizScoreLabel.Text = active
                ? $"// IN PROGRESS\n>> score: {score}/{total}"
                : total == 0
                    ? "no quiz taken yet."
                    : $"// COMPLETED\n>> score: {score}/{total}";
        }

        // ── Render messages ──────────────────────────────────────
        private void AppendUserMessage(string text)
        {
            var (border, bg, fg) = BubbleColors["user"];

            var tb = new TextBlock
            {
                Text        = text,
                TextWrapping= TextWrapping.Wrap,
                FontSize    = 13,
                FontFamily  = new FontFamily("Consolas"),
                Foreground  = new SolidColorBrush(fg),
                LineHeight  = 18
            };

            var bubble = new Border
            {
                Background      = new SolidColorBrush(bg),
                BorderBrush     = new SolidColorBrush(border),
                BorderThickness = new Thickness(1),
                CornerRadius    = new CornerRadius(0, 0, 0, 8),
                Padding         = new Thickness(12, 8, 12, 8),
                MaxWidth        = 700,
                HorizontalAlignment = HorizontalAlignment.Right,
                Child           = tb
            };

            var label = new TextBlock
            {
                Text            = $"operator  •  {DateTime.Now:HH:mm:ss}",
                FontSize        = 9,
                FontFamily      = new FontFamily("Consolas"),
                Foreground      = new SolidColorBrush(Color.FromRgb(40, 80, 40)),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin          = new Thickness(0, 0, 2, 2)
            };

            var wrapper = new StackPanel { Margin = new Thickness(60, 5, 0, 5) };
            wrapper.Children.Add(label);
            wrapper.Children.Add(bubble);

            FadeIn(wrapper);
            ChatPanel.Children.Add(wrapper);
            ScrollToBottom();
        }

        private void AppendBotMessage(string text, string msgType = "bot")
        {
            if (!BubbleColors.TryGetValue(msgType, out var colors))
                colors = BubbleColors["bot"];
            var (borderColor, bgColor, textColor) = colors;

            string prefix = msgType switch
            {
                "success"   => "// CYBERPOE  [OK]",
                "warning"   => "// CYBERPOE  [WARN]",
                "error"     => "// CYBERPOE  [ERR]",
                "info"      => "// CYBERPOE  [INFO]",
                "topic"     => "// CYBERPOE  [TOPIC]",
                "sentiment" => "// CYBERPOE  [MOOD]",
                "farewell"  => "// CYBERPOE  [EXIT]",
                "system"    => "// SYS",
                _           => "// CYBERPOE"
            };

            var label = new TextBlock
            {
                Text        = $"{prefix}  •  {DateTime.Now:HH:mm:ss}",
                FontSize    = 9,
                FontFamily  = new FontFamily("Consolas"),
                Foreground  = new SolidColorBrush(borderColor) { Opacity = 0.7 },
                Margin      = new Thickness(2, 0, 0, 2)
            };

            var tb = new TextBlock
            {
                Text         = text,
                TextWrapping = TextWrapping.Wrap,
                FontSize     = 13,
                FontFamily   = new FontFamily("Consolas"),
                Foreground   = new SolidColorBrush(textColor),
                LineHeight   = 18
            };

            var bubble = new Border
            {
                Background      = new SolidColorBrush(bgColor),
                BorderBrush     = new SolidColorBrush(borderColor),
                BorderThickness = new Thickness(1),
                CornerRadius    = new CornerRadius(0, 8, 8, 0),
                Padding         = new Thickness(12, 8, 12, 8),
                MaxWidth        = 700,
                HorizontalAlignment = HorizontalAlignment.Left,
                Child           = tb,
                Effect          = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color       = borderColor,
                    BlurRadius  = 8,
                    ShadowDepth = 0,
                    Opacity     = 0.25
                }
            };

            var wrapper = new StackPanel { Margin = new Thickness(0, 5, 60, 5) };
            wrapper.Children.Add(label);
            wrapper.Children.Add(bubble);

            FadeIn(wrapper);
            ChatPanel.Children.Add(wrapper);
            ScrollToBottom();
        }

        private void AppendSystemMessage(string text)
        {
            var tb = new TextBlock
            {
                Text                = text,
                FontSize            = 10,
                FontFamily          = new FontFamily("Consolas"),
                Foreground          = new SolidColorBrush(Color.FromRgb(40, 80, 40)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin              = new Thickness(0, 6, 0, 6),
                TextWrapping        = TextWrapping.Wrap
            };
            FadeIn(tb);
            ChatPanel.Children.Add(tb);
            ScrollToBottom();
        }

        // ── Speech ───────────────────────────────────────────────
        private void SpeakAsync(string text)
        {
            if (!_speechEnabled) return;
            Task.Run(() =>
            {
                try { _synth.SpeakAsync(text); }
                catch { /* non-fatal */ }
            });
        }

        private void SpeakTopicIntro(string topic)
        {
            var intro = topic switch
            {
                "phishing"                  => "Here's what you need to know about phishing.",
                "password safety"           => "Let me tell you about keeping your passwords safe.",
                "two-factor authentication" => "Two-factor authentication is one of the best defences you have.",
                "ransomware"                => "Ransomware is a serious threat. Here's how to protect yourself.",
                "social engineering"        => "Social engineering targets your mind, not your machine.",
                "safe browsing"             => "Here are some safe browsing tips for you.",
                "malware"                   => "Let me explain malware and how to stay protected.",
                "cyber hygiene"             => "Good cyber hygiene is your daily digital health routine.",
                "scams"                     => "Scams are everywhere. Here's how to spot and avoid them.",
                "privacy"                   => "Your privacy matters. Here's how to protect it.",
                _                           => $"Here's some information on {topic}."
            };
            SpeakAsync(intro);
        }

        // ── Animation helpers ────────────────────────────────────
        private void ScrollToBottom()
        {
            Dispatcher.InvokeAsync(
                () => ChatScrollViewer.ScrollToBottom(),
                DispatcherPriority.Background);
        }

        private static void FadeIn(UIElement element)
        {
            element.Opacity = 0;
            var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            element.BeginAnimation(OpacityProperty, anim);
        }

        protected override void OnClosed(EventArgs e)
        {
            _clockTimer.Stop();
            _synth.Dispose();
            base.OnClosed(e);
        }
    }
}
