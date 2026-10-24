using UnityEngine;

public class ParryMechanic : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _aimTolerance = 0.85f;
    public ProjectileType _absorbedElementLeft = ProjectileType.None;
    public ProjectileType _absorbedElementRight = ProjectileType.None;

    public void TryAbsorbProjectile(bool isRightArm)
    {
        Transform targetProjectile = GetTargetedProjectile();

        if (targetProjectile != null)
        {
            CatchAndAbsorbElement(targetProjectile, isRightArm);
        }
    }

    private Transform GetTargetedProjectile()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var c in hitColliders)
        {
            if (!c.gameObject.CompareTag("Projectile") || c.gameObject == gameObject) continue;

            Renderer rend = c.GetComponent<Renderer>();
            if (rend == null || !rend.isVisible) continue;

            Vector3 directionToTarget = (c.transform.position - Camera.main.transform.position).normalized;
            float alignment = Vector3.Dot(Camera.main.transform.forward, directionToTarget);

            if (alignment < _aimTolerance)
            {
                continue;
            }

            float sqrDist = (c.transform.position - transform.position).sqrMagnitude;
            if (sqrDist < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDist;
                bestTarget = c.transform;
            }
        }

        return bestTarget;
    }

    private void CatchAndAbsorbElement(Transform projectile, bool isRightArm)
    {
        var projectileScript = projectile.GetComponent<Projectile>();

        Debug.Log($"Found {projectileScript.gameObject.name}");

        if (projectileScript != null)
        {
            if (!isRightArm)
            {
                _absorbedElementLeft = projectileScript._selectedProjectileType;
                Destroy(projectile.gameObject);
                Debug.Log($"Caught and absorbed {_absorbedElementLeft} type!");
            }
            else
            {
                _absorbedElementRight = projectileScript._selectedProjectileType;
                Destroy(projectile.gameObject);
                Debug.Log($"Caught and absorbed {_absorbedElementRight} type!");
            }
        }
    }
}