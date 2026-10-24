using UnityEngine;

public class PlayerAbsorbPowers : MonoBehaviour
{
    private ParryMechanic _parryMechanic;

    void Start()
    {
        _parryMechanic = GetComponent<ParryMechanic>();
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

    private void FirePowers() => Debug.Log("Use fire");
    private void IcePowers() => Debug.Log("Use ice");
    private void NecromancyPowers() => Debug.Log("Use necromancy");
}