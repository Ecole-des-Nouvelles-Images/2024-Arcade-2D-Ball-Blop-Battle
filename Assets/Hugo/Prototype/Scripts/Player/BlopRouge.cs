using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRouge : PlayerType
    {
        // Ball Components
        private PlayerController _playerController;
        private Rigidbody2D _rb2dBall;
        private BallHandler _ballHandler;
        private FakeBallHandler _fakeBallHandler;
        
        [Header("Red Player Data")]
        [SerializeField] private GameObject _fakeBallPrefab;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" ROUGE : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _rb2dBall = ball.GetComponent<Rigidbody2D>();
            _ballHandler = ball.GetComponent<BallHandler>();
            _fakeBallHandler = _fakeBallPrefab.GetComponent<FakeBallHandler>();
            
            // Change Constraints
            _rb2dBall.constraints = RigidbodyConstraints2D.None;
            _rb2dBall.constraints = RigidbodyConstraints2D.FreezeRotation;
            _ballHandler.ReversIsCatch();
            _ballHandler.InvokeMethodTimer("ReverseIsTrigger", 0.1f);
            
            // Special Spike
            if (Mathf.Approximately(_playerController.CountShootRedSpecialSpike, 1))
            {
                GameObject newBall = Instantiate(_fakeBallPrefab, player.transform.position, Quaternion.identity);
                newBall.GetComponent<FakeBallHandler>().Setup(direction, SpeedSpecialSpike);
                _playerController.RedSpecialSpike = true;
            }

            if (Mathf.Approximately(_playerController.CountShootRedSpecialSpike, 2))
            {
                if (direction == Vector2.zero)
                {
                    _rb2dBall.AddForce(new Vector2(1,0) * SpeedSpecialSpike, ForceMode2D.Impulse);
                }
                else
                {
                    _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
                }
            }
            
            if (Mathf.Approximately(_playerController.CountShootRedSpecialSpike, 3))
            {
                GameObject newBall = Instantiate(_fakeBallPrefab, player.transform.position, Quaternion.identity);
                newBall.GetComponent<FakeBallHandler>().Setup(direction, SpeedSpecialSpike);
                _playerController.RedSpecialSpike = true;
            }
        }
    }
}