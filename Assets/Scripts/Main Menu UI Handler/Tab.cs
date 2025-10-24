using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] private TabGroup tabGroup;
    public TextMeshProUGUI tabText;
    public Image tabBackground;
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnSelect(this, true);
        CanvasManager.instance.NotifyObserver(UserAction.Click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnSelect(this, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.ResetTab();
    }

}
