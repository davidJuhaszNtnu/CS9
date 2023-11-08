using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConnectionPanel : MonoBehaviour
{
    public GameObject[] cards;
    public GameObject left, right, portrait_text, landscape_text, cards_parent, connectionPanel, mainPanel, chooseIndustriesPanel, gameController;
    public GameObject amount_text_object;
    public Slider slider;
    public TextMeshProUGUI min_text, max_text;

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
        cards[from_index].SetActive(true);

        cards[to_index].transform.SetParent(right.transform, true);
        cards[to_index].transform.localPosition = Vector3.zero;
        cards[to_index].SetActive(true);

        amount_text_object.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cards[to_index].name + " receives:";
        amount_text_object.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Math.Round(value, 2).ToString();
        min_text.text = Math.Round(min_value, 2).ToString() + " m3";
        max_text.text = Math.Round(max_value, 2).ToString() + " m3";

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
        if(slider.value < 1E-05)
            slider.value = 0f;
        // Debug.Log("slider value: " + slider.value);
        gameController.GetComponent<gameController>().make_connection(slider.value, from_index + 1, to_index + 1);
        mainPanel.SetActive(true);
        connectionPanel.SetActive(false);
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
