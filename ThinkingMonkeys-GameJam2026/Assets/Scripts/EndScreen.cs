using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _endScene;
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Canvas _uI;

    void Start()
    {
        _backButton.onClick.AddListener(BackToStart);
    }
    void Update()
    {
        //Ends cutscene if its finished
        if (_endScene.frame > 0 && _endScene.isPlaying == false)
        {
            _endScene.gameObject.SetActive(false);
            _uI.gameObject.SetActive(true);
        }
    }
    void BackToStart()
    {
        SceneManager.LoadScene("WorkingStart");
    }
}
