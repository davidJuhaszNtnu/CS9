using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseNewUI : MonoBehaviour
{
    public GameObject info;
    public TextMeshProUGUI ecoenvText;
    public Button placeAbutton;
    public Button placeBbutton;
    public Button placeCbutton;
    public Button placeDbutton;
    public Button confirmButton;
    public Button backButton;
    public int selectedIndex;
    public string[] names={"place A","place B","place C","place D"};
    public float[] R_water,R_energy,R_material,W_water,W_energy,W_material,s_water,s_energy,s_material,pd;
    int n=4;
    public int nrOfChosen;


    public GameObject chooseNewUI;
    public GameObject selectLocationUI;
    public GameObject env;
    public GameObject legend;
    public GameObject gameController;
    public Canvas canvasMain;
    
    void Start()
    {
        info.SetActive(false);
        selectedIndex=-1;
        nrOfChosen=0;
        placeAbutton.onClick.AddListener(ButtonA);
        placeBbutton.onClick.AddListener(ButtonB);
        placeCbutton.onClick.AddListener(ButtonC);
        placeDbutton.onClick.AddListener(ButtonD);
        confirmButton.onClick.AddListener(Confirm);
        backButton.onClick.AddListener(Back);

        //new places
        pd=new float[n];
        R_water=new float[n];
        R_energy=new float[n];
        R_material=new float[n];
        W_water=new float[n];
        W_energy=new float[n];
        W_material=new float[n];
        s_water=new float[n];
        s_energy=new float[n];
        s_material=new float[n];
        for(int i=0;i<n;i++){
            pd[i]=20f;
            R_water[i]=1f;
            R_energy[i]=1f;
            R_material[i]=1f;
            W_water[i]=0.5f;
            W_energy[i]=0.5f;
            W_material[i]=0.5f;
            s_water[i]=0.5f;
            s_energy[i]=0.5f;
            s_material[i]=0.5f;
        }
    }

    private void ButtonA(){
        if(!info.activeSelf)    info.SetActive(true);
        selectedIndex=0;
        info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Water\nInput requires: "+R_water[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_water[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_water[selectedIndex];
        info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="Energy\nInput requires: "+R_energy[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_energy[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_energy[selectedIndex];
        info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text="Material\nInput requires: "+R_material[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_material[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_material[selectedIndex];
        info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text="Info about place A:";
        info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text="Product demand: "+pd[selectedIndex];
        
    }
    private void ButtonB(){
        if(!info.activeSelf)    info.SetActive(true);
        selectedIndex=1;
        info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Water\nInput requires: "+R_water[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_water[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_water[selectedIndex];
        info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="Energy\nInput requires: "+R_energy[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_energy[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_energy[selectedIndex];
        info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text="Material\nInput requires: "+R_material[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_material[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_material[selectedIndex];
        info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text="Info about place B:";
        info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text="Product demand: "+pd[selectedIndex];
    }
    private void ButtonC(){
        if(!info.activeSelf)    info.SetActive(true);
        selectedIndex=2;
        info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Water\nInput requires: "+R_water[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_water[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_water[selectedIndex];
        info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="Energy\nInput requires: "+R_energy[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_energy[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_energy[selectedIndex];
        info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text="Material\nInput requires: "+R_material[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_material[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_material[selectedIndex];
        info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text="Info about place C:";
        info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text="Product demand: "+pd[selectedIndex];
    }
    private void ButtonD(){
        if(!info.activeSelf)    info.SetActive(true);
        selectedIndex=3;
        info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Water\nInput requires: "+R_water[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_water[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_water[selectedIndex];
        info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="Energy\nInput requires: "+R_energy[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_energy[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_energy[selectedIndex];
        info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text="Material\nInput requires: "+R_material[selectedIndex]*pd[selectedIndex]+"\nWaste produced: "+W_material[selectedIndex]*pd[selectedIndex]+"\ns-factor: "+s_material[selectedIndex];
        info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text="Info about place D:";
        info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text="Product demand: "+pd[selectedIndex];
    }
    private void Confirm(){
        switch(selectedIndex){
            case -1:
                //alert window
                break;
            case 0:
                placeAbutton.interactable=false;
                nrOfChosen++;
                break;
            case 1:
                placeBbutton.interactable=false;
                nrOfChosen++;
                break;
            case 2:
                placeCbutton.interactable=false;
                nrOfChosen++;
                break;
            case 3:
                placeDbutton.interactable=false;
                nrOfChosen++;
                break;
        }
        if(selectedIndex>=0){
            chooseNewUI.SetActive(false);
            selectLocationUI.SetActive(true);
            env.SetActive(false);
            gameController.GetComponent<Main>().touchable=true;
            info.SetActive(false);
            selectLocationUI.GetComponent<SelectLocationUI>().indicator.SetActive(false);
            selectLocationUI.GetComponent<SelectLocationUI>().mapIndicator.SetActive(false);
        }
    }
    private void Back(){
        chooseNewUI.SetActive(false);
        gameController.GetComponent<Main>().touchable=true;
        canvasMain.gameObject.SetActive(true);
        ecoenvText.gameObject.SetActive(false);
        legend.SetActive(true);
    }
}
