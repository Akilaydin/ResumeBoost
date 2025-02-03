namespace ResumeBoost
{
    public class ScriptsConstants
    {
        public const string AuthorizationCheckScript = @"
            (function check_auth() {
                let elements = document.querySelectorAll('div');
                let filtered = Array.from(elements).filter(e => e.textContent.trim() === 'Мои резюме');
                return filtered.length;
            })();";

        public const string FindResumeScript = @"
            (function check_resumes_col() {
                let links = document.querySelectorAll('.bloko-link');
                let filtered = Array.from(links).filter(e => (/Поднять в поиске/i).test(e.textContent));
                return filtered.length;
            })();";

        public const string UpdateResumeScript = @"
            (function update_resumes() {
                let links = document.querySelectorAll('.bloko-link');
                let filtered = Array.from(links).filter(e => (/Поднять в поиске/i).test(e.textContent));
                filtered.forEach(el => { el.click(); });
                return filtered.length;
            })();";
    }
}