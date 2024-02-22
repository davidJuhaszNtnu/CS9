using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager;

    public bool gotFirst, gotSecond, gotBoth, first_time_playing;
    Vector3 firstPoint, secondPoint, direction;
    public float scale;

    public GameObject environment, gameController, floor, appPanel, calibrationPanel, welcomePanel, infographics, app, tutorial;
    public infographicsController infographicsController;
    public Sprite firstPoint_image, secondPoint_image;
    public Image calibration_image;

    // private GameObject aRSessionOrigin;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        arRaycastManager = GetComponent<ARRaycastManager>();
        // aRSessionOrigin = GameObject.Find("AR Session Origin");

        gotFirst = false;
        gotSecond = false;
        gotBoth = false;
        first_time_playing = true;
        calibrationPanel.SetActive(false);
        gameController.GetComponent<gameController>().mainPanel.SetActive(false);
        gameController.SetActive(false);
        environment.SetActive(false);
        appPanel.SetActive(false);
        tutorial.SetActive(false);

        infographics.SetActive(true);
        welcomePanel.SetActive(true);

        //-----

        // firstPoint = new Vector3(0f, 0f, 0f);
        // secondPoint = new Vector3(0f, 0f, 0.2f);
        // gotBoth = true;

        // infographics.SetActive(false);
        // welcomePanel.SetActive(false);
        // calibrate();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if(!gotBoth){
                        if(hitObject.collider.tag == "point1" && !gotSecond){
                            gotFirst = true;
                            firstPoint = hitObject.transform.position;

                            environment.transform.position = firstPoint;
                            calibration_image.sprite = secondPoint_image;
                        }
                        if(hitObject.collider.tag == "point2" && gotFirst){
                            gotSecond = true;
                            gotBoth = true;
                            secondPoint = hitObject.transform.position;
                            calibrate();
                        }
                    }
                }
            } 
        }
    }

    public void calibrate(){
        direction = secondPoint - firstPoint;
        direction = new Vector3(direction.x, 0f, direction.z);
        float offsetX, offsetY;
        offsetX = -0.12f;
        offsetY = -0.1f;

        environment.transform.position += Vector3.Normalize(Quaternion.Euler(0, 90, 0) * direction) * (1.5f + offsetX);
        environment.transform.position += (direction.normalized) * (floor.transform.localScale.z + offsetY);
        scale = 1f;

        gameController.SetActive(true);
        environment.SetActive(true);

        environment.transform.localScale *= scale;
        // environment.transform.position = firstPoint + new Vector3(-0.5f, -0.5f, 2f);
        environment.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        gameController.GetComponent<gameController>().start();
        if(first_time_playing)
            gameController.GetComponent<gameController>().setup_game();
        else gameController.GetComponent<gameController>().restart();

        if (environment.GetComponent<ARAnchor>() == null)
            environment.AddComponent<ARAnchor>();

        gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        calibrationPanel.SetActive(false);
        if(first_time_playing){
            gameController.GetComponent<gameController>().tutorialOn = true;
            tutorial.SetActive(true);
        }
    }

    public void questionmark_bttn(){
        welcomePanel.SetActive(true);
        appPanel.SetActive(false);
    }

    public void escapeRoom_bttn(){
        appPanel.SetActive(false);
        infographics.SetActive(false);
        calibrationPanel.SetActive(true);
        calibration_image.sprite = firstPoint_image;
        foreach(GameObject industry_model in infographicsController.industry_models)
            industry_model.SetActive(false);
    }

    public void start_bttn(){
        welcomePanel.SetActive(false);
        appPanel.SetActive(true);
    }

    public void exit_bttn(){
        Application.Quit();
    }
}
