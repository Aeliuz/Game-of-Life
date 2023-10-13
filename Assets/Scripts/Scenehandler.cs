using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenehandler : MonoBehaviour
{
    public static bool custom_start = false;
    public static bool pattern_start = false;

    public void Simulate()
    {
        pattern_start = false;
        custom_start = false;
        SceneManager.LoadScene("SampleScene");
        
    }

    public void custom() 
    {
        pattern_start = false;
        custom_start = true;
        SceneManager.LoadScene("SampleScene");
    }

    public void menu()
    {
        SceneManager.LoadScene("menu");
    }

    //Click the cell in the menu to trigger the easter egg
    public void Easter_egg()
    {
        custom_start = false;
        pattern_start = true;
        SceneManager.LoadScene("SampleScene");
    }
}
