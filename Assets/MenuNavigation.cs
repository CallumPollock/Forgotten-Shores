using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] GameObject navButton;
    [SerializeField] Transform content;
    [SerializeField] Transform currentActiveTab;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            if(child.name != "Navigation")
            {
                Button newNavButton = Instantiate(navButton).GetComponent<Button>();
                newNavButton.transform.SetParent(content);
                newNavButton.transform.localScale = Vector3.one;
                newNavButton.onClick.AddListener(delegate { SetActiveTab(child); });
                newNavButton.GetComponentInChildren<TextMeshProUGUI>().text = child.name;
                child.gameObject.SetActive(false);
            }
        }
        currentActiveTab = transform.GetChild(0);
    }

    private void OnEnable()
    {
        SetActiveTab(transform.GetChild(0));
    }

    private void SetActiveTab(Transform tab)
    {
        currentActiveTab.gameObject.SetActive(false);
        currentActiveTab = tab;
        currentActiveTab.gameObject.SetActive(true);
    }
}
