using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewIndustryPanel : MonoBehaviour
{
    public GameObject gameController, center_to_compare, tutorial;
    public Button confirm_button;
    public bool[] new_industries_placed;

    // Start is called before the first frame update
    void Start()
    {
        restart();
    }

    public void restart(){
        new_industries_placed = new bool[gameController.GetComponent<gameController>().new_count];
        for (int i = 0; i < gameController.GetComponent<gameController>().new_count; i++)
            new_industries_placed[i] = false;
    }

    void Update(){
        if(!new_industries_placed[center_to_compare.GetComponent<SnapToCenter>().indexMin])
            confirm_button.interactable = true;
        else confirm_button.interactable = false;
    }

    public void back_bttn(){
        gameController.GetComponent<gameController>().addNewIndustryPanel.SetActive(false);
        gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
    }
    
    public void confirm_bttn(){
        gameController.GetComponent<gameController>().new_industry_index = center_to_compare.GetComponent<SnapToCenter>().indexMin + gameController.GetComponent<gameController>().existing_count + 1;
        gameController.GetComponent<gameController>().addNewIndustryPanel.SetActive(false);
        gameController.GetComponent<gameController>().placementOfNewIndustryPanel.SetActive(true);
        if(gameController.GetComponent<gameController>().tutorialOn){
            tutorial.GetComponent<Tutorial>().placementOfNewIndustryTutorial.SetActive(true);
        }else{
            tutorial.GetComponent<Tutorial>().placementOfNewIndustryTutorial.SetActive(false);
        }

        // Debug.Log(gameController.GetComponent<gameController>().new_industry_index);
    }
}
