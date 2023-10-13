using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DataHandler : MonoBehaviour
{
    public static int fillRate = 50;
    public static int cameraSize = 40;

    public TextMeshProUGUI spawn_rate_text;
    public TextMeshProUGUI camera_size_text;

    public Toggle toggle;

    public Slider fillRateUI;
    public Slider cameraSizeUI;

    public static bool toggle_ui;

    // Start is called before the first frame update
    void Start()
    {
        fillRateUI.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        cameraSizeUI.onValueChanged.AddListener(delegate { ValueChangeCheck2(); });
    }

    private void Update()
    {
        spawn_rate_text.text = fillRate + "%".ToString();
        camera_size_text.text = cameraSize.ToString();
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        fillRate = (int)fillRateUI.value;
    }

    public void ValueChangeCheck2()
    {
        cameraSize = (int)cameraSizeUI.value;
    }

    public void ValueChangeCheckUI()
    {
        Game_data.ui_enabled = toggle.isOn;
    }
}
