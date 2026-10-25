using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _velocity = 5f;
    [SerializeField] private float _timeToLive = 10f;
    private float _timer;

    public ProjectileType _selectedProjectileType;

    [SerializeField] private Sprite[] _projectileSprites;
    //private Sprite _selectedProjectileSprite;

    bool _isProjectile = true;


    private void Start()
    {
        /*switch (_selectedProjectileType)
        {
            case ProjectileType.Fire:
                this.GetComponentInChildren<Image>().sprite = _projectileSprites[0];
                break;
            case ProjectileType.Ice:
                this.GetComponentInChildren<Image>().sprite = _projectileSprites[1];
                break;
            default:
                this.GetComponentInChildren<Image>().sprite = _projectileSprites[0];
                break;
        }*/

        if (gameObject.GetComponent<MinionScript>() != null)
            _isProjectile = false;
    }

    private void Update()
    {
        if (_isProjectile)
        {
            transform.position += transform.forward * _velocity * Time.deltaTime;

            _timer += Time.deltaTime;
            if (_timer >= _timeToLive)
            {
                Destroy(gameObject);
            }
        }
    }
}
