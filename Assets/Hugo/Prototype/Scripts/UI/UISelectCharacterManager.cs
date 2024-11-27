using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.UI
{
    public class UISelectCharacterManager : MonoBehaviour
    {
        [SerializeField] private PlayerType _playerOneType;
        [SerializeField] private PlayerType _playerTwoType;
        
        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
            
            GameManager.FirstPlayerScriptableObject = _playerOneType;
            GameManager.SecondPlayerScriptableObject = _playerTwoType;
        }
    }
}