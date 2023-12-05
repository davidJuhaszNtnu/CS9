using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrowProps : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] points;
    public float t;
    public Vector3 up;

    public Vector3 curve(float t){
        return ((float)Math.Pow(1-t, 2))*points[0]+(2*(1-t)*t)*points[1]+((float)Math.Pow(t, 2))*points[2];
    }
    public Vector3 curveDerivative(float t){
        return (2*(1-t)*(points[1]-points[0])+2*t*(points[2]-points[1]));
    }
}
