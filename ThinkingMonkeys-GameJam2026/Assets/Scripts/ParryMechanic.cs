using TMPro;
using UnityEngine;

public class ParryMechanic : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _aimTolerance = 0.85f;
    public ProjectileType _absorbedElementLeft = ProjectileType.None;
    public ProjectileType _absorbedElementRight = ProjectileType.None;

    [SerializeField] private TMP_Text _leftCrosshair;
    [SerializeField] private TMP_Text _rightCrosshair;

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



    private void ChangeCrosshairColor(TMP_Text text, Color color)
    {
        text.GetComponent<TextMeshProUGUI>().color = color;
    }


    private void CatchAndAbsorbElement(Transform projectile, bool isRightArm)
    {
        var projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript == null) return;

        Debug.Log($"Found {projectileScript.gameObject.name}");

        Color darkGreen = new Color(0f, 0.5f, 0f);

        if (!isRightArm)
        {
            _absorbedElementLeft = projectileScript._selectedProjectileType;
            Destroy(projectile.gameObject);
            Debug.Log($"Caught and absorbed {_absorbedElementLeft} type!");

            switch (_absorbedElementLeft)
            {
                case ProjectileType.Fire:
                    ChangeCrosshairColor(_leftCrosshair, Color.red);
                    break;
                case ProjectileType.Ice:
                    ChangeCrosshairColor(_leftCrosshair, Color.cyan);
                    break;
                case ProjectileType.Necromancy:
                    ChangeCrosshairColor(_leftCrosshair, darkGreen);
                    break;
            }
        }
        else
        {
            _absorbedElementRight = projectileScript._selectedProjectileType;
            Destroy(projectile.gameObject);
            Debug.Log($"Caught and absorbed {_absorbedElementRight} type!");

            switch (_absorbedElementRight)
            {
                case ProjectileType.Fire:
                    ChangeCrosshairColor(_rightCrosshair, Color.red);
                    break;
                case ProjectileType.Ice:
                    ChangeCrosshairColor(_rightCrosshair, Color.cyan);
                    break;
                case ProjectileType.Necromancy:
                    ChangeCrosshairColor(_rightCrosshair, darkGreen);
                    break;
            }
        }
    }
}