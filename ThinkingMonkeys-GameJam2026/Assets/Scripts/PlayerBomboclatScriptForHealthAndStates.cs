using UnityEngine;

public class PlayerBomboclatScriptForHealthAndStates : MonoBehaviour
{
    public float MaxHealth, CurrentHealth;
    private bool _isDead = false;
    public PlayerHandleHealth Healthy;

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
        CurrentHealth -= 1;
        if (CurrentHealth <= 0)
        {
            _isDead = true;
        }
        Healthy.DrawHearts();
    }
}
