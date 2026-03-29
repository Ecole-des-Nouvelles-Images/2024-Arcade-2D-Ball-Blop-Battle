using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Branches.Hugo.OldScripts.Player
{
    public class PlayerSfxPart : MonoBehaviour
    {
        private OldPlayerController _oldPlayerController;
        private AudioSource _audioSource;
        
        [Header("Blop")]
        public List<AudioClip> BlopClips;
        
        private void Awake()
        {
            _oldPlayerController = GetComponent<OldPlayerController>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            // Subscribe
            _oldPlayerController.OnAppears += OldPlayerControllerOnAppears;
            _oldPlayerController.OnMove += OldPlayerControllerOnMove;
            _oldPlayerController.OnDash += OldPlayerControllerOnDash;
            _oldPlayerController.OnJump += OldPlayerControllerOnJump;
            _oldPlayerController.OnDoubleJump += OldPlayerControllerOnDoubleJump;
            _oldPlayerController.OnLand += OldPlayerControllerOnLand;
            _oldPlayerController.OnPerfectReception += OldPlayerControllerOnPerfectReception;
            _oldPlayerController.OnPunch += OldPlayerControllerOnPunch;
            _oldPlayerController.OnCanAbsorb += OldPlayerControllerOnCanAbsorb;
            _oldPlayerController.OnAbsorb += OldPlayerControllerOnAbsorb;
            _oldPlayerController.OnHasTheBall += OldPlayerControllerOnHasTheBall;
            _oldPlayerController.OnDrawn += OldPlayerControllerOnDrawn;
            _oldPlayerController.OnIsWalled += OldPlayerControllerOnIsWalled;
            _oldPlayerController.OnWallJump += OldPlayerControllerOnWallJump;
            _oldPlayerController.OnActiveSpecialSpike += OldPlayerControllerOnActiveSpecialSpike;
            _oldPlayerController.OnShootSpecialSpike += OldPlayerControllerOnShootSpecialSpike;
            _oldPlayerController.OnAbsorbSpecialSpike += OldPlayerControllerOnAbsorbSpecialSpike;
            _oldPlayerController.OnDeath += OldPlayerControllerOnDeath;
            
        }

        private void OnDisable()
        {
            // Unsubscribe
            _oldPlayerController.OnAppears -= OldPlayerControllerOnAppears;
            _oldPlayerController.OnMove -= OldPlayerControllerOnMove;
            _oldPlayerController.OnDash -= OldPlayerControllerOnDash;
            _oldPlayerController.OnJump -= OldPlayerControllerOnJump;
            _oldPlayerController.OnDoubleJump -= OldPlayerControllerOnDoubleJump;
            _oldPlayerController.OnLand -= OldPlayerControllerOnLand;
            _oldPlayerController.OnPerfectReception -= OldPlayerControllerOnPerfectReception;
            _oldPlayerController.OnPunch -= OldPlayerControllerOnPunch;
            _oldPlayerController.OnCanAbsorb -= OldPlayerControllerOnCanAbsorb;
            _oldPlayerController.OnAbsorb -= OldPlayerControllerOnAbsorb;
            _oldPlayerController.OnHasTheBall -= OldPlayerControllerOnHasTheBall;
            _oldPlayerController.OnDrawn -= OldPlayerControllerOnDrawn;
            _oldPlayerController.OnIsWalled -= OldPlayerControllerOnIsWalled;
            _oldPlayerController.OnWallJump -= OldPlayerControllerOnWallJump;
            _oldPlayerController.OnActiveSpecialSpike -= OldPlayerControllerOnActiveSpecialSpike;
            _oldPlayerController.OnShootSpecialSpike -= OldPlayerControllerOnShootSpecialSpike;
            _oldPlayerController.OnAbsorbSpecialSpike -= OldPlayerControllerOnAbsorbSpecialSpike;
            _oldPlayerController.OnDeath -= OldPlayerControllerOnDeath;
        }
        
        private void OldPlayerControllerOnAppears(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Appears ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnMove(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Move ");
        }
        
        private void OldPlayerControllerOnDash(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Dash ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Jump ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnDoubleJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: DoubleJump ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnLand(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Land ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnPerfectReception(object sender, EventArgs e)
        {
            Debug.Log(" SFX: PerfectReception ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnPunch(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Shoot ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnCanAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" SFX: CanAbsorb ");
        }
        
        private void OldPlayerControllerOnAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Absorb ");
        }
        
        private void OldPlayerControllerOnHasTheBall(object sender, EventArgs e)
        {
            Debug.Log(" SFX: HasTheBall ");
        }
        
        private void OldPlayerControllerOnDrawn(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Drawn ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnIsWalled(object sender, EventArgs e)
        {
            Debug.Log(" SFX: IsWalled ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnWallJump(object sender, EventArgs e)
        {
            Debug.Log(" SFX: WallJump ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnActiveSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: ActiveSpecialSpike ");
        }
        
        private void OldPlayerControllerOnShootSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: ShootSpecialSpike ");
            
            _audioSource.clip = BlopClips[RandomNumber(BlopClips.Count)];
            _audioSource.Play();
        }
        
        private void OldPlayerControllerOnAbsorbSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" SFX: AbsorbSpecialSpike ");
        }
        
        private void OldPlayerControllerOnDeath(object sender, EventArgs e)
        {
            Debug.Log(" SFX: Death ");
        }

        private int RandomNumber(int listCount)
        {
            int randomNumber = Random.Range(0, listCount);
            return randomNumber;
        }
    }
}
