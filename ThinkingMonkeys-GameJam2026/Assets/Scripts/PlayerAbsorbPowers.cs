using System.Collections;
using UnityEngine;

public class PlayerAbsorbPowers : MonoBehaviour
{
    private ParryMechanic _parryMechanic;

    [Header("Fire")]
    [SerializeField] private int _numberOfFlames;
    [SerializeField] private float _distanceBetweenFlames;
    [SerializeField] private GameObject _flameObject;
    [SerializeField] private float _timeBetweenFlames;
    private float _timer;

    private void Start()
    {
        _parryMechanic = GetComponent<ParryMechanic>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void UseAbsorbedElement()
    {
        switch (_parryMechanic._absorbedElement)
        {
            case ProjectileType.Fire:
                FirePowers();
                break;
            case ProjectileType.Ice:
                IcePowers();
                break;
            case ProjectileType.Necromancy:
                NecromancyPowers();
                break;
        }

        // Consume the element after activation
        _parryMechanic._absorbedElement = ProjectileType.None;
    }

    private IEnumerator FirePowers()
    {
        for (int i = 0; i < _numberOfFlames; i++)
        {
            float distance = i * _distanceBetweenFlames;
            Instantiate(_flameObject, 
                new Vector3(transform.position.x, transform.position.y, distance), 
                Quaternion.identity);

            yield return new WaitForSeconds(_timeBetweenFlames);
        }
    }
    private void IcePowers() => Debug.Log("Use ice");
    private void NecromancyPowers() => Debug.Log("Use necromancy");
}