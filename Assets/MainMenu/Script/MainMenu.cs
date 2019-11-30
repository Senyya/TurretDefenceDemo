using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartGame ()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadIntroduction()
    {
        SceneManager.LoadScene("Instruction");
    }
    public void Quit() 
    {
        Application.Quit();
    }
}
