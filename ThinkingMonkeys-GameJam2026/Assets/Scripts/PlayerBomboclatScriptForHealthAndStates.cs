using UnityEngine;

public class PlayerBomboclatScriptForHealthAndStates : MonoBehaviour
{
    public float MaxHealth, CurrentHealth;
    private bool _isDead = false;
    public PlayerHandleHealth Healthy;
    private float _damageTimer;
    [SerializeField] private float _damageCooldown = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_damageTimer > 0)
            _damageTimer -= Time.deltaTime;
    }
    public void TakeDamage()
    {
        CurrentHealth -= 1;
        if (CurrentHealth <= 0)
        {
            _isDead = true;
        }
        Healthy.DrawHearts();
    }

    public void Heal()
    {
        if (CurrentHealth >= MaxHealth) return;
        CurrentHealth += 2;
        Healthy.DrawHearts();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && _damageTimer <= 0)
        {
            TakeDamage();

            if (other.GetComponent<MinionScript>() == null)
            {
                Destroy(other.gameObject);
            }
            _damageTimer = _damageCooldown;
        }
    }
}
