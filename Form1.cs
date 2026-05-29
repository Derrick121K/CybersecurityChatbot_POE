using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.IO;

namespace CybersecurityChatbot;

public partial class Form1 : Form
{
    private ChatBot? _chatBot;
    private TextBox? txtAsciiArt;
    private RichTextBox? rtxtChatDisplay;
    private TextBox? txtUserInput;
    private Button? btnSend;

    public Form1()
    {
        InitializeComponent();
        SetupCustomControls();

        _chatBot = new ChatBot();

        PlayVoiceGreeting();
        DisplayAsciiArt();
        AppendBotMessage(_chatBot.GetGreeting());
    }

    private void SetupCustomControls()
    {
        // Form settings
        this.Text = "Cybersecurity Chatbot";
        this.Size = new Size(900, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(30, 30, 46);

        // ASCII Art TextBox (top section)
        txtAsciiArt = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            BackColor = Color.Black,
            ForeColor = Color.Lime,
            Font = new Font("Consolas", 8),
            Height = 150,
            Dock = DockStyle.Top,
            ScrollBars = ScrollBars.None
        };

        // Chat Display RichTextBox (middle section)
        rtxtChatDisplay = new RichTextBox
        {
            ReadOnly = true,
            BackColor = Color.FromArgb(45, 45, 61),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11),
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None
        };

        // Input Panel (bottom section)
        Panel inputPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(30, 30, 46)
        };

        // User Input TextBox
        txtUserInput = new TextBox
        {
            Location = new Point(10, 12),
            Size = new Size(700, 35),
            BackColor = Color.FromArgb(61, 61, 77),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };
        txtUserInput.KeyDown += TxtUserInput_KeyDown!;

        // Send Button
        btnSend = new Button
        {
            Text = "📤 SEND",
            Location = new Point(720, 10),
            Size = new Size(150, 40),
            BackColor = Color.DodgerBlue,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat
        };
        btnSend.Click += BtnSend_Click!;

        // Add controls to input panel
        inputPanel.Controls.Add(txtUserInput);
        inputPanel.Controls.Add(btnSend);

        // Add all controls to form (order matters - bottom to top)
        this.Controls.Add(rtxtChatDisplay);
        this.Controls.Add(inputPanel);
        this.Controls.Add(txtAsciiArt);
    }

    private void PlayVoiceGreeting()
    {
        try
        {
            string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
            if (File.Exists(wavPath))
            {
                using (SoundPlayer player = new SoundPlayer(wavPath))
                {
                    player.Play();
                }
            }
        }
        catch (Exception ex)
        {
            // Silent fail - voice greeting is optional
            System.Diagnostics.Debug.WriteLine($"Voice error: {ex.Message}");
        }
    }

    private void DisplayAsciiArt()
    {
        if (txtAsciiArt != null && _chatBot != null)
        {
            txtAsciiArt.Text = _chatBot.GetAsciiArt();
        }
    }

    private void BtnSend_Click(object sender, EventArgs e)
    {
        SendMessage();
    }

    private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SendMessage();
            e.SuppressKeyPress = true; // Prevents the "ding" sound
        }
    }

    private void SendMessage()
    {
        if (txtUserInput == null || rtxtChatDisplay == null || _chatBot == null)
            return;

        string userMessage = txtUserInput.Text.Trim();

        if (string.IsNullOrWhiteSpace(userMessage))
            return;

        // Display user message in chat
        AppendUserMessage(userMessage);

        // Clear input box
        txtUserInput.Clear();

        // Get bot response
        string botResponse = _chatBot.ProcessInput(userMessage);

        // Display bot response
        AppendBotMessage(botResponse);

        // Auto-scroll to bottom of chat
        rtxtChatDisplay.SelectionStart = rtxtChatDisplay.Text.Length;
        rtxtChatDisplay.ScrollToCaret();
    }

    private void AppendUserMessage(string message)
    {
        if (rtxtChatDisplay == null) return;

        rtxtChatDisplay.SelectionColor = Color.LightGreen;
        rtxtChatDisplay.AppendText($"\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        rtxtChatDisplay.SelectionColor = Color.Cyan;
        rtxtChatDisplay.AppendText($"🧑 YOU: ");
        rtxtChatDisplay.SelectionColor = Color.White;
        rtxtChatDisplay.AppendText($"{message}\n");
    }

    private void AppendBotMessage(string message)
    {
        if (rtxtChatDisplay == null) return;

        rtxtChatDisplay.SelectionColor = Color.DodgerBlue;
        rtxtChatDisplay.AppendText($"🤖 BOT: ");
        rtxtChatDisplay.SelectionColor = Color.White;
        rtxtChatDisplay.AppendText($"{message}\n");
    }
}