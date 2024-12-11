using UnityEngine;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIOptionsHandler : MonoBehaviour
    {
        
        
        private void SetResolution(int width, int height, bool fullscreen)
        {
            Screen.SetResolution(width, height, fullscreen);
            
            Debug.Log(Screen.currentResolution);
        }
    }
}
