using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ISRUI : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI unitsText;
    public TextMeshProUGUI units;
    public TextMeshProUGUI alpha;
    public TextMeshProUGUI beta;
    public TextMeshProUGUI cost;
    public GameObject gameController;
    public Button backButton;
    public Button confirmButton;
    public Button RemoveButton;
    public Slider unitsSlider;
    public Slider alphaSlider;
    public Slider betaSlider;
    public Slider costSlider;
    public string type;

    public GameObject isrUI;
    public GameObject manipulateUI;
    public GameObject canvasMain;
    public Material materialOriginal;

    int from, to;

    void Start()
    {
        backButton.onClick.AddListener(Back);
        confirmButton.onClick.AddListener(Confirm);
        RemoveButton.onClick.AddListener(Remove);

        unitsSlider.onValueChanged.AddListener(delegate {Units(); });
        alphaSlider.onValueChanged.AddListener(delegate {Alpha(); });
        betaSlider.onValueChanged.AddListener(delegate {Beta(); });
        costSlider.onValueChanged.AddListener(delegate {Cost(); });
    }

    public void Units(){
        units.text=Math.Round(unitsSlider.value,2).ToString();
    }
    public void Alpha(){
        alpha.text=Math.Round(alphaSlider.value,2).ToString();
    }
    public void Beta(){
        beta.text=Math.Round(betaSlider.value,2).ToString();
    }
    public void Cost(){
        cost.text=Math.Abs(Math.Round(costSlider.value,2)).ToString();
        if(costSlider.value>=0)
            costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name+":";
        else costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name+":";
    }

    private void Back(){
        manipulateUI.SetActive(true);
        isrUI.SetActive(false);
    }
    private void Confirm(){
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;

        if(unitsSlider.value==0f){
            //alert window
        }else{
            switch(type){
                case "water":
                    gameController.GetComponent<Main>().waterMatrix[from, to]=unitsSlider.value;
                    gameController.GetComponent<Main>().alpha_water[from, to]=alphaSlider.value;
                    gameController.GetComponent<Main>().beta_water[from, to]=betaSlider.value;
                    gameController.GetComponent<Main>().ec_water[from, to]=costSlider.value;
                break;
                case "energy":
                    gameController.GetComponent<Main>().energyMatrix[from, to]=unitsSlider.value;
                    gameController.GetComponent<Main>().alpha_energy[from, to]=alphaSlider.value;
                    gameController.GetComponent<Main>().beta_energy[from, to]=betaSlider.value;
                    gameController.GetComponent<Main>().ec_energy[from, to]=costSlider.value;
                break;
                case "material":
                    gameController.GetComponent<Main>().materialMatrix[from, to]=unitsSlider.value;
                    gameController.GetComponent<Main>().alpha_material[from, to]=alphaSlider.value;
                    gameController.GetComponent<Main>().beta_material[from, to]=betaSlider.value;
                    gameController.GetComponent<Main>().ec_material[from, to]=costSlider.value;
                break;
            }

            //print(gameController.GetComponent<Main>().energyMatrix[from, to]);
            isrUI.SetActive(false);
            canvasMain.gameObject.SetActive(true);
            gameController.GetComponent<Main>().selected1 = false;
            gameController.GetComponent<Main>().selected2 = false;
            gameController.GetComponent<Main>().selectFirstText.gameObject.SetActive(true);
            canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(false);
            
            gameController.GetComponent<Main>().places[from].GetComponent<MeshRenderer> ().material = materialOriginal;
            gameController.GetComponent<Main>().places[to].GetComponent<MeshRenderer> ().material = materialOriginal;

            gameController.GetComponent<Main>().updateValueChains(from, to, type);
            gameController.GetComponent<Main>().computeENVandECO(from);
            gameController.GetComponent<Main>().computeENVandECO(to);
            gameController.GetComponent<Main>().touchable=true;

        }
    }
    private void Remove(){
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;
        
        switch(type){
            case "water":
                gameController.GetComponent<Main>().waterMatrix[from, to]=0f;
                gameController.GetComponent<Main>().alpha_water[from, to]=0f;
                gameController.GetComponent<Main>().beta_water[from, to]=0f;
                gameController.GetComponent<Main>().ec_water[from, to]=0f;
            break;
            case "energy":
                gameController.GetComponent<Main>().energyMatrix[from, to]=0f;
                gameController.GetComponent<Main>().alpha_energy[from, to]=0f;
                gameController.GetComponent<Main>().beta_energy[from, to]=0f;
                gameController.GetComponent<Main>().ec_energy[from, to]=0f;
            break;
            case "material":
                gameController.GetComponent<Main>().materialMatrix[from, to]=0f;
                gameController.GetComponent<Main>().alpha_material[from, to]=0f;
                gameController.GetComponent<Main>().beta_material[from, to]=0f;
                gameController.GetComponent<Main>().ec_material[from, to]=0f;
            break;
        }

        isrUI.SetActive(false);
        canvasMain.gameObject.SetActive(true);
        gameController.GetComponent<Main>().selected1 = false;
        gameController.GetComponent<Main>().selected2 = false;
        gameController.GetComponent<Main>().selectFirstText.gameObject.SetActive(true);
        canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(false);
        
        gameController.GetComponent<Main>().places[from].GetComponent<MeshRenderer> ().material = materialOriginal;
        gameController.GetComponent<Main>().places[to].GetComponent<MeshRenderer> ().material = materialOriginal;

        gameController.GetComponent<Main>().updateValueChains(from, to, type);
        gameController.GetComponent<Main>().computeENVandECO(from);
        gameController.GetComponent<Main>().computeENVandECO(to);
        gameController.GetComponent<Main>().touchable=true;
    }

    public void setSliders(float units, float alpha, float beta, float cost){
        unitsSlider.value=units;
        alphaSlider.value=alpha;
        betaSlider.value=beta;
        costSlider.value=cost;
    }
    public void setUnitMaxValue(float max){
        unitsSlider.maxValue=max;
    }
}
