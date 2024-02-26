using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InfoCardsPanel : MonoBehaviour
{
    public GameObject info_cards, mainPanel, infoCardsPanel, gameController, questionInfoCardPanel;
    public TextMeshProUGUI clean_amount_text, waste_amount_text, title, clean_text, waste_text;
    public Button ultimate_technology_button, question_button, ok_button;

    public int index_showed;
    bool UT_possible, UT_limit_reached;

    void Start()
    {
        UT_limit_reached = false;
    }

    public void restart(){
        UT_limit_reached = false;
        questionInfoCardPanel.SetActive(false);
    }

    void Update(){
        if(UT_possible){
            if(gameController.GetComponent<gameController>().has_technology[index_showed])
                ultimate_technology_button.interactable = true;
            else if(UT_limit_reached)
                ultimate_technology_button.interactable = false;
            else ultimate_technology_button.interactable = true;
        }else ultimate_technology_button.interactable = false;

        if(gameController.GetComponent<gameController>().tutorialOn)
            if(ultimate_technology_button.interactable)
                ultimate_technology_button.interactable = false;
        else if(!ultimate_technology_button.interactable)
            ultimate_technology_button.interactable = true;
    }

    public void set_info(int index, float clean, float waste, string name, bool UT, bool UT_reached){
        index_showed = index;
        UT_possible = UT;
        UT_limit_reached = UT_reached;
        // Debug.Log(index_showed);
        foreach(Transform card in info_cards.transform){
            card.gameObject.SetActive(false);
        }
        info_cards.transform.GetChild(index).gameObject.SetActive(true);

        title.text = name;

        if(index !=0 ){
            clean_amount_text.text = Math.Round(clean, 2).ToString() + " m<sup>3</sup>";
            waste_amount_text.text = Math.Round(waste, 2).ToString() + " m<sup>3</sup>";
            question_button.gameObject.SetActive(true);
        }else{
            question_button.gameObject.SetActive(false);
            clean_amount_text.gameObject.SetActive(false);
            waste_amount_text.gameObject.SetActive(false);
            clean_text.gameObject.SetActive(false);
            waste_text.gameObject.SetActive(false);
            ultimate_technology_button.gameObject.SetActive(false);
            // ut_info_text.gameObject.SetActive(false);
        }

        if(gameController.GetComponent<gameController>().has_technology[index_showed])
            ultimate_technology_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Remove Ultimate technology";
        else ultimate_technology_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use Ultimate technology";
    }

    public void close_bttn(){
        clean_amount_text.gameObject.SetActive(true);
        waste_amount_text.gameObject.SetActive(true);
        clean_text.gameObject.SetActive(true);
        waste_text.gameObject.SetActive(true);
        ultimate_technology_button.gameObject.SetActive(true);
        // ut_info_text.gameObject.SetActive(true);

        mainPanel.SetActive(true);
        
        if(index_showed != 0)
            gameController.GetComponent<gameController>().use_ultimate_technology(index_showed);
        
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        gameController.GetComponent<gameController>().count_number_of_UT();


        
        infoCardsPanel.SetActive(false);
    }

    public void ultimate_technology_bttn(){
        if(!gameController.GetComponent<gameController>().has_technology[index_showed]){
            info_cards.transform.GetChild(index_showed).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (100).ToString() + " %";
            info_cards.transform.GetChild(index_showed).transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.red;
            gameController.GetComponent<gameController>().has_technology[index_showed] = true;
            ultimate_technology_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Remove Ultimate technology";
        }else{
            info_cards.transform.GetChild(index_showed).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (gameController.GetComponent<gameController>().s_old[index_showed]*100).ToString() + " %";
            info_cards.transform.GetChild(index_showed).transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.black;
            gameController.GetComponent<gameController>().has_technology[index_showed] = false;
            ultimate_technology_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use Ultimate technology";
        }
    }

    public void question_bttn(){
        questionInfoCardPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameController.GetComponent<gameController>().industries[index_showed].name;
        questionInfoCardPanel.SetActive(true);
        infoCardsPanel.SetActive(false);
        gameController.GetComponent<gameController>().allowed_to_view_info = false;
    }

    public void ok_questionPanel_bttn(){
        questionInfoCardPanel.SetActive(false);
        infoCardsPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
    }

}
