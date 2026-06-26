using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityPOE
{
    // ============================================================
    //  QuizQuestion  — data model
    // ============================================================
    public class QuizQuestion
    {
        public string Question    { get; set; } = string.Empty;
        public string[] Options   { get; set; } = Array.Empty<string>();
        public int    CorrectIndex { get; set; }           // 0-based
        public string Explanation { get; set; } = string.Empty;
        public string Topic       { get; set; } = string.Empty;
    }

    // ============================================================
    //  QuizResult  — per-answer result
    // ============================================================
    public class QuizResult
    {
        public bool   IsCorrect   { get; set; }
        public string Feedback    { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int    Score       { get; set; }
        public int    Total       { get; set; }
        public bool   IsFinished  { get; set; }
        public string FinalSummary { get; set; } = string.Empty;
    }

    // ============================================================
    //  QuizEngine
    // ============================================================
    public class QuizEngine
    {
        private static readonly Random _rand = new();

        private List<QuizQuestion> _questions = new();
        private int  _currentIndex = 0;
        private int  _score        = 0;
        private bool _isActive     = false;
        private bool _awaitingAnswer = false;

        public bool IsActive        => _isActive;
        public bool IsAwaitingAnswer => _awaitingAnswer;
        public int  Score           => _score;
        public int  TotalQuestions  => _questions.Count;
        public QuizQuestion? CurrentQuestion =>
            (_currentIndex < _questions.Count) ? _questions[_currentIndex] : null;

        // ── Full question bank ────────────────────────────────────
        private static readonly List<QuizQuestion> QuestionBank = new()
        {
            new()
            {
                Question     = "What should you do if an email asks for your password?",
                Options      = new[] { "A. Reply with your password", "B. Delete the email", "C. Report it as phishing ✅", "D. Ignore it" },
                CorrectIndex = 2,
                Explanation  = "Reporting phishing emails helps your email provider block similar attacks and protects other users. Legitimate organisations NEVER ask for your password via email.",
                Topic        = "Phishing"
            },
            new()
            {
                Question     = "Which of the following is the STRONGEST password?",
                Options      = new[] { "A. password123", "B. John1990!", "C. P@ssw0rd", "D. Xk#9!mQ2$vL7@pR ✅" },
                CorrectIndex = 3,
                Explanation  = "A strong password is long (16+ characters), random, and mixes uppercase, lowercase, numbers and symbols. Avoid personal info like your name or birth year.",
                Topic        = "Password Safety"
            },
            new()
            {
                Question     = "What does 2FA stand for?",
                Options      = new[] { "A. Two-File Access", "B. Two-Factor Authentication ✅", "C. Twice-Fast Application", "D. Two-Form Authorisation" },
                CorrectIndex = 1,
                Explanation  = "Two-Factor Authentication adds a second verification step (like a one-time code) on top of your password, making it much harder for attackers to access your account even if they have your password.",
                Topic        = "Two-Factor Authentication"
            },
            new()
            {
                Question     = "What is ransomware?",
                Options      = new[] { "A. Software that speeds up your PC", "B. A type of firewall", "C. Malware that encrypts your files and demands payment ✅", "D. An online backup service" },
                CorrectIndex = 2,
                Explanation  = "Ransomware encrypts your files, making them inaccessible until you pay a ransom. Regular offline backups are the best defence — if you have a clean backup, you can restore without paying.",
                Topic        = "Ransomware"
            },
            new()
            {
                Question     = "A stranger calls claiming to be from Microsoft and says your PC has a virus. What do you do?",
                Options      = new[] { "A. Give them remote access", "B. Provide your credit card to fix it", "C. Hang up immediately ✅", "D. Follow their instructions" },
                CorrectIndex = 2,
                Explanation  = "This is a classic tech-support scam. Microsoft will never call you unsolicited. Hang up and report the number to your local cybercrime authority.",
                Topic        = "Social Engineering"
            },
            new()
            {
                Question     = "Which URL is SAFER to visit for online banking?",
                Options      = new[] { "A. http://mybank.com", "B. https://mybank.com ✅", "C. http://mybank-secure.net", "D. They are all equally safe" },
                CorrectIndex = 1,
                Explanation  = "HTTPS encrypts data between your browser and the server. Always look for the padlock icon. Note that HTTPS alone does not guarantee the site is legitimate — also check the domain carefully.",
                Topic        = "Safe Browsing"
            },
            new()
            {
                Question     = "What is a Trojan horse in cybersecurity?",
                Options      = new[] { "A. A strong firewall", "B. Malware disguised as legitimate software ✅", "C. A type of VPN", "D. An encrypted file" },
                CorrectIndex = 1,
                Explanation  = "Like the mythological Trojan Horse, this malware disguises itself as useful software. Once installed it can steal data, open backdoors, or download more malware.",
                Topic        = "Malware"
            },
            new()
            {
                Question     = "How often should you update your software and operating system?",
                Options      = new[] { "A. Once a year", "B. Never — updates cause problems", "C. Only when something breaks", "D. As soon as updates are available ✅" },
                CorrectIndex = 3,
                Explanation  = "Software updates patch security vulnerabilities. Attackers actively exploit unpatched systems — many major breaches have occurred because organisations delayed applying known patches.",
                Topic        = "Cyber Hygiene"
            },
            new()
            {
                Question     = "You receive an email saying you've won a R50,000 prize. What should you do?",
                Options      = new[] { "A. Claim it by providing your bank details", "B. Forward it to friends", "C. Delete it — it is almost certainly a scam ✅", "D. Click the link to verify" },
                CorrectIndex = 2,
                Explanation  = "Advance-fee and prize scams lure victims with fake winnings to steal personal and financial information. If it sounds too good to be true, it is.",
                Topic        = "Scams"
            },
            new()
            {
                Question     = "What is the POPIA Act (South Africa)?",
                Options      = new[] { "A. A firewall regulation", "B. A law protecting personal information ✅", "C. A cybercrime treaty", "D. A social-media moderation rule" },
                CorrectIndex = 1,
                Explanation  = "The Protection of Personal Information Act (POPIA) governs how organisations collect, store, and use your personal data in South Africa. It gives you rights over your own information.",
                Topic        = "Privacy"
            },
            new()
            {
                Question     = "What is the purpose of a VPN?",
                Options      = new[] { "A. Speed up your internet connection", "B. Encrypt your internet traffic and hide your IP address ✅", "C. Block all advertisements", "D. Scan for viruses" },
                CorrectIndex = 1,
                Explanation  = "A VPN (Virtual Private Network) creates an encrypted tunnel for your internet traffic, making it much harder for third parties — ISPs, hackers on public Wi-Fi, or surveillance — to monitor your activity.",
                Topic        = "Safe Browsing"
            },
            new()
            {
                Question     = "Which practice is BEST for protecting your email account?",
                Options      = new[] { "A. Using the same password as your other accounts for convenience", "B. Sharing login details with a trusted friend as backup", "C. Enabling 2FA and using a unique, strong password ✅", "D. Logging in only from home" },
                CorrectIndex = 2,
                Explanation  = "Your email is the master key to most of your online accounts (password resets go there). Protect it with a unique strong password AND two-factor authentication.",
                Topic        = "Password Safety"
            },
        };

        // ── Start a new quiz ──────────────────────────────────────
        public string StartQuiz(int questionCount = 5)
        {
            _questions    = QuestionBank.OrderBy(_ => _rand.Next()).Take(questionCount).ToList();
            _currentIndex = 0;
            _score        = 0;
            _isActive     = true;
            _awaitingAnswer = true;

            return BuildQuestionText(_questions[0], 1);
        }

        // ── Process answer: "A", "B", "C", "D" or "1","2","3","4" ──
        public QuizResult SubmitAnswer(string answer)
        {
            if (!_isActive || CurrentQuestion == null)
                return new QuizResult { Feedback = "No active quiz. Type 'start quiz' to begin!", IsFinished = true };

            var q = CurrentQuestion;
            int answerIndex = ParseAnswer(answer);

            if (answerIndex < 0)
                return new QuizResult
                {
                    Feedback    = "⚠ Please answer with A, B, C, or D.",
                    Explanation = string.Empty,
                    Score       = _score,
                    Total       = _questions.Count
                };

            bool correct = (answerIndex == q.CorrectIndex);
            if (correct) _score++;

            _currentIndex++;
            bool finished = (_currentIndex >= _questions.Count);
            _awaitingAnswer = !finished;

            string nextQuestion = string.Empty;
            if (!finished)
                nextQuestion = "\n\n" + BuildQuestionText(_questions[_currentIndex], _currentIndex + 1);

            string summary = string.Empty;
            if (finished)
            {
                _isActive = false;
                summary   = BuildFinalSummary();
            }

            return new QuizResult
            {
                IsCorrect    = correct,
                Feedback     = correct ? "✅ Correct!" : $"❌ Incorrect! The right answer was {OptionLetter(q.CorrectIndex)}.",
                Explanation  = q.Explanation + nextQuestion,
                Score        = _score,
                Total        = _questions.Count,
                IsFinished   = finished,
                FinalSummary = summary
            };
        }

        // ── Helpers ───────────────────────────────────────────────
        private static string BuildQuestionText(QuizQuestion q, int num) =>
            $"❓ Question {num}: {q.Question}\n\n" +
            string.Join("\n", q.Options) +
            "\n\n(Topic: " + q.Topic + ") — Type A, B, C or D to answer.";

        private string BuildFinalSummary()
        {
            string rating = _score switch
            {
                var s when s == _questions.Count => "🏆 Perfect score! You're a Cyber Expert!",
                var s when s >= _questions.Count * 0.8 => "🥇 Excellent! Very impressive cybersecurity knowledge.",
                var s when s >= _questions.Count * 0.6 => "🥈 Good job! A bit more study and you'll ace it.",
                var s when s >= _questions.Count * 0.4 => "🥉 Fair attempt. Keep learning to stay protected.",
                _ => "📚 Keep practicing — cybersecurity knowledge saves you from real threats!"
            };

            return $"🎯 Quiz Complete!\n\nYour Score: {_score}/{_questions.Count}\n{rating}\n\nType 'start quiz' to try again with new questions.";
        }

        private static int ParseAnswer(string answer)
        {
            var a = answer.Trim().ToUpper();
            return a switch
            {
                "A" or "1" => 0,
                "B" or "2" => 1,
                "C" or "3" => 2,
                "D" or "4" => 3,
                _ => -1
            };
        }

        private static string OptionLetter(int index) => index switch
        {
            0 => "A", 1 => "B", 2 => "C", 3 => "D", _ => "?"
        };
    }
}
