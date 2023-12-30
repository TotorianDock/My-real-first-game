using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class MenuScript : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;

    [Header("Audio")]
    [SerializeField] private TextMeshProUGUI _volumeText;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private SFXFields_Menu _sfxFields;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TextMeshProUGUI _resolutionText;
    private Resolution[] _resolutions;

    [Header("Options")]
    [SerializeField] private TextMeshProUGUI _backButtonText;
    [SerializeField] private TextMeshProUGUI _startButtonText;
    [SerializeField] private TextMeshProUGUI _optionsButtonText;
    [SerializeField] private TextMeshProUGUI _exitButtonText;
    [SerializeField] private TextMeshProUGUI _toggleText;
    [SerializeField] private Toggle _fulllscreenToggle;

    [Header("Language")]
    [SerializeField] private TextMeshProUGUI _languageText;
    [SerializeField] private TextMeshProUGUI _nameOfTheGame;
    public static bool isRussianTranslation = false;

    [Header("Hero")]
    public GameObject heroKnightPrefab;
    [SerializeField] private GameObject _heroWizardPrefab;
    [SerializeField] private GameObject _heroChangeButtons;
    public static GameObject hero;

    [Header("Other")]
    [SerializeField] private GameObject _loadingScreen;


    private void Awake()
    {
        if (isRussianTranslation == true)
        {
            _languageText.text = "Язык";
            _volumeText.text = "Громкость";
            _backButtonText.text = "Назад";
            _startButtonText.text = "Старт";
            _optionsButtonText.text = "Настройки";
            _exitButtonText.text = "Выход";
            _toggleText.text = "Полный экран";
            _resolutionText.text = "Разрешение";
            _tipText.text = "*Совет: лучше не играть в полноэкранном режиме*";
            _nameOfTheGame.text = "(Моя первая реальная игра)";
        }
        else
        {
            _languageText.text = "Language";
            _volumeText.text = "Volume";
            _backButtonText.text = "Back";
            _startButtonText.text = "Start";
            _optionsButtonText.text = "Options";
            _exitButtonText.text = "Exit";
            _toggleText.text = "Fullscreen";
            _resolutionText.text = "Resolution";
            _tipText.text = "*Tip:It's better to play in non-fullscreen mode*";
            _nameOfTheGame.text = string.Empty;
        }
    }
    private void Start()
    {
        hero = heroKnightPrefab;

        _sliderText.text = Convert.ToString(SFXSystem.MainVolume * 100);
        _slider.value = SFXSystem.MainVolume * 100;
        
        _sfxFields.MenuMusic();
        Screen.fullScreen = _fulllscreenToggle.isOn;
        if (!_fulllscreenToggle.isOn)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, _fulllscreenToggle.isOn);
        }

        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        List<string> options = new();

        int currentResolutionIndex = 0;
        int lastResolutionWidth = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].width != lastResolutionWidth)
            {
                string option = $"{_resolutions[i].width} x {_resolutions[i].height}";
                if (_resolutions[i].width == lastResolutionWidth)
                    options.Remove(option);
                options.Add(option);
                lastResolutionWidth = _resolutions[i].width;

                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _slider.onValueChanged.AddListener((v) =>
        {
            _sliderText.text = Convert.ToString(v);
        });
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        _sfxFields.ChangeVolume(volume);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (!isFullscreen)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        }
    }
    public void OnStartButton(bool isPressed)
    {
        if (_heroChangeButtons.activeInHierarchy == false)
            _heroChangeButtons.SetActive(isPressed);
        else
            _heroChangeButtons.SetActive(!isPressed);
    }
    public void OnChangeButtonToKnight()
    {
        _loadingScreen.SetActive(true);
        hero = heroKnightPrefab;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnChangeButtonToWizard()
    {
        _loadingScreen.SetActive(true);
        hero = _heroWizardPrefab;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
    public void OnRussianTranslationButton()
    {
        isRussianTranslation = true;
        Awake();
    }
    public void OnEnglishTranslationButton()
    {
        isRussianTranslation = false;
        Awake();
    }
}
