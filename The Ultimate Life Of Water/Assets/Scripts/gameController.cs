using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using TMPro;
using System;

public class gameController : MonoBehaviour
{
    public GameObject arrow_prefab, path_prefab, arrow_parent, path_parent, center_to_compare, env, app, industries_parent, tutorial;
    public GameObject mainPanel, addNewIndustryPanel, placementOfNewIndustryPanel, connectionPanel, 
                    chooseIndustriesPanel, connectionWarningPanel, infoCardsPanel, newConectionAnimationPanel, playerWonPanel, questionMainPanel,
                    questionInfoCardPanel, questionPlacementOfNewIndustryPanel, questionChooseIndustriesPanel, questionConnectionPanel,
                    questionNewConnectionAnimationPanel;
    public List<GameObject> industries;
    public Button make_connection_button, add_new_button, replay_button, exit_button, question_button;

    public float[] s; // indicates how much of the waste water received can be used for production
    public float[] s_old;
    public float[] w; // indicates how much of the water used for production becomes waste water
    public float[] p; // how much MINIMUM clean water the industry requires
    public float[] out_waste; // output waste
    public float[,] clean; // input clean
    public float[,] waste; // input waste
    public float total_in, total_out;
    public int new_industry_index;
    public int existing_count, new_count, industry_count;

    public int from_index, to_index;
    private int selected_index;
    public bool new_industries_added, all_new_industries_added, tutorialOn;
    public bool[] new_industries_placed;
    bool[] isReceivingEnough;
    public Material isReceivingEnough_material;
    public bool[] industry_chosen;
    bool player_won;

    // visualization of score
    float clean_water_reduction, waste_water_reduction, clean_total_start, waste_total_start;
    public GameObject clean_water_pillar, waste_water_pillar;
    public float max_reduction_clean, max_reduction_waste;
    public TextMeshProUGUI max_reduction_clean_text, max_reduction_waste_text, clean_water_reduction_text, waste_water_reduction_text;
    public Material totalIn_material, totalOut_material;

    //ultimate technology
    public bool[] has_technology;
    public GameObject ultimate_technology_prefab;
    public TextMeshProUGUI ut_score_text;

    // info cards
    Ray ray;
    RaycastHit hit;
    public Camera arCamera;
    public bool allowed_to_view_info;

    void Start()
    {
        // start();
        // tutorialOn = true;

        // tutorialOn = false;
    }

    public void start(){
        // mainPanel.SetActive(false);
        addNewIndustryPanel.SetActive(false);
        placementOfNewIndustryPanel.SetActive(false);
        chooseIndustriesPanel.SetActive(false);
        connectionPanel.SetActive(false);
        connectionWarningPanel.SetActive(false);
        infoCardsPanel.SetActive(false);
        newConectionAnimationPanel.SetActive(false);
        playerWonPanel.SetActive(false);
        questionMainPanel.SetActive(false);
        questionInfoCardPanel.SetActive(false);
        questionPlacementOfNewIndustryPanel.SetActive(false);
        questionChooseIndustriesPanel.SetActive(false);
        questionConnectionPanel.SetActive(false);
        questionNewConnectionAnimationPanel.SetActive(false);

        new_industries_added = false;
        allowed_to_view_info = true;
        clean_water_reduction = 0f;
        waste_water_reduction = 0f;

        existing_count = 4;
        new_count = 3;
        industry_count = existing_count + new_count + 1;

        s = new float[industry_count];
        s_old = new float[industry_count];
        w = new float[industry_count];
        p = new float[industry_count];
        out_waste = new float[industry_count];
        waste = new float[industry_count, industry_count];
        clean = new float[industry_count, industry_count];
        isReceivingEnough = new bool[new_count];
        new_industries_placed = new bool[new_count];
        industry_chosen = new bool[new_count];
        has_technology = new bool[industry_count];

        // setup_game();
    }

