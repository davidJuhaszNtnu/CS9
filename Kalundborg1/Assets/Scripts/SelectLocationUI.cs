using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectLocationUI : MonoBehaviour
{
    public Button confirmButton;
    public Button backButton;
    public GameObject env;
    public GameObject legend;
    public GameObject selectLocationUI;
    public Canvas canvasMain;
    public Camera arCamera;

    //camera manupilation
    private float currentSize;
    float minSize, maxSize;
    public Camera floorCamera;
    float panSpeed;
    float TouchTime;

    //placing the object
    Ray ray;
    RaycastHit hit;
    bool placed;
    bool dragging;
    public GameObject gameController;
    public GameObject chooseNewUI;
    public GameObject indicator;
    public Material indicatorSelectedMaterial;
    public Material indicatorNotSelectedMaterial;
    public Material selectedMaterial;
    bool indicatorSelected;
    public GameObject mapIndicator;

    private Vector3 newCamPos;

    void Start()
    {
        confirmButton.onClick.AddListener(Confirm);
        backButton.onClick.AddListener(Back);

        currentSize=floorCamera.orthographicSize;
        minSize=0.5f;
        maxSize=8.0f;
        panSpeed=currentSize*0.001f;
        TouchTime=0;
        indicatorSelected=false;
        placed=false;
        indicator.transform.localScale=Vector3.one*0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        PanZoom();
    }

    private void Back(){
        indicatorSelected=false;
        indicator.GetComponent<MeshRenderer>().material=indicatorNotSelectedMaterial;
        placed=false;
        dragging=false;
        selectLocationUI.SetActive(false);
        chooseNewUI.SetActive(true);
        switch(chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex){
            case 0:
                chooseNewUI.GetComponent<ChooseNewUI>().placeAbutton.interactable=true;
                break;
            case 1:
                chooseNewUI.GetComponent<ChooseNewUI>().placeBbutton.interactable=true;
                break;
            case 2:
                chooseNewUI.GetComponent<ChooseNewUI>().placeCbutton.interactable=true;
                break;
            case 3:
                chooseNewUI.GetComponent<ChooseNewUI>().placeDbutton.interactable=true;
                break;
        }
        chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex=-1;
        chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen--;
        env.SetActive(true);
    }

    private void Confirm(){
        if(placed && indicatorSelected==false){
            int selectedIndex=chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex;
            //creating new place in the environment
            GameObject newPlace = GameObject.Instantiate(gameController.GetComponent<Main>().newPlace);
            newPlace.SetActive(true);
            newPlace.transform.SetParent(env.transform.GetChild(0).transform,false);
            newPlace.transform.position=new Vector3(indicator.transform.position.x,0.5f,indicator.transform.position.z);
            newPlace.GetComponent<MeshRenderer>().material=selectedMaterial;
            newPlace.name=chooseNewUI.GetComponent<ChooseNewUI>().names[selectedIndex];
            newPlace.transform.GetChild(0).GetComponent<TextMeshPro>().text=chooseNewUI.GetComponent<ChooseNewUI>().names[selectedIndex];
            newPlace.transform.GetChild(0).GetComponent<TextMeshPro>().gameObject.SetActive(true);
            gameController.GetComponent<Main>().places.Add(newPlace);
            
            //its parameters
            int newIndex=12+chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen;
            gameController.GetComponent<Main>().R_water[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().R_water[selectedIndex];
            gameController.GetComponent<Main>().R_energy[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().R_energy[selectedIndex];
            gameController.GetComponent<Main>().R_material[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().R_material[selectedIndex];
            gameController.GetComponent<Main>().W_water[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().W_water[selectedIndex];
            gameController.GetComponent<Main>().W_energy[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().W_energy[selectedIndex];
            gameController.GetComponent<Main>().W_material[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().W_material[selectedIndex];
            gameController.GetComponent<Main>().s_water[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().s_water[selectedIndex];
            gameController.GetComponent<Main>().s_energy[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().s_energy[selectedIndex];
            gameController.GetComponent<Main>().s_material[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().s_material[selectedIndex];
            gameController.GetComponent<Main>().pd[newIndex]=chooseNewUI.GetComponent<ChooseNewUI>().pd[selectedIndex];

            //the new place is selected
            gameController.GetComponent<Main>().selected1=true;
            gameController.GetComponent<Main>().selectSecondText.text="From "+newPlace.name+" to (select the other place)";
            gameController.GetComponent<Main>().selectedPlaceIndex1=gameController.GetComponent<Main>().places.Count-1;
            gameController.GetComponent<Main>().selectSecondText.gameObject.SetActive(true);
            gameController.GetComponent<Main>().selectFirstText.gameObject.SetActive(false);
            //placing the indicator on the floor cube map
            GameObject newIndicator = GameObject.Instantiate(mapIndicator);
            newIndicator.transform.SetParent(selectLocationUI.transform,false);
            newIndicator.SetActive(true);
            newIndicator.transform.position=indicator.transform.position;
            newIndicator.name=chooseNewUI.GetComponent<ChooseNewUI>().names[chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex]+"Indicator";
            //its text
            newIndicator.transform.GetChild(0).GetComponent<TextMeshPro>().text=chooseNewUI.GetComponent<ChooseNewUI>().names[chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex];
            //update the distance matrix
            for(int i=0;i<newIndex;i++)
                gameController.GetComponent<Main>().d[newIndex,i]=Vector3.Distance(gameController.GetComponent<Main>().places[i].transform.position, newPlace.transform.position);
            for(int i=0;i<newIndex;i++)
                gameController.GetComponent<Main>().d[i,newIndex]=gameController.GetComponent<Main>().d[newIndex,i];

            indicator.SetActive(false);
            placed=false;
            dragging=false;

            env.SetActive(true);
            arCamera.enabled=true;
            canvasMain.gameObject.SetActive(true);
            selectLocationUI.SetActive(false);
            //update the list of places and their info
            gameController.GetComponent<Main>().updateListScrollText();
            legend.SetActive(true);
            gameController.GetComponent<Main>().touchable=true;
        }else{
            //alert window
        }
    }

    void PanZoom(){
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch (0);
            Touch touchOne = Input.GetTouch (1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            currentSize += deltaMagnitudeDiff*0.01f;
            newCamPos = Quaternion.Euler(0f, gameController.GetComponent<Main>().heading, 0f) * floorCamera.transform.position;
            //newCamPos = Quaternion.Euler(0f, 30f, 0f) * floorCamera.transform.position;
            if(newCamPos.z>5.0f || newCamPos.z<0.0f || newCamPos.x<-3.21f || newCamPos.x>3.21f){
                currentSize -= deltaMagnitudeDiff*0.01f;
            }
            else if (currentSize >= maxSize)
                currentSize = maxSize;
            else if (currentSize <= minSize)
                currentSize = minSize;
            floorCamera.orthographicSize = currentSize;
            //scaling the indicator
            if (floorCamera.orthographicSize!=maxSize && floorCamera.orthographicSize!=minSize)
                indicator.transform.localScale+=Vector3.one*deltaMagnitudeDiff*0.0005f;
            panSpeed=currentSize*0.001f;
        }
        if(Input.touchCount == 1){
            if(Input.GetTouch(0).phase == TouchPhase.Began) TouchTime=Time.time;
            if(dragging==false){
                floorCamera.transform.position -= Quaternion.Euler(0f, -gameController.GetComponent<Main>().heading, 0f) *(new Vector3(Input.GetTouch(0).deltaPosition.x,0,Input.GetTouch(0).deltaPosition.y))*panSpeed;
                newCamPos = Quaternion.Euler(0f, gameController.GetComponent<Main>().heading, 0f) * floorCamera.transform.position;
                //newCamPos = Quaternion.Euler(0f, 30f, 0f) * floorCamera.transform.position;
                if(newCamPos.z>5.0f || newCamPos.z<0.0f || newCamPos.x<-3.21f || newCamPos.x>3.21f){
                    floorCamera.transform.position += Quaternion.Euler(0f, -gameController.GetComponent<Main>().heading, 0f) *(new Vector3(Input.GetTouch(0).deltaPosition.x,0,Input.GetTouch(0).deltaPosition.y))*panSpeed;
                }
            }
            //tap, placing the indicator sphere
            if (Input.GetTouch(0).phase == TouchPhase.Ended && Time.time - TouchTime <= 0.1f && placed==false){
                ray = floorCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    placed=true;
                    indicator.SetActive(true);
                    indicator.transform.position=hit.point;
                    //indicator.transform.localScale=new Vector3(0.2f,0.2f,0.2f);
                }
            }
            //unselect the indicator
            if(Input.GetTouch(0).phase == TouchPhase.Ended && Time.time - TouchTime <= 0.1f && placed && indicatorSelected){
                ray = floorCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.name=="Indicator"){
                        indicatorSelected=false;
                        indicator.GetComponent<MeshRenderer>().material=indicatorNotSelectedMaterial;
                    }
                }
            }
            // select the indicator
            if (Time.time - TouchTime > 0.5f && placed && indicatorSelected==false){
                ray = floorCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.name=="Indicator"){
                        indicator.GetComponent<MeshRenderer>().material=indicatorSelectedMaterial;
                        indicatorSelected=true;
                    }
                }
            }
            //changing the position of the selected indicator
            if(placed && indicatorSelected && Input.GetTouch(0).phase != TouchPhase.Ended && Time.time - TouchTime >= 0.1f){
                ray = floorCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.name=="Indicator" && dragging==false){
                        dragging=true;
                    }else if(dragging) indicator.transform.position=new Vector3(hit.point.x,0.01f,hit.point.z);
                }
            }else dragging=false;
        }
    }
}
