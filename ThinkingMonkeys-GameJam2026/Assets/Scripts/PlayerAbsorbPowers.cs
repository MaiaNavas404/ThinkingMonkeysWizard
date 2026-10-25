using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAbsorbPowers : MonoBehaviour
{
    private ParryMechanic _parryMechanic;

    [Header("Global")]
    [SerializeField] Transform _projectileSpawn;

    [Header("Fire")]
    [SerializeField] private int _numberOfFlames;
    [SerializeField] private float _distanceBetweenFlames;
    [SerializeField] private GameObject _flameObject;
    [SerializeField] private float _timeBetweenFlames;
    private Vector3 _flameStartingPosition;
    private Vector3 _flameStartingRotation;

    [Header("Ice")]
    [SerializeField] private GameObject _iceObject;

    [Header("Necromancy")]
    [SerializeField] private GameObject _necroBeamPrefab;
    [SerializeField] private float _necroRange = 12f;
    [SerializeField] private float _aimTolerance = 0.75f;
    [SerializeField] private float _beamDuration = 0.35f;
    [SerializeField] private AudioClip _necromancySound;
    private AudioSource _audioSource;

    [SerializeField] private TMP_Text _leftCrosshair;
    [SerializeField] private TMP_Text _rightCrosshair;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();   
        _parryMechanic = GetComponent<ParryMechanic>();
    }

    private void Update()
    {
    }

    public void UseAbsorbedElement(bool isRightArm)
    {
        if (!isRightArm)
        {
            switch (_parryMechanic._absorbedElementLeft)
            {
                case ProjectileType.Fire:
                    _flameStartingPosition = transform.position;
                    _flameStartingRotation = transform.forward;
                    StartCoroutine(FirePowers());
                    Debug.Log("Used Fire!");
                    break;
                case ProjectileType.Ice:
                    IcePowers(); 
                    break;
                case ProjectileType.Necromancy:
                    NecromancyPowers();
                    break;
            }
            ChangeCrosshairColor(_leftCrosshair, Color.grey);
            _parryMechanic._absorbedElementLeft = ProjectileType.None;
        }
        else if (isRightArm)
        {
            switch (_parryMechanic._absorbedElementRight)
            {
                case ProjectileType.Fire:
                    _flameStartingPosition = transform.position;
                    _flameStartingRotation = transform.forward;
                    StartCoroutine(FirePowers());
                    Debug.Log("Used Fire!");
                    break;
                case ProjectileType.Ice:
                    IcePowers();
                    break;
                case ProjectileType.Necromancy:
                    NecromancyPowers();
                    break;
            }
            ChangeCrosshairColor(_rightCrosshair, Color.grey);
            _parryMechanic._absorbedElementRight = ProjectileType.None;
        }
    }

    private IEnumerator FirePowers()
    {
        for (int i = 0; i < _numberOfFlames; i++)
        {
            float distance = i * _distanceBetweenFlames;
            /*Instantiate(_flameObject, 
               transform.position + distance * _flameStartingRotation + Vector3.up * 2.5f + Vector3.right * 1f + Vector3.forward * 2.5f, 
               Quaternion.identity);*/
            Instantiate(_flameObject, _projectileSpawn.position + distance * _flameStartingRotation + Camera.main.transform.up * 2f, Quaternion.identity);
            Debug.Log("Fire!!!");
            yield return new WaitForSeconds(_timeBetweenFlames);
        }
    }
    private void IcePowers()
    {
        Vector3 spawnOffset = Camera.main.transform.forward * 1.5f
                            //+ Camera.main.transform.right * 0.5f
                            + Camera.main.transform.up * 1f;

        Vector3 spawnPosition = _projectileSpawn.position + spawnOffset;

        Quaternion spawnRotation = Camera.main.transform.rotation;

        GameObject spawnedIce = Instantiate(_iceObject, spawnPosition, spawnRotation);
    }

    private void NecromancyPowers()
    {
        Transform targetEnemy = FindClosestAimedEnemy();

        if (targetEnemy == null)
        {
            Debug.Log("Necromancy cast, but no target in range!");
            return;
        }

        // damage
        EnemyMovement enemy = targetEnemy.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.TakeDamage();
        }

        // heal
        this.GetComponent<PlayerBomboclatScriptForHealthAndStates>().Heal();

        // the green curved beam
        if (_necroBeamPrefab != null)
        {
            Vector3 origin = _projectileSpawn != null ? _projectileSpawn.position : Camera.main.transform.position;
            GameObject beamObj = Instantiate(_necroBeamPrefab, origin, Quaternion.identity);

            NecroBeam beam = beamObj.GetComponent<NecroBeam>();
            if (beam != null)
            {
                beam.AnimateBeam(origin, targetEnemy, _beamDuration);
            }

            _audioSource.PlayOneShot(_necromancySound);
        }
    }

    private Transform FindClosestAimedEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _necroRange);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Camera cam = Camera.main;

        foreach (var c in hitColliders)
        {
            if (!c.gameObject.CompareTag("Enemy") || c.gameObject == gameObject) continue;

            // Target collider center to account for elevation
            Vector3 toTarget = c.bounds.center - cam.transform.position;
            Vector3 directionToTarget = toTarget.normalized;

            float alignment = Vector3.Dot(cam.transform.forward, directionToTarget);
            if (alignment < _aimTolerance) continue;

            float sqrDist = toTarget.sqrMagnitude;
            if (sqrDist < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDist;
                bestTarget = c.transform;
            }
        }

        return bestTarget;
    }

    private void ChangeCrosshairColor(TMP_Text text, Color color)
    {
        text.GetComponent<TextMeshProUGUI>().color = color;
    }
}
