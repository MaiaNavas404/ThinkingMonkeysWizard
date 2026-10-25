using System;
using System.Collections;
using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    [Range(0, 1)] public float defaultSpeed;
    [Range(1, 5)] public float cheeringSpeed;
    [Range(0, 1)] public float randomnessFactor;

    public float maximumHeight;

    [HideInInspector] public float currentSpeedFactor;


    private void OnEnable()
    {
        EnemyMovement.OnEnemyDied += HandleCrowdCheer;
    }

    private void OnDisable()
    {
        EnemyMovement.OnEnemyDied -= HandleCrowdCheer;
    }

    private void HandleCrowdCheer(EnemyMovement movement)
    {
        int random = UnityEngine.Random.Range(0, 2);
        //if (random == 0)
        {
            StartCoroutine(CrowdGoesWild());
        }
    }

    private IEnumerator CrowdGoesWild()
    {
        UpdateState("Cheer");
        yield return new WaitForSeconds(3);
        UpdateState("Idle");
    }

    void Start()
    {
        currentSpeedFactor = defaultSpeed;
        UpdateState("Idle");
    }

    private void UpdateState(string state)
    {
        switch(state)
        {
            case "Idle":
                currentSpeedFactor = defaultSpeed;
                break;
            case "Cheer":
                currentSpeedFactor = cheeringSpeed;
                break;
        }
    }

    void Update()
    {
        //UpdateState("Cheer"); when kill enemy
        //UpdateStatee("Idle");
    }
}
