namespace OriApps.ResumeBoost
{
    public class ScriptsConstants
    {
        public const string AuthorizationCheckScript = @"(function check_auth() { return document.querySelectorAll('[data-qa=""mainmenu_profileAndResumes""]').length; })();";

        public const string FindResumeScript = @"(function check_resumes_col() { return document.querySelectorAll('[data-qa^=""resume-update-button""]').length; })();";

        public const string UpdateResumeScript = @" (function update_resumes() {
            const buttons = document.querySelectorAll('[data-qa^=""resume-update-button""]');
            buttons.forEach(btn => btn.click());
            return buttons.length;
        })();";
    }
}