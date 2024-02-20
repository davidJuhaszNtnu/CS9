using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class ParamaterGameController : MonoBehaviour
{
    public Slider[] sliders;
    public GameObject[] cubes;

    private bool enough_water;

    void Start()
    {
        restart();
    }

    void Update()
    {
        if(enough_water){
            if(!transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(true);
            if(transform.GetChild(1).gameObject.activeSelf)
                transform.GetChild(1).gameObject.SetActive(false);
        }else{
            if(transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(false);
            if(!transform.GetChild(1).gameObject.activeSelf)
                transform.GetChild(1).gameObject.SetActive(true);
        }
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

        if(sliders[0].value > 0.6f && sliders[1].value > 0.6f && sliders[2].value > 0.6f)
            enough_water = true;
        else enough_water = false;
    }

    public void restart(){
        enough_water = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        sliders[0].value = 0f;
        sliders[1].value = 0f;
        sliders[2].value = 0f;

        for(int i = 0; i < 3; i++)
            updateCubes(i);
    }
}