    public void setup_game(){
        for (int i = 1; i < industries.Count; i++){
            if(i <= existing_count)
                createArrowPaths(0, i, 0.5f, arrow_prefab, path_prefab, Random.Range(0f, 1f));
        }

        for (int i = 0; i < new_count; i++){
            isReceivingEnough[i] = false;
            new_industries_placed[i] = false;
            industry_chosen[i] = false;
            //water flow
            industries[i + existing_count + 1].transform.GetChild(0).gameObject.SetActive(false);
            industries[i + existing_count + 1].SetActive(false);
            //bottom of industries
            industries[i + existing_count + 1].transform.GetChild(2).gameObject.SetActive(true);
        }

        player_won = false;
        replay_button.gameObject.SetActive(true);
        exit_button.gameObject.SetActive(true);

        //existing indutries
        s[1] = 0.9f;
        w[1] = 0.2f;
        p[1] = 5f;

        s[2] = 0.6f;
        w[2] = 0.4f;
        p[2] = 10f;

        s[3] = 0.4f;
        w[3] = 0.6f;
        p[3] = 15f;

        s[4] = 0.2f;
        w[4] = 0.9f;
        p[4] = 20f;

        //new industries
        s[5] = 0.8f;
        w[5] = 0.5f;
        p[5] = 4f;

        s[6] = 0.6f;
        w[6] = 0.6f;
        p[6] = 8f;

        s[7] = 0.4f;
        w[7] = 0.8f;
        p[7] = 12f;

        for (int i = 1; i < industry_count; i++){
            s_old[i] = s[i];
            has_technology[i] = false;
            //ultimate tech
            industries[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        update_texts_on_cards();
        ut_score_text.text = "0/2";

        for (int i = 0; i < industry_count; i++){
            out_waste[i] = 0f;
            for (int j = 0; j < industry_count; j++){
                waste[i, j] = 0f;
                clean[i, j] = 0f;
            }
        }
        
        float sum_in = 0f, sum_out = 0f;
        for (int i = 1; i <= existing_count; i++){
            clean[0, i] = p[i];
            out_waste[i] = clean[0, i]*w[i];
            sum_in += clean[0, i];
            sum_out += out_waste[i];
            //water flow
            industries[i].transform.GetChild(0).gameObject.SetActive(true);
            //bottom of industries
            industries[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        total_in = sum_in;
        total_out = sum_out;

        clean_total_start = total_in;
        waste_total_start = total_out;

        clean_water_reduction = 1 - total_in / clean_total_start;
        waste_water_reduction = 1 - total_out / waste_total_start;

        max_reduction_clean_text.text = Math.Round(max_reduction_clean * 100, 2).ToString() + "%";
        max_reduction_waste_text.text = Math.Round(max_reduction_waste * 100, 2).ToString() + "%";

        update_pillars();

        // check_if_winning();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform arrow in arrow_parent.transform)
                animateArrow(arrow.gameObject);

        if(!new_industries_added)
            make_connection_button.interactable = false;
        else make_connection_button.interactable = true;

        if(!tutorialOn){
            if(all_new_industries_added)
                add_new_button.interactable = false;
            else add_new_button.interactable = true;
        }

        //tutorial
        if(tutorialOn){
            replay_button.interactable = false;
            question_button.interactable = false;
            exit_button.interactable = false;
            // if(tutorial.GetComponent<Tutorial>().industry_bool)
            //     add_new_button.interactable = false;
            // else add_new_button.interactable = true;
        }else{
            replay_button.interactable = true;
            question_button.interactable = true;
            exit_button.interactable = true;
        }

        if(Input.GetMouseButtonDown(0) && allowed_to_view_info && !tutorialOn){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                foreach(GameObject industry in industries){
                    if(hit.collider.name == industry.name){
                        mainPanel.SetActive(false);
                        infoCardsPanel.SetActive(true);
                        int index = industries.IndexOf(industry);
                        selected_index = index;
                        float clean_in;
                        float sum_in = 0f;
                        for (int i = 1; i < industry_count; i++)
                            sum_in += waste[i, index];

                        // count the number of ultimate technologies
                        int k = 0;
                        for (int i = 1; i < industry_count; i++){
                            if(has_technology[i])
                                k++;
                        }
                        bool UT_limit_reached;
                        if(k == 2)
                            UT_limit_reached = true;
                        else UT_limit_reached = false;

                        if(index != 0){
                            if(index <= existing_count)
                                clean_in = clean[0, index] + sum_in * s[index];
                            else clean_in = sum_in * s[index];

                            infoCardsPanel.GetComponent<InfoCardsPanel>().set_info(index, clean_in, out_waste[index], industry.name, UT_possible(index), UT_limit_reached);
                            make_connection_button.gameObject.SetActive(true);
                        }else{
                            infoCardsPanel.GetComponent<InfoCardsPanel>().set_info(index, 0f, 0f, industry.name, UT_possible(index), UT_limit_reached);
                            make_connection_button.gameObject.SetActive(false);
                        }

                        allowed_to_view_info = false;
                    }
                }
            }
        }

        for (int i = 0; i < new_count; i++){
            if(new_industries_placed[i] && !industry_chosen[i]){
                if(!isReceivingEnough[i])
                    animate_new_industry_color(i + existing_count + 1);
                else industries[i + existing_count + 1].transform.GetChild(2).GetComponent<ColorProps>().apply_material(isReceivingEnough_material);
            }
        }

        //tutorial
        if(Input.GetMouseButtonDown(0) && tutorial.transform.GetComponent<Tutorial>().industry_secondTime && tutorialOn){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(hit.collider.name == "Utility"){
                    mainPanel.SetActive(false);
                    infoCardsPanel.SetActive(true);
                    infoCardsPanel.transform.GetComponent<InfoCardsPanel>().ok_button.interactable = false;
                    GameObject industry = industries[1];
                    int index = 2;
                    selected_index = 2;
                    float clean_in;
                    float sum_in = 0f;
                    for (int i = 1; i < industry_count; i++)
                        sum_in += waste[i, index];

                    // count the number of ultimate technologies
                    int k = 0;
                    for (int i = 1; i < industry_count; i++){
                        if(has_technology[i])
                            k++;
                    }
                    bool UT_limit_reached;
                    if(k == 2)
                        UT_limit_reached = true;
                    else UT_limit_reached = false;

                    if(index != 0){
                        if(index <= existing_count)
                            clean_in = clean[0, index] + sum_in * s[index];
                        else clean_in = sum_in * s[index];

                        infoCardsPanel.GetComponent<InfoCardsPanel>().set_info(index, clean_in, out_waste[index], "Utility", UT_possible(index), UT_limit_reached);
                    }else infoCardsPanel.GetComponent<InfoCardsPanel>().set_info(index, 0f, 0f, "Utility", UT_possible(index), UT_limit_reached);

                    allowed_to_view_info = false;
                }
            }
        }
    }

    public void add_new_bttn(){
        allowed_to_view_info = false;
        mainPanel.SetActive(false);
        addNewIndustryPanel.SetActive(true);
        // if(tutorialOn){
        //     tutorial.GetComponent<Tutorial>().addNewButtonTutorial.SetActive(false);
        // }
    }

    public void make_connection_bttn(){
        allowed_to_view_info = false;
        mainPanel.SetActive(false);
        chooseIndustriesPanel.SetActive(true);
        chooseIndustriesPanel.GetComponent<ChooseIndustriesPanel>().setup_connection(selected_index);
        infoCardsPanel.SetActive(false);
        if(tutorialOn){
            tutorial.GetComponent<Tutorial>().makeConnectionButtonTutorial.SetActive(false);
            tutorial.GetComponent<Tutorial>().connectionPanel.SetActive(true);
        }
    }

    public void replay_bttn(){
        restart();
    }

    public void question_mainPanel_bttn(){
        allowed_to_view_info = false;
        questionMainPanel.SetActive(true);
    }

    public void ok_questioMainPanel_bttn(){
        allowed_to_view_info = true;
        questionMainPanel.SetActive(false);
        if(placementOfNewIndustryPanel.activeSelf)
            placementOfNewIndustryPanel.GetComponent<PlacementOfNewIndustryPanel>().allow_to_place = true;
    }

    private void animateArrow(GameObject arrow){
        if(arrow.GetComponent<ArrowProps>().t<=1.0f){
            arrow.GetComponent<ArrowProps>().t+=0.01f;
            // if(arrow.name[5].ToString() == "0")
                arrow.transform.localPosition = arrow.GetComponent<ArrowProps>().curve(arrow.GetComponent<ArrowProps>().t);
            // else arrow.transform.position = arrow.GetComponent<ArrowProps>().curve(arrow.GetComponent<ArrowProps>().t);
            //arrow rotation
            Vector3 up = Quaternion.AngleAxis(1f, arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t))*arrow.GetComponent<ArrowProps>().up;
            arrow.GetComponent<ArrowProps>().up = up;
            Quaternion q = Quaternion.LookRotation(arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t), up)*Quaternion.Euler(90,90,0);
            // if(arrow.name[5].ToString() == "0")
                arrow.transform.localRotation = q;
            // else arrow.transform.rotation = q;
        }else arrow.GetComponent<ArrowProps>().t=0.0f;
    }

