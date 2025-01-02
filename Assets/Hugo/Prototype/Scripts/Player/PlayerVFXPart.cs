using System;
using Hugo.Prototype.Scripts.Arene;
using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerVfxPart : MonoBehaviour
    {
        // gameobject Components
        private PlayerController _playerController;
        private PlayerNumberTouchBallHandler _playerNumberTouchBallHandler;
        
        // References
        private BallHandler _ballHandler;
        private ArenaVFXHandler _arenaVFXHandler;
        
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
            _playerController = GetComponent<PlayerController>();
            _playerNumberTouchBallHandler = GetComponent<PlayerNumberTouchBallHandler>();
            
            _arenaVFXHandler = GameObject.FindWithTag("ArenaVFXHandler").GetComponent<ArenaVFXHandler>();
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ballHandler = other.gameObject.GetComponent<BallHandler>();
            }
        }

        private void PlayerControllerOnAppears(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Appears ");

            if (_vfxTrailBlue && _vfxTrailGreen && _vfxTrailYellow && _vfxTrailRed)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _vfxTrailBlue.SetActive(true);
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _vfxTrailGreen.SetActive(true);
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _vfxTrailYellow.SetActive(true);
                }
                if (_playerController.PlayerType.PlayerName == "Rouge")
                {
                    _vfxTrailRed.SetActive(true);
                }
            }
        }
        
        private void PlayerControllerOnMove(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Move ");
            
            // if (_vfxWalking)
            // {
            //     bool isWalking = _rb2d.velocity.x is > 0.3f or < -0.3f;
            //     ParticleSystem.EmissionModule emission = _vfxWalking.emission;
            //     emission.enabled = _isGrounded && isWalking;
            // }
        }
        
        private void PlayerControllerOnDash(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Dash ");
        }
        
        private void PlayerControllerOnJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Jump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void PlayerControllerOnDoubleJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: DoubleJump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void PlayerControllerOnLand(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Land ");
            
            if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _vfxLandingBlue.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _vfxLandingGreen.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _vfxLandingYellow.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Rouge")
                {
                    _vfxLandingRed.Play();
                }
            }
        }
        
        private void PlayerControllerOnPerfectReception(object sender, EventArgs e)
        {
            Debug.Log(" VFX: PerfectReception ");
            
            if (_vfxPerfectReception)
            {
                _vfxPerfectReception.Play();
            }
        }
        
        private void PlayerControllerOnPunch(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Shoot ");
            
            if (_vfxAttacking)
            {
                _vfxAttacking.Play();
            }
        }
        
        private void PlayerControllerOnCanAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" VFX: CanAbsorb ");
        }
        
        private void PlayerControllerOnAbsorb(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Absorb ");
        }
        
        private void PlayerControllerOnHasTheBall(object sender, EventArgs e)
        {
            Debug.Log(" VFX: HasTheBall ");
        }
        
        private void PlayerControllerOnDrawn(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Drawn ");
            
            if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _vfxSpitOutBlue.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _vfxSpitOutGreen.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _vfxSpitOutYellow.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Rouge")
                {
                    _vfxSpitOutRed.Play();
                }
            }
        }
        
        private void PlayerControllerOnIsWalled(object sender, EventArgs e)
        {
            Debug.Log(" VFX: IsWalled ");
        }
        
        private void PlayerControllerOnWallJump(object sender, EventArgs e)
        {
            Debug.Log(" VFX: WallJump ");
            
            if (_vfxJumping)
            {
                _vfxJumping.Play();
            }
        }
        
        private void PlayerControllerOnActiveSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: ActiveSpecialSpike ");
            
            _vfxActiveSpecialSpike.Play();
            
            if (_vfxImpactBackGround)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                    _playerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                    _playerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                    _playerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Rouge")
                {
                    _vfxImpactBackGround.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                    _playerController.ImpactBackgroundObject = Instantiate(_vfxImpactBackGround, Vector2.zero, Quaternion.identity);
                }
            }
            
            if (_playerNumberTouchBallHandler.IsPlayerOne)
            {
                _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerOne();
            }
            else
            {
                _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerTwo();
            }
        }
        
        private void PlayerControllerOnShootSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: ShootSpecialSpike ");
            
            if (_vfxShootSpecialSpikeBlue && _vfxShootSpecialSpikeGreen && _vfxShootSpecialSpikeYellow && _ballHandler !=null)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _ballHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeBlue.Play();
                    _ballHandler.VFXSpecialSpikeBlue.SetActive(true);
                    _ballHandler.VFXSpecialSpikeBlueImpact.Play();
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _ballHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeGreen.Play();
                    _ballHandler.VFXSpecialSpikeGreen.SetActive(true);
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _ballHandler.BallBaseTrail.SetActive(false);
                    
                    _vfxShootSpecialSpikeYellow.Play();
                    _ballHandler.VFXSpecialSpikeYellow.SetActive(true);
                    _ballHandler.VFXSpecialSpikeYellowImpact.Play();
                }
            }
        }
        
        private void PlayerControllerOnAbsorbSpecialSpike(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Death ");
            
            if (_vfxScroolBackGround)
            {
                if (_playerController.PlayerType.PlayerName == "Bleu")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Vert")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Jaune")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
                if (_playerController.PlayerType.PlayerName == "Rouge")
                {
                    _vfxScroolBackGround.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                    Instantiate(_vfxScroolBackGround, new Vector2(0, -9), Quaternion.identity);
                }
            }
            Destroy(_playerController.ImpactBackgroundObject);
            
            if (VfxShootSpecialSpikeRed)
            {
                if (_playerController.PlayerType.PlayerName == "Rouge" && _ballHandler != null)
                {
                    _ballHandler.BallBaseTrail.SetActive(false);
                    
                    VfxShootSpecialSpikeRed.Play();
                    _ballHandler.VFXSpecialSpikeRed.SetActive(true);
                }
            }
        }
        
        private void PlayerControllerOnDeath(object sender, EventArgs e)
        {
            Debug.Log(" VFX: Death ");
        }
    }
}