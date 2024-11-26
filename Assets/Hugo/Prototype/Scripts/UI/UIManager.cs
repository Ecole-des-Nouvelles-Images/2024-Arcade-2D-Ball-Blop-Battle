using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [FormerlySerializedAs("_playerOneData")] [SerializeField]
        private PlayerType _playerOneType;
        [FormerlySerializedAs("_playerTwoData")] [SerializeField]
        private PlayerType _playerTwoType;
        
        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
            
            GameManager.FirstPlayerScriptableObject = _playerOneType;
            GameManager.SecondPlayerScriptableObject = _playerTwoType;
        }
    }
}
