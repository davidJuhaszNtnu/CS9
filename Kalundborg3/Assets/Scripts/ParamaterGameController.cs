using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamaterGameController : MonoBehaviour
{
    public Slider[] sliders;
    public GameObject[] cubes;

    void Start()
    {
        sliders[0].value = 0.5f;
        sliders[1].value = 0f;
        sliders[2].value = 0f;
        sliders[3].value = 0f;

        updateCubes(0);
    }

    public void slider1_change(){

    }

    public void slider2_change(){

    }

    public void slider3_change(){

    }

    public void slider4_change(){

    }

    private void updateCubes(int i){
        Vector3 oldScale = cubes[i].transform.localScale;
        Vector3 oldPosition = cubes[i].transform.localPosition;
        cubes[i].transform.localScale = new Vector3(oldScale.x, sliders[i].value, oldScale.z);
        cubes[i].transform.localPosition += new Vector3(oldPosition.x, sliders[i].value/2f, oldPosition.z);
    }
}
