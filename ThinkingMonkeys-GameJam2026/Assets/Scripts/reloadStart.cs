using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class reloadStart : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _backButton.onClick.AddListener(ReloadStart);
    }

    // Update is called once per frame
    void ReloadStart()
    {
        SceneManager.LoadScene("WorkingStart");
    }
}
