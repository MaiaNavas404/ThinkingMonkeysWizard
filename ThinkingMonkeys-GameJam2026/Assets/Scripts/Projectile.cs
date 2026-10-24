using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ProjectileTypes
    {
        Fire,
        Ice,
        Necromancy
    }

    [SerializeField] private ProjectileTypes _selectedProjectileType;
}
