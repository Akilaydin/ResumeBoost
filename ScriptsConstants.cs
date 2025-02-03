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
        let buttons = document.querySelectorAll('div[class^=""magritte-button-view""]');
        let filtered = Array.from(buttons).filter(el => 
            el.innerText.trim() === 'Поднять в поиске'
        );
        return filtered.length;
    })();";

        public const string UpdateResumeScript = @"
    (function update_resumes() {
        let buttons = document.querySelectorAll('div[class^=""magritte-button-view""]');
        let filtered = Array.from(buttons).filter(el => 
            el.innerText.trim() === 'Поднять в поиске'
        );
        filtered.forEach(el => el.click());
        return filtered.length;
    })();";

    }
}