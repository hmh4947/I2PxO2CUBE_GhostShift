using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    /// <summary>
    /// Syncs the button to saving the settings.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SaveSettingsButton : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(delegate { SettingsHelper.Save(); });
        }
    }
}
