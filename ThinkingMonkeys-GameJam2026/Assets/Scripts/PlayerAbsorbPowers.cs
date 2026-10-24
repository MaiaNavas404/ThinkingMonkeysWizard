using System;
using UnityEngine;

public class PlayerAbsorbPowers : MonoBehaviour
{
    private ParryMechanic _parryMechanic;

    void Start()
    {
        _parryMechanic = gameObject.GetComponent<ParryMechanic>();
    }

    void Update()
    {
        SwitchElement();
    }

    private void SwitchElement()
    {
        switch (_parryMechanic._absorbedElement)
        {
            case ProjectileType.Fire:
                FirePowers();
                break;
            case ProjectileType.Ice:
                IcePowers();
                break;
        }
    }


    private void FirePowers()
    {

    }
    private void IcePowers()
    {

    }
}