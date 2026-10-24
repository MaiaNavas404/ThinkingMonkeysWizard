using System.Collections;
using UnityEngine;

public class IceBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            FreezeEnemy(other.gameObject);
        }
    }

    private IEnumerator FreezeEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyMovement>().enabled = false;
        // change enemy sprite to frozen
        yield return new WaitForSeconds(1);
        enemy.GetComponent<EnemyMovement>().enabled = true;
    }
}