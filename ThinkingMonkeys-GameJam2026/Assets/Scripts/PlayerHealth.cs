using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Sprite _fullHeart, _halfHeart, _emptyHeart;
    Image _heartImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _heartImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Epmty:
                _heartImage.sprite = _emptyHeart;   
                break;

            case HeartStatus.Half:
                _heartImage.sprite = _halfHeart;
                break;

            case HeartStatus.Full:
                _heartImage.sprite = _fullHeart;
                break;
        }
    }
    public enum HeartStatus
    {
        Epmty = 0,
        Half = 1,
        Full = 2
    }
}
