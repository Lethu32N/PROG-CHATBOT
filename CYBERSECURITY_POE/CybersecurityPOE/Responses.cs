using System;
using System.Collections.Generic;

namespace CybersecurityPOE
{
    public static class Responses
    {
        private static readonly Random _rand = new();

        public static readonly List<string> SecurityTips = new()
        {
            "Use a password manager to generate and store unique, strong passwords for every account.",
            "Enable two-factor authentication (2FA) on all accounts that support it — especially email and banking.",
            "Never click links in unexpected emails. Go directly to the website by typing the address yourself.",
            "Keep your operating system and apps updated — patches close security holes attackers exploit.",
            "Back up your data regularly using the 3-2-1 rule: 3 copies, 2 media types, 1 offsite.",
            "Use a VPN on public Wi-Fi to encrypt your internet traffic from prying eyes.",
            "Check URLs carefully — phishing sites often use domains like 'paypa1.com' instead of 'paypal.com'.",
            "Never share your OTP (one-time password) with anyone — legitimate services never ask for it.",
            "Lock your devices with a PIN, fingerprint, or face ID — even at home.",
            "Be suspicious of unsolicited calls claiming to be from your bank or tech support.",
            "Review app permissions regularly — does that flashlight app really need your contacts?",
            "Use HTTPS websites only when entering personal or payment information.",
            "Shred physical documents containing personal information before discarding them.",
            "Log out of accounts when using shared or public computers.",
            "A strong password is at least 16 characters with a mix of letters, numbers, and symbols."
        };

        public static readonly List<string> DefaultResponses = new()
        {
            "I'm not sure I understand. Can you try rephrasing? 🤔 Or choose a topic from the left panel.",
            "Hmm, I didn't quite catch that. Could you rephrase your question? I'm best with cybersecurity topics!",
            "I don't have information on that specific topic yet. Try asking about phishing, malware, passwords, or ransomware.",
            "That's outside my current knowledge base. I'm specialised in cybersecurity — want to explore a topic from the menu?",
            "I'm still learning! Try one of the listed topics, or rephrase your question. Type 'help' to see what I can do."
        };

        public static readonly List<string> GreetingResponses = new()
        {
            "Hello! Great to meet you! 👋 I'm CyberPOE, your Cybersecurity Awareness Bot. What's your name?",
            "Hey there! 👋 I'm CyberPOE — here to keep you safe in the digital world. What should I call you?",
            "Welcome! 🛡 I'm CyberPOE, your personal cybersecurity guide. May I know your name?"
        };

        public static List<string> NameResponses(string name) => new()
        {
            $"Great to meet you, {name}! 🎉 I'm here to help you stay safe online. What cybersecurity topic interests you?",
            $"Awesome name, {name}! 😄 Let's make sure your digital life is as secure as possible. Pick a topic to get started!",
            $"Nice to meet you, {name}! 🛡 Together we'll boost your cybersecurity awareness. What would you like to learn about?"
        };

        public static readonly List<string> PhishingResponses = new()
        {
            "🎣 PHISHING ATTACKS\n\nPhishing is a cyber attack where criminals impersonate trusted organisations to steal your sensitive data — passwords, banking details, or personal information.\n\n🔴 How it works:\n• Fake emails pretending to be your bank, Netflix, or SARS\n• Urgent messages: 'Your account will be suspended!'\n• Convincing login pages that steal your credentials\n\n✅ How to protect yourself:\n• Hover over links before clicking — check the real URL\n• Look for spelling mistakes and generic greetings like 'Dear Customer'\n• Contact the organisation directly through their official website\n• Never enter credentials from an email link\n• Use email filters and anti-phishing browser extensions",

            "🎣 PHISHING — Stay Alert!\n\nPhishing is one of the most common cybercrime methods in South Africa. Attackers craft convincing emails, SMS messages (smishing), or phone calls (vishing) to trick you.\n\n🚨 Warning signs:\n• Unexpected emails about account problems\n• Requests for passwords, OTPs, or banking details\n• Mismatched sender addresses (e.g., support@paypa1.com)\n• Suspicious shortened URLs (bit.ly links in emails)\n\n🛡 Defence tactics:\n• Enable email two-factor authentication\n• Report phishing emails to your IT department or abuse@[provider]\n• Use Google's Phishing Quiz to sharpen your skills\n• Install anti-phishing extensions like Bitdefender TrafficLight",

            "🎣 PHISHING DEEP DIVE\n\nTypes of phishing to know:\n\n📧 Email Phishing — bulk fake emails to millions of users\n🐋 Whaling — targeted attacks on executives or high-value targets\n📱 Smishing — phishing via SMS ('Your delivery failed, click here')\n📞 Vishing — phone call impersonation ('This is Microsoft Support...')\n💼 Spear Phishing — personalised attacks using your real details\n\n✅ Golden Rules:\n• Slow down — urgency is a manipulation tactic\n• Verify via a separate channel before acting\n• Your bank will NEVER ask for your PIN or OTP\n• When in doubt, don't click — report it instead"
        };

