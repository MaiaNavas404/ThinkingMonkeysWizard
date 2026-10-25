
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 2f;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        // Start completely white.
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;
    }

    private void Start()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float elapsed = 0f;
        Color color = _image.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / _fadeDuration);
            _image.color = color;

            yield return null;
        }

        color.a = 0f;
        _image.color = color;

        // Remove the overlay once it is invisible.
        gameObject.SetActive(false);
    }
}
