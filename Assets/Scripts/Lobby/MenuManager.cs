using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Opens the Menu by String
    /// </summary>
    /// <param name="menuName">Menu name</param>
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);    // Closes all other menus
            }
        }
    }

    /// <summary>
    /// Opens the Menu
    /// </summary>
    /// <param name="menu"> Menu to be opened</param>
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);    //Closes if other menu is opened
            }
        }
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
