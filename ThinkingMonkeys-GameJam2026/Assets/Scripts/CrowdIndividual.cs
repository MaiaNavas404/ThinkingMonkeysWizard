using UnityEngine;

public class CrowdIndividual : MonoBehaviour
{
    private CrowdCheer crowdCheer;

    private float angle;
    private float startingPosition;
    private float yOffset;
    private float randomSpeed;

    void Start()
    {
        crowdCheer = FindAnyObjectByType<CrowdCheer>();
        startingPosition = transform.position.y;
        randomSpeed = Random.Range(crowdCheer.defaultSpeed - crowdCheer.randomnessFactor, crowdCheer.defaultSpeed + crowdCheer.randomnessFactor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        yOffset = startingPosition + crowdCheer.maximumHeight;
        angle += crowdCheer.currentSpeedFactor * 0.1f * randomSpeed;

        Vector3 newPos = new Vector3(transform.position.x,
            yOffset = Mathf.Sin(angle) * crowdCheer.maximumHeight,
            transform.position.z);
        transform.position = newPos;
    }
}
