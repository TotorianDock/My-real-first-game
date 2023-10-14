using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [Header("Systems & Script")]
    [SerializeField] private MotionSystem _motionSystem;
    [SerializeField] private ShadingSystem _shadingScript;

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

    [Header("Damage & Attack")]
    [SerializeField] private TextMeshProUGUI E_dealsText;
    [SerializeField] private TextMeshProUGUI E_countOfAttackText;
    [SerializeField] private TextMeshProUGUI E_damageText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI E_healsText;
    [SerializeField] private TextMeshProUGUI E_countOfHeal;
    [SerializeField] private TextMeshProUGUI E_hpText;
    [SerializeField] private TextMeshProUGUI E_costTextH;
    [SerializeField] private TextMeshProUGUI E_costOfHealText;
    [SerializeField] private TextMeshProUGUI E_manaTextH;

    [Header("Block")]
    [SerializeField] private TextMeshProUGUI E_blockTextRu;
    [SerializeField] private TextMeshProUGUI E_blockTextEn;
    [SerializeField] private TextMeshProUGUI E_costTextB;
    [SerializeField] private TextMeshProUGUI E_countOfManaTextB;
    [SerializeField] private TextMeshProUGUI E_manaTextB;

    [Header("Information")]

    [Header("Damage")]
    [SerializeField] private TextMeshProUGUI I_dealsText;
    [SerializeField] private TextMeshProUGUI I_dealsDamageText;
    [SerializeField] private TextMeshProUGUI I_damageText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI I_healsText;
    [SerializeField] private TextMeshProUGUI I_healsHealthTextE;
    [SerializeField] private TextMeshProUGUI I_healsHealthTextR;
    [SerializeField] private TextMeshProUGUI I_healthText;

    [Header("Experience")]
    [SerializeField] private TextMeshProUGUI I_givesText;
    [SerializeField] private TextMeshProUGUI I_givesExpText;
    [SerializeField] private TextMeshProUGUI I_expText;

    [Header("Block")]
    [SerializeField] private TextMeshProUGUI I_blockTextE;
    [SerializeField] private TextMeshProUGUI I_blockTextR;

    public void SetHUD(Unit unit, bool isRussianTranslation, bool missingCombatButtons)
    {
        if (_motionSystem._SecondBackground.activeInHierarchy == true)
            _shadingScript.SetShading();
        if (!missingCombatButtons)
        {
            E_countOfAttackText.text = Convert.ToString(unit.damage);
            E_countOfHeal.text = Convert.ToString(unit.countOfHeal);
            E_costOfHealText.text = Convert.ToString(unit.costOfHeal);
            E_countOfManaTextB.text = Convert.ToString(unit.costOfBlock);

            E_countOfAttackText.text = Convert.ToString(unit.damage);
            E_countOfHeal.text = Convert.ToString(unit.countOfHeal);
            E_costOfHealText.text = Convert.ToString(unit.costOfHeal);
            E_countOfManaTextB.text = Convert.ToString(unit.costOfBlock);

            E_countOfAttackText.text = Convert.ToString(unit.damage);
            E_countOfHeal.text = Convert.ToString(unit.countOfHeal);
            E_costOfHealText.text = Convert.ToString(unit.costOfHeal);
            E_countOfManaTextB.text = Convert.ToString(unit.costOfBlock);
        }
        else
        {
            I_dealsDamageText.text = Convert.ToString(unit.damage);
            I_givesExpText.text = Convert.ToString(unit.givesExp);
        }
        if (isRussianTranslation)
        {
            _nameText.text = unit.unitNameRU;
            _levelText.text = "Ур " + unit.unitLevel;
            if (!missingCombatButtons)
            {
                _attackButtonText.text = "Атака";
                _healButtonText.text = "Лечение";
                _blockButtonText.text = "Блок";

                E_dealsText.text = "Наносит";
                E_damageText.text = "урона";

                E_healsText.text = "Лечит";
                E_hpText.text = "ОЗ";
                E_costTextH.text = "Стоит";
                E_manaTextH.text = "маны";

                E_blockTextEn.text = string.Empty;
                E_blockTextRu.text = "Блокирует следующую атаку врага";
                E_costTextB.text = "Стоит";
                E_manaTextB.text = "маны";
            }
            else
            {
                I_healsHealthTextR.text = Convert.ToString(unit.countOfHeal);

                I_dealsText.text = "Наносит";
                I_damageText.text = "урона";

                I_healsText.text = "Лечится на";
                I_healthText.text = "ОЗ";

                I_givesText.text = "Даёт";
                I_expText.text = "ОП";

                I_blockTextE.text = string.Empty;
                I_blockTextR.text = "Ваша следующая атака провалиться!";
            }
        }
        else
        {
            _nameText.text = unit.unitNameENG;
            _levelText.text = "Lvl " + unit.unitLevel;
            if (!missingCombatButtons)
            {
                _attackButtonText.text = "Attack";
                _healButtonText.text = "Heal";
                _blockButtonText.text = "Block";

                E_dealsText.text = "Deals";
                E_damageText.text = "damage";

                E_healsText.text = "Heals";
                E_hpText.text = "HP";
                E_costTextH.text = "Cost";
                E_manaTextH.text = "mana";

                E_blockTextRu.text = string.Empty;
                E_blockTextEn.text = "Blocks next enemy attack";
                E_costTextB.text = "Cost";
                E_manaTextB.text = "mana";
            }
            else
            {
                I_healsHealthTextE.text = Convert.ToString(unit.countOfHeal);

                I_dealsText.text = "Deals";
                I_damageText.text = "damage";

                I_healsText.text = "Heals";
                I_healthText.text = "HP";

                I_givesText.text = "Gives";
                I_expText.text = "Exp";

                I_blockTextR.text = string.Empty;
                I_blockTextE.text = "Your next attack will fail!";
            }
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
            _curentManaText.text = Convert.ToString(unit.curentMana);
            _maxManaText.text = Convert.ToString(unit.maxMana);

            _manaSlider.maxValue = unit.maxMana;
            _manaSlider.value = unit.curentMana;

            if (isRussianTranslation)
                _manaText.text = "Мана";
            else
                _manaText.text = "Mana";
            
            _curentExpText.text = Convert.ToString(unit.curentExp);
            _maxExpText.text = Convert.ToString(unit.maxExp);

            _expSlider.maxValue = unit.maxExp;
            _expSlider.value = unit.curentExp;

            if (isRussianTranslation)
                _expText.text = "ОП";
            else
                _expText.text = "Exp";
        }
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
