using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewIndustryPanel : MonoBehaviour
{
    public GameObject gameController, center_to_compare, tutorial, addNewButtonTutorial, questionAddNewIndustryPanel;
    public bool[] new_industries_placed;

    // Start is called before the first frame update
    void Start()
    {
        restart();
    }

    public void restart(){
        questionAddNewIndustryPanel.SetActive(false);
        new_industries_placed = new bool[gameController.GetComponent<gameController>().new_count];
        for (int i = 0; i < gameController.GetComponent<gameController>().new_count; i++)
            new_industries_placed[i] = false;
    }

    public void back_bttn(){
        // if(gameController.GetComponent<gameController>().tutorialOn)
        //     addNewButtonTutorial.SetActive(true);
        gameController.GetComponent<gameController>().addNewIndustryPanel.SetActive(false);
        gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        questionAddNewIndustryPanel.SetActive(false);
    }
    
    public void confirm_bttn(){
        if(!questionAddNewIndustryPanel.activeSelf){
            gameController.GetComponent<gameController>().new_industry_index = center_to_compare.GetComponent<SnapToCenter>().indexMin + gameController.GetComponent<gameController>().existing_count + 1;
            gameController.GetComponent<gameController>().addNewIndustryPanel.SetActive(false);
            gameController.GetComponent<gameController>().placementOfNewIndustryPanel.SetActive(true);
            gameController.GetComponent<gameController>().placementOfNewIndustryPanel.GetComponent<PlacementOfNewIndustryPanel>().allow_to_place = true;
        }
        // if(gameController.GetComponent<gameController>().tutorialOn){
        //     tutorial.GetComponent<Tutorial>().placementOfNewIndustryTutorial.SetActive(true);
        // }else{
        //     tutorial.GetComponent<Tutorial>().placementOfNewIndustryTutorial.SetActive(false);
        // }

        // Debug.Log(gameController.GetComponent<gameController>().new_industry_index);
    }

    public void question_bttn(){
        if(questionAddNewIndustryPanel.activeSelf)
            questionAddNewIndustryPanel.SetActive(false);
        else questionAddNewIndustryPanel.SetActive(true);
    }

    public void ok_questionAddNewIndustryPanel_bttn(){
        questionAddNewIndustryPanel.SetActive(false);
    }
}
