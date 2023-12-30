using TMPro;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [Header("Systems & fields")]
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private SFXSystem _sfxSystem;
    [SerializeField] private SFXFields_Game _sfxFields;
    [SerializeField] private PrefabsFields _prefabsFields;

    [Header("Other")]
    [SerializeField] private GameObject _debugPanel;
    [SerializeField] private BattleHud _heroHUD;
    [SerializeField] private GameObject _debugButton;
    [SerializeField] private TextMeshProUGUI _muteText;
    
    private string[] cheatCode;
    private int _index;
    private bool _isMute = false;
    private float _musicVolume = 0f;

    void Start()
    {
        cheatCode = new string[] { "i", "l", "o", "v", "e", "b", "o", "y", "s" };
        _index = 0;
    }

    void Update()
    {
        if (_index == cheatCode.Length)
        {
            _debugButton.SetActive(true);
            _index = 0;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCode[_index]))
                _index++;
            else
                _index = 0;
        }       
    }
    public void OnDebugButton(bool isPressed)
    {
        if (_debugPanel.activeInHierarchy == false)
            _debugPanel.SetActive(isPressed);
        else
            _debugPanel.SetActive(!isPressed);
    }
    public void LevelUpHero()
    {
        _battleSystem.SetHeroPrefab(_battleSystem.heroUnit.curentExp);
    }
    public void OnRestockManaButton()
    {
        _battleSystem.heroUnit.curentMana = _battleSystem.heroUnit.maxMana;
        _heroHUD.SetHUD(_battleSystem.heroUnit, _battleSystem.isRussianTranslation, _battleSystem.heroUnit.missingCombatButtons);
    }
    public void OnRestockHealthButton()
    {
        _battleSystem.heroUnit.curentHP = _battleSystem.heroUnit.maxHP;
        _heroHUD.SetHUD(_battleSystem.heroUnit, _battleSystem.isRussianTranslation, _battleSystem.heroUnit.missingCombatButtons);
    }
    public void OnInstaKillButton()
    {
        _sfxFields.HitSound();
        StartCoroutine(_battleSystem.EnemyDied());
    }
    public void OnMuteButton()
    {
        if (!_isMute)
        {
            SFXSystem.MainVolume = 0f;
            _musicVolume = _sfxSystem.ResetMusic(_musicVolume);
            _isMute = true;
            _muteText.text = "Unmute";
        }
        else
        {
            SFXSystem.MainVolume = 1f;
            _musicVolume = _sfxSystem.ResetMusic(_musicVolume);
            _isMute = false;
            _muteText.text = "Mute";
        }
    }
}
