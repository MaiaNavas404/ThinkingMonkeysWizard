using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PunchingMechanic : MonoBehaviour
{
    [Header("Arm Sprites")]
    [SerializeField] private Image _leftArm;
    [SerializeField] private Image _rightArm;

    private Sprite[] _activeArmSprites;
    [SerializeField] private Sprite[] _normalArmSprites;
    [SerializeField] private Sprite[] _fireArmSprites;
    [SerializeField] private Sprite[] _necroArmSprites;

    [Header("Values")]
    [SerializeField] private float _punchRadius = 5.0f;

    private ParryMechanic _parryMechanic;
    private PlayerAbsorbPowers _playerAbsorbPowers;
    [SerializeField] private float _switchInterval = 0.5f;
    private int _currentFrameIndex = 4;

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
        if (_parryMechanic._absorbedElement != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement();
            _activeArmSprites = _normalArmSprites;
            _leftArm.sprite = _activeArmSprites[4];
            _rightArm.sprite = _activeArmSprites[4];
        }
        else
        {
            // Rules 1 & 2: No Element -> Punch NEVERMIND
        }

        // Step 2: Check for projectiles to absorb (Rules 2 & 4)
        _parryMechanic.TryAbsorbProjectile();
    }

    private void ExecutePunch(bool isRightArm)
    {
        _isPunching = true;
        Debug.Log("Punch!");

        DoDamage();

        if (!isRightArm)
        {
            StartCoroutine(PunchSpriteSwitch(_leftArm));
        }
        else
        {
            StartCoroutine(PunchSpriteSwitch(_rightArm));
        }
    }

    private IEnumerator PunchSpriteSwitch(Image punch)
    {
        // Play punch frames 0 through 3
        for (int i = 0; i < 4; i++)
        {
            if (i < _activeArmSprites.Length)
            {
                punch.sprite = _activeArmSprites[i];
                Debug.Log(punch.sprite);
            }
            yield return new WaitForSeconds(_switchInterval);
        }

        // Return to the idle frame
        if (_activeArmSprites.Length > 4)
        {
            punch.sprite = _activeArmSprites[4];

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

            Vector3 directionToTarget = c.transform.position - transform.position;
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
        }
    }

    private void SwitchSpriteSheet()
    {
        switch (_parryMechanic._absorbedElement)
        {
            case ProjectileType.Fire:
                _activeArmSprites = _fireArmSprites;
                break;
            case ProjectileType.Ice:
                break;
            case ProjectileType.Necromancy:
                break;
            default:
                _activeArmSprites = _normalArmSprites;
                break;

        }
    }
}