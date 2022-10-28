using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    //public string play;
    [SerializeField] AudioSource sfxButton;
    public void playGame()
    {
        SceneManager.LoadScene("Animal");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void clickButton()
    {
        sfxButton.Play();
    }
}
