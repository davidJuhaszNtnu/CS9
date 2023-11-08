using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infographicsController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public Camera arCamera;

    public GameObject[] industry_models;

    void Start()
    {
        foreach(GameObject industry_model in industry_models)
            industry_model.SetActive(false);
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                foreach(GameObject industry_model in industry_models){
                    if(hit.collider.name == industry_model.name){
                        industry_model.SetActive(true);
                        industry_model.transform.position = hit.collider.transform.position;
                    }
                }
            }
        }       
    }
}
