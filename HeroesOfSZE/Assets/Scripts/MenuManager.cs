using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void ExitGame()
    {
        Debug.Log("Kilépés a játékból...");
        Application.Quit();
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void toUnitSelect()
    {
        SceneManager.LoadScene("SelectHero");
    }

}

