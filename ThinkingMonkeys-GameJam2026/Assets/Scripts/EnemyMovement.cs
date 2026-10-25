using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    private Transform _player;
    private NavMeshAgent _agent;
    [SerializeField] private float _shootCooldown = 3f;
    private float _shootTimer;
    private bool _isShooting = false;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _moveBackDistance = 10f;
    [SerializeField] private float _speed = 5;

    //HEALTH
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    private bool _isKilled = false;
    public bool _isFrozen = false;
    public enum EnemyType
    {
        Fire,
        Ice,
        Necro
    }

    public EnemyType enemyType;
    [SerializeField] private Sprite[] _fireWizardSprites;
    [SerializeField] private Sprite[] _iceWizardSprites;
    [SerializeField] private Sprite[] _necroWizardSprites;
    private Sprite[] _thisWizardSprites;
    private Image _currentWizardImage;
    [SerializeField] private GameObject[] _projectilePrefabs;
    private GameObject _selectedProjectile;

    public static event Action<EnemyMovement> OnEnemyDied;
    [SerializeField] private AudioClip _freezeSound;
    [SerializeField] private AudioClip _hurtSound;
    private AudioSource _audioSource;

    //sprites

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentWizardImage = GetComponentInChildren<Image>();

        switch (enemyType)
        {
            case EnemyType.Fire:
                _thisWizardSprites = _fireWizardSprites;
                //_projectilePrefab.GetComponent<Projectile>()._selectedProjectileType = ProjectileType.Fire;
                _selectedProjectile = _projectilePrefabs[0]; // fireball
                break;
            case EnemyType.Ice:
                _thisWizardSprites = _iceWizardSprites;
                //_projectilePrefab.GetComponent<Projectile>()._selectedProjectileType = ProjectileType.Ice;
                _selectedProjectile = _projectilePrefabs[1]; // iceball
                break;
            case EnemyType.Necro:
                _thisWizardSprites = _necroWizardSprites;
                //_projectilePrefab.GetComponent<Projectile>()._selectedProjectileType = ProjectileType.Necromancy;
                _selectedProjectile = _projectilePrefabs[2]; // minion
                break;
            default:
                _thisWizardSprites = _fireWizardSprites;
                //_projectilePrefab.GetComponent<Projectile>()._selectedProjectileType = ProjectileType.Fire;
                _selectedProjectile = _projectilePrefabs[0];
                break;
        }

        _currentWizardImage.sprite = _thisWizardSprites[0]; // idle

        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _currentHealth = _maxHealth;

        _projectilePrefab = _selectedProjectile;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFrozen) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance <= _moveBackDistance)
        {
            transform.position -= transform.forward * _speed * Time.deltaTime;
        }
        else
        {
            _agent.SetDestination(_player.position);
        }


        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
        //transform.LookAt(_player);

        if (!_isShooting)
        {
            _shootTimer += Time.deltaTime;

            if (enemyType != EnemyType.Necro)
            {
                if (_shootTimer >= _shootCooldown)
                {
                    _isShooting = true;
                    _shootTimer = 0f;
                }
            }

            if (enemyType == EnemyType.Necro)
            {
                if (_shootTimer >= _shootCooldown * 1.5f)
                {
                    _isShooting = true;
                    _shootTimer = 0f;
                }
            }
        }


        if (_isShooting)
        {
            Vector3 lookRotation = (_player.position - transform.position).normalized;
            Instantiate(_projectilePrefab, transform.position + transform.up, Quaternion.LookRotation(lookRotation, Vector3.up));
            _isShooting = false;
            StopCoroutine(DamageFeedbackRoutine());
            if (!_isFrozen) StartCoroutine(ChangeSprite(1, 1));
        }
    }

    private IEnumerator ChangeSprite(int index, float seconds)
    {
        _currentWizardImage.sprite = _thisWizardSprites[index];
        yield return new WaitForSeconds(seconds);
        _currentWizardImage.sprite = _thisWizardSprites[0]; // default
    }

    public void TakeDamage()
    {
        _audioSource.Play();

        if (_isKilled)
        {
            return;
        }

        _currentHealth -= 1;

        if (_currentHealth <= 0)
        {
            OnEnemyDied?.Invoke(this);
            _isKilled = true;

            AudioSource.PlayClipAtPoint(_hurtSound, transform.position);

            //if (_killSound != null && _audioSource != null)
            //{
            //    _audioSource.PlayOneShot(_killSound);
            //}

            //foreach (Renderer r in GetComponentsInChildren<Renderer>())
            //{
            //    r.enabled = false;
            //}

            //foreach (Collider c in GetComponentsInChildren<Collider>())
            //{
            //    c.enabled = false;
            //}

            Destroy(gameObject);
        }
        StopCoroutine(DamageFeedbackRoutine());
        StartCoroutine(DamageFeedbackRoutine());
    }

    private IEnumerator DamageFeedbackRoutine()
    {
        _currentWizardImage.sprite = _thisWizardSprites[2]; // white
        yield return new WaitForSeconds(0.15f);

        if (!_isFrozen) _currentWizardImage.sprite = _thisWizardSprites[3]; // hurt
        yield return new WaitForSeconds(0.4f);


        if (!_isFrozen) _currentWizardImage.sprite = _thisWizardSprites[0];
        else _currentWizardImage.sprite = _thisWizardSprites[4];
    }

    public IEnumerator FreezeRoutine(float duration)
    {
        _audioSource.PlayOneShot(_freezeSound);

        StopCoroutine(DamageFeedbackRoutine());
        StartCoroutine(ChangeSprite(4, duration)); // frozen
        _isFrozen = true;

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
        }

        yield return new WaitForSeconds(duration);

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
        }

        _isFrozen = false;
    }
}
