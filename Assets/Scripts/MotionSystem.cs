using UnityEngine;

public class MotionSystem : MonoBehaviour
{
    [HideInInspector] public bool isMoving = false;

    [SerializeField] private GameObject _FirstBackground;
    public GameObject _SecondBackground;
    
    [SerializeField] private GameObject _enemyStation1;
    [SerializeField] private GameObject _enemyStation2;
    private readonly float _step = 0.1f;
    
    [HideInInspector] private Vector3 _startPositionBG1;
    [HideInInspector] private Vector3 _startPositionBG2;
    [HideInInspector] private Vector3 _startPositionES1;
    [HideInInspector] private Vector3 _startPositionES2;

    private void Awake()
    {
        _startPositionBG1 = _FirstBackground.transform.position;
        _startPositionBG2 = _SecondBackground.transform.position;
        _startPositionES1 = _enemyStation1.transform.position;
        _startPositionES2 = _enemyStation2.transform.position;
        _SecondBackground.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (isMoving == true)
        {
            if (_SecondBackground.activeInHierarchy == true)
            {
                if (_FirstBackground.activeInHierarchy == true)
                {
                    _SecondBackground.transform.position -= new Vector3(_step, 0, 0);
                    _FirstBackground.transform.position -= new Vector3(_step, 0, 0);
                }
                else
                    _SecondBackground.transform.position -= new Vector3(_step, 0, 0);
            }
            else
                _FirstBackground.transform.position -= new Vector3(_step, 0, 0);
            
            _enemyStation1.transform.position -= new Vector3(_step, 0, 0);
        }
    }
    public void ResetEnemyStation(bool locationHasChanged)
    {
        if (_SecondBackground.activeInHierarchy == true && locationHasChanged == false)
            _enemyStation1.transform.position = _startPositionES2;
        else
            _enemyStation1.transform.position = _startPositionES1;
    }
    public void ResetBackground(bool locationHasChanged)
    {
        if (_FirstBackground.activeInHierarchy == false && locationHasChanged == false)
            _SecondBackground.transform.position = _startPositionBG2;
        else if (_FirstBackground.activeInHierarchy == false)
        {
            _SecondBackground.transform.position = _startPositionBG1;
            _SecondBackground.transform.position += new Vector3(0, 3.2033705f); //Height levelling
        }
        else
        {
            _FirstBackground.transform.position = _startPositionBG1;
            _SecondBackground.transform.position = _startPositionBG2;
        }
    }
    public void SetSecondBackground()
    {
        _SecondBackground.SetActive(true);
    }
    public void UnsetFirstBackground()
    {
        _FirstBackground.SetActive(false);
    }
}
