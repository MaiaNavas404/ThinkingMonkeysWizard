using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGameWorking : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private VideoPlayer _openingCutscene;
    [SerializeField] private GameObject _startScreen;

    void Start()
    {
        _startButton.onClick.AddListener(StartCutscene);
        _openingCutscene.prepareCompleted += OnPrepared;
        _openingCutscene.loopPointReached += OnFinished;
        _openingCutscene.errorReceived += OnError;
    }

    void OnDestroy()
    {
        if (_openingCutscene != null)
        {
            _openingCutscene.prepareCompleted -= OnPrepared;
            _openingCutscene.loopPointReached -= OnFinished;
            _openingCutscene.errorReceived -= OnError;
        }
    }

    void StartCutscene()
    {
        _startButton.interactable = false;
        _openingCutscene.Prepare();
    }

    void OnPrepared(VideoPlayer vp)
    {
        _startScreen.SetActive(false);
        _startButton.gameObject.SetActive(false);
        vp.Play();
    }

    void OnFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnError(VideoPlayer vp, string message)
    {
        Debug.LogError("Video failed in WebGL: " + message);
        SceneManager.LoadScene("GameScene");
    }
}