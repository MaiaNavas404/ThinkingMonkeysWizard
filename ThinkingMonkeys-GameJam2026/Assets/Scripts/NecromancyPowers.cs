using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NecroBeam : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private int _pointsCount = 12;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void AnimateBeam(Vector3 startPoint, Transform targetTransform, float duration)
    {
        StartCoroutine(BeamRoutine(startPoint, targetTransform, duration));
    }

    private IEnumerator BeamRoutine(Vector3 startPoint, Transform targetTransform, float duration)
    {
        _lineRenderer.positionCount = _pointsCount;
        float elapsed = 0f;

        // Moira's GRASP
        Vector3 randomPerpendicular = Vector3.Cross((targetTransform.position - startPoint).normalized, Vector3.up);
        if (randomPerpendicular == Vector3.zero) randomPerpendicular = Vector3.right;
        Vector3 midControlPoint = (startPoint + targetTransform.position) * 0.5f + (randomPerpendicular * 0.6f);

        while (elapsed < duration)
        {
            if (targetTransform == null) break;

            Vector3 endPoint = targetTransform.position + Vector3.up * 1f; // Aim toward torso/chest

            for (int i = 0; i < _pointsCount; i++)
            {
                float t = i / (float)(_pointsCount - 1);

                // Quadratic Bezier Curve: B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
                Vector3 point = Mathf.Pow(1 - t, 2) * startPoint
                              + 2 * (1 - t) * t * midControlPoint
                              + Mathf.Pow(t, 2) * endPoint;

                if (i > 0 && i < _pointsCount - 1)
                {
                    float jitterX = (Mathf.PerlinNoise(i * 1.5f, Time.time * 15f) - 0.5f) * 0.2f;
                    float jitterY = (Mathf.PerlinNoise(i * 2.5f, Time.time * 15f) - 0.5f) * 0.2f;
                    point += new Vector3(jitterX, jitterY, 0f);
                }

                _lineRenderer.SetPosition(i, point);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}