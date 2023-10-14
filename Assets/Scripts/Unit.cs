using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Name")]
    public string unitNameENG = "M̵̡͕̱̒̆́̿̕i҈̳̩͈̓̽̓͊̕͜s̴̡̞̱̫̭͎̣͉̆̈̆͞s̶̨̗̝̬̤̮̪̟͇̆̌̆̕i̵͖̠̫̭̊̆͌͢͝n҉̡̙̫̜̞̟̩̤̖͆́̄̌͡g҈̨̠͎̝̝̋̀͌̋̓̆͛͠ͅE҈̧̛̬͓͖̰͋͌͛̎͆͗͌n҈̠̞҇̆̐͜ ";
    public string unitNameRU = "M̵̡͕̱̒̆́̿̕i҈̳̩͈̓̽̓͊̕͜s̴̡̞̱̫̭͎̣͉̆̈̆͞s̶̨̗̝̬̤̮̪̟͇̆̌̆̕i̵͖̠̫̭̊̆͌͢͝n҉̡̙̫̜̞̟̩̤̖͆́̄̌͡g҈̨̠͎̝̝̋̀͌̋̓̆͛͠ͅE҈̧̛̬͓͖̰͋͌͛̎͆͗͌n҈̠̞҇̆̐͜ ";

    [Header("Level")]
    public int unitLevel = 999;

    [Header("Damage")]
    public int damage = 999;

    [Header("Health")]
    public int maxHP = 999;
    public int curentHP = 999;

    [Header("Mana")]
    public int maxMana = -1;
    public int curentMana = -1;

    [Header("Experience")]
    public short maxExp = -1;
    public short curentExp = -1; 
    public short givesExp = 0;

    [Header("Missing")]
    public bool missingManaAndExp = true;
    public bool missingCombatButtons = true;

    [Header("Skils")]
    public int countOfHeal = 999;
    public int costOfHeal = -1;
    public int costOfBlock = -1;

    [HideInInspector] public Animator animator;

    [Header("Next prefab")]
    public GameObject nextPrefab;

    [Header("Enemy name")]
    public EnemyName enemyName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool TakeDamage(int damage)
    {
        curentHP -= damage;

        if (curentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int countOfHeal)
    {
        curentHP += countOfHeal;
        if (curentHP >= maxHP)
            curentHP = maxHP;

    }

    public enum EnemyName
    {
        ThisIsNotTheEnemy = 0,
        Snake = 1,
        Scorpio = 2,
        Hyena = 3,
        Vulture = 4
    }
}
