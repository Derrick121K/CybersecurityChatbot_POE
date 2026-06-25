using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace CybersecurityChatbot;

public partial class Form1 : Form
{
    // ===== FIELDS =====
    private ChatBot? _chatBot;
    private Panel? _headerPanel;
    private Panel? _sidePanel;
    private Panel? _contentPanel;
    private RichTextBox? _rtxtChatDisplay;
    private Panel? _inputPanel;
    private TextBox? _txtUserInput;
    private Button? _btnSend;
    private Button? _btnQuiz;
    private Button? _btnTasks;
    private Button? _btnLog;
    private Button? _btnHelp;
    private Button? _btnQuickPassword;
    private Button? _btnQuickPhishing;
    private Button? _btnQuickPrivacy;
    private Button? _btnQuickTask;
    private Label? _lblTitle;
    private Label? _lblStatus;
    private System.Windows.Forms.Timer? _pulseTimer;
    private int _pulseStep = 0;
    private Panel? _notificationPanel;
    private Label? _notificationLabel;
    private System.Windows.Forms.Timer? _notificationTimer;
    private bool _asciiArtDisplayed = false;
    private TextBox? _txtAsciiArt;

    public Form1()
    {
        InitializeComponent();
        SetupModernUI();

        _chatBot = new ChatBot();

        PlayVoiceGreeting();
        DisplayAsciiArt();
        AppendBotMessage(_chatBot.GetGreeting());

        StartPulseAnimation();
        UpdateStatus("🟢 Online");
    }

