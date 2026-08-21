using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{   
    private float l1 = 2f;
    private float l2 = 2f;
    private float m1 = 1f;
    private float m2 = 1f;
    private float g = 9.81f;

    private float th1 = Mathf.PI / 2;
    private float th2 = Mathf.PI / 2;

    private float v1 = 0f;
    private float v2 = 0f;

    private LineRenderer lineRenderer;
    private Vector3 pivot;

    public Material mat;
   
    void Start()
    {
        this.gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.material = mat;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        pivot = Vector3.zero;
    }

    
    void Update()
    {
        float dt = Time.deltaTime;
        
        float t1 = -g * (2 * m1 + m2) *Mathf.Sin(th1);
        float t2 = -m2*g*Mathf.Sin(th1-2*th2); 
        float t3 = -2*Mathf.Sin(th1-th2)*m2*(v2*v2*l2+v1*v1*l1*Mathf.Cos(th1-th2));
        float t4 = l1*(2*m1+m2-m2*Mathf.Cos(2*th1-2*th2)); 

        float t5 = 2*Mathf.Sin(th1-th2);
        float t6 = v1*v1*l1*(m1+m2);
        float t7 = g*(m1+m2)*Mathf.Cos(th1);
        float t8 = v2*v2*l2*m2*Mathf.Cos(th1-th2);
        float t9 = l2*(2*m1+m2-m2*Mathf.Cos(2*th1-2*th2));

        float a1 = (t1+t2+t3)/t4;
        float a2 = (t5)*(t6+t7+t8)/t9;

        v1 += a1*dt;
        v2 += a2*dt;
        th1 += v1*dt;
        th2 += v2*dt;

        Vector3 pos1 = pivot + new Vector3(Mathf.Sin(th1), -Mathf.Cos(th1), 0) * l1;
        Vector3 pos2 = pos1 + new Vector3(Mathf.Sin(th2), -Mathf.Cos(th2), 0) * l2;

        lineRenderer.SetPosition(0, pivot);
        lineRenderer.SetPosition(1, pos1);
        lineRenderer.SetPosition(2, pos2);
    }
}
