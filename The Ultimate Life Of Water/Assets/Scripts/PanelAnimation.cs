using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public bool animate_forward, animate_backward, selected, interactable;

    public float t;
    private float sin_t, dt;
    private Vector3 start, end;
    private Transform parent;
    public GameObject panel1, panel2, panel3;

    Ray ray;
    RaycastHit hit;
    Camera arCamera;

    void Start()
    {
        animate_forward = false;
        animate_backward = false;
        selected = false;
        start = transform.localPosition;
        end = new Vector3(0f, start.y, 0.6f);
        parent = transform.parent;
        t = 0;
        dt = 0.03f;
        arCamera = Camera.main;
        interactable = true;
    }

    void Update()
    {
        if(animate_forward)
            AnimateForward();
        if(animate_backward)
            AnimateBackward();

        if(Input.GetMouseButtonDown(0)){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit) && interactable){
                if(hit.collider.tag == "panel1")
                    panel1.GetComponent<PanelAnimation>().OnClick();
                if(hit.collider.tag == "panel2")
                    panel2.GetComponent<PanelAnimation>().OnClick();
                if(hit.collider.tag == "panel3")
                    panel3.GetComponent<PanelAnimation>().OnClick();
            }
        }
    }

    public void AnimateForward(){
        sin_t = Mathf.Sin(t * Mathf.PI/2f);
        transform.localPosition = start + (end - start) * sin_t;
        //rotate
        if(transform.name == "Panel1")
            transform.Rotate(0f, -1f, 0f, Space.Self);
        if(transform.name == "Panel3")
            transform.Rotate(0f, 1f, 0f, Space.Self);
        if(t <= 1f)
            t += dt;
        else{
            animate_forward = false;
            foreach(Transform child in parent)
                child.GetComponent<PanelAnimation>().interactable = true;
                // child.transform.GetComponent<Button>().interactable = true;
            transform.localPosition = end;
        }
    }

    public void AnimateBackward(){
        sin_t = Mathf.Sin(t * Mathf.PI/2f);
        transform.localPosition = end + (start - end) * sin_t;
        //rotate
        if(transform.name == "Panel1")
            transform.Rotate(0f, 1f, 0f, Space.Self);
        if(transform.name == "Panel3")
            transform.Rotate(0f, -1f, 0f, Space.Self);
        if(t <= 1f)
            t += dt;
        else{
            bool someIsSelected = false;
            foreach(Transform child in parent){
                if(child.gameObject.GetComponent<PanelAnimation>().selected)
                    someIsSelected = true;
            }
            foreach(Transform child in parent){
                if(child.name == "Panel2" && !someIsSelected)
                        child.transform.SetAsLastSibling();
            }
            animate_backward = false;
            foreach(Transform child in parent)
                child.GetComponent<PanelAnimation>().interactable = true;
                // child.transform.GetComponent<Button>().interactable = true;
            transform.localPosition = start;
        }
    }

    public void OnClick(){
        if(selected){
            foreach(Transform child in parent)
                child.GetComponent<PanelAnimation>().interactable = false;
                // child.transform.GetComponent<Button>().interactable = false;
            selected = false;
            animate_backward = true;
            t = 0f;
        }else{
            foreach(Transform child in parent)
                child.GetComponent<PanelAnimation>().interactable = false;
                // child.transform.GetComponent<Button>().interactable = false;
            selected = true;
            animate_forward = true;
            t = 0f;
            transform.SetAsLastSibling();
            foreach(Transform child in parent){
                if(child.name != transform.name){
                    if(child.GetComponent<PanelAnimation>().selected){
                        child.GetComponent<PanelAnimation>().interactable = false;
                        // child.transform.GetComponent<Button>().interactable = false;
                        child.transform.GetComponent<PanelAnimation>().selected = false;
                        child.transform.GetComponent<PanelAnimation>().animate_backward = true;
                        child.transform.GetComponent<PanelAnimation>().t = 0f;
                    }
                }
            }
        }
    }

    public void restart(){
        foreach(Transform child in parent){
            if(child.GetComponent<PanelAnimation>().selected){
                child.GetComponent<PanelAnimation>().interactable = false;
                // child.transform.GetComponent<Button>().interactable = false;
                child.transform.GetComponent<PanelAnimation>().selected = false;
                child.transform.GetComponent<PanelAnimation>().animate_backward = true;
                child.transform.GetComponent<PanelAnimation>().t = 0f;
            }
        }
    }
}