    private void animate_new_industry_color(int index){
        if(industries[index].transform.GetChild(2).GetComponent<ColorProps>().forward)
            industries[index].transform.GetChild(2).GetComponent<ColorProps>().t += 0.02f;
        else industries[index].transform.GetChild(2).GetComponent<ColorProps>().t -= 0.02f;

        if(industries[index].transform.GetChild(2).GetComponent<ColorProps>().t > 1f){
            industries[index].transform.GetChild(2).GetComponent<ColorProps>().forward = false;
            industries[index].transform.GetChild(2).GetComponent<ColorProps>().t -= 0.02f;
        }
        if(industries[index].transform.GetChild(2).GetComponent<ColorProps>().t < 0f){
            industries[index].transform.GetChild(2).GetComponent<ColorProps>().forward = true;
            industries[index].transform.GetChild(2).GetComponent<ColorProps>().t += 0.02f;
        }

        industries[index].transform.GetChild(2).GetComponent<ColorProps>().flash_color();
    }

    private void createArrowPaths(int from, int to, float curvature, GameObject arrow_prefab, GameObject path_prefab, float t){
        var path = Instantiate(path_prefab);
        var arrow = Instantiate(arrow_prefab);
        arrow.transform.SetParent(arrow_parent.transform, false);
        path.transform.SetParent(path_parent.transform, false);
        // arrow.transform.localScale *= app.GetComponent<App>().scale;
        arrow.name = "arrow" + from.ToString() + "-" + to.ToString();
        path.name = "path" + from.ToString() + "-" + to.ToString();
        //get the points for the bezier curve
        Vector3[] anchors = new Vector3[3];
        Vector3 distanceVector = industries[to].transform.localPosition - industries[from].transform.localPosition;
        float k = distanceVector.magnitude * curvature;
        anchors[0] = industries[from].transform.localPosition + industries_parent.transform.localPosition;
        anchors[1] = (industries[from].transform.localPosition + industries[to].transform.localPosition)/2 + new Vector3(0, k * app.GetComponent<App>().scale, 0f) + industries_parent.transform.localPosition;
        anchors[2] = industries[to].transform.localPosition + industries_parent.transform.localPosition;
        arrow.GetComponent<ArrowProps>().points = anchors;
        arrow.GetComponent<ArrowProps>().t = t;
        arrow.GetComponent<ArrowProps>().up = Vector3.up;

        LineRenderer lr = path.GetComponent<LineRenderer>();
        lr.positionCount = 10;
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        Vector3[] linePos=new Vector3[lr.positionCount];
        for(int i = 0; i < lr.positionCount; i++){
            linePos[i]=(arrow.GetComponent<ArrowProps>().curve((float)i/(lr.positionCount-1)));
        }
        lr.numCornerVertices = 10;
        lr.SetPositions(linePos);        
    }

