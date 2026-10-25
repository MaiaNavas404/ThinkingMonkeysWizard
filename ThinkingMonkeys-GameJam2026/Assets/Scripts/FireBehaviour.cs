using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    private Transform _player;
    private float _timer;
    private float _timeToLive = 2.5f;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));

        _timer += Time.deltaTime;
        if (_timer >= _timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Damageed!");
            other.GetComponent<EnemyMovement>().TakeDamage();
        }
        if (other.gameObject.GetComponent<MinionScript>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
