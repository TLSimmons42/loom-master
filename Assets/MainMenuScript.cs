using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void ChangeToOnePlayer()
    {
        PlayerPrefs.SetInt("playerCount", 1);
    }

    public void ChangeToTwoPlayer()
    {
        PlayerPrefs.SetInt("playerCount", 2);
    }


}
