using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
    }

    private void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            KillEnemy(other.gameObject);
        }
    }


}
