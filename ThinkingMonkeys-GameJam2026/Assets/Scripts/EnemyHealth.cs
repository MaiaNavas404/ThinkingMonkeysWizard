using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    private bool _isKilled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        if (_isKilled)
        {
            return;
        }

        _currentHealth -= 1;

        if (_currentHealth <= 0)
        {
            _isKilled = true;

            //if (_killSound != null && _audioSource != null)
            //{
            //    _audioSource.PlayOneShot(_killSound);
            //}

            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }

            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            Destroy(gameObject);
        }
    }
}
