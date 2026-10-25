using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer _endScene;
    [SerializeField] private Button _backButton;
    [SerializeField] private Canvas _uI;

    private bool _isPlaying = false;

    void Start()
    {
        _backButton.onClick.AddListener(BackToStart);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _uI.gameObject.SetActive(false);

        _endScene.source = VideoSource.Url;
        _endScene.url = Application.streamingAssetsPath + "/EndAnimatic.mp4";

        _endScene.prepareCompleted += OnVideoPrepared;
        _endScene.loopPointReached += OnCutsceneFinished;
        _endScene.errorReceived += OnVideoError;

        _endScene.Prepare();
    }

    void OnDestroy()
    {
        if (_endScene != null)
        {
            _endScene.prepareCompleted -= OnVideoPrepared;
            _endScene.loopPointReached -= OnCutsceneFinished;
            _endScene.errorReceived -= OnVideoError;
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
        _isPlaying = true;
    }

    void OnCutsceneFinished(VideoPlayer vp)
    {
        if (!_isPlaying) return;
        ShowEndUI();
    }

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("End cutscene failed to load: " + message);
        ShowEndUI();
    }

    void ShowEndUI()
    {
        _endScene.gameObject.SetActive(false);
        _uI.gameObject.SetActive(true);
    }

    void BackToStart()
    {
        SceneManager.LoadScene("WorkingStart");
    }
}