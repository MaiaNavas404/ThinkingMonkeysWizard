using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    [Range(0, 1)] public float defaultSpeed;
    [Range(1, 5)] public float cheeringSpeed;
    [Range(0, 1)] public float randomnessFactor;

    public float maximumHeight;

    [HideInInspector] public float currentSpeedFactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
