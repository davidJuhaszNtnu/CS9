using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProps : MonoBehaviour
{
    public float t;
    public bool forward;
    public Material flashMaterial;
    
    private Color A, B;

    void Start()
    {
        t = 0f;
        forward = true;
        A = new Color(1f, 1f, 1f, 1f);
        B = flashMaterial.color;
    }

    public void flash_color(){
        if(transform.GetComponent<Renderer>().material != flashMaterial)
            transform.GetComponent<Renderer>().material = flashMaterial;
        Color c = new Color(A.r + (B.r-A.r) * t, A.g + (B.g-A.g) * t, A.b + (B.b-A.b) * t, 1f);
        transform.GetComponent<Renderer>().material.color = c;
    }

    public void apply_material(Material mat){
        transform.GetComponent<Renderer>().material = mat;
    }
}
