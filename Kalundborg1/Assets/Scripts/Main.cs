using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Security.Cryptography;
using UnityEngine.XR.ARFoundation;

public class Main : MonoBehaviour
{
    public GameObject env;
    Ray ray;
    RaycastHit hit;

    public List<GameObject> places;
    public List<GameObject> arrows;
    public List<string> lineNames;
    public Material materialSelected;
    public Material materialOriginal;
    public Material materialStick;
    public Material energyMaterial;
    public Material waterMaterial;
    public Material materialMaterial;
    public Camera arCamera;

    //UIs
    public GameObject chooseNewUI;
    public GameObject selectLocationUI;
    public GameObject isrUI;
    public GameObject newPlace;
    public Canvas canvasMain;
    public GameObject scrollViewUI;
    public TextMeshProUGUI listScrollText;
    //[SerializeField] private ManipulateUI manipulateWindow;
    public GameObject manipulateWindow;
    public TextMeshProUGUI selectSecondText;
    public TextMeshProUGUI selectFirstText;

    public bool selected1= false;
    public bool selected2= false;
    public float heading;
    private bool headingRecieved= false;
    public int selectedPlaceIndex1;
    public int selectedPlaceIndex2;

    //value chain representation, old places n=13, new places n=4
    public int n=17;
    public float[,] energyMatrix;
    public float[,] waterMatrix;
    public float[,] materialMatrix;

    //parameters for the rules of the game
    /*
    params that are common for all
    tc = transportation cost of 1 unit of waste per km
    rc = cost of treating 1 unit of waste
    cc = cost of signing a contract
    upc_water = purchase cost of 1 unit of water input
    upc_energy = purchase cost of 1 unit of water energy
    upc_material = purchase cost of 1 unit of water material
    udc_water = cost to dispose 1 unit of water waste
    udc_energy = cost to dispose 1 unit of energy waste
    udc_material = cost to dispose 1 unit of material waste
    d_ij = distance matrix

    params unique to every place
    pd = product demand
    R_k = primary input coeff, number of units of input required per 1 unit of output produced
    R_water, R_energy, R_material
    W_k = number of units of waste k generated per unit of output produced
    W_water, W_energy, W_material
    s_k = conversion coefficient, s_k units of primary input can be replaced by 1 unit of waste k, [0,1] (1=100% can be replaced by waste, 0=0% can be replaced)
    s_water, s_energy, s_material

    params used when creating a value chain
    e_k[i,j] = amount of waste k to be exchanged from i to j = this is the matrices above (energyMatrix,...)
    ec_k[i,j] = waste k exchange cost paid by i to j (negative value if j pays to i)
    alpha_k[i,j] = percentage of transportation costs paid by i, [0,1]
    beta_k[i,j] = percentage of treatment costs paid by i, [0,1]

    goal of each company is to maximize the environmental and economic performance indicators ENV[i], ECO[i]
    other factors
    EB_k[i,j] = economic benefits i gains from the value chain with j with waste k
    */
    public float[,] d, ec_water,ec_energy,ec_material,alpha_water,alpha_energy,alpha_material,beta_water,beta_energy,beta_material;
    float[,] EB_water,EB_energy,EB_material;
    float tc,rc,cc,upc_water,upc_energy,upc_material,udc_water,udc_energy,udc_material;
    public float[] pd,R_water,R_energy,R_material,W_water,W_energy,W_material,s_water,s_energy,s_material;
    float[] ENV,ECO;

    public GameObject Arrow;
    //Animator animator;

    bool now;
    public bool isActive, touchable, gotPositionDirection;
    public float fromNow;
    private GameObject aRSessionOrigin;
    public Vector3 envPosition,envPosition1,envPosition2;
    public Quaternion envRotation;

