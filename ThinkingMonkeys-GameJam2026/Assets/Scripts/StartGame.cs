using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    void Start()
    {
        _startButton.onClick.AddListener(LoadSceneOnClick);
    }
    void LoadSceneOnClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
