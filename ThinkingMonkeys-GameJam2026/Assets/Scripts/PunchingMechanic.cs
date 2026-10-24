using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PunchingMechanic : MonoBehaviour
{
    [Header("Arm Sprites")]
    [SerializeField] private Image _leftArmIdle;
    [SerializeField] private Image _rightArmIdle;
    [SerializeField] private Image _leftArmPunch;
    [SerializeField] private Image _rightArmPunch;

    [Header("Values")]
    [SerializeField] private float _punchRadius = 5.0f;

    private ParryMechanic _parryMechanic;
    private PlayerAbsorbPowers _playerAbsorbPowers;

    [Header("Enemies")]
    [SerializeField] private Transform _closestEnemy;
    private bool _isPunching;

    void Start()
    {
        _leftArmIdle.enabled = true;
        _rightArmIdle.enabled = true;

        _parryMechanic = GetComponent<ParryMechanic>();
        _playerAbsorbPowers = GetComponent<PlayerAbsorbPowers>();
    }

    void Update()
    {
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

        // Step 1: Check element state BEFORE absorbing any new projectile
        if (_parryMechanic._absorbedElement != ProjectileType.None)
        {
            // Rules 3 & 4: Has Element -> Use Powers
            _playerAbsorbPowers.UseAbsorbedElement();
        }
        else
        {
            // Rules 1 & 2: No Element -> Punch
            ExecutePunch(isRightArm);
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
            StartCoroutine(PunchSpriteSwitch(_leftArmIdle, _leftArmPunch));
        }
        else
        {
            StartCoroutine(PunchSpriteSwitch(_rightArmIdle, _rightArmPunch));
        }
    }

    private IEnumerator PunchSpriteSwitch(Image idleImage, Image punchImage)
    {
        idleImage.enabled = false;
        punchImage.enabled = true;

        yield return new WaitForSeconds(1f);

        idleImage.enabled = true;
        punchImage.enabled = false;
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
}