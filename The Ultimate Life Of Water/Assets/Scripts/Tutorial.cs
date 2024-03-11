using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameController, arrow_distInd_prefab, arrow_ind_prefab, distribution_industry, app, utility, tutorial;
    public GameObject welcomePanel, welcomePanel2, distributionIndustryPanel, industryPanel1, industryPanel2, howtoplayPanel, addNewButtonTutorial, placementOfNewIndustryTutorial,
                      makeConnectionButtonTutorial, connectionPanel, congratulationsPanel;
    public GameObject arrow_distInd, arrow_ind;

    public bool interactable, distributionIndustry_bool, industry_bool;
    public bool industry_secondTime;

    Ray ray;
    RaycastHit hit;
    public Camera arCamera;

    void Start()
    {
        welcomePanel.SetActive(true);
        welcomePanel2.SetActive(false);
        distributionIndustryPanel.SetActive(false);
        industryPanel1.SetActive(false);
        industryPanel2.SetActive(false);
        howtoplayPanel.SetActive(false);
        addNewButtonTutorial.SetActive(false);
        makeConnectionButtonTutorial.SetActive(false);
        connectionPanel.SetActive(false);
        congratulationsPanel.SetActive(false);
        interactable = false;
        distributionIndustry_bool = true;
        industry_bool = false;
        industry_secondTime = false;
    }

    public void next_welcomePanel_bttn(){
        welcomePanel.SetActive(false);
        welcomePanel2.SetActive(true);
    }

    public void next_welcomePanel2_bttn(){
        welcomePanel2.SetActive(false);
        arrow_distInd = Instantiate(arrow_distInd_prefab);
        arrow_distInd.transform.SetParent(distribution_industry.transform, true);
        arrow_distInd.transform.localPosition = new Vector3(0f, 15f, 0f);
        arrow_distInd.transform.localRotation = Quaternion.Euler(0f, -90f, 90f);
        arrow_distInd.transform.localScale *= app.GetComponent<App>().scale*2f;
        interactable = true;
        gameController.transform.GetComponent<gameController>().add_new_button.interactable = false;
    }

    public void next_distributionIndustryPanel_bttn(){
        distributionIndustryPanel.SetActive(false);
        arrow_ind = Instantiate(arrow_ind_prefab);
        arrow_ind.transform.SetParent(utility.transform, true);
        arrow_ind.transform.localPosition = new Vector3(0f, 80f, 0f);
        arrow_ind.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
        arrow_ind.transform.localScale *= app.GetComponent<App>().scale*2f;
        interactable = true;
        industry_bool = true;
    }

    public void next_industryPanel_bttn(){
        industryPanel1.SetActive(false);
        industryPanel2.SetActive(true);
    }

    public void next_industryPanel2_bttn(){
        industryPanel2.SetActive(false);
        howtoplayPanel.SetActive(true);
    }

    public void next_howtoplayPanel_bttn(){
        howtoplayPanel.SetActive(false);
        addNewButtonTutorial.SetActive(true);
        gameController.transform.GetComponent<gameController>().add_new_button.interactable = true;
    }

    // public void next_newIndustryInfoPanel_bttn(){
    //     newIndustryInfoPanel.SetActive(false);
    //     makeConnectionButtonTutorial.SetActive(true);
    //     arrow_ind = Instantiate(arrow_ind_prefab);
    //     arrow_ind.transform.SetParent(industry1.transform, true);
    //     arrow_ind.transform.localPosition = new Vector3(0f, 1.2f, 0f);
    //     arrow_ind.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
    //     arrow_ind.transform.localScale *= app.GetComponent<App>().scale*2f;
    //     industry_bool = true;
    //     interactable = true;
    //     industry_secondTime = true;
    // }

    public void next_connectionPanel_bttn(){
        connectionPanel.SetActive(false);
    }

    public void next_connectionAnimationPanel_bttn(){
        congratulationsPanel.SetActive(false);

        tutorial.SetActive(false);
        gameController.GetComponent<gameController>().tutorialOn = false;
    }

    void Update(){
         if(Input.GetMouseButtonDown(0) && interactable){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(hit.collider.name == "Ultimate City" && distributionIndustry_bool){
                    interactable = false;
                    distributionIndustryPanel.SetActive(true);
                    Destroy(arrow_distInd);
                    distributionIndustry_bool = false;
                }
                if(hit.collider.name == "Utility" && industry_bool){
                    interactable = false;
                    if(!industry_secondTime){
                        industryPanel1.SetActive(true);
                        industryPanel2.SetActive(false);
                    }
                    Destroy(arrow_ind);
                    industry_bool = false;
                }
            }
         }
    }
}
