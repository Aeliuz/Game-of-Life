using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Game_data : MonoBehaviour
{

    public TextMeshProUGUI stable;
    public TextMeshProUGUI generation;
    public TextMeshProUGUI gen_til_stable;

    public Toggle toggle;

    bool stable_check = false;

    public static bool ui_enabled = false;

    int stable_gen;

    // Update is called once per frame
    void Update()
    {
        Control_UI();
    }

    public void Control_UI()
    {
        if (ui_enabled)
        {

            stable.text = "Stable: " + GameOfLife.Instance.stable;
            generation.text = "Generations: " + GameOfLife.Instance.generations.ToString();


            if (GameOfLife.Instance.stable)
            {
                if (!stable_check)
                {
                    stable_gen = GameOfLife.Instance.generations;
                    stable_check = true;
                }
            }
            else
                stable_check = false;

            if (stable_check)
                gen_til_stable.text = "Generations until stable: " + stable_gen.ToString();
            else
                gen_til_stable.text = "Generations until stable: ";

        }
    }
}