    void Start()
    {
        now=false;
        isActive=false;
        gotPositionDirection=false;
        fromNow=0f;
        heading=0f;
        Input.compass.enabled = true;
        Input.location.Start();

        // aRSessionOrigin = GameObject.Find("AR Session Origin");
        // var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();

        //aRScript.MakeContentAppearAt(env.transform, env.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));
        //aRScript.MakeContentAppearAt(selectLocationUI.transform, selectLocationUI.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));

        //generate the sticks
        foreach(GameObject place in places){
            GameObject stick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stick.GetComponent<MeshRenderer> ().material = materialStick;
            stick.transform.parent=place.transform;
            stick.transform.localPosition = new Vector3(0.0f, -1.5f , 0.0f);
            stick.transform.localScale = new Vector3(0.05f, 1.5f , 0.05f);
        }
        //new place
        GameObject stick2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stick2.GetComponent<MeshRenderer> ().material = materialStick;
        stick2.transform.parent=newPlace.transform;
        stick2.transform.localPosition = new Vector3(0.0f, -1.5f , 0.0f);
        stick2.transform.localScale = new Vector3(0.05f, 1.5f , 0.05f);
        newPlace.SetActive(false);
        
        //parameters
        energyMatrix = new float[n,n];
        waterMatrix = new float[n,n];
        materialMatrix = new float[n,n];
        ec_water = new float[n,n];
        ec_energy = new float[n,n];
        ec_material = new float[n,n];
        alpha_water = new float[n,n];
        alpha_energy = new float[n,n];
        alpha_material = new float[n,n];
        beta_water = new float[n,n];
        beta_energy = new float[n,n];
        beta_material = new float[n,n];
        EB_water = new float[n,n];
        EB_energy = new float[n,n];
        EB_material = new float[n,n];

        d=new float[n,n];
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
        ENV=new float[n];
        ECO=new float[n];


        //value chains
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++){
                energyMatrix[i,j]=0f;
                waterMatrix[i,j]=0f;
                materialMatrix[i,j]=0f;
                ec_water[i,j]=-0f;
                ec_energy[i,j]=0f;
                ec_material[i,j]=0f;
                alpha_water[i,j]=0f;
                alpha_energy[i,j]=0f;
                alpha_material[i,j]=0f;
                beta_water[i,j]=0f;
                beta_energy[i,j]=0f;
                beta_material[i,j]=0f;

                EB_water[i,j]=0f;
                EB_energy[i,j]=0f;
                EB_material[i,j]=0f;
            }

        //distance matrix
        for(int i=0;i<n;i++)
            for(int j=0;j<n;j++){
                if(j<i && i<13 && j<13) d[i,j]=Vector3.Distance(places[i].transform.position, places[j].transform.position);
                else d[i,j]=0;
            }
        
        for(int i=0;i<n;i++)
            for(int j=n-1;j>i;j--)
                d[i,j]=d[j,i];
        
        //global parameters
        tc=1f;
        rc=1f;
        cc=0f;
        upc_water=1;
        upc_energy=1f;
        upc_material=1f;
        udc_water=1;
        udc_energy=1f;
        udc_material=1f;
        //local parameters
        for(int i=0;i<n;i++){
            pd[i]=30f;
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

        //initial connections
        energyMatrix[0,2]=5f;
        energyMatrix[0,8]=3f;

        materialMatrix[1,0]=8f;

        waterMatrix[2,4]=5f;

        waterMatrix[4,2]=7f;
        waterMatrix[4,7]=5f;
        energyMatrix[4,7]=6f;

        waterMatrix[5,7]=6f;

        waterMatrix[7,4]=3f;
        materialMatrix[7,6]=4f;
        energyMatrix[7,12]=4f;
        energyMatrix[7,5]=4f;
        energyMatrix[7,8]=1f;
        energyMatrix[7,10]=1f;
        waterMatrix[7,2]=5f;

        waterMatrix[8,7]=6f;

        waterMatrix[10,7]=7f;

        materialMatrix[12,6]=9f;

        //compute initial indicators
         for(int i=0;i<13;i++)
            computeENVandECO(i);

        //update the list of places and their info
        updateListScrollText();

        //activate names
        foreach(GameObject place in places)
            place.transform.GetChild(0).gameObject.SetActive(true);

        selectFirstText.gameObject.SetActive(false);
        selectFirstText.text="Select where from";
        selectSecondText.gameObject.SetActive(false);
        manipulateWindow.GetComponent<ManipulateUI>().max_water=1f;
        manipulateWindow.gameObject.SetActive(false);
        chooseNewUI.SetActive(false);
        canvasMain.gameObject.SetActive(false);
        canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(false);
        selectLocationUI.SetActive(false);
        isrUI.GetComponent<ISRUI>().unitsSlider.value=0f;
        isrUI.SetActive(false);
        scrollViewUI.SetActive(false);
        touchable=true;
    }

