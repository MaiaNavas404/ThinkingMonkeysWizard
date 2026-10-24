using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private VideoPlayer _openingCutscene;
    [SerializeField]
    private GameObject _startScreen;
    private float _elapsedTime = 0;
    private bool _cutsceneStarted = false;
    //Load gamescene after 5 seconds
    void Start()
    {
        _startButton.onClick.AddListener(PlayOpening);
    }
    private void Update()
    {
        if (_cutsceneStarted)
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 5)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    void PlayOpening()
    {
        _startScreen.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(false);
        _openingCutscene.Play();
        _cutsceneStarted=true;
    }
}