        public static readonly List<string> PasswordSafetyResponses = new()
        {
            "🔑 PASSWORD SAFETY\n\nWeak passwords are the #1 cause of data breaches worldwide. Your password is your first line of defence.\n\n❌ Avoid these common mistakes:\n• Using 'password', '123456', or your birthday\n• Reusing the same password across multiple sites\n• Short passwords under 10 characters\n• Using your name or pet's name\n\n✅ Best practices:\n• Use at least 16 characters (longer = stronger)\n• Mix uppercase, lowercase, numbers, and symbols\n• Use a passphrase: 'BlueCat$JumpsOver42Moons!'\n• Use a password manager (Bitwarden, 1Password, LastPass)\n• Enable 2FA everywhere possible\n• Change passwords after any suspected breach",

            "🔑 BUILDING FORTRESS PASSWORDS\n\nPassword strength checklist:\n\n✅ At least 16 characters long\n✅ Includes uppercase and lowercase letters\n✅ Includes numbers (0-9)\n✅ Includes symbols (!@#$%^&*)\n✅ Not a dictionary word or personal detail\n✅ Unique — used only on one site\n\n🔐 Password Managers to try:\n• Bitwarden (free, open-source)\n• 1Password (premium, excellent UX)\n• KeePass (local, offline storage)\n\n⚡ Pro Tip: Use a memorable passphrase as your master password — something like 'Correct-Horse-Battery-Staple-7!' is both memorable and extremely strong.",

            "🔑 PASSWORD SAFETY — ADVANCED\n\nDid you know?\n• 81% of data breaches involve stolen or weak passwords (Verizon DBIR)\n• Hackers use 'credential stuffing' — testing leaked passwords on other sites\n• 'Brute force' tools can crack 8-character passwords in seconds\n\n🛡 Advanced strategies:\n• Use HIBP (haveibeenpwned.com) to check if your email was in a breach\n• Enable login alerts on all important accounts\n• Use hardware security keys (YubiKey) for maximum protection\n• Set up account recovery options before you need them\n• Never store passwords in plain text or browser notes"
        };

        public static readonly List<string> TwoFactorResponses = new()
        {
            "🛡 TWO-FACTOR AUTHENTICATION (2FA)\n\n2FA adds a second lock on your accounts — even if a hacker has your password, they can't get in without the second factor.\n\n📋 The three factors:\n• Something you KNOW — your password\n• Something you HAVE — your phone or hardware key\n• Something you ARE — your fingerprint or face\n\n✅ Best 2FA methods (ranked):\n1. Hardware key (YubiKey) — most secure\n2. Authenticator app (Google Authenticator, Authy) — very secure\n3. SMS OTP — convenient but vulnerable to SIM swapping\n\n🚀 Enable 2FA on: Email, Banking, Social Media, Cloud Storage, Work accounts",

            "🛡 2FA — YOUR SAFETY NET\n\nWhy 2FA is essential:\n• Accounts with 2FA are 99.9% less likely to be compromised (Microsoft)\n• Even if your password leaks in a breach, attackers can't access your account\n• SIM swapping is a growing threat — use authenticator apps instead of SMS\n\n📱 Top Authenticator Apps:\n• Google Authenticator\n• Microsoft Authenticator\n• Authy (supports cloud backup)\n• Aegis (open-source, Android)\n\n⚠ Warning: Never share your OTP with ANYONE — not even someone claiming to be from your bank or network provider. Legitimate services will NEVER ask for it."
        };

