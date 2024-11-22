using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerData _playerOneData;
        [SerializeField]
        private PlayerData _playerTwoData;
        
        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
            
            GameManager.FirstPlayerScriptableObject = _playerOneData;
            GameManager.SecondPlayerScriptableObject = _playerTwoData;
        }
    }
}