    void destroyArrowPaths(int from, int to){
        foreach(Transform arrow in arrow_parent.transform){
            if(arrow.gameObject.name == "arrow" + from.ToString() + "-" + to.ToString())
                Destroy(arrow.gameObject);
        }
        foreach(Transform path in path_parent.transform){
            if(path.gameObject.name == "path" + from.ToString() + "-" + to.ToString())
                Destroy(path.gameObject);
        }
    }

    public void count_number_of_UT(){
        // count the number of ultimate technologies
        int k = 0;
        for (int i = 1; i < industry_count; i++){
            if(has_technology[i])
                k++;
        }

        ut_score_text.text = k.ToString() + "/2";
    }

    public void make_connection(float clean_water_amount, int from, int to){
        from_index = from;
        to_index = to;

        if(clean_water_amount != 0f && waste[from_index, to_index] == 0f)
            createArrowPaths(from_index, to_index, 0.6f, arrow_prefab, path_prefab, Random.Range(0f, 1f));
        if(clean_water_amount == 0f && waste[from_index, to_index] != 0f)
            destroyArrowPaths(from_index, to_index);

        waste[from_index, to_index] = clean_water_amount / s[to_index];
        // Debug.Log(from_index + ", " + to_index + " => " + waste[from_index, to_index]);

        float sum_out_from = 0f, sum_in_from = 0f, sum_out_to = 0f, sum_in_to = 0f;
        for (int i = 1; i < industry_count; i++){
            sum_out_from += waste[from_index, i];
            sum_in_from += waste[i, from_index];
            sum_out_to += waste[to_index, i];
            sum_in_to += waste[i, to_index];
        }

        //from existing to new
        if(from_index <= existing_count){
            out_waste[from_index] = clean[0, from_index] * w[from_index] + sum_in_from * (1-s[from_index] + s[from_index] * w[from_index]) - sum_out_from;
            out_waste[to_index] = sum_in_to * (1-s[to_index] + s[to_index] * w[to_index]) - sum_out_to;
            // is receiving enough clean water?
            if(sum_in_to * s[to_index] < p[to_index])
                isReceivingEnough[to_index - existing_count - 1] = false;
            else isReceivingEnough[to_index - existing_count - 1] = true;
        }// from new to existing
        else if(to_index <= existing_count){
            clean[0, to_index] = p[to_index] - sum_in_to * s[to_index];
            if(clean[0, to_index] <= 0f){
                clean[0, to_index] = 0f;
            }

            out_waste[from_index] = sum_in_from * (1-s[from_index] + s[from_index] * w[from_index]) - sum_out_from;
            out_waste[to_index] = clean[0, to_index] * w[to_index] + sum_in_to * (1-s[to_index] + s[to_index] * w[to_index]) - sum_out_to;

            // is receiving enough clean water?
            if(sum_in_from * s[from_index] < p[from_index])
                isReceivingEnough[from_index - existing_count - 1] = false;
            else isReceivingEnough[from_index - existing_count - 1] = true;
        }// from new to new
        else{
            out_waste[from_index] = sum_in_from * (1-s[from_index] + s[from_index] * w[from_index]) - sum_out_from;
            out_waste[to_index] = sum_in_to * (1-s[to_index] + s[to_index] * w[to_index]) - sum_out_to;
            // out_waste[to_index] = clean[0, to_index] * w[to_index] + sum_in_to * (1-s[to_index] + s[to_index] * w[to_index]) - sum_out_to;

            // is receiving enough clean water?
            if(sum_in_to * s[to_index] < p[to_index])
                isReceivingEnough[to_index - existing_count - 1] = false;
            else isReceivingEnough[to_index - existing_count - 1] = true;

            if(sum_in_from * s[from_index] < p[from_index])
                isReceivingEnough[from_index - existing_count - 1] = false;
            else isReceivingEnough[from_index - existing_count - 1] = true;
        }

        if(out_waste[from_index] < 1E-05)
            out_waste[from_index] = 0f;
        if(out_waste[to_index] < 1E-05)
            out_waste[to_index] = 0f;

        float sum_in = 0f, sum_out = 0f;
        for (int i = 1; i < industry_count; i++){
            sum_out += out_waste[i];
            if(i <= existing_count)
                sum_in += clean[0, i];
        }
        total_in = sum_in;
        total_out = sum_out;

        // new connection animation
        float in_from = clean_water_reduction;
        float in_to = 1 - total_in / clean_total_start;
        float out_from = waste_water_reduction;
        float out_to = 1 - total_out / waste_total_start;

        if(Math.Abs(in_to - in_from) > 1E-05 || Math.Abs(out_to - out_from) > 1E-05){
            newConectionAnimationPanel.GetComponent<NewConnectionAnimationPanel>().setup_animation(in_from, in_to, out_from , out_to);
            newConectionAnimationPanel.SetActive(true);
            mainPanel.SetActive(false);
            allowed_to_view_info = false;
        }

        clean_water_reduction = 1 - total_in / clean_total_start;
        waste_water_reduction = 1 - total_out / waste_total_start;

        update_pillars();
        check_water_flow(from, to);

        // Debug.Log(total_in + ", " + total_out);
    }

