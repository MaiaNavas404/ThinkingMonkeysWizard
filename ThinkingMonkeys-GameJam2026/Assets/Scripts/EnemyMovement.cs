using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
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
            if (_shootTimer >= _shootCooldown)
            {
                _isShooting = true;
                _shootTimer = 0f;
            }
        }


        if (_isShooting)
        {
            Vector3 lookRotation = (_player.position - transform.position).normalized;
            Instantiate(_projectilePrefab, transform.position + transform.up, Quaternion.LookRotation(lookRotation, Vector3.up));
            _isShooting = false;
        }
    }
}
