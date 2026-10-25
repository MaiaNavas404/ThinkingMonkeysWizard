using UnityEngine;

public class AndTheCrowdGoesWild : MonoBehaviour
{
    [SerializeField] private AudioSource _andTheCrowdGoesWildSound;

    void Start()
    {
        _andTheCrowdGoesWildSound = GetComponent<AudioSource>();
    }

    public void AndTheCrowdGoesWildPlaySound()
    {
        _andTheCrowdGoesWildSound.Play();
    }
}
