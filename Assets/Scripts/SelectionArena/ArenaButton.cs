using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionArena
{
    public class ArenaButton : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private TextMeshProUGUI _txtName;
        [SerializeField] private Image _image;
        
        public void Setup(string arenaName, Sprite arenaVisual)
        {
            _txtName.text = arenaName;
            _image.sprite = arenaVisual;
        }
    }
}