        public static readonly List<string> RansomwareResponses = new()
        {
            "💀 RANSOMWARE\n\nRansomware is malicious software that encrypts all your files, then demands payment (usually in cryptocurrency) to restore access.\n\n🔴 How infections happen:\n• Clicking malicious email attachments\n• Visiting compromised websites\n• Downloading pirated software\n• Exploiting unpatched vulnerabilities\n• RDP brute-force attacks\n\n🛡 Protection strategies:\n• Regular backups using the 3-2-1 rule (3 copies, 2 media, 1 offsite)\n• Keep OS and software fully patched\n• Disable macros in Microsoft Office\n• Use reputable antivirus/EDR solutions\n• Never pay the ransom — it funds criminals and doesn't guarantee recovery",

            "💀 RANSOMWARE — REAL-WORLD IMPACT\n\nFamous attacks you should know:\n• WannaCry (2017) — infected 200,000+ computers in 150 countries\n• NotPetya (2017) — caused $10 billion in global damages\n• Colonial Pipeline (2021) — shut down US fuel supplies\n• Conti, LockBit, REvil — active ransomware gangs still operating\n\n🛡 Business & personal defence:\n• Maintain offline, air-gapped backups\n• Segment your network — limit lateral movement\n• Train staff to recognise phishing (the primary infection vector)\n• Have an incident response plan before you need it\n• Contact CERT-SA or your local cybersecurity authority if attacked\n\n⚠ Paying the ransom: Only 65% of victims who pay actually recover their data."
        };

        public static readonly List<string> SocialEngineeringResponses = new()
        {
            "🎭 SOCIAL ENGINEERING\n\nSocial engineering manipulates human psychology rather than exploiting technical vulnerabilities. Attackers target your emotions and trust.\n\n🧠 Common techniques:\n• Pretexting — creating a fake scenario to extract information\n• Baiting — leaving infected USB drives in car parks or offices\n• Tailgating — following authorised personnel into secure areas\n• Quid Pro Quo — 'I'll fix your computer if you give me your login'\n• Authority — impersonating IT support, police, or management\n\n✅ How to defend yourself:\n• Verify identities through official channels before sharing anything\n• Slow down — urgency is always a manipulation tactic\n• Trust your instincts — if something feels off, it probably is\n• Never plug in found USB drives\n• Challenge unknown people in secure areas politely",

            "🎭 SOCIAL ENGINEERING — MANIPULATION TACTICS\n\nPsychological principles attackers exploit:\n\n😱 Fear — 'Your account has been hacked!'\n⏰ Urgency — 'Act now or lose access!'\n🏆 Authority — 'This is the CEO, do it immediately'\n🤝 Reciprocity — 'I helped you, now help me'\n❤ Liking — building fake rapport before asking for something\n🐑 Social proof — 'Everyone else already submitted their details'\n\n🛡 Golden rules:\n• Pause before reacting to any urgent request\n• Call back using a known number, not one provided in the message\n• No legitimate organisation will pressure you to act immediately\n• Discuss suspicious requests with a colleague or manager"
        };

        public static readonly List<string> SafeBrowsingResponses = new()
        {
            "🌐 SAFE BROWSING\n\nYour browser is a gateway to both the world's information and its threats. Browse defensively!\n\n✅ Safe browsing checklist:\n• Always check for HTTPS (padlock icon) before entering data\n• Keep your browser updated — updates patch critical vulnerabilities\n• Use privacy-focused browsers: Firefox, Brave, or Chrome with hardened settings\n• Install uBlock Origin — blocks malicious ads and trackers\n• Avoid piracy sites — they're riddled with malware\n• Don't save passwords in your browser — use a dedicated manager\n• Use private/incognito mode on shared devices\n\n🌐 Safer search engines:\n• DuckDuckGo — no tracking\n• Startpage — Google results without the tracking\n• Brave Search — independent index",

            "🌐 SAFE BROWSING — ADVANCED TIPS\n\nPublic Wi-Fi dangers:\n• Anyone on the same network can intercept unencrypted traffic\n• Attackers set up 'evil twin' hotspots (fake 'Coffee Shop Free WiFi')\n• Always use a VPN on public networks\n\n🔐 Recommended browser extensions:\n• uBlock Origin — ad and malware blocker\n• Privacy Badger — stops hidden trackers\n• HTTPS Everywhere — forces secure connections\n• Bitwarden — password manager built into your browser\n\n⚠ Warning signs of a dangerous site:\n• HTTP instead of HTTPS\n• Browser security warnings (never ignore these!)\n• Excessive pop-ups or redirect loops\n• Requests to disable your antivirus\n• Domains mimicking real ones (g00gle.com)"
        };

