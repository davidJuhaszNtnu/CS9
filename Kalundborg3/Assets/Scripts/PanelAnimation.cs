using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public bool animate_forward, animate_backward, selected;

    public float t;
    private float sin_t, dt;
    private Vector3 start, end;
    private Transform parent;

    void Start()
    {
        animate_forward = false;
        animate_backward = false;
        selected = false;
        start = transform.localPosition;
        end = new Vector3(0f, start.y, start.z/2f);
        parent = transform.parent;
        t = 0;
        dt = 0.01f;
    }

    void Update()
    {
        if(animate_forward)
            AnimateForward();
        if(animate_backward)
            AnimateBackward();
    }

    public void AnimateForward(){
        sin_t = Mathf.Sin(t * Mathf.PI/2f);
        transform.localPosition = start + (end - start) * sin_t;
        if(t <= 1f)
            t += dt;
        else{
            animate_forward = false;
            foreach(Transform child in parent)
                child.transform.GetComponent<Button>().interactable = true;
            transform.localPosition = end;
        }
    }

    public void AnimateBackward(){
        sin_t = Mathf.Sin(t * Mathf.PI/2f);
        transform.localPosition = end + (start - end) * sin_t;
        if(t <= 1f)
            t += dt;
        else{
            animate_backward = false;
            foreach(Transform child in parent)
                child.transform.GetComponent<Button>().interactable = true;
            transform.localPosition = start;
        }
    }

    public void OnClick(){
        if(selected){
            foreach(Transform child in parent)
                child.transform.GetComponent<Button>().interactable = false;
            selected = false;
            animate_backward = true;
            foreach(Transform child in parent){
                if(child.name == "Panel2")
                    child.transform.SetAsLastSibling();
            }
            t = 0f;
        }else{
            foreach(Transform child in parent)
                child.transform.GetComponent<Button>().interactable = false;
            selected = true;
            animate_forward = true;
            t = 0f;
            transform.SetAsLastSibling();
            foreach(Transform child in parent){
                if(child.name != transform.name){
                    if(child.GetComponent<PanelAnimation>().selected){
                        child.transform.GetComponent<Button>().interactable = false;
                        child.transform.GetComponent<PanelAnimation>().selected = false;
                        child.transform.GetComponent<PanelAnimation>().animate_backward = true;
                        child.transform.GetComponent<PanelAnimation>().t = 0f;
                    }
                }
            }
        }
    }
}
