using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGameWorking : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private VideoPlayer _openingCutscene;
    [SerializeField]
    private GameObject _startScreen;
    void Start()
    {
        _startButton.onClick.AddListener(StartCutscene);
    }
    private void Update()
    {
        if (_openingCutscene.frame > 0 && _openingCutscene.isPlaying == false)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    void StartCutscene()
    {
        _startScreen.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(false);
        _openingCutscene.Play();
    }
}
