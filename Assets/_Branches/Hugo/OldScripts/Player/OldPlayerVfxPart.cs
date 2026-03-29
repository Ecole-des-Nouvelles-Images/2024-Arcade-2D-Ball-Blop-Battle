using System;
using _Branches.Hugo.OldScripts.Ball;
using UnityEngine;

namespace _Branches.Hugo.OldScripts.Player
{
    public class OldPlayerVfxPart : MonoBehaviour
    {
        // gameobject Components
        private OldPlayerController _oldPlayerController;
        private OldPlayerNumberTouchBallHandler _oldPlayerNumberTouchBallHandler;
        
        // References
        private OldBallHandler _oldBallHandler;
        // private ArenaVFXHandler _arenaVFXHandler;
        
        [Header("   VFX Effects")]
        [Header("Common")]
        [SerializeField] private ParticleSystem _vfxJumping;
        [SerializeField] private ParticleSystem _vfxAttacking;
        [SerializeField] private ParticleSystem _vfxPerfectReception;
        [SerializeField] private ParticleSystem _vfxActiveSpecialSpike;
        [Header("SpitOut and Landing")]
        [SerializeField] private ParticleSystem _vfxSpitOutBlue;
        [SerializeField] private ParticleSystem _vfxLandingBlue;
        [SerializeField] private ParticleSystem _vfxSpitOutGreen;
        [SerializeField] private ParticleSystem _vfxLandingGreen;
        [SerializeField] private ParticleSystem _vfxSpitOutYellow;
        [SerializeField] private ParticleSystem _vfxLandingYellow;
        [SerializeField] private ParticleSystem _vfxSpitOutRed;
        [SerializeField] private ParticleSystem _vfxLandingRed;
        [Header("Shoot Special Spike")]
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeBlue;
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeGreen;
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeYellow;
        public ParticleSystem VfxShootSpecialSpikeRed;
        [Header("Trails")]
        [SerializeField] private GameObject _vfxTrailBlue;
        [SerializeField] private GameObject _vfxTrailGreen;
        [SerializeField] private GameObject _vfxTrailYellow;
        [SerializeField] private GameObject _vfxTrailRed;
        [Header("BackGrounds")]
        [SerializeField] private GameObject _vfxImpactBackGround;
        [SerializeField] private GameObject _vfxScroolBackGround;

        private void Awake()
        {
            _oldPlayerController = GetComponent<OldPlayerController>();
            _oldPlayerNumberTouchBallHandler = GetComponent<OldPlayerNumberTouchBallHandler>();
            
            // _arenaVFXHandler = GameObject.FindWithTag("ArenaVFXHandler").GetComponent<ArenaVFXHandler>();
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _oldBallHandler = other.gameObject.GetComponent<OldBallHandler>();
            }
        }

        private void OldPlayerControllerOnAppears(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Appears ");

            if (_vfxTrailBlue && _vfxTrailGreen && _vfxTrailYellow && _vfxTrailRed)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _vfxTrailBlue.SetActive(true);
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _vfxTrailGreen.SetActive(true);
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _vfxTrailYellow.SetActive(true);
                }
                if (_oldPlayerController._blop.PlayerName == "Rouge")
                {
                    _vfxTrailRed.SetActive(true);
                }
            }
        }
        
        private void OldPlayerControllerOnMove(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Move ");
            
            // if (_vfxWalking)
            // {
            //     bool isWalking = _rb2d.velocity.x is > 0.3f or < -0.3f;
            //     ParticleSystem.EmissionModule emission = _vfxWalking.emission;
            //     emission.enabled = _isGrounded && isWalking;
            // }
        }
        
        private void OldPlayerControllerOnDash(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Dash ");
        }
        
        private void OldPlayerControllerOnJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Jump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void OldPlayerControllerOnDoubleJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: DoubleJump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void OldPlayerControllerOnLand(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Land ");
            
            if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _vfxLandingBlue.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _vfxLandingGreen.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _vfxLandingYellow.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Rouge")
                {
                    _vfxLandingRed.Play();
                }
            }
        }
        
        private void OldPlayerControllerOnPerfectReception(object sender, EventArgs e)
        {
            Debug.Log(" VFX: PerfectReception ");
            
            if (_vfxPerfectReception)
            {
                _vfxPerfectReception.Play();
            }
        }
        
        private void OldPlayerControllerOnPunch(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Shoot ");
            
            if (_vfxAttacking)
            {
                _vfxAttacking.Play();
            }
        }
        
        private void OldPlayerControllerOnCanAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" VFX: CanAbsorb ");
        }
        
        private void OldPlayerControllerOnAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Absorb ");
        }
        
        private void OldPlayerControllerOnHasTheBall(object sender, EventArgs e)
        {
            Debug.Log(" VFX: HasTheBall ");
        }
        
        private void OldPlayerControllerOnDrawn(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Drawn ");
            
            if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _vfxSpitOutBlue.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _vfxSpitOutGreen.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _vfxSpitOutYellow.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Rouge")
                {
                    _vfxSpitOutRed.Play();
                }
            }
        }
        
        private void OldPlayerControllerOnIsWalled(object sender, EventArgs e)
        {
            Debug.Log(" VFX: IsWalled ");
        }
        
        private void OldPlayerControllerOnWallJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: WallJump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void OldPlayerControllerOnActiveSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: ActiveSpecialSpike ");
            
            _vfxActiveSpecialSpike.Play();
            
            if (_vfxImpactBackGround)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                    _oldPlayerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                    _oldPlayerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                    _oldPlayerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Rouge")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                    _oldPlayerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
            }
            
            // if (_playerNumberTouchBallHandler.IsPlayerOne)
            // {
            //     _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerOne();
            // }
            // else
            // {
            //     _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerTwo();
            // }
        }
        
        private void OldPlayerControllerOnShootSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: ShootSpecialSpike ");
            
            if (_vfxShootSpecialSpikeBlue && _vfxShootSpecialSpikeGreen && _vfxShootSpecialSpikeYellow && _oldBallHandler !=null)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _oldBallHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeBlue.Play();
                    _oldBallHandler.VFXSpecialSpikeBlue.SetActive(true);
                    _oldBallHandler.VFXSpecialSpikeBlueImpact.Play();
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _oldBallHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeGreen.Play();
                    _oldBallHandler.VFXSpecialSpikeGreen.SetActive(true);
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _oldBallHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeYellow.Play();
                    _oldBallHandler.VFXSpecialSpikeYellow.SetActive(true);
                    _oldBallHandler.VFXSpecialSpikeYellowImpact.Play();
                }
            }
        }
        
        private void OldPlayerControllerOnAbsorbSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Death ");
            
            if (_vfxScroolBackGround)
            {
                if (_oldPlayerController._blop.PlayerName == "Bleu")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Vert")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Jaune")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_oldPlayerController._blop.PlayerName == "Rouge")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
            }
            Destroy(_oldPlayerController.ImpactBackgroundObject);
            
            if (VfxShootSpecialSpikeRed)
            {
                if (_oldPlayerController._blop.PlayerName == "Rouge" && _oldBallHandler != null)
                {
                    _oldBallHandler.BallBaseTrail.SetActive(false);
                    
                    VfxShootSpecialSpikeRed.Play();
                    _oldBallHandler.VFXSpecialSpikeRed.SetActive(true);
                }
            }
        }
        
        private void OldPlayerControllerOnDeath(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Death ");
        }
    }
}