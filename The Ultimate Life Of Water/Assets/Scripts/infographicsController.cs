using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infographicsController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public Camera arCamera;

    public GameObject[] industry_models;
    public GameObject appPanel, markerWarningPanel;

    private bool proceed;

    void Start()
    {
        proceed = true;
        markerWarningPanel.SetActive(false);
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
                        foreach(GameObject industry_model2 in industry_models){
                            if(industry_model2.tag != hit.collider.tag && industry_model2.activeSelf){
                                appPanel.SetActive(false);
                                markerWarningPanel.SetActive(true);
                                proceed = false;
                            }
                        }
                        if(proceed){
                            appPanel.SetActive(false);
                            industry_model.SetActive(true);
                            industry_model.transform.position = hit.collider.transform.position;
                            Vector3 dir = arCamera.transform.forward;
                            industry_model.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
                            industry_model.transform.GetChild(0).gameObject.SetActive(true);
                            if(industry_model.tag == "City"){
                                industry_model.transform.GetChild(1).gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }   
    }

    public void ok_markerWarningPanel_bttn(){
        markerWarningPanel.SetActive(false);
        appPanel.SetActive(true);
    }

    public void exit_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                if(industry_model.tag == "Utility" || industry_model.tag == "City")
                    industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SinglePanelAnimation>().restart();
                else industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PanelAnimation>().restart();
                industry_model.SetActive(false);
            }
        }
        proceed = true;
        appPanel.SetActive(true);
    }

    public void explore_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).gameObject.SetActive(false);
                industry_model.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        appPanel.SetActive(false);
    }

    public void back_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(0).gameObject.SetActive(true);
                industry_model.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        // appPanel.SetActive(true);
    }

    public void restart_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                industry_model.transform.GetChild(1).GetComponent<ParamaterGameController>().restart();
            }
        }
    }

    public void exit_paramater_bttn(){
        foreach(GameObject industry_model in industry_models){
            if(industry_model.activeSelf){
                if(industry_model.tag == "Utility" || industry_model.tag == "City")
                    industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SinglePanelAnimation>().restart();    
                else industry_model.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PanelAnimation>().restart();
                industry_model.SetActive(false);
                if(industry_model.tag == "City"){
                    industry_model.transform.GetChild(0).gameObject.SetActive(true);
                    industry_model.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
        proceed = true;
        appPanel.SetActive(true);
    }
}
