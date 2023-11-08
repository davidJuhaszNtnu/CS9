using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToCenter : MonoBehaviour
{
    public RectTransform scrollPanel;
    public GameObject[] images;
    // public GameObject appPanel;
    public float lerpSpeed;
    public Sprite selectedImage, notSelectedImage;

    private float[] distances;
    private bool dragging;
    private float distanceBetweenImages;
    public int indexMin; //index of the layer closest to the center

    void Start()
    {
        int images_count = images.Length;
        distances = new float[images_count];
        distanceBetweenImages = Mathf.Abs(images[1].GetComponent<RectTransform>().anchoredPosition.x - images[0].GetComponent<RectTransform>().anchoredPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < images.Length; i++){
            distances[i] = Mathf.Abs(transform.position.x - images[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distances);

        for (int i = 0; i < images.Length; i++){
            if(distances[i] == minDistance){
                indexMin = i;
                // appPanel.GetComponent<ButtonsController>().current_layer = indexMin;
                // images[i].GetComponent<Image>().sprite = selectedImage;
            }
            // else images[i].GetComponent<Image>().sprite = notSelectedImage;
        }

        if(!dragging){
            LerpToIndustry(indexMin * (-distanceBetweenImages));
        }
    }

    void LerpToIndustry(float position){
        float newX = Mathf.Lerp(scrollPanel.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);
        Vector2 newPosition = new Vector2(newX, scrollPanel.anchoredPosition.y);

        scrollPanel.anchoredPosition = newPosition;
    }

    public void StartDrag(){
        dragging = true;
    }

    public void EndDrag(){
        dragging = false;
    }
}
