using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerBomboclatScriptForHealthAndStates : MonoBehaviour
{
    [SerializeField] private Screenshake2 _screenShake;

    public float MaxHealth, CurrentHealth;
    private bool _isDead = false;
    public PlayerHandleHealth Healthy;
    private float _damageTimer;
    [SerializeField] private float _damageCooldown = 1f;
    [SerializeField] private string _loseScene = "LoseScene";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_damageTimer > 0)
            _damageTimer -= Time.deltaTime;

        if (_isDead)
        {
            SceneManager.LoadScene(_loseScene);
        }
    }
    public void TakeDamage()
    {
        _screenShake.Shake();

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
        CurrentHealth += 1;
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
