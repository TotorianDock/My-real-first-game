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
    [SerializeField] private SFXSystem _sfxSystem;

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

    
    [Header("Other")]

    [SerializeField] private GameObject _InstaKillButton;

    [HideInInspector] public bool isRussianTranslation;


    private bool _heroIsBlocking = false;
    private bool _enemyIsBlocking = false;

    [HideInInspector] public Unit heroUnit;
    protected Unit _enemyUnit;

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
        
        if (MenuScript.hero != null)
            _heroPrefab = MenuScript.hero;
        SetupBattle();
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
    private void SetupBattle()
    {
        if (heroUnit.unitLevel < 10)
            _sfxFields.BatleMusic();
        else
            _sfxFields.FinalBatleMusic();
        
        if (state != BattleState.WON)
            StartCoroutine(SBStart());
        else
            StartCoroutine(SBDuringTheAction());
    }
    private IEnumerator SBStart()
    {
        GameObject heroGO = Instantiate(_heroPrefab, _heroBattleStation);
        heroUnit = heroGO.GetComponent<Unit>();

        _heroAnim = heroUnit.animator;
        _heroAnim.CrossFade(AIdle, 0);
        _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);

        GameObject enemyGO = Instantiate(_enemyPrefab, _enemyBattleStation);
        _enemyUnit = enemyGO.GetComponent<Unit>();

        _enemyAnim = _enemyUnit.animator;
        _enemyAnim.CrossFade(AIdle, 0);

        _enemyHUD.SetHUD(_enemyUnit, isRussianTranslation, _enemyUnit.missingCombatButtons);

        StartCoroutine(Run());

        _dialogueSystem.EnemyApproaches(_enemyUnit, isRussianTranslation, 5f);
        yield return new WaitForSeconds(5f);

        PlayerTurn();
    }
    private IEnumerator SBDuringTheAction()
    {
        _enemyPrefab = _prefabsFields.RandomEnemyPrefab();
        _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);
        _motionSystem.ResetEnemyStation(_locationHasChanged);
        Destroy(_enemyUnit.gameObject);

        if (_movementCounter == 2)
        {
            _motionSystem.ResetBackground(_locationHasChanged);
            _movementCounter = 0;
        }

        GameObject enemyGO = Instantiate(_enemyPrefab, _enemyBattleStation);
        _enemyUnit = enemyGO.GetComponent<Unit>();

        _enemyAnim = _enemyUnit.animator;
        _enemyAnim.CrossFade(AIdle, 0);

        _enemyHUD.SetHUD(_enemyUnit, isRussianTranslation, _enemyUnit.missingCombatButtons);

        _movementCounter++;

        StartCoroutine(Run());

        if (_motionSystem._SecondBackground.activeInHierarchy == true && _locationHasChanged == false)
        {
            yield return new WaitForSeconds(11f);
            _dialogueSystem.EnemyApproaches(_enemyUnit, isRussianTranslation, 3f);
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            _dialogueSystem.EnemyApproaches(_enemyUnit, isRussianTranslation, 2.5f);
            yield return new WaitForSeconds(2.5f);
        }

        PlayerTurn();
    }
    public void SetHeroPrefab(short heroCurentExp)
    {
        _heroPrefab = heroUnit.nextPrefab;
        Destroy(heroUnit.gameObject);
        GameObject heroGO = Instantiate(_heroPrefab, _heroBattleStation);
        heroUnit = heroGO.GetComponent<Unit>();
        _prefabsFields.IncreaseEnemiesLevel();

        _heroAnim = heroUnit.animator;
        _heroAnim.CrossFade(AIdle, 0);
        _heroHUD.SetHUD(heroUnit, isRussianTranslation, heroUnit.missingCombatButtons);
        heroUnit.curentExp = heroCurentExp;
        _heroHUD.SetExpAndLvl(heroUnit, isRussianTranslation);
    }
    private void GetExpAndLvl(Unit hero, Unit enemy)
    {
        short receivedExp = enemy.givesExp;
        short heroCurentExp;

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
                    heroCurentExp = receivedExp;
                    SetHeroPrefab(heroCurentExp);
                }
            }
            else
            {
                heroCurentExp = receivedExp;
                SetHeroPrefab(heroCurentExp);
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
        if (_enemyIsBlocking)
        {
            _enemyIsBlocking = false;
            _popupSystemI.object3IsActive = false;

            _heroAnim.CrossFade(AIdle, 0);
            _dialogueSystem.HeroAttack(isRussianTranslation, 1f);
            yield return new WaitForSeconds(1f);
            _enemyAnim.CrossFade(AHurt, 0);
            _sfxFields.FailedAttackSound();
            _dialogueSystem.AttackFailed1(isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);
            _enemyAnim.CrossFade(AIdle, 0);
            _dialogueSystem.AttackFailed2(isRussianTranslation, 2f);
            yield return new WaitForSeconds(2f);

            EnemyTurn(_enemyUnit);
        }
        else
        {
            _enemyAnim.CrossFade(AHurt, 0);
            _sfxFields.HitSound();
            bool isDead = _enemyUnit.TakeDamage(heroUnit.damage);

            _enemyHUD.RemoveHP(_enemyUnit.curentHP, _enemyUnit, isDead);
            _dialogueSystem.AttackSuccessful(isRussianTranslation, 2f);
            yield return new WaitForSeconds(1.1f);
            _enemyAnim.CrossFade(AIdle, 0);
            yield return new WaitForSeconds(0.9f);

            if (isDead)
                StartCoroutine(EnemyDied());
            else
            {
                _heroAnim.CrossFade(AIdle, 0);
                EnemyTurn(_enemyUnit);
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
        EnemyTurn(_enemyUnit);
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
        EnemyTurn(_enemyUnit);
    }

    private void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        _heroIsBlocking = false;
        _heroAnim.CrossFade(AIdle, 0);
        _enemyAnim.CrossFade(AIdle, 0);
        _dialogueSystem.ChooseAnAction(isRussianTranslation, 2f);
        _InstaKillButton.SetActive(true);
    }
    public IEnumerator EnemyDied()
    {
        _enemyUnit.curentHP = 0;
        _enemyHUD.SetHUD(_enemyUnit, isRussianTranslation, _enemyUnit.missingCombatButtons);

        _enemyAnim.CrossFade(ADeath, 0);
        _heroAnim.CrossFade(AIdle, 0);
        _dialogueSystem.EnemyIsDead(_enemyUnit, isRussianTranslation, 2f);
        yield return new WaitForSeconds(2f);
        _enemyAnim.CrossFade(ADied, 0);
        _sfxFields.EnemyDeathSound();
        yield return new WaitForSeconds(0.5f);

        GetExpAndLvl(heroUnit, _enemyUnit);
        if (heroUnit.unitLevel == 99)
        {
            _dialogueSystem.YouWonGame(isRussianTranslation, 7f);
            //StartCoroutine(_sfxSystem.SoundErrors());
            //Time.timeScale = 0.1f;
            yield return new WaitForSeconds(7f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            _dialogueSystem.YouWonBattle(isRussianTranslation, 1f);
            state = BattleState.WON;
            yield return new WaitForSeconds(1f);

            _heroAnim.CrossFade(ARun, 0);

            SetupBattle();
        }
    }
    private IEnumerator EnemyAttack()
    {
        _enemyAnim.CrossFade(AAttack, 0);
        if (_heroIsBlocking)
        {
            _heroIsBlocking = false;

            _dialogueSystem.EnemyAttackFail(_enemyUnit, isRussianTranslation, 2f);
            yield return new WaitForSeconds(1.9f);
            _enemyAnim.CrossFade(AIdle, 0);
            yield return new WaitForSeconds(0.1f);
            _dialogueSystem.YouBlock(isRussianTranslation, 2f);
            _heroAnim.CrossFade(ABlockingAttack, 0);
            _sfxFields.BlockSound();

            yield return new WaitForSeconds(2f);
            _heroAnim.CrossFade(AIdle, 0);

            PlayerTurn();
        }
        else
        {
            _dialogueSystem.EnemyAttackSuccesful(_enemyUnit, isRussianTranslation, 2f);

            bool isDead = heroUnit.TakeDamage(_enemyUnit.damage);
            _heroHUD.RemoveHP(heroUnit.curentHP, heroUnit, isDead);
            _heroAnim.CrossFade(AHurt, 0);
            _sfxFields.HitSound();

            yield return new WaitForSeconds(1.1f);
            _heroAnim.CrossFade(AIdle, 0);
            _enemyAnim.CrossFade(AIdle, 0);
            yield return new WaitForSeconds(0.9f);
            
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
                PlayerTurn();
        }
    }
    private IEnumerator EnemyHeal()
    {
        _dialogueSystem.EnemyHealed1(_enemyUnit, isRussianTranslation, 2f);
        yield return new WaitForSeconds(2f);
        _dialogueSystem.EnemyHealed2(isRussianTranslation, _enemyUnit, 2f);
        
        _enemyAnim.CrossFade(AHeal, 0);
        _sfxFields.HealSound();
        _enemyUnit.Heal(_enemyUnit.countOfHeal);
        _enemyHUD.AddHP(_enemyUnit.curentHP, _enemyUnit);
        yield return new WaitForSeconds(1f);
        _enemyAnim.CrossFade(AIdle, 0);

        yield return new WaitForSeconds(1f);
        PlayerTurn();
    }
    private IEnumerator EnemyBlock()
    {
        _enemyIsBlocking = true;
        _popupSystemI.object3IsActive = true;

        _dialogueSystem.EnemyReadyToBlock1(_enemyUnit, isRussianTranslation, 2.5f);
        yield return new WaitForSeconds(2.5f);
        _dialogueSystem.EnemyReadyToBlock2(isRussianTranslation, _enemyUnit, 2.5f);

        _sfxFields.DebuffSound();
        yield return new WaitForSeconds(2.5f);

        PlayerTurn();
    }
    private void EnemyTurn(Unit enemyUnit)
    {
        state = BattleState.ENEMYTURN;
        _enemyIsBlocking = false;
        ChoiseOfAction choiseOfAction = EnemyAI(enemyUnit);
        
        switch (choiseOfAction)
        {
            case ChoiseOfAction.Attack:
                _didEnemyBlock = true;
                _didEnemyHeal = false;
                StartCoroutine(EnemyBlock());
                break;
            
            case ChoiseOfAction.Heal:
                _didEnemyBlock = false;
                _didEnemyHeal = false;
                StartCoroutine(EnemyAttack());
                break;
            
            case ChoiseOfAction.Block:
                _didEnemyHeal = true;
                _didEnemyBlock = false;
                StartCoroutine(EnemyHeal());
                break;
            
            default:
                throw new System.Exception();
        }
    }
    private ChoiseOfAction EnemyAI(Unit enemyUnit)
    {
        System.Random random = new();

        if ((enemyUnit.curentHP == enemyUnit.maxHP || enemyUnit.countOfHeal >= heroUnit.damage) && !_didEnemyBlock)
        {
            return (ChoiseOfAction)random.Next((int)ChoiseOfAction.Attack, (int)ChoiseOfAction.Heal + 1);
        }
        else if (_didEnemyHeal && enemyUnit.countOfHeal < heroUnit.damage)
        {
            return (ChoiseOfAction)random.Next((int)ChoiseOfAction.Attack, (int)ChoiseOfAction.Block + 1);
        }
        else if (_didEnemyBlock)
        {
            if (enemyUnit.countOfHeal >= heroUnit.damage || enemyUnit.curentHP == enemyUnit.maxHP)
                return ChoiseOfAction.Heal;
            else
                return (ChoiseOfAction)random.Next((int)ChoiseOfAction.Heal, (int)ChoiseOfAction.Block + 1);
        }
        else
            return (ChoiseOfAction)random.Next((int)ChoiseOfAction.Attack, (int)ChoiseOfAction.Block + 1);
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
public enum ChoiseOfAction
{
    Attack,
    Heal,
    Block
}