    private void check_water_flow(int from, int to){
        if(out_waste[from] == 0f)
            industries[from].transform.GetChild(0).gameObject.SetActive(false);
        else industries[from].transform.GetChild(0).gameObject.SetActive(true);

        if(out_waste[to] == 0f)
            industries[to].transform.GetChild(0).gameObject.SetActive(false);
        else industries[to].transform.GetChild(0).gameObject.SetActive(true);
    }

    public bool UT_possible(int index){
        float sum_out = 0f;
        for (int i = 1; i < industry_count; i++){
            sum_out += waste[index, i];
        }

        if(sum_out == 0f)
            return true;
        else return false;
    }

    public void use_ultimate_technology(int index){
        // only available if there are no outgoing connections!, there can be incoming
        float sum_out = 0f, sum_in = 0f;
        for (int i = 1; i < industry_count; i++){
            sum_out += waste[index, i];
            sum_in += waste[i, index];
        }

        if(has_technology[index]){
            s[index] = 1f;
            // create prefab
            // if(industries[index].transform.childCount == 1){
            //     var instance = Instantiate(ultimate_technology_prefab);
            //     instance.transform.SetParent(industries[index].transform, true);
            //     instance.transform.localPosition = new Vector3(0f, 1f, 0f);
            //     instance.transform.localScale *= app.GetComponent<App>().scale*1.5f;
            // }
            if(!industries[index].transform.GetChild(1).gameObject.activeSelf){
                industries[index].transform.GetChild(1).gameObject.SetActive(true);
            }
        }else{
            s[index] = s_old[index];
            //destroy prefab
            // if(industries[index].transform.childCount > 1)
            //     Destroy(industries[index].transform.GetChild(1).gameObject);
            if(industries[index].transform.GetChild(1).gameObject.activeSelf){
                industries[index].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        //if the industry is new
        if(index > existing_count){
            out_waste[index] = sum_in * (1-s[index] + s[index] * w[index]) - sum_out;
            // is receiving enough clean water?
            if(sum_in * s[index] < p[index])
                isReceivingEnough[index - existing_count - 1] = false;
            else isReceivingEnough[index - existing_count - 1] = true;
        }
        else{
            //if the industry is existing
            clean[0, index] = p[index] - sum_in * s[index];
            if(clean[0, index] <= 0f){
                clean[0, index] = 0f;
            }
            out_waste[index] = clean[0, index] * w[index] + sum_in * (1-s[index] + s[index] * w[index]) - sum_out;
        }

        if(out_waste[index] < 1E-05)
            out_waste[index] = 0f;

        sum_in = 0f;
        sum_out = 0f;
        for (int i = 1; i < industry_count; i++){
            sum_out += out_waste[i];
            if(i <= existing_count)
                sum_in += clean[0, i];
        }
        total_in = sum_in;
        total_out = sum_out;

        float in_from = clean_water_reduction;
        float in_to = 1 - total_in / clean_total_start;
        float out_from = waste_water_reduction;
        float out_to = 1 - total_out / waste_total_start;

        if(Math.Abs(in_to - in_from) > 1E-05 || Math.Abs(out_to - out_from) > 1E-05){
            newConectionAnimationPanel.GetComponent<NewConnectionAnimationPanel>().setup_animation(in_from, in_to, out_from , out_to);
            newConectionAnimationPanel.SetActive(true);
            infoCardsPanel.SetActive(false);
            allowed_to_view_info = false;
        }

        clean_water_reduction = 1 - total_in / clean_total_start;
        waste_water_reduction = 1 - total_out / waste_total_start;

        update_pillars();
        check_water_flow(index, index);
    }

    void update_pillars(){

        clean_water_reduction_text.text = Math.Round(clean_water_reduction * 100, 2).ToString() + "%";
        waste_water_reduction_text.text = Math.Round(waste_water_reduction * 100, 2).ToString() + "%";

        // max reduction = 1, new reduction = ?
        float new_height_clean = clean_water_reduction / max_reduction_clean;
        float new_height_waste = waste_water_reduction / max_reduction_waste;
        // if there is more reduction than max reduction
        if(new_height_clean >= 1f)
            new_height_clean = 1f;
        if(new_height_waste >= 1f)
            new_height_waste = 1f;

        clean_water_pillar.transform.localScale = new Vector3(clean_water_pillar.transform.localScale.x, new_height_clean, clean_water_pillar.transform.localScale.z);
        clean_water_pillar.transform.localPosition = new Vector3(0f, new_height_clean - 1, 0f);

        waste_water_pillar.transform.localScale = new Vector3(waste_water_pillar.transform.localScale.x, new_height_waste, waste_water_pillar.transform.localScale.z);
        waste_water_pillar.transform.localPosition = new Vector3(0f, new_height_waste - 1, 0f);

        //change the color of rectangles to green when achieving the max reduction
        if(clean_water_reduction >= max_reduction_clean)
            clean_water_pillar.transform.GetComponent<Renderer>().material.color = new Color(0, 1f, 0, 1f);
        else clean_water_pillar.transform.GetComponent<Renderer>().material.color = new Color(0, 0, 1f, 1f);
        if(waste_water_reduction >= max_reduction_waste){
            waste_water_pillar.transform.GetComponent<Renderer>().material.color = new Color(0, 1f, 0, 1f);
        }else waste_water_pillar.transform.GetComponent<Renderer>().material.color = new Color(0.75f, 0.314f, 0.035f, 1f);
    }

    public void check_if_winning(){
        bool everyNewisReceivingEoungh = true;
        for (int i = 0; i < new_count; i++){
            if(!isReceivingEnough[i])
                everyNewisReceivingEoungh = false;
        }
        if(clean_water_reduction >=  max_reduction_clean && waste_water_reduction >= max_reduction_waste && everyNewisReceivingEoungh && !player_won){
            mainPanel.SetActive(false);
            playerWonPanel.SetActive(true);
            allowed_to_view_info = false;
            player_won = true;
        }else allowed_to_view_info = true;

        // mainPanel.SetActive(false);
        // playerWonPanel.SetActive(true);
        // allowed_to_view_info = false;
        // player_won = true;
    }

    public void update_texts_on_cards(){
        // add new industry panel
        for (int i = 0; i < new_count; i++){
            center_to_compare.GetComponent<SnapToCenter>().images[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = industries[i + existing_count + 1].name;
            center_to_compare.GetComponent<SnapToCenter>().images[i].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (s[i + existing_count + 1]*100).ToString() + " %";
            center_to_compare.GetComponent<SnapToCenter>().images[i].transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = (w[i + existing_count + 1]*100).ToString() + " %";
            center_to_compare.GetComponent<SnapToCenter>().images[i].transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = (p[i + existing_count + 1]).ToString() + " m<sup>3</sup>";
        }

        // connection panel
        for (int i = 0; i < industry_count - 1; i++){
            connectionPanel.GetComponent<ConnectionPanel>().cards[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = industries[i].name;
            connectionPanel.GetComponent<ConnectionPanel>().cards[i].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (s[i + 1]*100).ToString() + " %";
            connectionPanel.GetComponent<ConnectionPanel>().cards[i].transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = (w[i + 1]*100).ToString() + " %";
            connectionPanel.GetComponent<ConnectionPanel>().cards[i].transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = (p[i + 1]).ToString() + " m<sup>3</sup>";
        }

        // info cards panel
        for (int i = 1; i < industry_count; i++){
            infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = industries[i].name;
            infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (s[i]*100).ToString() + " %";
            infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = (w[i]*100).ToString() + " %";
            infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = (p[i]).ToString() + " m<sup>3</sup>";
        }
    }

    public void restart(){
        foreach(Transform arrow in arrow_parent.transform)
            Destroy(arrow.gameObject);
        
        foreach(Transform path in path_parent.transform)
            Destroy(path.gameObject);
        
        for (int i = 1; i < industry_count; i++){
            if(has_technology[i]){
                infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = (s_old[i]*100).ToString() + " %";
                infoCardsPanel.GetComponent<InfoCardsPanel>().info_cards.transform.GetChild(i).transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.black;
                infoCardsPanel.GetComponent<InfoCardsPanel>().ultimate_technology_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use Ultimate technology";
            }
            s[i] = s_old[i];
        }

        new_industries_added = false;
        all_new_industries_added = false;
        allowed_to_view_info = true;
        clean_water_reduction = 0f;
        waste_water_reduction = 0f;

        for (int i = 0; i < new_count; i++){
            // industries[i + existing_count + 1].transform.GetChild(2).GetComponent<ColorProps>().apply_material(mater);
            industries[i + existing_count + 1].SetActive(false);
        }
        
        setup_game();
        
        addNewIndustryPanel.GetComponent<AddNewIndustryPanel>().restart();
        placementOfNewIndustryPanel.GetComponent<PlacementOfNewIndustryPanel>().restart();
        chooseIndustriesPanel.GetComponent<ChooseIndustriesPanel>().restart();
    }
}
