using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private Color hoverBackgroundColor;
    [SerializeField] private Color hoverTextColor;
    private Color originalBackgroundColor;
    private Color originalTextColor;
    private Image buttonBackground;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonBackground = GetComponent<Image>();
        originalBackgroundColor = buttonBackground.color;
        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonBackground.color = hoverBackgroundColor;
        if (buttonText!=null)
        {
            buttonText.color = hoverTextColor;
        }
        CanvasManager.instance.NotifyObserver(UserAction.Hover);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        buttonBackground.color = originalBackgroundColor;
        if (buttonText != null)
        {
            buttonText.color = originalTextColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CanvasManager.instance.NotifyObserver(UserAction.Click);
    }
}
