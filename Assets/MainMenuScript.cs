using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("Integration Main");
    }
    public void ChangeToOnePlayer()
    {
        PlayerPrefs.SetInt("playerCount", 1);
        ChangeToPlayerOneChoice();
    }

    public void ChangeToTwoPlayer()
    {
        PlayerPrefs.SetInt("playerCount", 2);
    }

    public void ChangeToEasy()
    {
        PlayerPrefs.SetString("gameDifficulty", "easy");
    }
    public void ChangeToMedium()
    {
        PlayerPrefs.SetString("gameDifficulty", "medium");
    }
    public void ChangeToHard()
    {
        PlayerPrefs.SetString("gameDifficulty", "hard");
    }

    public void ChangeToPlayerOneChoice()
    {
        PlayerPrefs.SetString("playerChoice", "P1");
    }

    public void ChangeToPlayerTwoChoice()
    {
        PlayerPrefs.SetString("playerChoice", "P2");
    }

    public void ChangeToMouse()
    {
        PlayerPrefs.SetString("player head", "mouse");
    }

    public void ChangeToHuman()
    {
        PlayerPrefs.SetString("player head", "human");
    }

    public void ChangeToCat()
    {
        PlayerPrefs.SetString("player head", "cat");
    }
}
