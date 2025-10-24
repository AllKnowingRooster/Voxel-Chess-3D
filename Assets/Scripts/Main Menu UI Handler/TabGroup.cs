using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<Tab> listTab= new List<Tab>();
    [SerializeField] private List<GameObject> listPage;
    private Tab activeTab;
    [SerializeField] private Color idleColor;
    [SerializeField] private Color idleTextColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color activeTextColor;

    private void Start()
    {
        activeTab = listTab[0];
        activeTab.tabBackground.color = activeColor;
        activeTab.tabText.color = activeTextColor;
        listPage[0].SetActive(true);
    }
    public void ResetTab()
    {
        for (int i=0;i<listTab.Count;i++)
        {
            if (activeTab!=null && listTab[i]==activeTab)
            {
                continue;
            }
            listTab[i].tabBackground.color= idleColor;
            listTab[i].tabText.color= idleTextColor;
        }
    }

    public void OnSelect(Tab tab,bool isClicked)
    {
        if (tab==activeTab)
        {
            return;
        }
        ResetTab();
        tab.tabBackground.color = activeColor;
        tab.tabText.color = activeTextColor;
        if (isClicked)
        {
            Deselect(tab,activeTab.transform.GetSiblingIndex());
            activeTab = tab;
            int index=tab.transform.GetSiblingIndex();
            listPage[index].gameObject.SetActive(true);
            CanvasManager.instance.NotifyObserver(UserAction.Click);
        }
        else
        {
            CanvasManager.instance.NotifyObserver(UserAction.Hover);
        }
    }

    private void Deselect(Tab tab,int index)
    {
        activeTab.tabBackground.color = idleColor;
        activeTab.tabText.color = idleTextColor;
        listPage[index].gameObject.SetActive(false);
    }


}
