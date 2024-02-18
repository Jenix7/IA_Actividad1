using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Menu
{
    public List<GameObject> menuElements;
}

public class GameManager : MonoBehaviour
{
    public List<Menu> menus;
    private int currentMenuIndex = -1;
    private List<GameObject> allMenuElements = new List<GameObject>();

    void Awake()
    {
        PopulateAllMenuElements();
        Time.timeScale = 0f;
    }

    void Start()
    {
        AdvanceMenu();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            AdvanceMenu();
        }
    }

    void PopulateAllMenuElements()
    {
        foreach (var menu in menus)
        {
            foreach (var element in menu.menuElements)
            {
                if (!allMenuElements.Contains(element))
                {
                    allMenuElements.Add(element);
                }
            }
        }
    }

    void AdvanceMenu()
    {
        DeactivateAllMenuElements();

        currentMenuIndex++;
        if (currentMenuIndex < menus.Count)
        {
            ActivateMenuElements(currentMenuIndex);
        }
        else
        {
            StartCoroutine(StartGameAfterDelay(0.5f));
        }
    }

    void DeactivateAllMenuElements()
    {
        foreach (var element in allMenuElements)
        {
            element.SetActive(false);
        }
    }

    void ActivateMenuElements(int menuIndex)
    {
        foreach (var element in menus[menuIndex].menuElements)
        {
            element.SetActive(true);
        }
    }

    IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
    }
}
