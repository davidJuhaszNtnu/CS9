using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class ParamaterGameController : MonoBehaviour
{
    public Slider[] sliders;
    public GameObject[] cubes;

    private float result;

    void Start()
    {
        restart();
    }

    void Update()
    {
        
    }

    public void slider1_change(){
        updateCubes(0);
    }

    public void slider2_change(){
        updateCubes(1);
    }

    public void slider3_change(){
        updateCubes(2);
    }

    private void updateCubes(int i){
        Vector3 oldScale = cubes[i].transform.localScale;
        Vector3 oldPosition = cubes[i].transform.localPosition;
        cubes[i].transform.localScale = new Vector3(oldScale.x, sliders[i].value, oldScale.z);
        cubes[i].transform.localPosition = new Vector3(oldPosition.x, sliders[i].value/2f - 0.5f, oldPosition.z);

        result = 1/(1 + Mathf.Exp(-(sliders[0].value + sliders[1].value + sliders[2].value - 1f) * Mathf.Pow(4f + 16f * (sliders[0].value - sliders[1].value - sliders[2].value),2)));
        Debug.Log(result);
    }

    public void restart(){
        sliders[0].value = 0f;
        sliders[1].value = 0f;
        sliders[2].value = 0f;

        for(int i = 0; i < 3; i++)
            updateCubes(i);
    }
}
