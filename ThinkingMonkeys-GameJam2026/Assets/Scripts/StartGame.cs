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
    [SerializeField]
    private GameObject _startScreen2;
    private float _elapsedTime = 0;
    private bool _cutsceneStarted = false;
    private bool _enabledCutsceneInput;
    //Load gamescene after 5 seconds
    void Start()
    {
        _startButton.onClick.AddListener(StartText);
    }
    private void Update()
    {
        if (_enabledCutsceneInput&& Input.anyKey)
            CutScenePlay();

        if (_cutsceneStarted)
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 5)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    void StartText()
    {
        _startScreen.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(false);
        _enabledCutsceneInput = true;
        
    }

    void CutScenePlay()
    {
        _openingCutscene.Play();
        _startScreen2.gameObject.SetActive(false);  
        _cutsceneStarted = true;
    }
}