        public static readonly List<string> MalwareResponses = new()
        {
            "🦠 MALWARE\n\nMalware (malicious software) is any program designed to harm, exploit, or gain unauthorised access to your system.\n\n🔴 Types of malware:\n• Virus — attaches to files and spreads when opened\n• Worm — self-replicates across networks without user action\n• Trojan — disguises itself as legitimate software\n• Spyware — secretly monitors your activity\n• Adware — displays unwanted advertisements\n• Rootkit — hides deep in the OS to avoid detection\n• Keylogger — records every keystroke (including passwords)\n• Ransomware — encrypts files for ransom\n\n✅ Protection:\n• Keep antivirus definitions updated\n• Scan downloads before opening\n• Avoid pirated software — it's a prime malware vector\n• Don't open email attachments from unknown senders",

            "🦠 MALWARE — DETECTION & REMOVAL\n\nSigns your device may be infected:\n• Unusually slow performance\n• Unexpected pop-ups or browser redirects\n• Programs opening or closing on their own\n• Your antivirus has been disabled\n• Contacts receiving strange messages from your accounts\n• Unexplained data usage or battery drain (mobile)\n\n🛡 Recommended free tools:\n• Malwarebytes — excellent malware scanner\n• Windows Defender — solid built-in AV for Windows\n• Bitdefender Free — high detection rates\n• Kaspersky Free — strong real-time protection\n\n🆘 If infected:\n1. Disconnect from the internet immediately\n2. Boot into Safe Mode\n3. Run a full malware scan\n4. Remove detected threats\n5. Change all passwords from a clean device"
        };

        public static readonly List<string> CyberHygieneResponses = new()
        {
            "🧹 CYBER HYGIENE\n\nCyber hygiene is the regular practice of habits that keep your digital life healthy and secure — just like brushing your teeth for your online safety!\n\n📋 Daily habits:\n• Lock your screen when stepping away\n• Log out of accounts on shared devices\n• Be cautious about what you share on social media\n• Check app permissions on your phone regularly\n\n📋 Weekly habits:\n• Review account activity for suspicious logins\n• Clear browser cookies and cached data\n• Check for software updates\n\n📋 Monthly habits:\n• Review which apps have access to your Google/Apple account\n• Check haveibeenpwned.com for new breaches\n• Audit your social media privacy settings\n• Back up important files",

            "🧹 CYBER HYGIENE — COMPLETE CHECKLIST\n\n✅ Device security:\n□ Auto-lock enabled (30 seconds or less)\n□ Full-disk encryption enabled\n□ Antivirus installed and updated\n□ OS and apps fully patched\n□ Bluetooth off when not in use\n\n✅ Account security:\n□ Unique password for every account\n□ 2FA enabled on all important accounts\n□ Recovery email/phone number verified\n□ Unused accounts deleted\n\n✅ Network security:\n□ Home router password changed from default\n□ Guest network for IoT devices\n□ VPN used on public Wi-Fi\n□ Router firmware updated\n\n✅ Data protection:\n□ Regular backups (3-2-1 rule)\n□ Sensitive files encrypted\n□ Physical documents shredded"
        };

        public static readonly List<string> ScamResponses = new()
        {
            "⚠ ONLINE SCAMS\n\nSouth Africa loses billions of rands annually to online scams. Here are the most common ones to watch out for:\n\n🔴 Common scams:\n• Advance fee fraud ('You've won R50,000 — pay the processing fee first')\n• Romance scams — fake relationships built to request money\n• Job offer scams — 'Work from home, earn R20,000/month'\n• Lottery scams — 'You've won a prize you never entered'\n• Investment scams — Ponzi schemes disguised as crypto or forex\n• Technical support scams — 'Your PC has a virus, call us now'\n• SARS/bank impersonation — threatening legal action for fake debts\n\n✅ How to avoid scams:\n• If it sounds too good to be true — it is\n• Never pay upfront fees to receive a prize or job\n• Verify all investment platforms with the FSCA (fsca.co.za)\n• Report scams to the SAPS or Cybercrime Hub",

            "⚠ SCAM AWARENESS — PROTECT YOURSELF\n\n🚨 Red flags of a scam:\n• Pressure to act immediately or lose the opportunity\n• Requests for gift cards, cryptocurrency, or wire transfers\n• Unsolicited contact claiming you've won something\n• Poor grammar and spelling in official-looking messages\n• Requests for remote access to your device\n• No verifiable physical address or company registration\n\n📞 Where to report scams in South Africa:\n• SAPS Cybercrime: 0860 010 111\n• Banking Ombudsman: 0860 800 900\n• FSCA (investment scams): 0800 110 443\n• Report phishing emails to: phishing@absa.co.za / fraudline@standardbank.co.za\n\n💡 Remember: Legitimate organisations never ask for your PIN, OTP, or full card number."
        };

