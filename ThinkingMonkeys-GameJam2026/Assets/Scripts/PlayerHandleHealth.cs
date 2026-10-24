using UnityEngine;
using System.Collections.Generic;

public class PlayerHandleHealth : MonoBehaviour
{
    public GameObject HeartPrefab;
    public float MaxHealth, CurrentHealth;
    List<PlayerHealth> hearts = new List<PlayerHealth>();
    private bool _isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DrawHearts()
    {
        ClearHearts();

        //determine how many hearts to make total based off the max health
        float maxHealthRemainder = MaxHealth % 2;
        int heartsToMake = (int)((MaxHealth / 2) + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(CurrentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((PlayerHealth.HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(HeartPrefab);
        newHeart.transform.SetParent(transform);

        PlayerHealth heartComponent = newHeart.GetComponent<PlayerHealth>();
        heartComponent.SetHeartImage(PlayerHealth.HeartStatus.Epmty);
        hearts.Add(heartComponent);
    }
    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<PlayerHealth>();
    }



    //AAAAAAAAAAAAAAHHHHHHHHHHHHHHHH

    public void TakeDamage()
    {
        CurrentHealth -= 1;
        if (CurrentHealth <= 0)
        {
            _isDead = true;
        }
        DrawHearts();
    }
}
