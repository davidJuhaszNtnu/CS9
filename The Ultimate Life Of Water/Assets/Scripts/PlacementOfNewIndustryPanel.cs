using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlacementOfNewIndustryPanel : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject placementMarker_prefab, floor, gameController, addNewIndustryPanel, app, tutorial;
    public Button confirm_button, back_button, question_button, ok_button;

    private GameObject placementMarker;
    private Vector3 place_position;
    public GameObject[] industry_cards;
    bool placed, visible;
    public bool allow_to_place;
    
    void Start()
    {
        placementMarker = Instantiate(placementMarker_prefab);
        placementMarker.transform.SetParent(transform, true);
        placementMarker = placementMarker.transform.GetChild(0).gameObject;
        placementMarker.transform.localScale *= app.GetComponent<App>().scale;
        placementMarker.SetActive(false);

        placed = false;
        visible = false;
    }

    public void restart(){
        placementMarker.SetActive(false);

        placed = false;
        visible = false;
        allow_to_place = false;

        //fade back in the industry cards
        for (int i = 0; i < gameController.GetComponent<gameController>().new_count; i++){
            Color c = industry_cards[i].GetComponent<Image>().color;
            c.a = 1f;
            industry_cards[i].GetComponent<Image>().color = c;
            foreach(Transform  child in industry_cards[i].transform){
                if(child.GetComponent<Image>() != null){
                    c = child.GetComponent<Image>().color;
                    c.a = 1f;
                    child.GetComponent<Image>().color = c;
                }else{
                    c = child.GetComponent<TextMeshProUGUI>().color;
                    c.a = 1f;
                    child.GetComponent<TextMeshProUGUI>().color = c;
                }
            }
            industry_cards[i].transform.GetComponent<Button>().interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2f, Screen.height/2f));

        if (Physics.Raycast(ray, out hit)){
            if(hit.collider.name=="Floor"){
                placementMarker.transform.position = hit.point + (new Vector3(0f, 0.01f ,0f));
                if (!visible){
                    placementMarker.SetActive(true);
                    visible = true;
                }
                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && allow_to_place){
                    if (!RectTransformUtility.RectangleContainsScreenPoint( confirm_button.GetComponent<RectTransform>(), Input.GetTouch(0).position) && !RectTransformUtility.RectangleContainsScreenPoint(back_button.GetComponent<RectTransform>(), Input.GetTouch(0).position)){
                        gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].SetActive(true);
                        if(gameController.GetComponent<gameController>().new_industry_index == 5)
                            place_position = hit.point + new Vector3(0f, 0.1f*app.GetComponent<App>().scale, 0f);
                        if(gameController.GetComponent<gameController>().new_industry_index == 6)
                            place_position = hit.point + new Vector3(0f, 0.03f*app.GetComponent<App>().scale, 0f);
                        if(gameController.GetComponent<gameController>().new_industry_index == 7)
                            place_position = hit.point + new Vector3(-0.12f, 0.03f*app.GetComponent<App>().scale, -0.03f);
                        gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].transform.position = place_position;
                        placed = true;
                    }
                }
            }else if(visible){
                placementMarker.SetActive(false);
                visible=false;
            }
        }else if(visible){
                placementMarker.SetActive(false);
                visible=false;
        }

        // if(Input.GetMouseButtonDown(0)){
        //     if (!RectTransformUtility.RectangleContainsScreenPoint( confirm_button.GetComponent<RectTransform>(), Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(back_button.GetComponent<RectTransform>(), Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(question_button.GetComponent<RectTransform>(), Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(ok_button.GetComponent<RectTransform>(), Input.mousePosition)){
        //         ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         if (Physics.Raycast(ray, out hit)){
        //             if(hit.collider.name=="Floor" && allow_to_place){
        //                 // Debug.Log("hit the floor " + placed);
        //                 gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].SetActive(true);
        //                 if(gameController.GetComponent<gameController>().new_industry_index == 5)
        //                     gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].transform.localPosition = new Vector3(-0.1272f,-0.0451f,-0.4087f);
        //                 if(gameController.GetComponent<gameController>().new_industry_index == 6)
        //                     gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].transform.localPosition = new Vector3(0.60688f,-0.0914f,-0.46265f);
        //                 if(gameController.GetComponent<gameController>().new_industry_index == 7)
        //                     gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].transform.localPosition = new Vector3(0.54309f,-0.0914f,-0.27736f);
        //                 placed = true;
        //             }
        //         }
        //     }
        // }

        if(placed)
            confirm_button.interactable = true;
        else confirm_button.interactable = false;
    }

    public void confirm_new_industry_bttn(){
        if(placed){
            gameController.GetComponent<gameController>().placementOfNewIndustryPanel.SetActive(false);
            gameController.GetComponent<gameController>().mainPanel.SetActive(true);

            //fade in the industry card
            Color c = industry_cards[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1].GetComponent<Image>().color;
            c.a = 0.5f;
            industry_cards[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1].GetComponent<Image>().color = c;
            foreach(Transform  child in industry_cards[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1].transform){
                if(child.GetComponent<Image>() != null){
                    c = child.GetComponent<Image>().color;
                    c.a = 0.5f;
                    child.GetComponent<Image>().color = c;
                }else{
                    c = child.GetComponent<TextMeshProUGUI>().color;
                    c.a = 0.5f;
                    child.GetComponent<TextMeshProUGUI>().color = c;
                }
            }
            industry_cards[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1].transform.GetComponent<Button>().interactable = false;


            addNewIndustryPanel.GetComponent<AddNewIndustryPanel>().new_industries_placed[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1] = true;
            gameController.GetComponent<gameController>().new_industries_placed[gameController.GetComponent<gameController>().new_industry_index - gameController.GetComponent<gameController>().existing_count - 1] = true;
            int k = 0;
            for (int i = 0; i < gameController.GetComponent<gameController>().new_count; i++){
                if(addNewIndustryPanel.GetComponent<AddNewIndustryPanel>().new_industries_placed[i])
                    k++;
            }
            if(k == gameController.GetComponent<gameController>().new_count)
                gameController.GetComponent<gameController>().all_new_industries_added = true;
            else gameController.GetComponent<gameController>().all_new_industries_added = false;

            gameController.GetComponent<gameController>().new_industries_added = true;
            gameController.GetComponent<gameController>().allowed_to_view_info = true;

            if(gameController.GetComponent<gameController>().tutorialOn){
                // tutorial.GetComponent<Tutorial>().placementOfNewIndustryTutorial.SetActive(false);
                // tutorial.GetComponent<Tutorial>().newIndustryInfoPanel.SetActive(true);
                // tutorial.GetComponent<Tutorial>().addNewButtonTutorial.SetActive(false);
                gameController.transform.GetComponent<gameController>().add_new_button.interactable = false;
                // newIndustryInfoPanel.SetActive(false);
                tutorial.GetComponent<Tutorial>().makeConnectionButtonTutorial.SetActive(true);
                tutorial.GetComponent<Tutorial>().arrow_ind = Instantiate(tutorial.GetComponent<Tutorial>().arrow_ind_prefab);
                tutorial.GetComponent<Tutorial>().arrow_ind.transform.SetParent(tutorial.GetComponent<Tutorial>().utility.transform, true);
                tutorial.GetComponent<Tutorial>().arrow_ind.transform.localPosition = new Vector3(0f, 1.2f, 0f);
                tutorial.GetComponent<Tutorial>().arrow_ind.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
                tutorial.GetComponent<Tutorial>().arrow_ind.transform.localScale *= app.GetComponent<App>().scale*2f;
                tutorial.GetComponent<Tutorial>().industry_bool = true;
                tutorial.GetComponent<Tutorial>().interactable = true;
                tutorial.GetComponent<Tutorial>().industry_secondTime = true;
                tutorial.GetComponent<Tutorial>().addNewButtonTutorial.SetActive(false);
            }

            placed = false;
            allow_to_place = false;
        }
    }

    public void back_bttn(){
        gameController.GetComponent<gameController>().placementOfNewIndustryPanel.SetActive(false);
        gameController.GetComponent<gameController>().addNewIndustryPanel.SetActive(true);

        if(placed){
            gameController.GetComponent<gameController>().industries[gameController.GetComponent<gameController>().new_industry_index].SetActive(false);
        }

        placed = false;
        allow_to_place = false;
    }

    public void question_bttn(){
        // gameController.GetComponent<gameController>().questionPlacementOfNewIndustryPanel.SetActive(true);
        gameController.GetComponent<gameController>().questionMainPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = false;
        confirm_button.interactable = false;
        back_button.interactable = false;
        allow_to_place = false;
    }

    public void ok_questioPanel_bttn(){
        // gameController.GetComponent<gameController>().questionPlacementOfNewIndustryPanel.SetActive(false);
        gameController.GetComponent<gameController>().questionMainPanel.SetActive(false);
        allow_to_place = true;
    }
}
