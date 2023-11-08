using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MainCanvasUI : MonoBehaviour
{
    public Button escapeRoomButton;
    public Button exitButton;

    public GameObject env;
    public GameObject imageTracking;
    public GameObject mainCanvasUI;
    public GameObject canvasMain;
    public GameObject listScroll;
    public GameObject arrow;
    public GameObject gameController;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI loadingText;
    public GameObject arSessionOrigin;
    public GameObject visualObject;
    public GameObject pointCloud;
    public bool envSet;

    void Start()
    {
        escapeRoomButton.onClick.AddListener(EscapeRoomB);
        exitButton.onClick.AddListener(Exit);

        env.SetActive(false);
        mainCanvasUI.SetActive(true);
        imageTracking.SetActive(true);
        canvasMain.SetActive(false);
        selectText.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);
        envSet=false;
        //escapeRoomButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Select placement";
        //arrow.SetActive(false);
    }

    private void EscapeRoomB(){      
        if(envSet){
            gameController.GetComponent<Main>().fromNow=Time.time;
            gameController.GetComponent<Main>().isActive=true;
            mainCanvasUI.SetActive(false);
            env.SetActive(true);
            canvasMain.SetActive(true);
            imageTracking.SetActive(false);
            visualObject.SetActive(false);
            arrow.SetActive(true);
        }else{
            visualObject.SetActive(true);
            selectText.gameObject.SetActive(true);
            arSessionOrigin.GetComponent<ARPointCloudManager>().pointCloudPrefab=pointCloud;
            arSessionOrigin.GetComponent<PlaceOnPlane>().planeDetectionEnabled=true;
            escapeRoomButton.gameObject.SetActive(false);
        }
    }
    private void Exit(){
        Application.Quit();
    }
}
