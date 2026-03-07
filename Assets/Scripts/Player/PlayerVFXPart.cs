using Player.ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerVFXPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;

        private void OnEnable()
        {
            _playerController.PlayerEvents.OnJump += PlayerEventsOnOnJump;
            _playerController.PlayerEvents.OnPunch += PlayerEventsOnOnPunch;
            _playerController.PlayerEvents.OnDrawn += PlayerEventsOnOnDrawn;
            _playerController.PlayerEvents.OnPerfectReception += PlayerEventsOnOnPerfectReception;
            _playerController.PlayerEvents.OnLand += PlayerEventsOnOnLand;
            _playerController.PlayerEvents.OnActiveSpecialSpike += PlayerEventsOnOnActiveSpecialSpike;
            _playerController.PlayerEvents.OnShootSpecialSpike += PlayerEventsOnOnShootSpecialSpike;
        }
        
        private void OnDisable()
        {
            _playerController.PlayerEvents.OnJump -= PlayerEventsOnOnJump;
            _playerController.PlayerEvents.OnPunch -= PlayerEventsOnOnPunch;
            _playerController.PlayerEvents.OnDrawn -= PlayerEventsOnOnDrawn;
            _playerController.PlayerEvents.OnPerfectReception -= PlayerEventsOnOnPerfectReception;
            _playerController.PlayerEvents.OnLand -= PlayerEventsOnOnLand;
            _playerController.PlayerEvents.OnActiveSpecialSpike -= PlayerEventsOnOnActiveSpecialSpike;
            _playerController.PlayerEvents.OnShootSpecialSpike -= PlayerEventsOnOnShootSpecialSpike;
        }

        private void PlayerEventsOnOnJump(Blop blop)
        {
            Instantiate(blop.PSJump, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnPunch(Blop blop)
        {
            Instantiate(blop.PSChocWave, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnDrawn(Blop blop)
        {
            Instantiate(blop.PSSpitOut, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnPerfectReception(Blop blop)
        {
            Instantiate(blop.PSPerfectReception, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnLand(Blop blop)
        {
            Instantiate(blop.PSLanding, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnActiveSpecialSpike(Blop blop)
        {
            Instantiate(blop.PSActiveSpecialSpike, transform.position, transform.rotation);
        }

        private void PlayerEventsOnOnShootSpecialSpike(Blop blop)
        {
            Instantiate(blop.PSShootSpecialSpike, transform.position, transform.rotation);
        }
    }
}
