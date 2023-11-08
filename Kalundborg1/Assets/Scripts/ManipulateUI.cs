using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ManipulateUI : MonoBehaviour
{
    public Button waterButton;
    public Button energyButton;
    public Button materialButton;
    public Button backButton;
    public TextMeshProUGUI infoText;
    public Toggle toggleEnergy;
    public Toggle toggleWater;
    public Toggle toggleMaterial;

    public GameObject gameController;

    [SerializeField] private ManipulateUI manipulateWindow;
    public Material materialOriginal;
    public GameObject isrUI;
    public Canvas canvasMain;

    public float max_water,max_energy,max_material;

    int from;
    int to;

    void Start()
    {
        waterButton.onClick.AddListener(water);
        energyButton.onClick.AddListener(energy);
        materialButton.onClick.AddListener(material);
        backButton.onClick.AddListener(Back);
        //disbale the toggles
        toggleEnergy.interactable=false;
        toggleWater.interactable=false;
        toggleMaterial.interactable=false;
    }

    public void setToggles(float water, float energy, float material){
        if(energy>0)   toggleEnergy.isOn=true;
        else toggleEnergy.isOn=false;
        if(water>0)   toggleWater.isOn=true;
        else toggleWater.isOn=false;
        if(material>0)   toggleMaterial.isOn=true;
        else toggleMaterial.isOn=false;
    }

    public void setButtons(){
        //print(max_water);
        if(max_water==0)   waterButton.interactable=false;
        else waterButton.interactable=true;
        if(max_energy==0f)   energyButton.interactable=false;
        else energyButton.interactable=true;
        if(max_material==0f)   materialButton.interactable=false;
        else materialButton.interactable=true;
    }

    private void water(){
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;

        manipulateWindow.gameObject.SetActive(false);
        //initilize the isr ui
        isrUI.SetActive(true);
        isrUI.GetComponent<ISRUI>().infoText.text="Choose the parameters of this value chain.\nFrom "
        +gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name
        +" to "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name;
        //calculate how much waste is left in the "from" place
        float sum=0f;
        if(gameController.GetComponent<Main>().waterMatrix[from, to]==0f)
            for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                sum+=gameController.GetComponent<Main>().waterMatrix[from, i];
        else for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                if(i!=to)
                    sum+=gameController.GetComponent<Main>().waterMatrix[from, i];
        
        max_water=(float)Math.Round(gameController.GetComponent<Main>().W_water[from]*gameController.GetComponent<Main>().pd[from]-sum,2);

        isrUI.GetComponent<ISRUI>().setUnitMaxValue(max_water);
        isrUI.GetComponent<ISRUI>().setSliders(gameController.GetComponent<Main>().waterMatrix[from, to],
        gameController.GetComponent<Main>().alpha_water[from, to],gameController.GetComponent<Main>().beta_water[from, to],
        gameController.GetComponent<Main>().ec_water[from, to]);
        //isrUI.GetComponent<ISRUI>().setUnits(gameController.GetComponent<Main>().waterMatrix[from, to]);
        isrUI.GetComponent<ISRUI>().unitsText.text="Units to exchange.\n"+ max_water+" units of waste water left.";
        isrUI.GetComponent<ISRUI>().units.text=Math.Round(gameController.GetComponent<Main>().waterMatrix[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().alpha.text=Math.Round(gameController.GetComponent<Main>().alpha_water[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().beta.text=Math.Round(gameController.GetComponent<Main>().beta_water[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().cost.text=Math.Round(gameController.GetComponent<Main>().ec_water[from, to],2).ToString();
        if(gameController.GetComponent<Main>().ec_water[from, to]>=0)
            isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name+":";
        else isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name+":";
        isrUI.GetComponent<ISRUI>().type="water";
    }
    private void energy(){
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;

        manipulateWindow.gameObject.SetActive(false);
        isrUI.SetActive(true);
        isrUI.GetComponent<ISRUI>().infoText.text="Choose the parameters of this value chain.\nFrom "
        +gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name
        +" to "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name;
        //calculate how much waste is left in the "from" place
        float sum=0f;
        if(gameController.GetComponent<Main>().energyMatrix[from, to]==0f)
            for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                sum+=gameController.GetComponent<Main>().energyMatrix[from, i];
        else for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                if(i!=to)
                    sum+=gameController.GetComponent<Main>().energyMatrix[from, i];
        max_energy=(float)Math.Round(gameController.GetComponent<Main>().W_energy[from]*gameController.GetComponent<Main>().pd[from]-sum,2);

        isrUI.GetComponent<ISRUI>().setUnitMaxValue(max_energy);
        isrUI.GetComponent<ISRUI>().setSliders(gameController.GetComponent<Main>().energyMatrix[from, to],
        gameController.GetComponent<Main>().alpha_energy[from, to],gameController.GetComponent<Main>().beta_energy[from, to],
        gameController.GetComponent<Main>().ec_energy[from, to]);
        isrUI.GetComponent<ISRUI>().unitsText.text="Units to exchange.\n"+max_energy+" units of energy water left.";
        isrUI.GetComponent<ISRUI>().units.text=Math.Round(gameController.GetComponent<Main>().energyMatrix[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().alpha.text=Math.Round(gameController.GetComponent<Main>().alpha_energy[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().beta.text=Math.Round(gameController.GetComponent<Main>().beta_energy[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().cost.text=Math.Round(gameController.GetComponent<Main>().ec_energy[from, to],2).ToString();
        if(gameController.GetComponent<Main>().ec_water[from, to]>=0)
            isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name+":";
        else isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name+":";
        isrUI.GetComponent<ISRUI>().type="energy";
    }
    private void material(){
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;

        manipulateWindow.gameObject.SetActive(false);
        isrUI.SetActive(true);
        isrUI.GetComponent<ISRUI>().infoText.text="Choose the parameters of this value chain.\nFrom "
        +gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name
        +" to "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name;
        //calculate how much waste is left in the "from" place
        float sum=0f;
        if(gameController.GetComponent<Main>().materialMatrix[from, to]==0f)
            for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                sum+=gameController.GetComponent<Main>().materialMatrix[from, i];
        else for(int i=0;i<gameController.GetComponent<Main>().n;i++)
                if(i!=to)
                    sum+=gameController.GetComponent<Main>().materialMatrix[from, i];
        max_material=(float)Math.Round(gameController.GetComponent<Main>().W_material[from]*gameController.GetComponent<Main>().pd[from]-sum,2);

        isrUI.GetComponent<ISRUI>().setUnitMaxValue(max_material);
        isrUI.GetComponent<ISRUI>().setSliders(gameController.GetComponent<Main>().materialMatrix[from, to],
        gameController.GetComponent<Main>().alpha_material[from, to],gameController.GetComponent<Main>().beta_material[from, to],
        gameController.GetComponent<Main>().ec_material[from, to]);
        isrUI.GetComponent<ISRUI>().unitsText.text="Units to exchange.\n"+max_material+" units of waste materials left.";
        isrUI.GetComponent<ISRUI>().units.text=Math.Round(gameController.GetComponent<Main>().materialMatrix[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().alpha.text=Math.Round(gameController.GetComponent<Main>().alpha_material[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().beta.text=Math.Round(gameController.GetComponent<Main>().beta_material[from, to],2).ToString();
        isrUI.GetComponent<ISRUI>().cost.text=Math.Round(gameController.GetComponent<Main>().ec_material[from, to],2).ToString();
        if(gameController.GetComponent<Main>().ec_water[from, to]>=0)
            isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex1].name+":";
        else isrUI.GetComponent<ISRUI>().costText.text="Exchange cost paid by "+gameController.GetComponent<Main>().places[gameController.GetComponent<Main>().selectedPlaceIndex2].name+":";
        isrUI.GetComponent<ISRUI>().type="material";
    }

    private void Back(){
        manipulateWindow.gameObject.SetActive(false);
        canvasMain.gameObject.SetActive(true);
        gameController.GetComponent<Main>().touchable=true;
        gameController.GetComponent<Main>().selected1 = false;
        gameController.GetComponent<Main>().selected2 = false;
        canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(false);
        gameController.GetComponent<Main>().selectFirstText.gameObject.SetActive(true);
        
        from=gameController.GetComponent<Main>().selectedPlaceIndex1;
        to=gameController.GetComponent<Main>().selectedPlaceIndex2;
        gameController.GetComponent<Main>().places[from].GetComponent<MeshRenderer> ().material = materialOriginal;
        gameController.GetComponent<Main>().places[to].GetComponent<MeshRenderer> ().material = materialOriginal;
    }
}
