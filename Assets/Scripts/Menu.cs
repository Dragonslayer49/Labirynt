using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Open1()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }
    public void Open2()
    {
        SceneManager.LoadSceneAsync("Level 2");
    }
    public void Open3()
    {
        SceneManager.LoadSceneAsync("Level 3");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
