using Managers;
using UI;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCountTouchBall : MonoBehaviour
    {
        public int CurrentTouchCount { get; private set; }
        
        [Header("Settings")]
        [SerializeField] private int _maxTouchCount = 3;
        [SerializeField] private float _hitInterval = 0.2f;
        
        [Header("Display")]
        [SerializeField] private Vector2 _positionCanvas = new Vector2(0.75f, 1f);
        
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _canvasCountTouchBall;

        private bool _canHit = true;
        private float _time;

        #region === EVENTS ===

        private void OnEnable()
        {
            ResetComponent();
            
            EventBus.OnPlayerScored += OnPlayerScored;
            EventBus.OnPlayerTouchedBall += OnPlayerTouchedBall;
            EventBus.OnFoul += Foul;
        }

        private void OnPlayerScored(int playerId)
        {
            ResetTouchCount();
        }
        
        private void OnPlayerTouchedBall(int playerId)
        {
            if (playerId != playerController.PlayerId)
            {
                ResetTouchCount();
            }
        }
        
        private void Foul()
        {
            ResetTouchCount();
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= OnPlayerScored;
            EventBus.OnPlayerTouchedBall -= OnPlayerTouchedBall;
            EventBus.OnFoul -= Foul;
        }

        #endregion

        private void Update()
        {
            if (!_canHit)
            {
                _time += Time.deltaTime;

                if (_time >= _hitInterval)
                {
                    _time = 0f;
                    _canHit = true;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_canHit) return;
            
            if (other.gameObject.CompareTag("Ball"))
            {
                CurrentTouchCount++;
                Debug.Log("Touch : " + CurrentTouchCount);
                
                // Display
                Vector3 localHitPoint = transform.InverseTransformPoint(other.contacts[0].point);
                Vector3 spawnPos = new Vector3();
                if (localHitPoint.x >= 0f)
                {
                    spawnPos = new Vector3(-_positionCanvas.x, _positionCanvas.y, 0f);
                }
                else
                {
                    spawnPos = new Vector3(_positionCanvas.x, _positionCanvas.y, 0f);
                }
                Vector3 finalSpawnPosition = transform.TransformPoint(spawnPos);

                GameObject go = Instantiate(_canvasCountTouchBall, finalSpawnPosition, Quaternion.identity);
                go.GetComponent<UICountTouchBall>().Setup(CurrentTouchCount);

                if (CurrentTouchCount >= _maxTouchCount)
                {
                    MatchManager.Instance.Foul(playerController.PlayerId);
                    ResetTouchCount();
                }

                _canHit = false;
            }
        }
        
        private void ResetComponent()
        {
            _canHit = true;
            CurrentTouchCount = 0;
        }

        private void ResetTouchCount()
        {
            CurrentTouchCount = 0;
        }
    }
}