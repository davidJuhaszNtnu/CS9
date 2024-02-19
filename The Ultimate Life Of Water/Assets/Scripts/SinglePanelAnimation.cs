using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePanelAnimation : MonoBehaviour
{
    public bool animate_forward, animate_backward, selected;

    public float t;
    private float sin_t, dt;
    private Vector3 start, end;

    Ray ray;
    RaycastHit hit;
    Camera arCamera;

    void Start()
    {
        animate_forward = false;
        animate_backward = false;
        start = transform.localPosition;
        end = new Vector3(0f, start.y, 0.4f);
        t = 0;
        dt = 0.03f;
        selected = false;
        arCamera = Camera.main;
    }

    void Update()
    {
        if(animate_forward)
            AnimateForward();
        if(animate_backward)
            AnimateBackward();

        if(Input.GetMouseButtonDown(0)){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(hit.collider.tag == "panel2")
                    OnClick();
            }
        }
    }

    public void AnimateForward(){
        sin_t = Mathf.Sin(t * Mathf.PI/2f);
        transform.localPosition = start + (end - start) * sin_t;
        if(t <= 1f)
            t += dt;
        else{
            animate_forward = false;
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
            transform.localPosition = start;
        }
    }

    public void OnClick(){
        if(selected){
            animate_backward = true;
            t = 0f;
            selected = false;
        }else{
            animate_forward = true;
            t = 0f;
            selected = true;
        }
    }

    public void restart(){
        selected = false;
        animate_backward = true;
        t = 0f;
    }
}
