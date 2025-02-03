// ReSharper disable LocalizableElement
namespace ResumeBoost
{
    using Microsoft.Web.WebView2.Core;

    public partial class MainForm : Form
    {
        private int _step;
        private string _lastTime = "";

        private const string Domain = "https://hh.ru/";

        private const string AuthorizationCheckScript = @"
    (function check_auth() {
        let elements = document.querySelectorAll('div');
        let filtered = Array.from(elements).filter(e => e.textContent.trim() === 'Мои резюме');
        return filtered.length;
    })();";

        private const string FindResumeScript = @"
                    (function check_resumes_col() {
                        let links = document.querySelectorAll('.bloko-link');
                        let filtered = Array.from(links).filter(e => (/Поднять в поиске/i).test(e.textContent));
                        return filtered.length;
                    })();";

        private const string UpdateResumeScript = @"
                    (function update_resumes() {
                        let links = document.querySelectorAll('.bloko-link');
                        let filtered = Array.from(links).filter(e => (/Поднять в поиске/i).test(e.textContent));
                        filtered.forEach(el => { el.click(); });
                        return filtered.length;
                    })();";

        public MainForm()
        {
            InitializeComponent();

            notifyIcon.Text = Application.ProductName;
            notifyIcon.Visible = false;
            // Initialize timers (intervals in milliseconds)
            timerStart.Interval = 3000;
            timerMain.Interval = 3000;
            timerLong.Interval = 3600000; // For example, 60 sec wait

            // WebView2 events
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

        private async void timerStart_Tick(object? sender,
            EventArgs e)
        {
            var result = await webView.CoreWebView2.ExecuteScriptAsync(AuthorizationCheckScript);
            // The result is returned as a JSON value (e.g., "0" or "1")
            var count = int.TryParse(result.Replace("\"", ""), out var n) ? n : 0;

            // Check if we are on the login page
            if (webView.CoreWebView2.Source.Contains("login"))
            {
                statusLabel.Text = "Status: waiting for user authorization...";
            }
            else if (count > 0)
            {
                statusLabel.Text = "Status: authorization successful. Loading resumes...";
                timerStart.Stop();
                timerMain.Start();
            }
            else
            {
                statusLabel.Text = "Status: waiting for authorization...";
            }
        }

        private async void timerMain_Tick(object? sender, EventArgs e)
        {
            if (_step == 0)
            {
                // Navigate to resumes page
                webView.CoreWebView2.Navigate(Domain + "applicant/resumes");
                _step = 1;
                return;
            }

            if (_step == 1)
            {
                var result = await webView.CoreWebView2.ExecuteScriptAsync(AuthorizationCheckScript);
                var count = int.TryParse(result.Replace("\"", ""), out var n) ? n : 0;

                if (count > 0)
                {
                    statusLabel.Text = "Status: authorization verified.";
                    _step = 2;
                }

                return;
            }

            if (_step == 2)
            {
                var result = await webView.CoreWebView2.ExecuteScriptAsync(FindResumeScript);
                var count = int.TryParse(result.Replace("\"", ""), out var n) ? n : 0;

                if (count > 0)
                {
                    statusLabel.Text = $"Status: can boost {count} resumes.";
                    _step = 3;
                }
                else
                {
                    statusLabel.Text = "Status: boosting not available. Waiting...";
                    timerMain.Stop();
                    timerLong.Start();
                }

                return;
            }

            if (_step == 3)
            {
                statusLabel.Text = "Status: boosting resumes...";

                await webView.CoreWebView2.ExecuteScriptAsync(UpdateResumeScript);
                _lastTime = DateTime.Now.ToString("HH:mm");
                statusLabel.Text = $"Status: resumes boosted at {_lastTime}. Waiting...";
                timerMain.Stop();
                _step = 0;
                timerLong.Start();
            }
        }

        private void timerLong_Tick(object? sender, EventArgs e)
        {
            timerLong.Stop();
            timerMain.Start();
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

        private void WebView_CoreWebView2InitializationCompleted(object? sender,
            CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show("WebView2 initialization failed: " + e.InitializationException.Message);
            }
        }
    }
}