        public static readonly List<string> PrivacyResponses = new()
        {
            "🔒 PRIVACY PROTECTION\n\nYour personal data is valuable — companies and criminals both want it. Take control of your digital privacy.\n\n📋 Social media privacy:\n• Set profiles to private or friends-only\n• Don't post your location in real-time\n• Avoid sharing your ID number, address, or financial details\n• Regularly audit what apps can access your social accounts\n• Think before you post — the internet never forgets\n\n🌐 Online privacy:\n• Use a privacy-focused browser (Brave, Firefox)\n• Install uBlock Origin and Privacy Badger\n• Use DuckDuckGo instead of Google\n• Use a VPN to mask your IP address\n• Opt out of data collection where possible\n\n📱 Mobile privacy:\n• Review app permissions — revoke what isn't needed\n• Disable location access for apps that don't need it\n• Turn off personalised ads in your phone settings",

            "🔒 PRIVACY — YOUR DATA RIGHTS\n\nIn South Africa, the POPIA (Protection of Personal Information Act) gives you rights over your data:\n\n✅ Your POPIA rights:\n• Right to know what data is collected about you\n• Right to access your personal information\n• Right to correct inaccurate data\n• Right to object to processing of your data\n• Right to have your data deleted (right to be forgotten)\n\n🛡 Practical privacy tools:\n• ProtonMail — encrypted email\n• Signal — end-to-end encrypted messaging\n• Tor Browser — anonymous browsing\n• ProtonVPN — privacy-first VPN\n• Authy — 2FA without phone number exposure\n\n📞 Report POPIA violations to:\nInformation Regulator (SA): inforeg.org.za | 010 023 5200"
        };

        public static readonly string HelpText =
            "📖 CYBERSECURITY POE HELP MENU\n\n" +

            "🛡 TOPICS — type any of these:\n" +
            "  phishing | password safety | two-factor authentication\n" +
            "  ransomware | social engineering | safe browsing\n" +
            "  malware | cyber hygiene | scams | privacy\n\n" +

            "📋 TASK & REMINDER COMMANDS:\n" +
            "  add task [name]           — add a cybersecurity task\n" +
            "  remind me to [task]       — natural-language task creation\n" +
            "  create task [name]        — create a new task\n" +
            "  show tasks / my tasks     — view all saved tasks\n" +
            "  delete task [id]          — delete a task by ID\n" +
            "  complete task [id]        — mark a task as completed\n\n" +

            "🎯 QUIZ COMMANDS:\n" +
            "  start quiz / quiz         — begin a 5-question cybersecurity quiz\n" +
            "  (answer with A, B, C or D during quiz)\n\n" +

            "📜 ACTIVITY LOG:\n" +
            "  show activity log         — view everything CyberPOE has done\n" +
            "  activity log / show log   — same as above\n" +
            "  what have you done        — same as above\n" +
            "  log summary               — see a session summary\n\n" +

            "⚙ GENERAL COMMANDS:\n" +
            "  help                      — show this menu\n" +
            "  clear                     — clear the chat window\n" +
            "  exit / quit               — close the application\n" +
            "  my name is [name]         — tell CyberPOE your name\n" +
            "  i feel [mood]             — share your mood\n\n" +

            "💡 TIP: Click topic buttons on the left panel for instant responses!";

        public static string GetRandom(List<string> list)
        {
            if (list == null || list.Count == 0) return string.Empty;
            return list[_rand.Next(list.Count)];
        }

        public static string GetRandomTip() => GetRandom(SecurityTips);
        public static string GetRandomDefault() => GetRandom(DefaultResponses);
    }
}