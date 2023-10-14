using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private TextMeshProUGUI _resumeButtonText;
    [SerializeField] private TextMeshProUGUI _menuButtonText;
    [SerializeField] private SFXSystem _sfxSystem;

    private void Awake()
    {
        if (MenuScript.isRussianTranslation)
        {
            _resumeButtonText.text = "Продолжить";
            _menuButtonText.text = "Меню";
        }
        else
        {
            _resumeButtonText.text = "Resume";
            _menuButtonText.text = "Menu";
        }
    }
    public void OnPauseButton()
    {
        Time.timeScale = 0f;
        _sfxSystem.PauseAll();
        _pauseMenuUI.SetActive(true);
    }
    public void OnResumeButton()
    {
        _pauseMenuUI.SetActive(false);
        _sfxSystem.UnPauseAll();
        Time.timeScale = 1f;
    }
    public void OnMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
