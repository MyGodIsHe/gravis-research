using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour
{
    public static LineArrow instance;
    public GameObject arrow;
    public GraphManager gm;
    public GameObject lastNode;
    public LineRenderer lineRenderer;
    public bool setArrow = true;
    
    private void Awake() {
        instance = this;
        gm = GraphManager.Get();
    }

    private void OnEnable() {
        setArrow = true;
    }

    public void SetArrow()
    {
        lineRenderer = lastNode.GetComponent<LineRenderer>();
        RaycastHit hit;
        for (int i = 1; i < lineRenderer.positionCount-1; i++)
        {
            Physics.Raycast(transform.position, lineRenderer.GetPosition(i), out hit);
            if(hit.transform != null)
            {
                print(hit.transform.position);
                GameObject arr = Instantiate(arrow, hit.transform);
                arr.transform.position = hit.point;
                arr.transform.LookAt(transform.position);
            }
            else
            {
                print("none");
            }
            if(i == lineRenderer.positionCount-1)
            {
                //setArrow = false;
            }
        }
    }
    
    public void ArrowPosition(GameObject start, GameObject target)
    {
        RaycastHit hit;

        if(Physics.Linecast(start.transform.position, target.transform.position, out hit))
        {
            GameObject arr = Instantiate(arrow, hit.transform);
            arr.transform.position = hit.point;
            arr.transform.LookAt(start.transform);
        }     
    }

    public GameObject GetLastNode(GameObject createdNode)
    {
        return createdNode;
    }

    
}
