using UnityEngine;

public class PrefabsFields : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private MotionSystem _motionSystem;

    [Header("Prefabs")]
    [SerializeField] private GameObject _snakePrefab;
    private Unit _snakeUnit;

    [SerializeField] private GameObject _scorpioPrefab;
    private Unit _scorpioUnit;

    [SerializeField] private GameObject _hyenaPrefab;
    private Unit _hyenaUnit;

    [SerializeField] private GameObject _vulturePrefab;
    //private Unit _vultureUnit;

    protected Unit _heroUnit;


    private int _enemysLevel = 1;
    private bool _isEnemyDifficult = false;

    private void Awake()
    {
        _snakeUnit = _snakePrefab.GetComponent<Unit>();
        _scorpioUnit = _scorpioPrefab.GetComponent<Unit>();
        _hyenaUnit = _hyenaPrefab.GetComponent<Unit>();
        //_vultureUnit = _vulturePrefab.GetComponent<Unit>();
    }
    private void Start()
    {
        _heroUnit = _battleSystem.heroUnit;
    }
    private void LevelUpAllPrefabsOfFirstStage()
    {
        _snakePrefab = _snakeUnit.nextPrefab;
        _scorpioPrefab = _scorpioUnit.nextPrefab;
        _hyenaPrefab = _hyenaUnit.nextPrefab;
    }
    public GameObject RandomEnemyPrefab(Unit heroUnit)
    {
        System.Random random = new();
        EnemiesRegistry enemysRegistry;
        int prefabRandomiser = 0;

        if (heroUnit.unitLevel <= 5)
        {
            short isEnemyDifficult = (short)random.Next(0, 2);
            if (isEnemyDifficult == 1)
                _isEnemyDifficult = true;
            else
                _isEnemyDifficult = false;
            
            if (_isEnemyDifficult == true)
                enemysRegistry = EnemiesRegistry.Hyena;
            else
                enemysRegistry = (EnemiesRegistry)random.Next(0, 2);

            Start();

            if (_enemysLevel < heroUnit.unitLevel && heroUnit.unitLevel < 6)
            {
                LevelUpAllPrefabsOfFirstStage();
                _enemysLevel++;
                    
                Awake();
            }
        }
        else if (heroUnit.unitLevel == 6)
        {
            _motionSystem.SetSecondBackground();

            //short isEnemyDifficult = (short)random.Next(0, 2);
            //if (isEnemyDifficult == 0)
            //    _isEnemyDifficult = true;
            //else
            //    _isEnemyDifficult = false;

            //if (_isEnemyDifficult == true)
            //    enemysRegistry = EnemiesRegistry.Hyena;
            //else
            //    enemysRegistry = (EnemiesRegistry)random.Next(0, 2);
            
            enemysRegistry = (EnemiesRegistry)random.Next(3, 4);

            Start();

            //if (_enemysLevel < heroUnit.unitLevel && heroUnit.unitLevel < 6)
            //{
            //    _vulturePrefab = _vultureUnit.nextPrefab;
            //    _enemysLevel++;
            //    Awake();
            //}
        }
        else
            enemysRegistry = EnemiesRegistry.Snake;

        if (_enemysLevel == 1)
            prefabRandomiser = random.Next(0, 2);

        switch (enemysRegistry)
        {
            case EnemiesRegistry.Snake:
                return _snakePrefab;

            case EnemiesRegistry.Scorpio:
                if (prefabRandomiser == 1)
                    return _scorpioPrefab;
                else
                    return _scorpioUnit.nextPrefab;

            case EnemiesRegistry.Hyena:
                if (prefabRandomiser == 1)
                    return _hyenaPrefab;
                else
                    return _hyenaUnit.nextPrefab;

            case EnemiesRegistry.Vulture:
                return _vulturePrefab;

            default:
                return _snakePrefab;
        }
    }
    private enum EnemiesRegistry
    {
        Snake = 0,
        Scorpio = 1,
        Hyena = 2,
        Vulture = 3,
        Mummy = 4,
        Deceased = 5
    }
}
