using System.Collections;
using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    [Range(0, 1)] public float defaultSpeed;
    [Range(1, 5)] public float cheeringSpeed;
    [Range(0, 1)] public float randomnessFactor;

    public float maximumHeight;

    [HideInInspector] public float currentSpeedFactor;
    [SerializeField] private GameObject _crowdGoingWild;

    private Coroutine _cheerCoroutine;
    private AudioSource _andTheCrowdGoesWildAudio;

    void Start()
    {
        if (_crowdGoingWild != null)
        {
            _andTheCrowdGoesWildAudio = _crowdGoingWild.GetComponent<AudioSource>();
        }

        currentSpeedFactor = defaultSpeed;
        UpdateState("Idle");
    }

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
        // Stop the specific running coroutine instance
        if (_cheerCoroutine != null)
        {
            StopCoroutine(_cheerCoroutine);
        }

        _cheerCoroutine = StartCoroutine(CrowdGoesWild());
    }

    private IEnumerator CrowdGoesWild()
    {
        UpdateState("Cheer");

        if (_andTheCrowdGoesWildAudio != null)
        {
            _andTheCrowdGoesWildAudio.Play();
        }

        yield return new WaitForSeconds(4f);

        UpdateState("Idle");
        _cheerCoroutine = null;
    }

    private void UpdateState(string state)
    {
        switch (state)
        {
            case "Idle":
                currentSpeedFactor = defaultSpeed;
                break;
            case "Cheer":
                currentSpeedFactor = cheeringSpeed;
                break;
        }
    }
}