using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PunchingMechanic : MonoBehaviour
{
    [SerializeField] private ScreenShake _screenShake;

    [Header("Arm Sprites")]
    [SerializeField] private Image _leftArm;
    [SerializeField] private Image _rightArm;

    private Sprite[] _activeLeftArmSprites;
    private Sprite[] _activeRightArmSprites;
    [SerializeField] private Sprite[] _normalArmSprites;
    [SerializeField] private Sprite[] _fireArmSprites;
    [SerializeField] private Sprite[] _necroArmSprites;
    [SerializeField] private Sprite[] _iceArmSprites;

    [Header("Values")]
    [SerializeField] private float _punchRadius = 5.0f;

    private ParryMechanic _parryMechanic;
    private PlayerAbsorbPowers _playerAbsorbPowers;
    [SerializeField] private float _switchInterval = 0.5f;
    private int _currentFrameIndex = 4;
    [SerializeField] private float _aimTolerance = 0.85f;

    [Header("Enemies")]
    [SerializeField] private Transform _closestEnemy;
    private bool _isPunchingLeft;
    private bool _isPunchingRight;
    private AudioSource _punchAudioSource;

    void Start()
    {
        _punchAudioSource = GetComponent<AudioSource>();

        _leftArm.enabled = true;
        _rightArm.enabled = true;

        _parryMechanic = GetComponent<ParryMechanic>();
        _playerAbsorbPowers = GetComponent<PlayerAbsorbPowers>();
    }

    void Update()
    {
        SwitchSpriteSheet();

        if (Input.GetMouseButtonDown(0))
        {
            HandleAttackInput(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAttackInput(true);
        }
    }

    private void HandleAttackInput(bool isRightArm)
    {
        // Guard check: only block the specific arm that is currently on cooldown
        if (!isRightArm && _isPunchingLeft) return;
        if (isRightArm && _isPunchingRight) return;

        // Lock the targeted arm immediately
        if (!isRightArm) _isPunchingLeft = true;
        else _isPunchingRight = true;

        _punchAudioSource.Play();

        ExecutePunch(isRightArm);

        // Step 1: Check element state BEFORE absorbing any new projectile
        if (!isRightArm && _parryMechanic._absorbedElementLeft != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement(isRightArm);
            _activeLeftArmSprites = _normalArmSprites;
            _leftArm.sprite = _activeLeftArmSprites[4];
        }
        else if (isRightArm && _parryMechanic._absorbedElementRight != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement(isRightArm);
            _activeRightArmSprites = _normalArmSprites;
            _rightArm.sprite = _activeRightArmSprites[4];
        }
        else
        {
            // Rules 1 & 2: No Element -> Punch
        }

        // Step 2: Check for projectiles to absorb (Rules 2 & 4)
        _parryMechanic.TryAbsorbProjectile(isRightArm);
    }

    private void ExecutePunch(bool isRightArm)
    {
        Debug.Log("Punch!");

        DoDamage();

        if (!isRightArm)
        {
            StartCoroutine(PunchSpriteSwitch(_leftArm, false));
        }
        else
        {
            StartCoroutine(PunchSpriteSwitch(_rightArm, true));
        }
    }

    private IEnumerator PunchSpriteSwitch(Image punch, bool isRightArm)
    {
        // Play punch frames 0 through 3
        for (int i = 0; i < 4; i++)
        {
            Sprite[] currentSprites = isRightArm ? _activeRightArmSprites : _activeLeftArmSprites;
            if (currentSprites != null && i < currentSprites.Length)
            {
                punch.sprite = currentSprites[i];
            }
            yield return new WaitForSeconds(_switchInterval);
        }

        // Return to the idle frame (frame 4)
        Sprite[] idleSprites = isRightArm ? _activeRightArmSprites : _activeLeftArmSprites;
        if (idleSprites != null && idleSprites.Length > 4)
        {
            punch.sprite = idleSprites[4];
            Debug.Log(punch.sprite);
        }

        // Unlock ONLY the arm that completed its animation cycle
        if (!isRightArm)
        {
            _isPunchingLeft = false;
        }
        else
        {
            _isPunchingRight = false;
        }
    }

    private void DoDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _punchRadius);
        float closestDistanceSqr = Mathf.Infinity;
        _closestEnemy = null;

        foreach (var c in hitColliders)
        {
            if (!c.gameObject.CompareTag("Enemy")) continue;
            if (c.gameObject == gameObject) continue;

            Renderer rend = c.GetComponent<Renderer>();
            if (rend != null && rend.isVisible)
            {
                Debug.Log("Wizard sees the enemy.");
            }

            Vector3 targetPoint = c.bounds.center;

            Vector3 directionToTarget = (targetPoint - Camera.main.transform.position).normalized;
            float alignment = Vector3.Dot(Camera.main.transform.forward, directionToTarget);

            if (alignment < _aimTolerance)
            {
                continue;
            }

            float sqrDist = (targetPoint - Camera.main.transform.position).sqrMagnitude;

            if (sqrDist < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDist;
                _closestEnemy = c.transform;
            }
        }

        if (_closestEnemy != null)
        {
            EnemyMovement enemy = _closestEnemy.gameObject.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                _screenShake.Shake();
            }
        }
    }

    private void SwitchSpriteSheet()
    {
        switch (_parryMechanic._absorbedElementLeft)
        {
            case ProjectileType.Fire:
                _activeLeftArmSprites = _fireArmSprites;
                break;
            case ProjectileType.Ice:
                _activeLeftArmSprites = _iceArmSprites;
                break;
            case ProjectileType.Necromancy:
                _activeLeftArmSprites = _necroArmSprites;
                break;
            default:
                _activeLeftArmSprites = _normalArmSprites;
                break;
        }

        switch (_parryMechanic._absorbedElementRight)
        {
            case ProjectileType.Fire:
                _activeRightArmSprites = _fireArmSprites;
                break;
            case ProjectileType.Ice:
                _activeRightArmSprites = _iceArmSprites;
                break;
            case ProjectileType.Necromancy:
                _activeRightArmSprites = _necroArmSprites;
                break;
            default:
                _activeRightArmSprites = _normalArmSprites;
                break;
        }
    }
}