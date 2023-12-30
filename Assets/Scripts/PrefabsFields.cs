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
    private Unit _vultureUnit;

    [SerializeField] private GameObject _mummyPrefab;
    private Unit _mummyUnit;

    [SerializeField] private GameObject _deceasedPrefab;
    private Unit _deceasedUnit;

    protected Unit _heroUnit;

    [Header("Wizard")]
    [SerializeField] private GameObject _wizardPrefab;

    [Header("???")]
    [SerializeField] private GameObject _missingEn;

    private int _enemiesLevel = 1;
    private int prefabRandomiser = 0;
    private EnemiesRegistry enemiesRegistry;
    private readonly System.Random random = new();

    private void Awake()
    {
        _snakeUnit = _snakePrefab.GetComponent<Unit>();
        _scorpioUnit = _scorpioPrefab.GetComponent<Unit>();
        _hyenaUnit = _hyenaPrefab.GetComponent<Unit>();
        _vultureUnit = _vulturePrefab.GetComponent<Unit>();
        _mummyUnit = _mummyPrefab.GetComponent<Unit>();
        _deceasedUnit = _deceasedPrefab.GetComponent<Unit>();
    }
    private void Start()
    {
        _heroUnit = _battleSystem.heroUnit;
    }
    public GameObject RandomEnemyPrefab()
    {
        switch (_enemiesLevel)
        {
            case <= 5:
                if (RandomDifficult() == true)
                    enemiesRegistry = EnemiesRegistry.Hyena;
                else
                    enemiesRegistry = (EnemiesRegistry)random.Next((int)EnemiesRegistry.Snake, (int)EnemiesRegistry.Scorpio + 1);

                Start();
                break;

            case <= 10:
                _motionSystem.SetSecondBackground();

                if (RandomDifficult() == true)
                    enemiesRegistry = EnemiesRegistry.Deceased;
                else
                    enemiesRegistry = (EnemiesRegistry)random.Next((int)EnemiesRegistry.Vulture, (int)EnemiesRegistry.Mummy + 1);

                Start();
                break;

            case 11:
                enemiesRegistry = EnemiesRegistry.Wizard;
                Start();
                break;

            default:
                enemiesRegistry = EnemiesRegistry.MissingEn;
                break;
        }
        
        return GetEnemyPrefab();
    }

    private GameObject GetEnemyPrefab()
    { 
        if (_enemiesLevel == 1)
            prefabRandomiser = random.Next(0, 2);
        else
            prefabRandomiser = 1;

        switch (enemiesRegistry)
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

            case EnemiesRegistry.Mummy: 
                return _mummyPrefab;

            case EnemiesRegistry.Deceased:
                return _deceasedPrefab;

            case EnemiesRegistry.Wizard:
                return _wizardPrefab;

            default:
                return _missingEn;
        }
    }
    private bool RandomDifficult()
    {
        if ((short)random.Next(0, 2) == 0)
            return true;
        else
            return false;
    }
    public void IncreaseEnemiesLevel()
    {
        _enemiesLevel++;
        if (_motionSystem._SecondBackground.activeInHierarchy == false)
        {
            _snakePrefab = _snakeUnit.nextPrefab;
            _scorpioPrefab = _scorpioUnit.nextPrefab;
            _hyenaPrefab = _hyenaUnit.nextPrefab;
        }
        else
        {
            _vulturePrefab = _vultureUnit.nextPrefab;
            _mummyPrefab = _mummyUnit.nextPrefab;
            _deceasedPrefab = _deceasedUnit.nextPrefab;
        }
        Awake();
    }
    private enum EnemiesRegistry
    {
        Snake,
        Scorpio,
        Hyena,
        Vulture,
        Mummy,
        Deceased,
        Wizard,
        MissingEn
    }
}
