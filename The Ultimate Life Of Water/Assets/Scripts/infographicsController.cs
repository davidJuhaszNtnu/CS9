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
                    if(hit.collider.tag == industry_model.tag){
                        industry_model.SetActive(true);
                        industry_model.transform.position = hit.collider.transform.position;
                        Vector3 dir = arCamera.transform.forward;
                        industry_model.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
                        industry_model.transform.GetChild(0).gameObject.SetActive(true);
                        industry_model.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }   
    }

    public void exit_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PanelAnimation>().restart();
                industry_model.SetActive(false);
            }
        }
    }

    public void explore_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).gameObject.SetActive(false);
                industry_model.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void back_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).gameObject.SetActive(true);
                industry_model.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void restart_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(1).GetComponent<ParamaterGameController>().restart();
            }
        }
    }

    public void info_bttn(){
        
    }

    public void exit_paramater_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PanelAnimation>().restart();
                industry_model.SetActive(false);
            }
        }
    }
}
