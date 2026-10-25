using System.Collections;
using UnityEngine;

public class IceBehaviour : MonoBehaviour
{
    [SerializeField] private float _velocity = 5f;
    [SerializeField] private float _timeToLive = 10f;
    private float _timer;

    void Update()
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
        Debug.Log("Frozen!");
        if (other.gameObject.CompareTag("Enemy"))
        {
            FreezeEnemy(other.gameObject);
        }
    }

    private void FreezeEnemy(GameObject enemy)
    {
        var enemyMovement = enemy.GetComponent<EnemyMovement>();
        // change enemy sprite to frozen
        //enemyMovement._isFrozen = true;
        enemyMovement.StartCoroutine(enemyMovement.FreezeRoutine(4f));
        //enemyMovement._isFrozen = false;
    }
}