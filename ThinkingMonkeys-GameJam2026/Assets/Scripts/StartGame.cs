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
    [SerializeField]
    private GameObject _textBox;
    void Start()
    {
        _startButton.onClick.AddListener(StartText);
    }
    private void Update()
    {
        if (_openingCutscene.frame > 0 && _openingCutscene.isPlaying == false)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    void StartText()
    {
        _startScreen.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(false);
    }
}
