using System;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerSfxPart : MonoBehaviour
    {
        private PlayerController _playerController;
        private AudioSource _audioSource;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            // Subscribe
            _playerController.OnAppears += PlayerControllerOnAppears;
            _playerController.OnMove += PlayerControllerOnMove;
            _playerController.OnDash += PlayerControllerOnDash;
            _playerController.OnJump += PlayerControllerOnJump;
            _playerController.OnDoubleJump += PlayerControllerOnDoubleJump;
            _playerController.OnLand += PlayerControllerOnLand;
            _playerController.OnPerfectReception += PlayerControllerOnPerfectReception;
            _playerController.OnPunch += PlayerControllerOnPunch;
            _playerController.OnCanAbsorb += PlayerControllerOnCanAbsorb;
            _playerController.OnAbsorb += PlayerControllerOnAbsorb;
            _playerController.OnHasTheBall += PlayerControllerOnHasTheBall;
            _playerController.OnDrawn += PlayerControllerOnDrawn;
            _playerController.OnIsWalled += PlayerControllerOnIsWalled;
            _playerController.OnWallJump += PlayerControllerOnWallJump;
            _playerController.OnActiveSpecialSpike += PlayerControllerOnActiveSpecialSpike;
            _playerController.OnShootSpecialSpike += PlayerControllerOnShootSpecialSpike;
            _playerController.OnAbsorbSpecialSpike += PlayerControllerOnAbsorbSpecialSpike;
            _playerController.OnDeath += PlayerControllerOnDeath;
            
        }

        private void OnDisable()
        {
            // Unsubscribe
            _playerController.OnAppears -= PlayerControllerOnAppears;
            _playerController.OnMove -= PlayerControllerOnMove;
            _playerController.OnDash -= PlayerControllerOnDash;
            _playerController.OnJump -= PlayerControllerOnJump;
            _playerController.OnDoubleJump -= PlayerControllerOnDoubleJump;
            _playerController.OnLand -= PlayerControllerOnLand;
            _playerController.OnPerfectReception -= PlayerControllerOnPerfectReception;
            _playerController.OnPunch -= PlayerControllerOnPunch;
            _playerController.OnCanAbsorb -= PlayerControllerOnCanAbsorb;
            _playerController.OnAbsorb -= PlayerControllerOnAbsorb;
            _playerController.OnHasTheBall -= PlayerControllerOnHasTheBall;
            _playerController.OnDrawn -= PlayerControllerOnDrawn;
            _playerController.OnIsWalled -= PlayerControllerOnIsWalled;
            _playerController.OnWallJump -= PlayerControllerOnWallJump;
            _playerController.OnActiveSpecialSpike -= PlayerControllerOnActiveSpecialSpike;
            _playerController.OnShootSpecialSpike -= PlayerControllerOnShootSpecialSpike;
            _playerController.OnAbsorbSpecialSpike -= PlayerControllerOnAbsorbSpecialSpike;
            _playerController.OnDeath -= PlayerControllerOnDeath;
        }
        
        private void PlayerControllerOnAppears(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Appears ");
        }
        
        private void PlayerControllerOnMove(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Move ");
        }
        
        private void PlayerControllerOnDash(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Dash ");
        }
        
        private void PlayerControllerOnJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Jump ");
        }
        
        private void PlayerControllerOnDoubleJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: DoubleJump ");
        }
        
        private void PlayerControllerOnLand(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Land ");
        }
        
        private void PlayerControllerOnPerfectReception(object sender, EventArgs e)
        {
            Debug.Log(" SFX: PerfectReception ");
        }
        
        private void PlayerControllerOnPunch(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Shoot ");
        }
        
        private void PlayerControllerOnCanAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" SFX: CanAbsorb ");
        }
        
        private void PlayerControllerOnAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Absorb ");
        }
        
        private void PlayerControllerOnHasTheBall(object sender, EventArgs e)
        {
            Debug.Log(" SFX: HasTheBall ");
        }
        
        private void PlayerControllerOnDrawn(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Drawn ");
        }
        
        private void PlayerControllerOnIsWalled(object sender, EventArgs e)
        {
            Debug.Log(" SFX: IsWalled ");
        }
        
        private void PlayerControllerOnWallJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: WallJump ");
        }
        
        private void PlayerControllerOnActiveSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: ActiveSpecialSpike ");
        }
        
        private void PlayerControllerOnShootSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: ShootSpecialSpike ");
        }
        
        private void PlayerControllerOnAbsorbSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: AbsorbSpecialSpike ");
        }
        
        private void PlayerControllerOnDeath(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Death ");
        }
    }
}
