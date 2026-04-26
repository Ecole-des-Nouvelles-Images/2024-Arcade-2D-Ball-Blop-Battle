using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.Menu
{
    public class UILocaleSelector : MonoBehaviour
    {
        public void LocalisationSettings(int localId)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localId];
        }
    }
}
