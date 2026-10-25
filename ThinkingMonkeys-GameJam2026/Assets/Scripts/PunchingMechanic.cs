using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PunchingMechanic : MonoBehaviour
{
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
    private bool _isPunching;

    void Start()
    {
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
        if (_isPunching) return;

        ExecutePunch(isRightArm);
        // Step 1: Check element state BEFORE absorbing any new projectile
        if (!isRightArm && _parryMechanic._absorbedElementLeft != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement(isRightArm);
            if (!isRightArm)
            {
                _activeLeftArmSprites = _normalArmSprites;
                _leftArm.sprite = _activeLeftArmSprites[4];
            }
        }
        if (isRightArm && _parryMechanic._absorbedElementRight != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement(isRightArm);
            if (isRightArm)
            {
                _activeRightArmSprites = _normalArmSprites;
                _rightArm.sprite = _activeRightArmSprites[4];
            }
        }
        else
        {
            // Rules 1 & 2: No Element -> Punch NEVERMIND
        }

        // Step 2: Check for projectiles to absorb (Rules 2 & 4)
        _parryMechanic.TryAbsorbProjectile(isRightArm);
    }

    private void ExecutePunch(bool isRightArm)
    {
        _isPunching = true;
        Debug.Log("Punch!");

        DoDamage();

        if (!isRightArm)
        {
            StartCoroutine(PunchSpriteSwitch(_leftArm, isRightArm));
        }
        else
        {
            StartCoroutine(PunchSpriteSwitch(_rightArm, isRightArm));
        }
    }

    private IEnumerator PunchSpriteSwitch(Image punch, bool isRightArm)
    {
        // Play punch frames 0 through 3
        for (int i = 0; i < 4; i++)
        {
            if (i < _activeLeftArmSprites.Length)
            {
                if (!isRightArm) punch.sprite = _activeLeftArmSprites[i];
                if (isRightArm ) punch.sprite = _activeRightArmSprites[i];
                //Debug.Log(punch.sprite);
            }
            yield return new WaitForSeconds(_switchInterval);
        }

        // Return to the idle frame
        if (_activeLeftArmSprites.Length > 4)
        {
            if (!isRightArm) punch.sprite = _activeLeftArmSprites[4];
            if (isRightArm) punch.sprite = _activeRightArmSprites[4];
            Debug.Log(punch.sprite);
        }

        // Unlock punching only after the full punch cycle completes
        _isPunching = false;
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

            Vector3 directionToTarget = (c.transform.position - Camera.main.transform.position).normalized;
            float alignment = Vector3.Dot(Camera.main.transform.forward, directionToTarget);

            if (alignment < _aimTolerance)
            {
                continue;
            }

            float sqrDist = directionToTarget.sqrMagnitude;

            if (sqrDist < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDist;
                _closestEnemy = c.transform;
            }
        }

        if (_closestEnemy != null)
        {
            // Apply damage to _closestEnemy here
            EnemyMovement enemy = _closestEnemy.gameObject.GetComponent<EnemyMovement>();
            enemy.TakeDamage();
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