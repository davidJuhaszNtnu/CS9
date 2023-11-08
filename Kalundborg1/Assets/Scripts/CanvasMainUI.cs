using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Touch;

public class CanvasMainUI : MonoBehaviour
{
    public Button addButton;
    public Button exitButton;
    public Button restartButton;
    public Button listButton;
    bool listOn;
    public Canvas canvasMain;
    public GameObject chooseNewUI;
    public GameObject legend;
    public GameObject env;
    public GameObject gameController;
    public TextMeshProUGUI envEcoText;
    public GameObject listScroll;
    public GameObject verticalScrollBar;

    public GameObject imageTracking;
    public GameObject mainCanvasUI;
    public GameObject arrow;

    void Start()
    {
        addButton.onClick.AddListener(Add);
        restartButton.onClick.AddListener(Restart);
        listButton.onClick.AddListener(List);
        exitButton.onClick.AddListener(Exit);
        verticalScrollBar.SetActive(true);
        listScroll.gameObject.SetActive(false);
    }

    private void List(){
        //Debug.Log("heading is "+Input.compass.magneticHeading+" "+Input.compass.trueHeading);
        if(listOn==false){
            gameController.GetComponent<Main>().touchable=false;
            legend.SetActive(false);
            listScroll.SetActive(true);
            listButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Hide list of places.";
            listOn=true;
            restartButton.interactable=false;
            addButton.interactable=false;
        }else{
            legend.SetActive(true);
            gameController.GetComponent<Main>().touchable=true;
            listScroll.SetActive(false);
            listButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Show list of places.";
            listOn=false;
            restartButton.interactable=true;
            addButton.interactable=true;
        }
        
    }

    private void Restart(){
         gameController.GetComponent<Main>().reset();
    }

    private void Add(){
        gameController.GetComponent<Main>().touchable=false;
        chooseNewUI.SetActive(true);
        listScroll.SetActive(false);
        legend.SetActive(false);
        canvasMain.gameObject.SetActive(false);
        chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex=-1;
        gameController.GetComponent<Main>().selectSecondText.gameObject.SetActive(false);
        gameController.GetComponent<Main>().selectFirstText.gameObject.SetActive(true);
        gameController.GetComponent<Main>().selected1=false;
        gameController.GetComponent<Main>().selected2=false;
        gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].GetComponent<MeshRenderer> ().material = gameController.GetComponent<Main>().materialOriginal;
        gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].GetComponent<MeshRenderer> ().material = gameController.GetComponent<Main>().materialOriginal;
        
        
    }

    private void Exit(){
        //mainCanvasUI.GetComponent<MainCanvasUI>().escapeRoomButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Escape Room";
        gameController.GetComponent<Main>().touchable=true;
        env.SetActive(false);
        imageTracking.SetActive(true);
        canvasMain.gameObject.SetActive(false);
        mainCanvasUI.SetActive(true);
        arrow.SetActive(false);
    }
}
