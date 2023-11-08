using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseIndustriesPanel : MonoBehaviour
{
    public Camera arCamera;
    public GameObject gameController, chooseIndustriesPanel, connectionPanel, connectionWarningPanel;
    public TextMeshProUGUI from_text, to_text;
    public Button confirm_button;
    public Material material_selected;

    Ray ray;
    RaycastHit hit;
    int from, to;
    bool selected_from, selected_to;
    Material from_material, to_material;

    void Start()
    {
        restart();
    }

    public void restart(){
        selected_from = false;
        selected_to = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(selected_from == false && selected_to == false){
                    foreach(GameObject industry in gameController.GetComponent<gameController>().industries){
                        if(hit.collider.name == industry.name && hit.collider.name != "Distribution Industry"){
                            selected_from = true;
                            from = gameController.GetComponent<gameController>().industries.IndexOf(industry);
                            from_material = industry.GetComponent<MeshRenderer> ().material;
                            if(from > gameController.GetComponent<gameController>().existing_count)
                                gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = true;
                            industry.GetComponent<MeshRenderer> ().material = material_selected;
                            from_text.text = industry.name;
                        }
                    }
                }else if(selected_from == true && selected_to == false){
                    foreach(GameObject industry in gameController.GetComponent<gameController>().industries){
                        if(hit.collider.name == industry.name && gameController.GetComponent<gameController>().industries.IndexOf(industry) != from){
                            to = gameController.GetComponent<gameController>().industries.IndexOf(industry);
                            if(from > gameController.GetComponent<gameController>().existing_count || to > gameController.GetComponent<gameController>().existing_count){
                                selected_to = true;
                                if(to > gameController.GetComponent<gameController>().existing_count)
                                    gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = true;
                                to_material = industry.GetComponent<MeshRenderer> ().material;
                                industry.GetComponent<MeshRenderer> ().material = material_selected;
                                to_text.text = industry.name;
                            }
                        }
                    }
                }
            }
        }

        if(selected_from && selected_to)
            confirm_button.interactable = true;
        else confirm_button.interactable = false;
    }

    public void back_bttn(){
        if(selected_from){
            gameController.GetComponent<gameController>().industries[from].GetComponent<MeshRenderer> ().material = from_material;
            if(from > gameController.GetComponent<gameController>().existing_count)
                gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = false;
            from_text.text = " ";
            selected_from = false;
        }
        if(selected_to){
            gameController.GetComponent<gameController>().industries[to].GetComponent<MeshRenderer> ().material = to_material;
            if(to > gameController.GetComponent<gameController>().existing_count)
                gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = false;
            to_text.text = " ";
            selected_to = false;
        }
        from = 99;
        to = 99;

        gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        chooseIndustriesPanel.SetActive(false);
    }

    public void back(){
        gameController.GetComponent<gameController>().industries[from].GetComponent<MeshRenderer> ().material = from_material;
        if(from > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[from - gameController.GetComponent<gameController>().existing_count -1] = false;
        from_text.text = " ";
        selected_from = false;

        gameController.GetComponent<gameController>().industries[to].GetComponent<MeshRenderer> ().material = to_material;
        if(to > gameController.GetComponent<gameController>().existing_count)
            gameController.GetComponent<gameController>().industry_chosen[to - gameController.GetComponent<gameController>().existing_count -1] = false;
        to_text.text = " ";
        selected_to = false;

        from = 99;
        to = 99;
    }

    public void confirm_bttn(){
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
            else connectionWarningPanel.GetComponent<ConnectionWarningPanel>().set_warning("This connection can't be modified");
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
        }
    }

    public void question_bttn(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(true);
    }

    public void ok_questionPanel_bttn(){
        gameController.GetComponent<gameController>().questionChooseIndustriesPanel.SetActive(false);
    }
}
