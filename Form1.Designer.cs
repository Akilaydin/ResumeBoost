namespace ResumeBoost;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        webView = new Microsoft.Web.WebView2.WinForms.WebView2();
        statusStrip = new StatusStrip();
        statusLabel = new Label();
        timerStart = new System.Windows.Forms.Timer(components);
        timerMain = new System.Windows.Forms.Timer(components);
        timerLong = new System.Windows.Forms.Timer(components);
        notifyIcon = new NotifyIcon(components);
        ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
        SuspendLayout();
        // 
        // webView
        // 
        webView.AllowExternalDrop = true;
        webView.CreationProperties = null;
        webView.DefaultBackgroundColor = Color.White;
        webView.Dock = DockStyle.Fill;
        webView.Location = new Point(0, 0);
        webView.Name = "webView";
        webView.Size = new Size(800, 450);
        webView.TabIndex = 0;
        webView.ZoomFactor = 1D;
        // 
        // statusStrip
        // 
        statusStrip.Location = new Point(0, 428);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(800, 22);
        statusStrip.TabIndex = 1;
        statusStrip.Text = "statusStrip1";
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = true;
        statusLabel.Location = new Point(67, 142);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(38, 15);
        statusLabel.TabIndex = 2;
        statusLabel.Text = "label1";
        // 
        // timerStart
        // 
        timerStart.Tick += timerStart_Tick;
        // 
        // timerMain
        // 
        timerMain.Tick += timerMain_Tick;
        // 
        // timerLong
        // 
        timerLong.Tick += timerLong_Tick;
        // 
        // notifyIcon
        // 
        notifyIcon.Text = "Resume Boost";
        notifyIcon.Click += notifyIcon_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(statusLabel);
        Controls.Add(statusStrip);
        Controls.Add(webView);
        Name = "MainForm";
        Text = "Form1";
        Load += MainForm_Load;
        ((System.ComponentModel.ISupportInitialize)webView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    private StatusStrip statusStrip;
    private Label statusLabel;
    private System.Windows.Forms.Timer timerStart;
    private System.Windows.Forms.Timer timerMain;
    private System.Windows.Forms.Timer timerLong;
    private NotifyIcon notifyIcon;
}