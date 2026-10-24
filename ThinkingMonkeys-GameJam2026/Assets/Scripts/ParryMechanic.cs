using UnityEngine;

public class ParryMechanic : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private Transform _closestProjectile;
    private Vector3 _currentPosition;
    //public ProjectileType _absorbedElement;

    void Start()
    {
        _currentPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _currentPosition = transform.position;
            //CatchAndAbsorbElement();
        }
    }

    private Transform GetClosestProjectile()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, _radius);
        foreach (var c in hitColliders)
        {
            {
                if (c.gameObject.CompareTag("Projectile") == false) continue;
                if (c.gameObject == gameObject) continue;

                Renderer rend = c.GetComponent<Renderer>();

                if (rend != null && rend.isVisible)
                {
                    Debug.Log("Wizard see the projectile.");
                }


                Vector3 directionToTarget = c.transform.position - _currentPosition;
                float sqrDist = directionToTarget.sqrMagnitude;

                if (sqrDist < _radius)
                {
                    _radius = sqrDist;
                    _closestProjectile = c.transform;
                }
            }

            return _closestProjectile;
        }
        return null;
    }

    private void CatchAndAbsorbElement(GameObject projectile)
    {
       // var projectileType = projectile.GetComponent<ProjectileType>();
       // _absorbedElement = projectileType;
    }
}
