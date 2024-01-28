using Microsoft.Extensions.Localization;

namespace AviationSalonWeb.Resources
{
    public class Localizer
    {
        private readonly IStringLocalizer<Localizer> _stringLocalizer;

        public Localizer(IStringLocalizer<Localizer> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public string this[string key] => _stringLocalizer[key];

        public string this[string key, params object[] arguments] => _stringLocalizer[key, arguments];

        public string this[string key, Dictionary<string, string> values]
        {
            get
            {
                var localizedString = _stringLocalizer[key];
                string localizedValue = localizedString.Value;

                foreach (var value in values)
                {
                    localizedValue = localizedValue.Replace($"{{{value.Key}}}", value.Value);
                }

                return localizedValue;
            }
        }
    }
}
