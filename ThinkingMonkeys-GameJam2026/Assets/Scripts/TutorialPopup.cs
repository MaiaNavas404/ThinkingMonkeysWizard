using System.Collections;
using UnityEngine;

public class TutorialSlidePopup : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private RectTransform _panelRect;

    [Header("Positions")]
    [SerializeField] private Vector2 _hiddenPosition = new Vector2(0f, 600f);
    [SerializeField] private Vector2 _visiblePosition = new Vector2(0f, 0f);

    [Header("Timings")]
    [SerializeField] private float _slideDuration = 0.5f;
    [SerializeField] private float _pauseDuration = 5.0f;

    public void Awake()
    {
        TriggerTutorial();
    }

    public void TriggerTutorial()
    {
        StartCoroutine(SlideRoutine());
    }

    private IEnumerator SlideRoutine()
    {
        _panelRect.gameObject.SetActive(true);
        _panelRect.anchoredPosition = _hiddenPosition;

        yield return StartCoroutine(MovePanel(_hiddenPosition, _visiblePosition, _slideDuration));

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(_pauseDuration);

        Time.timeScale = 1f;

        yield return StartCoroutine(MovePanel(_visiblePosition, _hiddenPosition, _slideDuration));

        _panelRect.gameObject.SetActive(false);
    }

    private IEnumerator MovePanel(Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            _panelRect.anchoredPosition = Vector2.Lerp(from, to, smoothT);
            yield return null;
        }

        _panelRect.anchoredPosition = to;
    }
}