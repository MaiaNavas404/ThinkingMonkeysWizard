
using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.15f;
    [SerializeField] private float _shakeMagnitude = 0.08f;

    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        _shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            float strength = 1f - elapsed / _shakeDuration;

            Vector3 offset = Random.insideUnitSphere
                             * _shakeMagnitude
                             * strength;

            transform.localPosition = _originalPosition + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPosition;
        _shakeCoroutine = null;
    }
}
