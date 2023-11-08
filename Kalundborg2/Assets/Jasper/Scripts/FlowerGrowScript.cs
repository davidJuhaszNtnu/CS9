using UnityEngine;
using UnityEngine.UI;

public class FlowerGrowScript : MonoBehaviour
{
    //This script is to update the shader with the value of the total water slider in the scene 
    public Material material;
    public string propertyName;
    public Slider slider;

    private void Start()
    {
        // Set the initial value of the shader property to the slider value
        UpdateShaderValue(slider.value);
    }

    public void OnSliderValueChanged()
    {
        // Update the shader property when the slider value changes
        UpdateShaderValue(slider.value);
    }

    private void UpdateShaderValue(float value)
    {
        // Set the value of the specified shader property
        material.SetFloat(propertyName, value);
    }
}