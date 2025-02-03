// ReSharper disable LocalizableElement
namespace ResumeBoost
{
    using Microsoft.Web.WebView2.Core;

    public partial class MainForm : Form
    {
        private ResumeBoostState _state = ResumeBoostState.Initializing;
        
        private string _lastBoostTime = "";

        private const string Domain = "https://hh.ru/";

        public MainForm()
        {
            InitializeComponent();

            notifyIcon.Text = Application.ProductName;
            notifyIcon.Visible = false;

            timerStart.Interval = 3000;
            timerMain.Interval = 3000;
            timerLong.Interval = 3600000;

            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async(null);
            statusLabel.Text = "Status: Opening hh.ru ...";
            webView.CoreWebView2.Navigate(Domain + "locale?language=RU");
            timerStart.Start();
        }

        private async void timerStart_Tick(object? sender, EventArgs e)
        {
            if (await IsUserLoggedIn())
            {
                statusLabel.Text = "Status: Authorization successful. Loading resumes...";
                timerStart.Stop();
                StartResumeBoostProcess();
            }
            else if (webView.CoreWebView2.Source.Contains("login"))
            {
                statusLabel.Text = "Status: Waiting for user authorization...";
            }
            else
            {
                statusLabel.Text = "Status: Still waiting for authorization...";
            }
        }

        private async void timerMain_Tick(object? sender, EventArgs e)
        {
            switch (_state)
            {
                case ResumeBoostState.CheckingAuthorization:
                    if (await IsUserLoggedIn())
                    {
                        statusLabel.Text = "Status: Authorization verified.";
                        _state = ResumeBoostState.CheckingResumes;
                    }
                    break;

                case ResumeBoostState.CheckingResumes:
                    int availableResumes = await CountAvailableResumes();
                    if (availableResumes > 0)
                    {
                        statusLabel.Text = $"Status: Can boost {availableResumes} resumes.";
                        _state = ResumeBoostState.BoostingResumes;
                    }
                    else
                    {
                        statusLabel.Text = "Status: Boosting not available. Waiting...";
                        timerMain.Stop();
                        timerLong.Start();
                    }
                    break;

                case ResumeBoostState.BoostingResumes:
                    await BoostResumes();
                    _lastBoostTime = DateTime.Now.ToString("HH:mm");
                    statusLabel.Text = $"Status: Resumes boosted at {_lastBoostTime}. Waiting...";
                    timerMain.Stop();
                    _state = ResumeBoostState.WaitingForNextCycle;
                    timerLong.Start();
                    break;
            }
        }

        private void timerLong_Tick(object? sender, EventArgs e)
        {
            timerLong.Stop();
            StartResumeBoostProcess();
        }

        private async Task<bool> IsUserLoggedIn()
        {
            string result = await webView.CoreWebView2.ExecuteScriptAsync(ScriptsConstants.AuthorizationCheckScript);
            int count = int.TryParse(result.Replace("\"", ""), out var n) ? n : 0;
            return count > 0;
        }

        private async Task<int> CountAvailableResumes()
        {
            string result = await webView.CoreWebView2.ExecuteScriptAsync(ScriptsConstants.FindResumeScript);
            return int.TryParse(result.Replace("\"", ""), out var n) ? n : 0;
        }

        private async Task BoostResumes()
        {
            await webView.CoreWebView2.ExecuteScriptAsync(ScriptsConstants.UpdateResumeScript);
        }

        private void StartResumeBoostProcess()
        {
            statusLabel.Text = "Status: Navigating to resumes page...";
            webView.CoreWebView2.Navigate(Domain + "applicant/resumes");
            _state = ResumeBoostState.CheckingAuthorization;
            timerMain.Start();
        }

        private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show("WebView2 initialization failed: " + e.InitializationException.Message);
            }
        }
    }
}