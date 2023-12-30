using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [Header("Systems & Script")]
    [SerializeField] private MotionSystem _motionSystem;
    [SerializeField] private ShadingSystem _shadingScript;

    [Header("HUDs")]

    [Header("Name & Level")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI _curentHPText;
    [SerializeField] private TextMeshProUGUI _maxHPText;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _HPText;

    [Header("Mana")]
    [SerializeField] private TextMeshProUGUI _curentManaText;
    [SerializeField] private TextMeshProUGUI _maxManaText;
    [SerializeField] private Slider _manaSlider;
    [SerializeField] private TextMeshProUGUI _manaText;

    [Header("Experience")]
    [SerializeField] private TextMeshProUGUI _curentExpText;
    [SerializeField] private TextMeshProUGUI _maxExpText;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _expText;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI _attackButtonText;
    [SerializeField] private TextMeshProUGUI _healButtonText;
    [SerializeField] private TextMeshProUGUI _blockButtonText;

    [Header("Explanation")]

    [Header("Attack Button")]
    [SerializeField] private TextMeshProUGUI E_attackButton;

    [Header("Heal Button")]
    [SerializeField] private TextMeshProUGUI E_healButton;
    [SerializeField] private TextMeshProUGUI E_healButton_Cost;

    [Header("Block Button")]
    [SerializeField] private TextMeshProUGUI E_blockTextRu;
    [SerializeField] private TextMeshProUGUI E_blockTextEn;
    [SerializeField] private TextMeshProUGUI E_blockButton_Cost;

    [Header("Information About")]

    [Header("Block")]
    [SerializeField] private TextMeshProUGUI I_blockTextE;
    [SerializeField] private TextMeshProUGUI I_blockTextR;

    [Header("Dmg & Healing & Exp")]
    [SerializeField] private TextMeshProUGUI InformationAboutDmg;
    [SerializeField] private TextMeshProUGUI InformationAboutHealing;
    [SerializeField] private TextMeshProUGUI InformationAboutExp;

    public void SetHUD(Unit unit, bool isRussianTranslation, bool missingCombatButtons)
    {
        if (_motionSystem._SecondBackground.activeInHierarchy == true)
            _shadingScript.SetShading();
        if (isRussianTranslation)
        {
            _nameText.text = unit.unitNameRU;
            _levelText.text = "Ур " + unit.unitLevel;
            
            if (missingCombatButtons)
                SetEnemyInformationRus(unit);
            else
                SetBattleButtonsRus(unit);
        }
        else
        {
            _nameText.text = unit.unitNameENG;
            _levelText.text = "Lvl " + unit.unitLevel;
            
            if (missingCombatButtons)
                SetEnemyInformationEng(unit);
            else
                SetBattleButtonsEng(unit);
        }

        _curentHPText.text = Convert.ToString(unit.curentHP);
        _maxHPText.text = Convert.ToString(unit.maxHP);

        _hpSlider.maxValue = unit.maxHP;
        _hpSlider.value = unit.curentHP;

        if (isRussianTranslation)
            _HPText.text = "ОЗ";
        else
            _HPText.text = "HP";

        if (!unit.missingManaAndExp)
        {
            if (isRussianTranslation)
            {
                _manaText.text = "Мана";
                _expText.text = "ОП";
            }
            else
            {
                _manaText.text = "Mana";
                _expText.text = "Exp";
            }

            _curentManaText.text = Convert.ToString(unit.curentMana);
            _maxManaText.text = Convert.ToString(unit.maxMana);

            _manaSlider.maxValue = unit.maxMana;
            _manaSlider.value = unit.curentMana;

            _curentExpText.text = Convert.ToString(unit.curentExp);
            _maxExpText.text = Convert.ToString(unit.maxExp);

            _expSlider.maxValue = unit.maxExp;
            _expSlider.value = unit.curentExp;
        }
    }
    private void SetBattleButtonsRus(Unit unit)
    {
        _attackButtonText.text = "Атака";
        _healButtonText.text = "Лечение";
        _blockButtonText.text = "Блок";

        E_attackButton.text = unit.damage switch
        {
            >= 100 => $"Наносит {unit.damage} урона",
            >= 10 => $"Наносит {unit.damage} урона",
            < 10 => $"Наносит  {unit.damage} урона"
        };

        E_healButton.text = $"Лечит {unit.countOfHeal} ОЗ";
        E_healButton_Cost.text = unit.costOfHeal switch
        {
            >= 10 => $"Стоит {unit.costOfHeal} маны",
            < 10 => $"Стоит  {unit.costOfHeal} маны"
        };

        E_blockTextEn.text = string.Empty;
        E_blockTextRu.text = "Блокирует следующую атаку врага";

        E_blockButton_Cost.text = unit.costOfBlock switch
        {
            >= 10 => $"Стоит {unit.costOfBlock} маны",
            < 10 => $"Стоит {unit.costOfBlock} маны"
        };
    }
    private void SetBattleButtonsEng(Unit unit)
    {
        _attackButtonText.text = "Attack";
        _healButtonText.text = "Heal";
        _blockButtonText.text = "Block";

        E_attackButton.text = unit.damage switch
        {
            >= 100 => $"Deals  {unit.damage} damage",
            >= 10 => $"Deals  {unit.damage} damage",
            < 10 => $"Deals   {unit.damage} damage"
        };

        E_healButton.text = $"Heals {unit.countOfHeal} HP";
        E_healButton_Cost.text = unit.costOfHeal switch
        {
            >= 10 => $"Cost  {unit.costOfHeal} mana",
            < 10 => $"Cost   {unit.costOfHeal} mana"
        };

        E_blockTextRu.text = string.Empty;
        E_blockTextEn.text = "Blocks next enemy attack";

        E_blockButton_Cost.text = unit.costOfBlock switch
        {
            >= 10 => $"Cost {unit.costOfBlock} mana",
            < 10 => $"Cost  {unit.costOfBlock} mana"
        };
    }
    private void SetEnemyInformationRus(Unit unit)
    {
        InformationAboutExp.text = unit.givesExp switch
        {
            >= 10 => $"Падает  {unit.givesExp}  ОП",
            < 10 => $"Падает  {unit.givesExp}   ОП"
        };
        InformationAboutHealing.text = unit.countOfHeal switch
        {
            >= 100 => $" Лечится на {unit.countOfHeal} ОЗ",
            >= 10 => $" Лечится на {unit.countOfHeal} ОЗ",
            < 10 => $" Лечится на {unit.countOfHeal} ОЗ"
        };
        InformationAboutDmg.text = unit.damage switch
        {
            >= 100 => $"Наносит {unit.damage} урона",
            >= 10 => $"Наносит {unit.damage} урона",
            < 10 => $"Наносит  {unit.damage}  урона"
        };

        I_blockTextE.text = string.Empty;
        I_blockTextR.text = "Ваша следующая атака провалиться!";
    }
    private void SetEnemyInformationEng(Unit unit)
    {
        InformationAboutExp.text = unit.givesExp switch
        {
            >= 100 => $"Gives {unit.givesExp}  Exp",
            >= 10 => $"Gives  {unit.givesExp}  Exp",
            < 10 => $"Gives   {unit.givesExp}  Exp"
        };
        InformationAboutHealing.text = unit.countOfHeal switch
        {
            >= 100 => $" Heals {unit.countOfHeal}  HP",
            >= 10 => $" Heals  {unit.countOfHeal}  HP",
            < 10 => $" Heals   {unit.countOfHeal}  HP"
        };
        InformationAboutDmg.text = unit.damage switch
        {
            >= 100 => $"Deals {unit.damage} damage",
            >= 10 => $"Deals  {unit.damage} damage",
            < 10 => $"Deals  {unit.damage}  damage"
        };

        I_blockTextR.text = string.Empty;
        I_blockTextE.text = "Your next attack will fail!";
    }
    public void RemoveHP(int hp, Unit unit, bool isDead)
    {
        if (isDead)
        {
            _hpSlider.value = 0;
            _curentHPText.text = "0";
        }
        else
        {
            _hpSlider.value = hp;
            _curentHPText.text = Convert.ToString(unit.curentHP);
        }
    }
    public void AddHP(int hp, Unit unit)
    {
        _hpSlider.value = hp;
        _curentHPText.text = Convert.ToString(unit.curentHP);
    }
    public void RemoveMana(int removedMana, Unit hero)
    {
        if(removedMana == hero.curentMana)
        {
            hero.curentMana = 0;
            _manaSlider.value = 0;
            _curentManaText.text = "0";
        }
        else
        {
            hero.curentMana -= removedMana;
            _manaSlider.value -= removedMana;
            _curentManaText.text = Convert.ToString(hero.curentMana);
        }
    }
    public void SetExpAndLvl(Unit hero, bool isRussianTranslation)
    {
        _curentExpText.text = Convert.ToString(hero.curentExp);
        _maxExpText.text = Convert.ToString(hero.maxExp);

        _expSlider.maxValue = hero.maxExp;
        _expSlider.value = hero.curentExp;

        if(isRussianTranslation)
            _levelText.text = "Ур " + hero.unitLevel;
        else
            _levelText.text = "Lvl " + hero.unitLevel;
    }
}
