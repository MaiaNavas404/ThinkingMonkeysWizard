using UnityEngine;
using System.Collections.Generic;

public class PlayerHandleHealth : MonoBehaviour
{
    public GameObject HeartPrefab;
    List<PlayerHealth> hearts = new List<PlayerHealth>();

    public PlayerBomboclatScriptForHealthAndStates HealthOfPlayer;
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
        float maxHealthRemainder = HealthOfPlayer.MaxHealth % 2;
        int heartsToMake = (int)((HealthOfPlayer.MaxHealth / 2) + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(HealthOfPlayer.CurrentHealth - (i * 2), 0, 2);
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
}