    private void SetupModernUI()
    {
        // ===== FORM SETTINGS =====
        this.Text = "🔐 Cybersecurity Outbox - v3.0";
        this.Size = new Size(1100, 750);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(18, 20, 34);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimumSize = new Size(900, 600);

        // ===== MAIN CONTAINER =====
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,  // Added a row for ASCII art
            BackColor = Color.FromArgb(18, 20, 34)
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));   // Header
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));  // ASCII Art
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // Chat

        // ===== HEADER PANEL (Row 0) =====
        _headerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(28, 32, 56),
            Padding = new Padding(15, 10, 15, 10)
        };
        mainLayout.Controls.Add(_headerPanel, 0, 0);
        mainLayout.SetColumnSpan(_headerPanel, 2);

        // Title
        _lblTitle = new Label
        {
            Text = "🔐 CYBERSECURITY OUTBOX",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 191, 255),
            Location = new Point(15, 10),
            AutoSize = true,
            BackColor = Color.Transparent
        };

        // Status
        _lblStatus = new Label
        {
            Text = "🟢 Online",
            Font = new Font("Segoe UI", 9, FontStyle.Regular),
            ForeColor = Color.FromArgb(100, 200, 100),
            Location = new Point(15, 40),
            AutoSize = true,
            BackColor = Color.Transparent
        };

        // Header buttons
        _btnQuiz = CreateHeaderButton("🎮 Quiz", Color.FromArgb(255, 107, 107));
        _btnQuiz.Location = new Point(750, 15);
        _btnQuiz.Click += (s, e) => { SendQuickCommand("start quiz"); };

        _btnTasks = CreateHeaderButton("📋 Tasks", Color.FromArgb(78, 205, 196));
        _btnTasks.Location = new Point(860, 15);
        _btnTasks.Click += (s, e) => { SendQuickCommand("view tasks"); };

        _btnLog = CreateHeaderButton("📊 Log", Color.FromArgb(255, 159, 67));
        _btnLog.Location = new Point(970, 15);
        _btnLog.Click += (s, e) => { SendQuickCommand("show activity log"); };

        _headerPanel.Controls.Add(_lblTitle);
        _headerPanel.Controls.Add(_lblStatus);
        _headerPanel.Controls.Add(_btnQuiz);
        _headerPanel.Controls.Add(_btnTasks);
        _headerPanel.Controls.Add(_btnLog);

        // ===== ASCII ART PANEL (Row 1) =====
        var asciiPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(18, 20, 34),
            Padding = new Padding(10)
        };
        mainLayout.Controls.Add(asciiPanel, 0, 1);
        mainLayout.SetColumnSpan(asciiPanel, 2);

        // ASCII Art TextBox - Designed specifically for ASCII art
        _txtAsciiArt = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            BackColor = Color.FromArgb(18, 20, 34),
            ForeColor = Color.Lime,
            Font = new Font("Consolas", 9, FontStyle.Regular),
            BorderStyle = BorderStyle.None,
            Text = @"  ██████╗ ██╗   ██╗██████╗ ███████╗██████╗  ██████╗ ██╗   ██╗████████╗
  ██╔═══╝ ╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔═══██╗╚██╗ ██╔╝╚══██╔══╝
  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██║   ██║ ╚████╔╝    ██║   
  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║   ██║  ╚██╔╝     ██║   
  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║╚██████╔╝   ██║      ██║   
   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝ ╚═════╝    ╚═╝      ╚═╝   
                                                                        
        🔐  C Y B E R S E C U R I T Y   O U T B O X   🔐              
                             v3.0",
            Height = 110,
            Cursor = Cursors.Default
        };

        asciiPanel.Controls.Add(_txtAsciiArt);

        // ===== SIDE PANEL (Row 2, Column 0) =====
        _sidePanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(24, 28, 48),
            Padding = new Padding(8, 10, 8, 10)
        };
        mainLayout.Controls.Add(_sidePanel, 0, 2);

        var sideLabel = new Label
        {
            Text = "⚡ QUICK ACTIONS",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(150, 160, 200),
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };
        _sidePanel.Controls.Add(sideLabel);

        _btnQuickPassword = CreateSideButton("🔐 Passwords", "Tell me about passwords");
        _btnQuickPhishing = CreateSideButton("🎣 Phishing", "Tell me about phishing");
        _btnQuickPrivacy = CreateSideButton("🛡️ Privacy", "Tell me about privacy");
        _btnQuickTask = CreateSideButton("📋 Add Task", "add task - Review my security settings");

        var separator = new Label
        {
            Text = "─────────────",
            Dock = DockStyle.Top,
            Height = 15,
            ForeColor = Color.FromArgb(60, 70, 100),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };

        _btnHelp = CreateSideButton("❓ Help", "help");
        _btnHelp.BackColor = Color.FromArgb(60, 50, 100);

        _sidePanel.Controls.Add(_btnHelp);
        _sidePanel.Controls.Add(separator);
        _sidePanel.Controls.Add(_btnQuickTask);
        _sidePanel.Controls.Add(_btnQuickPrivacy);
        _sidePanel.Controls.Add(_btnQuickPhishing);
        _sidePanel.Controls.Add(_btnQuickPassword);

        // ===== CONTENT PANEL (Row 2, Column 1) =====
        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(18, 20, 34),
            Padding = new Padding(10)
        };
        mainLayout.Controls.Add(_contentPanel, 1, 2);

        // ===== CHAT DISPLAY =====
        _rtxtChatDisplay = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = Color.FromArgb(22, 26, 44),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.None,
            ScrollBars = RichTextBoxScrollBars.Vertical,
            WordWrap = true
        };

        // ===== INPUT PANEL =====
        _inputPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 65,
            BackColor = Color.FromArgb(24, 28, 48),
            Padding = new Padding(10, 10, 10, 10)
        };

        _txtUserInput = new TextBox
        {
            Location = new Point(10, 15),
            Size = new Size(700, 35),
            BackColor = Color.FromArgb(38, 42, 68),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };
        _txtUserInput.KeyDown += TxtUserInput_KeyDown!;
        _txtUserInput.Enter += (s, e) => { _txtUserInput.BackColor = Color.FromArgb(48, 52, 78); };
        _txtUserInput.Leave += (s, e) => { _txtUserInput.BackColor = Color.FromArgb(38, 42, 68); };

        _btnSend = new Button
        {
            Text = "📤 SEND",
            Location = new Point(720, 12),
            Size = new Size(110, 40),
            BackColor = Color.FromArgb(0, 191, 255),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 }
        };
        _btnSend.Click += BtnSend_Click!;
        _btnSend.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 200, 255);
        _btnSend.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 150, 200);

        _inputPanel.Controls.Add(_txtUserInput);
        _inputPanel.Controls.Add(_btnSend);

        _contentPanel.Controls.Add(_rtxtChatDisplay);
        _contentPanel.Controls.Add(_inputPanel);

        // ===== NOTIFICATION PANEL =====
        _notificationPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 0,
            BackColor = Color.FromArgb(0, 191, 255)
        };
        _notificationPanel.Visible = false;

        _notificationLabel = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _notificationPanel.Controls.Add(_notificationLabel);

        this.Controls.Add(mainLayout);
        this.Controls.Add(_notificationPanel);

        // ===== TIMERS =====
        _pulseTimer = new System.Windows.Forms.Timer { Interval = 50 };
        _pulseTimer.Tick += PulseTimer_Tick!;

        _notificationTimer = new System.Windows.Forms.Timer { Interval = 3000 };
        _notificationTimer.Tick += NotificationTimer_Tick!;
    }

    // ===== HELPER: HEADER BUTTON =====
    private Button CreateHeaderButton(string text, Color color)
    {
        return new Button
        {
            Text = text,
            Size = new Size(100, 38),
            BackColor = color,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 }
        };
    }

    // ===== HELPER: SIDE BUTTON =====
    private Button CreateSideButton(string text, string command)
    {
        var btn = new Button
        {
            Text = text,
            Dock = DockStyle.Top,
            Height = 35,
            Margin = new Padding(0, 3, 0, 3),
            BackColor = Color.FromArgb(38, 42, 68),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 },
            Tag = command,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 191, 255);
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 150, 200);
        btn.Click += QuickTopic_Click!;
        return btn;
    }

    // ===== QUICK TOPIC =====
    private void QuickTopic_Click(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn?.Tag != null && _txtUserInput != null)
        {
            _txtUserInput.Text = btn.Tag.ToString();
            SendMessage();
        }
    }

    private void SendQuickCommand(string command)
    {
        if (_txtUserInput != null)
        {
            _txtUserInput.Text = command;
            SendMessage();
        }
    }

    // ===== PULSE ANIMATION =====
    private void StartPulseAnimation()
    {
        _pulseTimer?.Start();
    }

    private void PulseTimer_Tick(object sender, EventArgs e)
    {
        if (_btnSend == null) return;
        _pulseStep = (_pulseStep + 1) % 100;
        float pulse = 1 + 0.03f * (float)Math.Sin(_pulseStep * 0.08);
        int width = (int)(110 * pulse);
        int height = (int)(40 * pulse);
        _btnSend.Size = new Size(width, height);
        _btnSend.Location = new Point(720 - (width - 110) / 2, 12 - (height - 40) / 2);
    }

    // ===== TYPEWRITER ANIMATION =====
    private async Task AnimateMessage(string message, bool isUser)
    {
        if (_rtxtChatDisplay == null) return;

        var color = isUser ? Color.Cyan : Color.DodgerBlue;
        var icon = isUser ? "🧑 YOU: " : "🤖 BOT: ";

        _rtxtChatDisplay.SelectionColor = color;
        _rtxtChatDisplay.AppendText($"\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        _rtxtChatDisplay.SelectionColor = color;
        _rtxtChatDisplay.AppendText(icon);
        _rtxtChatDisplay.SelectionColor = Color.White;
        _rtxtChatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);

        for (int i = 0; i < message.Length; i++)
        {
            _rtxtChatDisplay.AppendText(message[i].ToString());
            await Task.Delay(3);
        }
        _rtxtChatDisplay.AppendText("\n");
        _rtxtChatDisplay.SelectionStart = _rtxtChatDisplay.Text.Length;
        _rtxtChatDisplay.ScrollToCaret();
    }

    // ===== NOTIFICATIONS =====
    private void ShowNotification(string message, Color color)
    {
        if (_notificationPanel == null || _notificationLabel == null) return;
        _notificationPanel.BackColor = color;
        _notificationLabel.Text = message;
        _notificationPanel.Height = 35;
        _notificationPanel.Visible = true;
        _notificationTimer?.Start();
    }

    private void NotificationTimer_Tick(object sender, EventArgs e)
    {
        if (_notificationPanel == null) return;
        _notificationTimer?.Stop();
        _notificationPanel.Height = 0;
        _notificationPanel.Visible = false;
    }

    private void UpdateStatus(string status)
    {
        if (_lblStatus != null)
            _lblStatus.Text = status;
    }

    // ===== VOICE GREETING =====
    private void PlayVoiceGreeting()
    {
        try
        {
            string[] possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav"),
                Path.Combine(Application.StartupPath, "greeting.wav"),
                Path.Combine(Directory.GetCurrentDirectory(), "greeting.wav"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "greeting.wav")
            };

            string? wavPath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    wavPath = path;
                    break;
                }
            }

            if (wavPath != null && File.Exists(wavPath))
            {
                using (SoundPlayer player = new SoundPlayer(wavPath))
                {
                    player.Play();
                }
            }
        }
        catch (Exception) { /* Silent fail */ }
    }

    // ===== ASCII ART =====
    private void DisplayAsciiArt()
    {
        if (_txtAsciiArt == null || _chatBot == null || _asciiArtDisplayed) return;

        _asciiArtDisplayed = true;
        // ASCII art is already set in the TextBox, but we can update it if needed
        _txtAsciiArt.Text = _chatBot.GetAsciiArt();
    }

    // ===== SEND MESSAGE =====
    private void BtnSend_Click(object sender, EventArgs e)
    {
        SendMessage();
    }

    private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SendMessage();
            e.SuppressKeyPress = true;
        }
    }

    private async void SendMessage()
    {
        if (_txtUserInput == null || _rtxtChatDisplay == null || _chatBot == null)
            return;

        string userMessage = _txtUserInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(userMessage)) return;

        await AnimateMessage(userMessage, true);
        _txtUserInput.Clear();
        UpdateStatus("🟡 Thinking...");

        try
        {
            string botResponse = _chatBot.ProcessInput(userMessage);
            await AnimateMessage(botResponse, false);
        }
        finally
        {
            UpdateStatus("🟢 Online");
        }
    }

    private void AppendBotMessage(string message)
    {
        if (_rtxtChatDisplay == null) return;
        _rtxtChatDisplay.SelectionColor = Color.DodgerBlue;
        _rtxtChatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
        _rtxtChatDisplay.AppendText($"\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        _rtxtChatDisplay.SelectionColor = Color.DodgerBlue;
        _rtxtChatDisplay.AppendText($"🤖 BOT: ");
        _rtxtChatDisplay.SelectionColor = Color.White;
        _rtxtChatDisplay.AppendText($"{message}\n");
        _rtxtChatDisplay.ScrollToCaret();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _pulseTimer?.Stop();
        _notificationTimer?.Stop();
        base.OnFormClosing(e);
    }
}