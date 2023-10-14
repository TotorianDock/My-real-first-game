using UnityEngine;

public class ShadingSystem : MonoBehaviour
{
    [Header("Hero HUD")]
    [SerializeField] private GameObject _shading_HH;

    [Header("Enemy HUD")]
    [SerializeField] private GameObject _shading_EH;
    
    [Header("Dialogue panel")]
    [SerializeField] private GameObject _shadingDP;

    [Header("Combat panel")]
    [SerializeField] private GameObject _shading_CP;
    [SerializeField] private GameObject _shading_CP_AttackButton;
    [SerializeField] private GameObject _shading_CP_HealButton;
    [SerializeField] private GameObject _shading_CP_BlockButton;

    [Header("Combat panel & Explanation")]
    [SerializeField] private GameObject _shading_CP_AttackButton_E;
    [SerializeField] private GameObject _shading_CP_HealButton_E;
    [SerializeField] private GameObject _shading_CP_BlockButton_E;

    public void SetShading()
    {
        _shading_HH.SetActive(true);
        _shading_EH.SetActive(true);
        
        _shading_CP.SetActive(true);
        _shading_CP_AttackButton.SetActive(true);
        _shading_CP_HealButton.SetActive(true);
        _shading_CP_BlockButton.SetActive(true);
        
        _shading_CP_AttackButton_E.SetActive(true);
        _shading_CP_HealButton_E.SetActive(true);
        _shading_CP_BlockButton_E.SetActive(true);
        
        _shadingDP.SetActive(true);
    }
}
