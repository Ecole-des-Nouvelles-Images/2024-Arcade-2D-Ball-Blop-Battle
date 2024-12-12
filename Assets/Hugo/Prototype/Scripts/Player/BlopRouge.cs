using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRouge : PlayerType
    {
        // Random
        private bool _randomNumberAlreadyChoose;
        private int _randomNumber;
        
        // Ball Components
        private PlayerController _playerController;
        private Rigidbody2D _rb2dBall;
        private BallHandler _ballHandler;
        
        [Header("Red Player Data")]
        [SerializeField] private GameObject _fakeBallPrefab;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" ROUGE : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _rb2dBall = ball.GetComponent<Rigidbody2D>();
            _ballHandler = ball.GetComponent<BallHandler>();

            if (_randomNumberAlreadyChoose == false)
            {
                _randomNumber = Random.Range(0, 2);
                _randomNumberAlreadyChoose = true;
            }
            
            // Special Spike
            if (_playerController.CountShootSpecialSpike == 0)
            {
                if (_randomNumber == 0)
                {
                    RealBall(direction);
                }
                else
                {
                    FakeBall(direction, player);
                }
                return;
            }

            if (_playerController.CountShootSpecialSpike == 1)
            {
                if (_randomNumber == 1)
                {
                    RealBall(direction);
                }
                else
                {
                    FakeBall(direction, player);
                }
                return;
            }
            
            if (_playerController.CountShootSpecialSpike == 2)
            {
                if (_randomNumber == 2)
                {
                    RealBall(direction);
                }
                else
                {
                    FakeBall(direction, player);
                }
                
                _randomNumberAlreadyChoose = false;
                _playerController.ResetStatesAfterSpecialSpike();
            }
        }

        private void RealBall(Vector2 direction)
        {
            // Debug.Log(" real shoot ");
            
            // Change Constraints
            _rb2dBall.constraints = RigidbodyConstraints2D.None;
            _rb2dBall.constraints = RigidbodyConstraints2D.FreezeRotation;
            _ballHandler.ReversIsCatch();
                
            if (direction == Vector2.zero)
            {
                _rb2dBall.AddForce(new Vector2(1,0) * SpeedSpecialSpike, ForceMode2D.Impulse);
            }
            else
            {
                _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
            }
            _playerController.CountShootSpecialSpike++;
        }

        private void FakeBall(Vector2 direction, GameObject player)
        {
            // Debug.Log(" fake shoot ");
                
            GameObject newBall = Instantiate(_fakeBallPrefab, player.transform.position, Quaternion.identity);
            newBall.GetComponent<FakeBallHandler>().Setup(direction, SpeedSpecialSpike);
                
            _playerController.CountShootSpecialSpike++;
        }
    }
}