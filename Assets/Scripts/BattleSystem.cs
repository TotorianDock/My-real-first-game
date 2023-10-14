using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    [Header("System & Fields")]
    [SerializeField] private PrefabsFields _prefabsFields;
    [SerializeField] private MotionSystem _motionSystem;
    [SerializeField] private DialogueSystem _dialogueSystem;
    [SerializeField] private SFXFields_Game _sfxFields;

    [Header("Popup Systems")]
    [SerializeField] private PopupSystem _popupSystemA;
    [SerializeField] private PopupSystem _popupSystemB;
    [SerializeField] private PopupSystem _popupSystemH;
    [SerializeField] private PopupSystem _popupSystemI;

    [Header("Prefabs")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _heroPrefab;

    [Header("Battle stations")]
    [SerializeField] private Transform _enemyBattleStation;
    [SerializeField] private Transform _heroBattleStation;

    [Header("HUDs")]
    [SerializeField] private BattleHud _heroHUD;
    [SerializeField] private BattleHud _enemyHUD;

    [Space]
    public bool isRussianTranslation = false;

    
    private bool _heroIsBlocking = false;
    private bool _enemyIsBlocking = false;

    [HideInInspector] public Unit heroUnit;
    protected Unit enemyUnit;

    private Animator _heroAnim;
    private Animator _enemyAnim;

    private readonly int AIdle = Animator.StringToHash("Idle");
    private readonly int ADeath = Animator.StringToHash("Death");
    private readonly int AHurt = Animator.StringToHash("Hurt");
    private readonly int AHeal = Animator.StringToHash("Heal");
    private readonly int ARun = Animator.StringToHash("Run");
    private readonly int AAttack = Animator.StringToHash("Attack");
    private readonly int ABlockIdle = Animator.StringToHash("BlockIdle");
    private readonly int ABlockingAttack = Animator.StringToHash("BlockingAttack");
    private readonly int ADied = Animator.StringToHash("Died");

    [HideInInspector] public BattleState state = BattleState.START;
    
    private bool _didEnemyBlock = false;
    private bool _didEnemyHeal = false;

    private byte _movementCounter = 0;

    protected bool _locationHasChanged = false;

    private void Awake()
    {
        isRussianTranslation = MenuScript.isRussianTranslation;
        StartCoroutine(SetupBattle());
    }
    private IEnumerator Run()
    {
        _heroAnim.CrossFade(ARun, 0);
        _enemyAnim.CrossFade(ARun, 0);
        _motionSystem.isMoving = true;
        if (_motionSystem._SecondBackground.activeInHierarchy == true && _locationHasChanged == false)
        {
            _dialogueSystem.LocationChange(isRussianTranslation, 12f);
            yield return new WaitForSeconds(12f);
            _motionSystem.UnsetFirstBackground();
            _locationHasChanged = true;
        }
        else
            yield return new WaitForSeconds(2f);
        _motionSystem.isMoving = false;
        _enemyAnim.CrossFade(AIdle, 0);
        _heroAnim.CrossFade(AIdle, 0);
    }
    private IEnumerator SetupBattle()
    {
        if(heroUnit.unitLevel != 6)
            _sfxFields.BatleMusic();
        else
            _sfxFields.FinalBatleMusic();
        
        if (state != BattleState.WON)
        {
            GameObject heroGO = Instantiate(_heroPrefab, _heroBattleStation);
            heroUnit = heroGO.GetComponent<Unit>();

            _heroAnim = heroUnit.animator;
            _heroAnim.CrossFade(AIdle, 0);
            _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);

            GameObject enemyGO = Instantiate(_enemyPrefab, _enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit>();

            _enemyAnim = enemyUnit.animator;
            _enemyAnim.CrossFade(AIdle, 0);

            _enemyHUD.SetHUD(enemyUnit, isRussianTranslation, enemyUnit.missingCombatButtons);

            StartCoroutine(Run());

            _dialogueSystem.EnemyApproaches(enemyUnit, isRussianTranslation, 5f);
            yield return new WaitForSeconds(5f);

            PlayerTurn();
        }
        else
        {
            _enemyPrefab = _prefabsFields.RandomEnemyPrefab(heroUnit);
            _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);
            _motionSystem.ResetEnemyStation(_locationHasChanged);
            Destroy(enemyUnit.gameObject);

            if(_movementCounter == 2)
            {
                _motionSystem.ResetBackground(_locationHasChanged);
                _movementCounter = 0;
            }
            
            GameObject enemyGO = Instantiate(_enemyPrefab, _enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit>();

            _enemyAnim = enemyUnit.animator;
            _enemyAnim.CrossFade(AIdle, 0);

            _enemyHUD.SetHUD(enemyUnit, isRussianTranslation, enemyUnit.missingCombatButtons);

            _movementCounter++;

            StartCoroutine(Run());

            if (_motionSystem._SecondBackground.activeInHierarchy == true && _locationHasChanged == false)
            {
                yield return new WaitForSeconds(11f);
                _dialogueSystem.EnemyApproaches(enemyUnit, isRussianTranslation, 3f);
                yield return new WaitForSeconds(3f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
                _dialogueSystem.EnemyApproaches(enemyUnit, isRussianTranslation, 2.5f);
                yield return new WaitForSeconds(2.5f);
            }
            
            PlayerTurn();
        }
    }
    public void SetHeroPrefab(short HCE)
    {
        _heroPrefab = heroUnit.nextPrefab;
        Destroy(heroUnit.gameObject);
        GameObject heroGO = Instantiate(_heroPrefab, _heroBattleStation);
        heroUnit = heroGO.GetComponent<Unit>();

        _heroAnim = heroUnit.animator;
        _heroAnim.CrossFade(AIdle, 0);
        _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);
        heroUnit.curentExp = HCE;
        _heroHUD.SetExpAndLvl(heroUnit, isRussianTranslation);
    }
    public void GetExpAndLvl(Unit hero, Unit enemy)
    {
        short receivedExp = enemy.givesExp;

        if (receivedExp >= hero.maxExp - hero.curentExp)
        {
            hero.unitLevel++;
            receivedExp -= (short)(hero.maxExp - hero.curentExp);
            if (receivedExp > hero.maxExp)
            {
                while (receivedExp > hero.maxExp)
                {
                    hero.unitLevel++;
                    receivedExp -= hero.maxExp;
                    short HCE = receivedExp;
                    SetHeroPrefab(HCE);
                }
            }
            else
            {
                short HCE = receivedExp;
                SetHeroPrefab(HCE);
            }
        }
        else
        {
            hero.curentExp += receivedExp;
            _heroHUD.SetExpAndLvl(heroUnit, isRussianTranslation);
        }
        _sfxFields.LevelUpSound();
    }

    private IEnumerator HeroAttack()
    {
        state = BattleState.ACTIONTIME; 
        _heroAnim.CrossFade(AAttack, 0);
        _heroAnim.CrossFade(AIdle, 0);
        if (_enemyIsBlocking)
        {
            _enemyIsBlocking = false;
            _popupSystemI.object3IsActive = false;

            _dialogueSystem.HeroAttack(isRussianTranslation, 1f);
            yield return new WaitForSeconds(1f);
            _enemyAnim.CrossFade(AHurt, 0);
            _sfxFields.HitSound();
            _dialogueSystem.AttackFailed1(isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);
            _enemyAnim.CrossFade(AIdle, 0);
            _dialogueSystem.AttackFailed2(isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);

            EnemyTurnAndEnemyAI(enemyUnit);
        }
        else
        {
            _enemyAnim.CrossFade(AHurt, 0);
            _sfxFields.HitSound();
            bool isDead = enemyUnit.TakeDamage(heroUnit.damage);

            _enemyHUD.RemoveHP(enemyUnit.curentHP, enemyUnit, isDead);
            _dialogueSystem.AttackSuccessful(isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                _enemyAnim.CrossFade(ADeath, 0);
                _heroAnim.CrossFade(AIdle, 0);
                _dialogueSystem.EnemyIsDead(enemyUnit, isRussianTranslation, 2f);
                yield return new WaitForSeconds(2f);
                _enemyAnim.CrossFade(ADied, 0);
                _sfxFields.EnemyDeathSound();
                yield return new WaitForSeconds(0.5f);

                GetExpAndLvl(heroUnit, enemyUnit);
                if (heroUnit.unitLevel == 99)
                {
                    _dialogueSystem.YouWonGame(isRussianTranslation, 7f);
                    yield return new WaitForSeconds(7f);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
                else
                {
                    _dialogueSystem.YouWonBattle(isRussianTranslation, 1f);
                    state = BattleState.WON;
                    yield return new WaitForSeconds(1f);

                    _heroAnim.CrossFade(ARun, 0);

                    StartCoroutine(SetupBattle());
                }
            }
            else
            {
                _enemyAnim.CrossFade(AIdle, 0);
                _heroAnim.CrossFade(AIdle, 0);
                EnemyTurnAndEnemyAI(enemyUnit);
            }
        }
    }
    private IEnumerator HeroHeal()
    {
        state = BattleState.ACTIONTIME;
        heroUnit.Heal(heroUnit.countOfHeal);
        _heroAnim.CrossFade(AHeal, 0);
        _sfxFields.HealSound();
        _heroAnim.CrossFade(AIdle, 0);

        _heroHUD.AddHP(heroUnit.curentHP, heroUnit);
        _heroHUD.RemoveMana(heroUnit.costOfHeal , heroUnit);
        _dialogueSystem.YouHealed(isRussianTranslation, 2f);

        yield return new WaitForSeconds(2f);
        EnemyTurnAndEnemyAI(enemyUnit);
    }
    private IEnumerator HeroBlock()
    {
        state = BattleState.ACTIONTIME;
        _heroIsBlocking = true;
        
        _heroHUD.RemoveMana(heroUnit.costOfBlock, heroUnit);
        _dialogueSystem.YouReady(isRussianTranslation, 2f);
        _sfxFields.ShieldUpSound();
        _heroAnim.CrossFade(ABlockIdle, 0);

        yield return new WaitForSeconds(2f);
        EnemyTurnAndEnemyAI(enemyUnit);
    }

    private void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        _heroIsBlocking = false;
        _heroAnim.CrossFade(AIdle, 0);
        _enemyAnim.CrossFade(AIdle, 0);
        _dialogueSystem.ChooseAnAction(isRussianTranslation, 2f);
    }
    private IEnumerator EnemyAttack()
    {
        _enemyAnim.CrossFade(AAttack, 0);
        if (_heroIsBlocking)
        {
            _heroIsBlocking = false;

            _dialogueSystem.EnemyAttackFail(enemyUnit, isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);
            _enemyAnim.CrossFade(AIdle, 0);
            _dialogueSystem.YouBlock(isRussianTranslation, 2f);
            _heroAnim.CrossFade(ABlockingAttack, 0);
            _sfxFields.BlockSound();

            yield return new WaitForSeconds(2f);
            _heroAnim.CrossFade(AIdle, 0);

            PlayerTurn();
        }
        else
        {
            _dialogueSystem.EnemyAttackSuccesful(enemyUnit, isRussianTranslation, 2f);

            bool isDead = heroUnit.TakeDamage(enemyUnit.damage);
            _heroHUD.RemoveHP(heroUnit.curentHP, heroUnit, isDead);
            _heroAnim.CrossFade(AHurt, 0);
            _sfxFields.HitSound();

            yield return new WaitForSeconds(2f);
            _heroAnim.CrossFade(AIdle, 0);
            _enemyAnim.CrossFade(AIdle, 0);

            if (isDead)
            {
                _heroAnim.CrossFade(ADeath, 0);
                _sfxFields.HeroDeathSound();
                _dialogueSystem.YouIsDead(isRussianTranslation, 2f);
                yield return new WaitForSeconds(2f);

                _dialogueSystem.YouLost(isRussianTranslation, 3f);
                
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            else
            {
                PlayerTurn();
            }
        }
    }
    private IEnumerator EnemyHeal()
    {
        _dialogueSystem.EnemyHealed1(enemyUnit, isRussianTranslation, 2f);
        yield return new WaitForSeconds(2f);
        
        _dialogueSystem.EnemyHealed2(isRussianTranslation, enemyUnit, 2f);
        
        _enemyAnim.CrossFade(AHeal, 0);
        _sfxFields.HealSound();
        enemyUnit.Heal(enemyUnit.countOfHeal);
        _enemyHUD.AddHP(enemyUnit.curentHP, enemyUnit);
        _enemyAnim.CrossFade(AIdle, 0);

        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }
    private IEnumerator EnemyBlock()
    {
        _enemyIsBlocking = true;
        _popupSystemI.object3IsActive = true;

        _dialogueSystem.EnemyReadyToBlock1(enemyUnit, isRussianTranslation, 2.5f);
        yield return new WaitForSeconds(2.5f);
        _dialogueSystem.EnemyReadyToBlock2(isRussianTranslation, enemyUnit, 2.5f);

        _sfxFields.DebuffSound();
        yield return new WaitForSeconds(2.5f);

        PlayerTurn();
    }
    private void EnemyTurnAndEnemyAI(Unit enemyUnit)
    {
        state = BattleState.ENEMYTURN;
        _enemyIsBlocking = false;
        int AICounter;
        System.Random random = new();

        if ((enemyUnit.curentHP == enemyUnit.maxHP || enemyUnit.countOfHeal >= heroUnit.damage) && !_didEnemyBlock)
        {
            AICounter = random.Next(0, 2);
        }
        else if (_didEnemyHeal && enemyUnit.countOfHeal < heroUnit.damage)
        {
            AICounter = random.Next(0, 3);
        }
        else if (_didEnemyBlock)
        {
            if (enemyUnit.countOfHeal >= heroUnit.damage || enemyUnit.curentHP == enemyUnit.maxHP)
                AICounter = 1;
            else
                AICounter = random.Next(1, 3);
        }
        else
            AICounter = random.Next(0, 3);

        switch (AICounter)
        {
            case 0:
                _didEnemyBlock = true;
                _didEnemyHeal = false;
                StartCoroutine(EnemyBlock());
                break;
            
            case 1:
                _didEnemyBlock = false;
                _didEnemyHeal = false;
                StartCoroutine(EnemyAttack());
                break;
            
            case 2:
                _didEnemyHeal = true;
                _didEnemyBlock = false;
                StartCoroutine(EnemyHeal());
                break;
            
            default:
                throw new System.Exception();
        }
    }
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        _popupSystemI.object3IsActive = false;
        StartCoroutine(HeroAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN || heroUnit.curentMana < heroUnit.costOfHeal || heroUnit.curentHP == heroUnit.maxHP)
            return;

        _popupSystemI.object3IsActive = false;
        StartCoroutine(HeroHeal());
    }
    public void OnBlockButton()
    {
        if (state != BattleState.PLAYERTURN || heroUnit.curentMana < heroUnit.costOfBlock)
            return;

        _popupSystemI.object3IsActive = false;
        StartCoroutine(HeroBlock());
    }
}
public enum BattleState
{
    START,
    PLAYERTURN,
    ACTIONTIME,
    ENEMYTURN,
    WON
}