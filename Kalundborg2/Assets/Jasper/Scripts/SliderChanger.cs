using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChanger : MonoBehaviour
{
    public Slider TotalWater;  // the slider that starts at 1
    public Slider WaterPeople;  // Slider to send water to the people
    public Slider WaterIndustry;  // Slider to send water to the industries
    public Slider WaterSymbiosisSlider;
    
    public GameObject WaterSymbiosis;
    public GameObject WaterSymbiosisHintText;

    private float maxSlider2Value;  // the maximum value of slider 2
    private float maxSlider3Value;  // the maximum value of slider 3

    public GameObject textNoWater;
    public GameObject textOkWater;

    public ClickObject found;

    void Start()
    {        // set the starting values of the sliders
        TotalWater.value = 1f;
        WaterPeople.value = 0f;
        WaterIndustry.value = 0f;
        WaterSymbiosisSlider.value = 0f;

        // set the maximum values of sliders 2 and 3 to the starting value of slider 1
        maxSlider2Value = TotalWater.value;
        maxSlider3Value = TotalWater.value;
    }

    void Update()
    {

        if(found.FoundWaterSymbiosis)       //If statement to check if user has found water refinery industrie to unlock last slider
        {
           WaterSymbiosis.SetActive(true);
           WaterSymbiosisHintText.SetActive(false);
        }

        // calculate the new value of slider 1 based on the values of sliders 2 and 3
        float newSlider1Value = 1f - WaterPeople.value - WaterIndustry.value + WaterSymbiosisSlider.value;

        // make sure the new value of slider 1 is within the valid range (0-1)
        newSlider1Value = Mathf.Clamp(newSlider1Value, 0f, 1f);

        // set the value of slider 1 to the new value
        TotalWater.value = newSlider1Value;

        // update the maximum values of sliders 2 and 3 based on the current value of slider 1
        maxSlider2Value = 1f - WaterPeople.value;
        maxSlider3Value = 1f - WaterIndustry.value;

        // set the maximum values of sliders 2 and 3 to prevent them from going above the current value of slider 1
        WaterPeople.maxValue = maxSlider2Value;
        WaterIndustry.maxValue = maxSlider3Value;



        if (TotalWater.value <= 0.05f)
        {
            textNoWater.SetActive(true);
            textOkWater.SetActive(false);
        }
        else
        {
            textNoWater.SetActive(false);
            textOkWater.SetActive(true);
        }


    }
}