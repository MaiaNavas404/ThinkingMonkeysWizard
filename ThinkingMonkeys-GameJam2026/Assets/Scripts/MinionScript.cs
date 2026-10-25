using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MinionScript : MonoBehaviour
{
    [SerializeField] private float _damageCooldown = 1f;
    private float _damageTimer;
    private Transform _player;
    private NavMeshAgent _agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_player.position);
    }
}