    // private float nextActionTime = 0.0f;
    // public float period = 5f;
    // Update is called once per frame
    void Update()
    {
        if(gotPositionDirection){
            gotPositionDirection=false;
            aRSessionOrigin = GameObject.Find("AR Session Origin");
            var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();

            // aRScript.MakeContentAppearAt(env.transform, env.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));
            // aRScript.MakeContentAppearAt(selectLocationUI.transform, selectLocationUI.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));

            Vector3 direction=envPosition2-envPosition1;
            aRScript.MakeContentAppearAt(env.transform, new Vector3(envPosition1.x,0f,envPosition1.z),Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z), Vector3.up));
            //aRScript.MakeContentAppearAt(selectLocationUI.transform, envPosition,envRotation);
        }
        
        if(Input.compass.trueHeading==0){
        }else if(headingRecieved==false){
            headingRecieved=true;
            // heading=Input.compass.trueHeading;
            // env.transform.rotation=Quaternion.Euler(0.0f, heading , 0.0f);
            // selectLocationUI.transform.rotation=Quaternion.Euler(0.0f, heading , 0.0f);
            // aRSessionOrigin = GameObject.Find("AR Session Origin");
            // var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();

            // aRScript.MakeContentAppearAt(env.transform, env.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));
            //aRScript.MakeContentAppearAt(selectLocationUI.transform, selectLocationUI.transform.position, Quaternion.Euler(0, -Input.compass.trueHeading, 0));
            //env.transform.Rotate(0.0f, heading , 0.0f, Space.Self);
            //selectLocationUI.transform.Rotate(0.0f, heading , 0.0f, Space.Self);
        }

        if(Time.time>fromNow+0.0f && isActive && now==false){
            now=true;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++){
                    if(waterMatrix[i,j]>0f)
                        updateValueChains(i, j, "water");
                    if(energyMatrix[i,j]>0f)
                        updateValueChains(i, j, "energy");
                    if(materialMatrix[i,j]>0f)
                        updateValueChains(i, j, "material");
                }
        }


        // if (Time.time > nextActionTime ) {
        //     nextActionTime += period;
        //     // env.transform.rotation=Quaternion.Euler(0.0f, -Input.compass.trueHeading , 0.0f);
        //     // selectLocationUI.transform.rotation=Quaternion.Euler(0.0f, -Input.compass.trueHeading , 0.0f);
        // }
        
        ray = arCamera.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && touchable)
        {
            
            if(Physics.Raycast(ray, out hit)){
                //only is there are new places
                //if(selected1==false && selected2==false && places.Count>13){
                if(selected1==false && selected2==false){
                    foreach(GameObject place in places){
                        if(hit.collider.name == place.name){
                            selected1=true;
                            selectedPlaceIndex1=places.IndexOf(place);
                            selectSecondText.gameObject.SetActive(true);
                            selectFirstText.gameObject.SetActive(false);
                            selectSecondText.text="From "+place.name+" to (select the other place)";
                            place.GetComponent<MeshRenderer> ().material = materialSelected;
                            canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(true);
                            canvasMain.GetComponent<CanvasMainUI>().envEcoText.text="ENV: "+Math.Round(ENV[selectedPlaceIndex1],2)+"\nECO: "+Math.Round(ECO[selectedPlaceIndex1],2);
                        }
                    }
                }else if(selected1==true && selected2==false){
                    foreach(GameObject place in places){
                        if(hit.collider.name == place.name && places.IndexOf(place)!=selectedPlaceIndex1){
                            //print(place.name+" selected second");
                            selectedPlaceIndex2=places.IndexOf(place);
                            //only from new places to old
                            //only from old places to new
                            //only from new places to new
                            //if((selectedPlaceIndex1>12 && selectedPlaceIndex2<=12) || (selectedPlaceIndex1<=12 && selectedPlaceIndex2>12) || (selectedPlaceIndex1>12 && selectedPlaceIndex2>12)){
                                selected2=true;
                                selectSecondText.gameObject.SetActive(false);
                                place.GetComponent<MeshRenderer> ().material = materialSelected;
                                selectedPlaceIndex2=places.IndexOf(place);

                                //manipulateWindow.gameObject.SetActive(true);
                                manipulateWindow.SetActive(true);
                                canvasMain.gameObject.SetActive(false);
                                //set toggles
                                touchable=false;
                                manipulateWindow.gameObject.GetComponent<ManipulateUI>().setToggles(waterMatrix[selectedPlaceIndex1,selectedPlaceIndex2],
                                energyMatrix[selectedPlaceIndex1,selectedPlaceIndex2],materialMatrix[selectedPlaceIndex1,selectedPlaceIndex2]);
                                // manipulateWindow.infoText.text="From "+places[selectedPlaceIndex1].name+" to "+places[selectedPlaceIndex2].name+".\n Add or remove value chains.";
                                manipulateWindow.GetComponent<ManipulateUI>().infoText.text="From "+places[selectedPlaceIndex1].name+" to "+places[selectedPlaceIndex2].name+".\n Add or remove value chains.";
                            // }else{
                            //     //alert window
                            // }
                        }else if(hit.collider.name == place.name && places.IndexOf(place)==selectedPlaceIndex1){
                            //tapped the same place
                            selected1=false;
                            selectSecondText.gameObject.SetActive(false);
                            selectFirstText.gameObject.SetActive(true);
                            place.GetComponent<MeshRenderer> ().material = materialOriginal;
                            canvasMain.GetComponent<CanvasMainUI>().envEcoText.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        foreach(GameObject arrow in arrows){
            animateArrow(arrow);
        }
        
    }

    public void updateValueChains(int from, int to, string type){
        switch(type){
            case "energy":
                //energy
                string energyName="Energy-"+from.ToString()+"-"+to.ToString();
                //lineNames.IndexOf(energyName)<0 means if there is yet no such value chain
                if(energyMatrix[from, to]>0f && lineNames.IndexOf(energyName)<0){
                    createValueChain(from, to, energyName, energyMaterial);
                }else if(energyMatrix[from, to]==0f && lineNames.IndexOf(energyName)>=0){
                    GameObject line = GameObject.Find(energyName);
                    GameObject arrow = GameObject.Find("Arrow"+energyName);
                    GameObject.Destroy(line);
                    arrows.Remove(arrow);
                    GameObject.Destroy(arrow);
                    lineNames.Remove(energyName);
                }
                break;
            case "water":
                //water
                string waterName="Water-"+from.ToString()+"-"+to.ToString();
                if(waterMatrix[from, to]>0f && lineNames.IndexOf(waterName)<0){
                    createValueChain(from, to, waterName, waterMaterial);
                }else if(waterMatrix[from, to]==0f && lineNames.IndexOf(waterName)>=0){
                    GameObject line = GameObject.Find(waterName);
                    GameObject arrow = GameObject.Find("Arrow"+waterName);
                    GameObject.Destroy(line);
                    arrows.Remove(arrow);
                    GameObject.Destroy(arrow);
                    lineNames.Remove(waterName);
                }
                break;
            case "material":
                //material
                string materialName="Material-"+from.ToString()+"-"+to.ToString();
                if(materialMatrix[from, to]>0f && lineNames.IndexOf(materialName)<0){
                    createValueChain(from, to, materialName, materialMaterial);
                }else if(materialMatrix[from, to]==0f && lineNames.IndexOf(materialName)>=0){
                    GameObject line = GameObject.Find(materialName);
                    GameObject arrow = GameObject.Find("Arrow"+materialName);
                    GameObject.Destroy(line);
                    arrows.Remove(arrow);
                    GameObject.Destroy(arrow);
                    lineNames.Remove(materialName);
                }
                break;
        }
    }

    private void createValueChain(int from, int to, string name, Material material){
        GameObject Line = new GameObject();
        Line.transform.parent=places[from].transform;
        Line.name=name;
        lineNames.Add(name);
        Line.AddComponent<LineRenderer>();
        LineRenderer lr = Line.GetComponent<LineRenderer>();
        //if there is a place in the way between from and to, make it a curve that goes over
        Vector3 direction = places[to].transform.position - places[from].transform.position;
        //minus the radius of the spheres
        float dist = Vector3.Distance(places[from].transform.position, places[to].transform.position)-places[from].transform.localScale.x;

        RaycastHit[] hits;
        //ignore layer nr 7 (arrows)
        hits = Physics.RaycastAll(places[from].transform.position, direction, dist ,7,QueryTriggerInteraction.Ignore);
        //height separation of the lines
        float d=0.3f;
        Vector3[] anchors = new Vector3[3];
        string type=name.Split('-')[0];
        //if there is nothing between, do straight line
        if(hits.Length==0){
            switch(type){
                case "Water":
                    Anchors(anchors, 0f, from, to);
                    break;
                case "Energy":
                    Anchors(anchors, d, from, to);
                    break;
                case "Material":
                    Anchors(anchors, -d, from, to);
                    break;
            }
        }else{
            switch(type){
                case "Water":
                    Anchors(anchors, h(dist), from, to);
                    break;
                case "Energy":
                    Anchors(anchors, h(dist)+d, from, to);
                    break;
                case "Material":
                    Anchors(anchors, h(dist)-d, from, to);
                    break;
            }
        }

        //create an arrow
        GameObject arrow = GameObject.Instantiate(Arrow);
        arrow.SetActive(true);
        arrow.transform.parent=places[from].transform;
        arrow.transform.position=places[from].transform.position;
        arrow.name="Arrow"+name;
        arrow.transform.localScale = new Vector3(0.1f, 0.2f , 0.1f);
        arrow.GetComponent<ArrowProps>().points=anchors;
        arrow.GetComponent<ArrowProps>().t=0.0f;
        //rotate the arrow
        arrow.transform.rotation=Quaternion.LookRotation(arrow.GetComponent<ArrowProps>().curveDerivative(0f), Vector3.up);
        arrow.GetComponent<ArrowProps>().up=Vector3.up;
        arrows.Add(arrow);

        //compute the position of line points
        lr.positionCount=10;
        lr.startWidth=0.02f;
        lr.endWidth=0.02f;
        lr.material = material;
        Vector3[] linePos=new Vector3[lr.positionCount];
        for(int i=0;i<lr.positionCount;i++){
            linePos[i]=(arrow.GetComponent<ArrowProps>().curve((float)i/(lr.positionCount-1)));
        }
        lr.numCornerVertices=10;
        lr.SetPositions(linePos);
    }

    //height function of the lines
    float h(float dist){
        //return 0.5f*dist+0.5f;
        return 0.25f*dist+0.75f;
    }
    //create anchor points for the bezier curve
    void Anchors(Vector3[] anchors, float h, int from, int to){
        anchors[0]=places[from].transform.position;
        anchors[1]=((places[from].transform.position+places[to].transform.position)/2.0f+(new Vector3(0f,h,0f)));
        anchors[2]=places[to].transform.position;
    }

    public void animateArrow(GameObject arrow){
        if(arrow.GetComponent<ArrowProps>().t<=1.0f){
            arrow.GetComponent<ArrowProps>().t+=0.005f;
            arrow.transform.position =arrow.GetComponent<ArrowProps>().curve(arrow.GetComponent<ArrowProps>().t);
            //arrow rotation
            Vector3 up=Quaternion.AngleAxis(2f, arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t))*arrow.GetComponent<ArrowProps>().up;
            arrow.GetComponent<ArrowProps>().up=up;
            Quaternion q=Quaternion.LookRotation(arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t), up);
            arrow.transform.rotation=q;
        }else arrow.GetComponent<ArrowProps>().t=0.0f;
    }

    public void computeENVandECO(int i){
        float sum_e_water=0f,sum_e_energy=0f,sum_e_material=0f;
        float sum_se_water=0f,sum_se_energy=0f,sum_se_material=0f;
        float env_j,env_i;
        float eco_j,eco_i;
        for(int j=0;j<n;j++){
            //for env_j, going out
            sum_e_water+=waterMatrix[i,j];
            sum_e_energy+=energyMatrix[i,j];
            sum_e_material+=materialMatrix[i,j];

            //for env_i, coming in
            sum_se_water+=s_water[i]*waterMatrix[j,i];
            sum_se_energy+=s_energy[i]*energyMatrix[j,i];
            sum_se_material+=s_material[i]*materialMatrix[j,i];
        }
        env_j=sum_e_water/(W_water[i]*pd[i])+sum_e_energy/(W_energy[i]*pd[i])+sum_e_material/(W_material[i]*pd[i]);
        env_i=sum_se_water/(R_water[i]*pd[i])+sum_se_energy/(R_energy[i]*pd[i])+sum_se_material/(R_material[i]*pd[i]);

        //must be in range [0,1]
        ENV[i]=env_j+env_i;

        //ECO
        float sum_eb_water=0f,sum_eb_energy=0f,sum_eb_material=0f;
        float sum_eb_in_water=0f,sum_eb_in_energy=0f,sum_eb_in_material=0f;
        for(int j=0;j<n;j++){
            //for eco_j, going out, producer i
            updateEB(i,j);
            sum_eb_water+=EB_water[i,j];
            sum_eb_energy+=EB_energy[i,j];
            sum_eb_material+=EB_material[i,j];

            //for eco_i, coming in, reciever i
            updateEB(j,i);
            sum_eb_in_water+=EB_water[j,i];
            sum_eb_in_energy+=EB_energy[j,i];
            sum_eb_in_material+=EB_material[j,i];
        }
        eco_j=sum_eb_water/(udc_water*W_water[i]*pd[i])+sum_eb_energy/(udc_energy*W_energy[i]*pd[i])+sum_eb_material/(udc_material*W_material[i]*pd[i]);
        eco_i=sum_eb_in_water/(upc_water*R_water[i]*pd[i])+sum_eb_in_energy/(upc_energy*R_energy[i]*pd[i])+sum_eb_in_material/(upc_material*R_material[i]*pd[i]);

        //range can be from -inf to +inf
        ECO[i]=eco_j+eco_i;
    }

    public void updateEB(int from, int to){
        //compute EB_k[i,j]
        if(waterMatrix[from,to]!=0){
            EB_water[from,to]=(udc_water-alpha_water[from,to]*tc*d[from,to]-beta_water[from,to]*rc)*waterMatrix[from,to]-ec_water[from,to]-cc;
            EB_water[to,from]=(upc_water*s_water[to]-(1-alpha_water[from,to])*tc*d[to,from]-(1-beta_water[from,to])*rc)*waterMatrix[from,to]+ec_water[from,to]-cc;
        }else{
            EB_water[from,to]=0f;
            EB_water[to,from]=0f;
        }
        if(energyMatrix[from,to]!=0){
            EB_energy[from,to]=(udc_energy-alpha_energy[from,to]*tc*d[from,to]-beta_energy[from,to]*rc)*energyMatrix[from,to]-ec_energy[from,to]-cc;
            EB_energy[to,from]=(upc_energy*s_energy[to]-(1-alpha_energy[from,to])*tc*d[to,from]-(1-beta_energy[from,to])*rc)*energyMatrix[from,to]+ec_energy[from,to]-cc;
        }else{
            EB_energy[from,to]=0f;
            EB_energy[to,from]=0f;
        }
        if(materialMatrix[from,to]!=0){
            EB_material[from,to]=(udc_material-alpha_material[from,to]*tc*d[from,to]-beta_material[from,to]*rc)*materialMatrix[from,to]-ec_material[from,to]-cc;
            EB_material[to,from]=(upc_material*s_material[to]-(1-alpha_material[from,to])*tc*d[to,from]-(1-beta_material[from,to])*rc)*materialMatrix[from,to]+ec_material[from,to]-cc;
        }else{
            EB_material[from,to]=0f;
            EB_material[to,from]=0f;
        }
    }

    public void updateListScrollText(){
        listScrollText.text="";
        float sum_w,sum_e,sum_m,left_w,left_e,left_m;
        for(int i=0;i<13+chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen;i++){
        //for(int i=0;i<1;i++){
            sum_w=0f;
            sum_e=0f;
            sum_m=0f;
            for(int j=0;j<13+chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen;j++){
                sum_w+=waterMatrix[i, j];
                sum_e+=energyMatrix[i, j];
                sum_m+=materialMatrix[i, j];
            }
            left_w=(float)Math.Round(W_water[i]*pd[i]-sum_w,2);
            left_e=(float)Math.Round(W_energy[i]*pd[i]-sum_e,2);
            left_m=(float)Math.Round(W_material[i]*pd[i]-sum_m,2);
                
            listScrollText.text+="Place: "+places[i].name+"\nWater waste left: "+
            left_w+" units\nEnergy waste left: "+left_e+" units\nMaterial waste left: "+
            left_m+" units\nEnvironmnetal indicator is: "+(float)Math.Round(ENV[i],2)+"\nEconomical indicator is: "+(float)Math.Round(ECO[i],2)+"\n\n\n";
        }
        
    }

    public void reset(){
        //value chains
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++){
                ec_water[i,j]=0f;
                ec_energy[i,j]=0f;
                ec_material[i,j]=0f;
                alpha_water[i,j]=0f;
                alpha_energy[i,j]=0f;
                alpha_material[i,j]=0f;
                beta_water[i,j]=0f;
                beta_energy[i,j]=0f;
                beta_material[i,j]=0f;

                EB_water[i,j]=0f;
                EB_energy[i,j]=0f;
                EB_material[i,j]=0f;
                if(waterMatrix[i,j]>0f){
                    waterMatrix[i,j]=0f;
                    updateValueChains(i, j, "water");
                }
                if(energyMatrix[i,j]>0f){
                    energyMatrix[i,j]=0f;
                    updateValueChains(i, j, "energy");
                }
                if(materialMatrix[i,j]>0f){
                    materialMatrix[i,j]=0f;
                    updateValueChains(i, j, "material");
                }                
            }
        //initial connections
        energyMatrix[0,2]=5f;
        energyMatrix[0,8]=3f;

        materialMatrix[1,0]=8f;

        waterMatrix[2,4]=5f;

        waterMatrix[4,2]=7f;
        waterMatrix[4,7]=5f;
        energyMatrix[4,7]=6f;

        waterMatrix[5,7]=6f;

        waterMatrix[7,4]=3f;
        materialMatrix[7,6]=4f;
        energyMatrix[7,12]=4f;
        energyMatrix[7,5]=4f;
        energyMatrix[7,8]=1f;
        energyMatrix[7,10]=1f;
        waterMatrix[7,2]=5f;

        waterMatrix[8,7]=6f;

        waterMatrix[10,7]=7f;

        materialMatrix[12,6]=9f;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++){
                updateValueChains(i, j, "water");
                updateValueChains(i, j, "energy");
                updateValueChains(i, j, "material");
            }

        //distance matrix
        for(int i=0;i<n;i++)
            for(int j=0;j<n;j++){
                if(j<i && i<13 && j<13) d[i,j]=Vector3.Distance(places[i].transform.position, places[j].transform.position);
                else d[i,j]=0;
            }
        
        for(int i=0;i<n;i++)
            for(int j=n-1;j>i;j--)
                d[i,j]=d[j,i];

        //compute initial indicators
         for(int i=0;i<13;i++)
            computeENVandECO(i);
        
        for(int i=chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen-1;i>=0;i--){
            GameObject.Destroy(selectLocationUI.transform.Find(places[13+i].name+"Indicator").gameObject);
            GameObject.Destroy(GameObject.Find(places[13+i].name));
            
            places.RemoveAt(13+i);
        }
        chooseNewUI.GetComponent<ChooseNewUI>().selectedIndex=-1;
        chooseNewUI.GetComponent<ChooseNewUI>().nrOfChosen=0;
        selected1=false;
        selected2=false;
        selectedPlaceIndex1=0;
        selectedPlaceIndex2=1;
        chooseNewUI.GetComponent<ChooseNewUI>().placeAbutton.interactable=true;
        chooseNewUI.GetComponent<ChooseNewUI>().placeBbutton.interactable=true;
        chooseNewUI.GetComponent<ChooseNewUI>().placeCbutton.interactable=true;
        chooseNewUI.GetComponent<ChooseNewUI>().placeDbutton.interactable=true;
        selectFirstText.gameObject.SetActive(false);
        selectSecondText.gameObject.SetActive(false);
        //update the list of places and their info
        updateListScrollText();
        canvasMain.gameObject.SetActive(true);
    }
}
