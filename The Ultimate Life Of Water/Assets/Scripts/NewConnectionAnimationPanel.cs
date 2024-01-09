using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class NewConnectionAnimationPanel : MonoBehaviour
{
    public TextMeshProUGUI reduction_in_text, reduction_out_text;
    public GameObject newConectionAnimationPanel, mainPanel, rectangle, gameController, arrow_in, arrow_out, rectangle_in, rectangle_out, tutorial;
    public Sprite arrow_up, arrow_down;
    public Button close_button;

    bool animation_in_on, animation_out_on;
    // for the number
    float in_from_value, in_to_value, out_from_value, out_to_value, t_in, t_out, in_value, out_value;
    float dt_in, dt_out;

    // for the rectangle
    float in_from_value_rectangle, in_to_value_rectangle, out_from_value_rectangle, out_to_value_rectangle, in_value_rectangle, out_value_rectangle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animation_in_on)
            animate_in();
        if(animation_out_on){
            animate_out();
        }
    }

    void animate_in(){
        
        if(t_in < 1f){
            t_in += dt_in;
            in_value = in_from_value + (in_to_value - in_from_value) * t_in;
            in_value_rectangle = in_from_value_rectangle + (in_to_value_rectangle - in_from_value_rectangle) * t_in;
            reduction_in_text.text = Math.Round(in_value * 100, 2).ToString() + "%";
            if(in_value_rectangle >= 1f)
                in_value_rectangle = 1f;
            rectangle_in.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, in_value_rectangle);
        }else{
            animation_in_on = false;
            if(!animation_out_on)
                close_button.interactable = true;
        }
    }

    void animate_out(){
        
        if(t_out < 1f){
            t_out += dt_out;
            out_value = out_from_value + (out_to_value - out_from_value) * t_out;
            out_value_rectangle = out_from_value_rectangle + (out_to_value_rectangle - out_from_value_rectangle) * t_out;
            reduction_out_text.text = Math.Round(out_value * 100, 2).ToString() + "%";
            if(out_value_rectangle >= 1f)
                out_value_rectangle = 1f;
            rectangle_out.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, out_value_rectangle);
        }else{
            animation_out_on = false;
            if(!animation_in_on)
                close_button.interactable = true;
        }
    }

    public void setup_animation(float in_from, float in_to, float out_from, float out_to){
        //change the color of rectangles to green when achieving the max reduction
        if(in_to >= gameController.GetComponent<gameController>().max_reduction_clean)
            rectangle_in.GetComponent<Image>().color = new Color(0, 255, 0, 255);
        else rectangle_in.GetComponent<Image>().color = new Color(0, 0, 255, 255);
        if(out_to >= gameController.GetComponent<gameController>().max_reduction_waste)
            rectangle_out.GetComponent<Image>().color = new Color(0, 255, 0, 255);
        else rectangle_out.GetComponent<Image>().color = new Color(255, 0, 255, 255);

        if(Math.Abs(in_to - in_from) < 1E-05)
            animation_in_on = false;
        else animation_in_on = true;
        if(Math.Abs(out_to - out_from) < 1E-05)
            animation_out_on = false;
        else animation_out_on = true;
        // Debug.Log(animation_in_on + ", " + animation_out_on);

        in_from_value = in_from;
        in_to_value = in_to;
        out_from_value = out_from;
        out_to_value = out_to;
        t_in = 0f;
        t_out = 0f;
        // dt_in = (in_to_value - in_from_value) * 0.04f;
        // dt_out = (out_to_value - out_from_value) * 0.04f;
        dt_in = 0.04f;
        dt_out = 0.04f;

        reduction_in_text.text = Math.Round(in_from_value * 100, 2).ToString() + "%";
        reduction_out_text.text = Math.Round(out_from_value * 100, 2).ToString() + "%";
        // max reduction = 1, current reduction = ?
        in_from_value_rectangle = in_from / gameController.GetComponent<gameController>().max_reduction_clean;
        out_from_value_rectangle = out_from / gameController.GetComponent<gameController>().max_reduction_waste;
        in_to_value_rectangle = in_to / gameController.GetComponent<gameController>().max_reduction_clean;
        out_to_value_rectangle = out_to / gameController.GetComponent<gameController>().max_reduction_waste;

        // if there is more reduction than max reduction
        if(in_from_value_rectangle >= 1f)
            in_from_value_rectangle = 1f;
        if(out_from_value_rectangle >= 1f)
            out_from_value_rectangle = 1f;

        rectangle_in.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, in_from_value_rectangle);
        rectangle_out.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, out_from_value_rectangle);

        if(in_to > in_from)
            arrow_in.GetComponent<Image>().sprite = arrow_up;
        else arrow_in.GetComponent<Image>().sprite = arrow_down;
        if(animation_in_on)
            arrow_in.SetActive(true);
        else arrow_in.SetActive(false);

        if(out_to > out_from){
            arrow_out.GetComponent<Image>().sprite = arrow_up;
        }else arrow_out.GetComponent<Image>().sprite = arrow_down;
        if(animation_out_on)
            arrow_out.SetActive(true);
        else arrow_out.SetActive(false);

        close_button.interactable = false;
    }

    public void close_bttn(){
        gameController.GetComponent<gameController>().check_if_winning();
        mainPanel.SetActive(true);
        newConectionAnimationPanel.SetActive(false);
        
        if(gameController.GetComponent<gameController>().tutorialOn){
            tutorial.GetComponent<Tutorial>().connectionAnimationPanel.SetActive(true);
            tutorial.GetComponent<Tutorial>().connectionAnimationPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "You have created a connection.\n\nYou have achieved a waste water reduction of " + Math.Round(out_to_value * 100, 2).ToString() + 
            "%. This means that so much waste water is now recycled in our symbiosis.\n\nThis is good news for the environment.";
        }
    }

    public void question_bttn(){
        gameController.GetComponent<gameController>().questionNewConnectionAnimationPanel.SetActive(true);
    }

    public void ok_questionPanel_bttn(){
        gameController.GetComponent<gameController>().questionNewConnectionAnimationPanel.SetActive(false);
    }
}
