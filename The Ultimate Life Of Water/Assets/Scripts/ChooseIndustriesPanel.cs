using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseIndustriesPanel : MonoBehaviour
{
    public Camera arCamera;
    public GameObject gameController, chooseIndustriesPanel, connectionPanel, connectionWarningPanel, tutorial;
    public TextMeshProUGUI from_text, to_text;
    public Button confirm_button;
    public Material material_selected, material_flash;

    Ray ray;
    RaycastHit hit;
    int from, to;
    bool selected_to;
    bool allowed_to_choose;

    void Start()
    {
        restart();
    }

    public void restart(){
        selected_to = false;
        allowed_to_choose = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(!selected_to && allowed_to_choose){
                    foreach(GameObject industry in gameController.GetComponent<gameController>().industries){
                        if(hit.collider.name == industry.name && gameController.GetComponent<gameController>().industries.IndexOf(industry) != from && gameController.GetComponent<gameController>().industries.IndexOf(industry) != 0){
                            to = gameController.GetComponent<gameController>().industries.IndexOf(industry);
                            if(from > gameController.GetComponent<gameController>().existing_count || to > gameController.GetComponent<gameController>().existing_count){
                                selected_to = true;
                                if(to > gameController.GetComponent<gameController>().existing_count)
                                    gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = true;
                                industry.transform.GetChild(2).GetComponent<MeshRenderer> ().material = material_selected;
                                to_text.text = industry.name;
                            }
                        }
                    }
                }
            }
        }

        if(selected_to)
            confirm_button.interactable = true;
        else confirm_button.interactable = false;
    }

    public void setup_connection(int f){
        from = f;
        GameObject industry = gameController.GetComponent<gameController>().industries[from];
        if(from <= gameController.GetComponent<gameController>().existing_count)
            industry.transform.GetChild(2).gameObject.SetActive(true);
        else industry.transform.GetChild(2).GetComponent<Renderer>().material = material_selected;
        if(from > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = true;
        from_text.text = industry.name;
    }

    public void back_bttn(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(false);
        allowed_to_choose = true;
        if(from <= gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industries[from].transform.GetChild(2).gameObject.SetActive(false);
        else gameController.GetComponent<gameController>().industries[from].transform.GetChild(2).GetComponent<Renderer>().material = material_flash;
        if(from > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = false;
        from_text.text = " ";

        if(selected_to){
            if(to <= gameController.GetComponent<gameController>().existing_count)
                gameController.GetComponent<gameController>().industries[to].transform.GetChild(2).gameObject.SetActive(false);
            if(to > gameController.GetComponent<gameController>().existing_count)
                gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = false;
            to_text.text = " ";
            selected_to = false;
        }
        from = 99;
        to = 99;

        // gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        gameController.GetComponent<gameController>().infoCardsPanel.SetActive(true);
        if(gameController.GetComponent<gameController>().tutorialOn)
            tutorial.GetComponent<Tutorial>().makeConnectionButtonTutorial.SetActive(true);
        chooseIndustriesPanel.SetActive(false);
    }

    public void back(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(false);
        allowed_to_choose = true;
        if(from <= gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industries[from].transform.GetChild(2).gameObject.SetActive(false);
        else gameController.GetComponent<gameController>().industries[from].transform.GetChild(2).GetComponent<Renderer>().material = material_flash;
        if(from > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = false;
        from_text.text = " ";
        
        if(to <= gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industries[to].transform.GetChild(2).gameObject.SetActive(false);
        if(to > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = false;
        to_text.text = " ";
        selected_to = false;

        from = 99;
        to = 99;
    }

    public void confirm_bttn(){
        if(gameController.GetComponent<gameController>().tutorialOn)
            tutorial.GetComponent<Tutorial>().industry_bool = false;
        float current_value = gameController.GetComponent<gameController>().waste[from, to];
        float max_value = (gameController.GetComponent<gameController>().out_waste[from] + gameController.GetComponent<gameController>().waste[from, to]);
        float min_value;
        float c = (1-gameController.GetComponent<gameController>().s[to] + gameController.GetComponent<gameController>().s[to] * gameController.GetComponent<gameController>().w[to]);
        min_value = gameController.GetComponent<gameController>().waste[from, to] - gameController.GetComponent<gameController>().out_waste[to]/c;
        if(min_value < 0f)
            min_value = 0f;
        if(min_value < 1E-05)
            min_value = 0f;
        
        // Debug.Log(from + " " + to + ", " + min_value + ", " +  max_value);
        
        if(max_value - min_value < 1E-05){
            //if you can't move the slider
            connectionWarningPanel.SetActive(true);
            if(max_value < 1E-05)
                connectionWarningPanel.GetComponent<ConnectionWarningPanel>().set_warning("There is not enough waste water to exchange");
            else connectionWarningPanel.GetComponent<ConnectionWarningPanel>().set_warning("This connection can't be modified. Try to remove or modify some of the outgoing connections of the receiver first.");
            chooseIndustriesPanel.SetActive(false);
        }else if(gameController.GetComponent<gameController>().waste[to, from] != 0f){
            // if there is already a connection
            connectionWarningPanel.SetActive(true);
            connectionWarningPanel.GetComponent<ConnectionWarningPanel>().set_warning("There is already a connection between these industries");
            chooseIndustriesPanel.SetActive(false);
        }else{
            connectionPanel.SetActive(true);
            // to display clean water instead of waste
            current_value *= gameController.GetComponent<gameController>().s[to];
            max_value *= gameController.GetComponent<gameController>().s[to];
            min_value *= gameController.GetComponent<gameController>().s[to];
            connectionPanel.GetComponent<ConnectionPanel>().setup_connection(from, to, current_value, max_value, min_value);

            chooseIndustriesPanel.SetActive(false);
            if(gameController.GetComponent<gameController>().tutorialOn){
                connectionPanel.GetComponent<ConnectionPanel>().back_button.interactable = false;
            }else connectionPanel.GetComponent<ConnectionPanel>().back_button.interactable = true;
        }
    }

    public void question_bttn(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(true);
        allowed_to_choose = false;
        chooseIndustriesPanel.SetActive(false);
    }

    public void ok_questionPanel_bttn(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(false);
        allowed_to_choose = true;
        chooseIndustriesPanel.SetActive(true);
    }
}
