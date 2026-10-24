using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _velocity = 5f;
    [SerializeField] private float _timeToLive = 10f;
    private float _timer;

    public ProjectileType _selectedProjectileType;


    private void Update()
    {
        transform.position += transform.forward * _velocity * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= _timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBomboclatScriptForHealthAndStates health = other.GetComponent<PlayerBomboclatScriptForHealthAndStates>();
            Debug.Log("bug");
            health.TakeDamage();

            Destroy(gameObject);
        }
    }
}
