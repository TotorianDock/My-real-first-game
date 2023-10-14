using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject @object;
    [SerializeField] private GameObject informationIcon;
    [SerializeField] private GameObject informationBlockPopup;
    
    [HideInInspector] public bool @object3IsActive = false;
    
    private void Start()
    {
        @object.SetActive(false);
        
        if ( informationIcon != null )
            informationIcon.SetActive(true);
        
        if  ( informationBlockPopup != null )
            informationBlockPopup.SetActive(false);
    }

    private void OnMouseOver()
    {
        @object.SetActive(true);
        if  (informationIcon != null )
            informationIcon.SetActive(false);
        
        if ( @object3IsActive && informationBlockPopup != null )
            informationBlockPopup.SetActive(true);
        else if ( informationBlockPopup != null )
            informationBlockPopup.SetActive(false);
    }

    private void OnMouseExit()
    {
        @object.SetActive(false);
        if ( informationIcon != null )
            informationIcon.SetActive(true);
        
        if ( informationBlockPopup != null )
            informationBlockPopup.SetActive(false);
    }
}
