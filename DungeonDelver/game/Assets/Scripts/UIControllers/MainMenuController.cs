using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public static Menu activeMenu = Menu.Start;
    private Menu _activeMenu;

    public GameObject startScreen, loseScreen, winScreen, controlsScreen;
    public Material startSkybox, loseSkybox, winSkybox;

    public List<GameObject> hideOnWin;

    // Not editable in the inspector, since this is used statically
    public const string menuScene = "StartScene";

    public string storyModeScene = "Level1";
    public string delveModeScene = "TileBuilder";

    void Start()
    {
        SwitchMenu(activeMenu);
    }

    void Update()
    {
        if (_activeMenu != activeMenu)
        {
            SwitchMenu(activeMenu);
        }
    }

    private void SwitchMenu(Menu menu)
    {
        MainMenuController.activeMenu = menu;
        _activeMenu = menu;

        switch (menu)
        {
            case Menu.Start:
                startScreen.SetActive(true);
                loseScreen.SetActive(false);
                winScreen.SetActive(false);
                controlsScreen.SetActive(false);
                RenderSettings.skybox = startSkybox;
                break;
            case Menu.Lose:
                startScreen.SetActive(false);
                loseScreen.SetActive(true);
                winScreen.SetActive(false);
                controlsScreen.SetActive(false);
                RenderSettings.skybox = loseSkybox;
                break;
            case Menu.Win:
                startScreen.SetActive(false);
                loseScreen.SetActive(false);
                winScreen.SetActive(true);
                controlsScreen.SetActive(false);
                RenderSettings.skybox = winSkybox;
                break;
            case Menu.Controls:
                startScreen.SetActive(false);
                loseScreen.SetActive(false);
                winScreen.SetActive(false);
                controlsScreen.SetActive(true);
                RenderSettings.skybox = startSkybox;
                break;
        }

        hideOnWin.ForEach(o => o.SetActive(menu != Menu.Win));
    }

    public void StoryClicked()
    {
        print("Story clicked.");
        SceneManager.LoadScene(storyModeScene);
    }

    public void DelveClicked()
    {
        print("Delve clicked.");
        SceneManager.LoadScene(delveModeScene);
    }

    public void ControlsClicked()
    {
        print("Controls clicked.");
        SwitchMenu(Menu.Controls);
    }

    public void ExitClicked()
    {
        print("Exit clicked.  Note that nothing will happen in the editor, but this does work when actually running the game.");
        Application.Quit();
    }

    public void ShowStartMenu()
    {
        SwitchMenu(Menu.Start);
    }

    public static void DisplayMenu(Menu menu)
    {
        SceneManager.LoadScene(menuScene);
        activeMenu = menu;
    }
}

public enum Menu
{
    Start,
    Lose,
    Win,
    Controls,
}
