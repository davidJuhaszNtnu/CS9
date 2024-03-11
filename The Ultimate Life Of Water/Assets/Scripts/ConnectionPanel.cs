using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConnectionPanel : MonoBehaviour
{
    public GameObject[] cards;
    public GameObject left, right, portrait_text, landscape_text, cards_parent, connectionPanel, mainPanel, chooseIndustriesPanel, gameController, infoCardsPanel;
    public GameObject amount_text_object;
    public Slider slider;
    public TextMeshProUGUI min_text, max_text;
    public Button back_button, confirm_button;

    int from_index, to_index;

    void Start()
    {

    }

    public void setup_connection(int from, int to, float value, float max_value, float min_value){
        //-1 because of there is no distribution industry here
        from_index = from - 1;
        to_index = to - 1;
        foreach(GameObject card in cards)
            card.SetActive(false);
        cards[from_index].transform.SetParent(left.transform, true);
        cards[from_index].transform.localPosition = Vector3.zero;
        cards[from_index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameController.GetComponent<gameController>().industries[from].name;
        //ultimate technology
        if(gameController.GetComponent<gameController>().has_technology[from]){
            cards[from_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "100 %";
            cards[from_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.red;
        }else{
            cards[from_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (gameController.GetComponent<gameController>().s_old[from]*100).ToString() + " %";
            cards[from_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        cards[from_index].SetActive(true);

        cards[to_index].transform.SetParent(right.transform, true);
        cards[to_index].transform.localPosition = Vector3.zero;
        cards[to_index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameController.GetComponent<gameController>().industries[to].name;
        //ultimate technology
        if(gameController.GetComponent<gameController>().has_technology[to]){
            cards[to_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "100 %";
            cards[to_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.red;
        }else{
            cards[to_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (gameController.GetComponent<gameController>().s_old[to]*100).ToString() + " %";
            cards[to_index].transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        cards[to_index].SetActive(true);

        amount_text_object.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cards[to_index].name + " receives:";
        amount_text_object.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Math.Round(value, 2).ToString();
        min_text.text = Math.Round(min_value, 2).ToString() + " m<sup>3</sup>";
        max_text.text = Math.Round(max_value, 2).ToString() + " m<sup>3</sup>";

        slider.maxValue = max_value;
        slider.minValue = min_value;
        slider.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.orientation == ScreenOrientation.Portrait){
            amount_text_object.transform.SetParent(portrait_text.transform, true);
            amount_text_object.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            amount_text_object.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight){
            amount_text_object.transform.SetParent(landscape_text.transform, true);
            amount_text_object.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            amount_text_object.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }

        if(gameController.GetComponent<gameController>().tutorialOn)
            if(slider.value < 1E-05)
                confirm_button.interactable = false;
            else confirm_button.interactable = true;
    }

    public void back_bttn(){
        cards[from_index].transform.SetParent(cards_parent.transform, true);
        cards[to_index].transform.SetParent(cards_parent.transform, true);
        foreach(GameObject card in cards)
            card.SetActive(false);
        mainPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        connectionPanel.SetActive(false);
        chooseIndustriesPanel.GetComponent<ChooseIndustriesPanel>().back();
    }

    public void confirm_bttn(){
        cards[from_index].transform.SetParent(cards_parent.transform, true);
        cards[to_index].transform.SetParent(cards_parent.transform, true);
        foreach(GameObject card in cards)
            card.SetActive(false);

        chooseIndustriesPanel.GetComponent<ChooseIndustriesPanel>().back();
        if(slider.value < 1E-05){
            slider.value = 0f;
            gameController.GetComponent<gameController>().allowed_to_view_info = true;
        }
        // Debug.Log("slider value: " + slider.value);
        gameController.GetComponent<gameController>().make_connection(slider.value, from_index + 1, to_index + 1);
        // mainPanel.SetActive(true);
        connectionPanel.SetActive(false);
        if(gameController.GetComponent<gameController>().tutorialOn)
            infoCardsPanel.transform.GetComponent<InfoCardsPanel>().ok_button.interactable = true;
    }

    public void slider_change(){
        amount_text_object.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cards[to_index].name + " receives:";
        amount_text_object.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Math.Round(slider.value,2).ToString();
    }

    public void question_bttn(){
        gameController.GetComponent<gameController>().questionConnectionPanel.SetActive(true);
    }

    public void ok_questionPanel_bttn(){
        gameController.GetComponent<gameController>().questionConnectionPanel.SetActive(false);
    }
}
