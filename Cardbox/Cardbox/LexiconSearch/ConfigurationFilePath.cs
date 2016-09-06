using System.Configuration;
using System.IO;
using System.Web;

namespace Cardbox.LexiconSearch
{
    public class ConfigurationFilePath : IFilePath
    {
        private readonly string _appSetting;

        public ConfigurationFilePath(string appSetting)
        {
            _appSetting = appSetting;
        }

        public string GetPath()
        {
            string appSettingLexiconPath = ConfigurationManager.AppSettings[_appSetting];
            string lexiconFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), appSettingLexiconPath);
            return lexiconFilePath;
        }
    }
}
