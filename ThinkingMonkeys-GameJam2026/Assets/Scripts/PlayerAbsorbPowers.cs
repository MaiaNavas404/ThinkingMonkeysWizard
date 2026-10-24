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
    private Vector3 _flameStartingPosition;
    private Vector3 _flameStartingRotation;

    private void Start()
    {
        _parryMechanic = GetComponent<ParryMechanic>();
    }

    private void Update()
    {
    }

    public void UseAbsorbedElement(bool isRightArm)
    {
        if (!isRightArm)
        {
            switch (_parryMechanic._absorbedElementLeft)
            {
                case ProjectileType.Fire:
                    _flameStartingPosition = transform.position;
                    _flameStartingRotation = transform.forward;
                    StartCoroutine(FirePowers());
                    Debug.Log("Used Fire!");
                    break;
                case ProjectileType.Ice:
                    IcePowers();
                    break;
                case ProjectileType.Necromancy:
                    NecromancyPowers();
                    break;
            }
            _parryMechanic._absorbedElementLeft = ProjectileType.None;
        }
        else if (isRightArm)
        {
            switch (_parryMechanic._absorbedElementRight)
            {
                case ProjectileType.Fire:
                    _flameStartingPosition = transform.position;
                    _flameStartingRotation = transform.forward;
                    StartCoroutine(FirePowers());
                    Debug.Log("Used Fire!");
                    break;
                case ProjectileType.Ice:
                    IcePowers();
                    break;
                case ProjectileType.Necromancy:
                    NecromancyPowers();
                    break;
            }
            _parryMechanic._absorbedElementRight = ProjectileType.None;
        }
    }

    private IEnumerator FirePowers()
    {
        for (int i = 0; i < _numberOfFlames; i++)
        {
            float distance = i * _distanceBetweenFlames;
            Instantiate(_flameObject, 
               transform.position + distance * _flameStartingRotation + Vector3.up * 2.5f + Vector3.right * 1f + Vector3.forward * 2.5f, 
               Quaternion.identity);
            Debug.Log("Fire!!!");
            yield return new WaitForSeconds(_timeBetweenFlames);
        }
    }
    private void IcePowers() => Debug.Log("Use ice");
    private void NecromancyPowers() => Debug.Log("Use necromancy");
}