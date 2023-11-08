using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameController, arrow_distInd_prefab, arrow_ind_prefab, distribution_industry, app, industry1, tutorial;
    public GameObject welcomePanel, distributionIndustryPanel, industryPanel, addNewButtonTutorial, placementOfNewIndustryTutorial,
                        newIndustryInfoPanel, makeConnectionButtonTutorial, connectionPanel, connectionAnimationPanel;
    GameObject arrow_distInd, arrow_ind;

    public bool interactable, distributionIndustry_bool, industry_bool;

    Ray ray;
    RaycastHit hit;
    public Camera arCamera;

    void Start()
    {
        welcomePanel.SetActive(true);
        distributionIndustryPanel.SetActive(false);
        industryPanel.SetActive(false);
        addNewButtonTutorial.SetActive(false);
        newIndustryInfoPanel.SetActive(false);
        makeConnectionButtonTutorial.SetActive(false);
        connectionPanel.SetActive(false);
        connectionAnimationPanel.SetActive(false);
        interactable = false;
        distributionIndustry_bool = true;
        industry_bool = true;
    }

    public void next_welcomePanel_bttn(){
        welcomePanel.SetActive(false);
        arrow_distInd = Instantiate(arrow_distInd_prefab);
        arrow_distInd.transform.SetParent(distribution_industry.transform, true);
        arrow_distInd.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        arrow_distInd.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        arrow_distInd.transform.localScale *= app.GetComponent<App>().scale*2f;
        interactable = true;
    }

    public void next_distributionIndustryPanel_bttn(){
        distributionIndustryPanel.SetActive(false);
        arrow_ind = Instantiate(arrow_ind_prefab);
        arrow_ind.transform.SetParent(industry1.transform, true);
        arrow_ind.transform.localPosition = new Vector3(0f, 1.2f, 0f);
        arrow_ind.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        arrow_ind.transform.localScale *= app.GetComponent<App>().scale*2f;
        interactable = true;
    }

    public void next_industryPanel_bttn(){
        industryPanel.SetActive(false);
        addNewButtonTutorial.SetActive(true);
    }

    public void next_newIndustryInfoPanel_bttn(){
        newIndustryInfoPanel.SetActive(false);
        makeConnectionButtonTutorial.SetActive(true);
    }

    public void next_connectionPanel_bttn(){
        connectionPanel.SetActive(false);
    }

    public void next_connectionAnimationPanel_bttn(){
        connectionAnimationPanel.SetActive(false);

        tutorial.SetActive(false);
        gameController.GetComponent<gameController>().tutorialOn = false;
    }

    void Update(){
         if(Input.GetMouseButtonDown(0) && interactable){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(hit.collider.name == "Distribution Industry" && distributionIndustry_bool){
                    interactable = false;
                    distributionIndustryPanel.SetActive(true);
                    Destroy(arrow_distInd);
                    distributionIndustry_bool = false;
                }
                if(hit.collider.name == "Industry 1" && industry_bool){
                    interactable = false;
                    industryPanel.SetActive(true);
                    Destroy(arrow_ind);
                    industry_bool = false;
                }
            }
         }
    }
}
