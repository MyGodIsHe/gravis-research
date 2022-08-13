using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour
{
    public static LineArrow instance;
    public GameObject arrow;
    private GraphManager gm;
    private bool setArrow = true;
    
    private void Awake() {
        instance = GameObject.Find("ClickNode").GetComponent<LineArrow>();
        gm = GraphManager.Get();
    }

    private void OnEnable() {
        setArrow = true;
    }
    
    public void ArrowPosition(GameObject start, GameObject target)
    {
        RaycastHit hit;

        if(Physics.Linecast(start.transform.position, target.transform.position, out hit))
        {
            GameObject arr = Instantiate(arrow, target.transform);
            arr.transform.position = hit.point;
            arr.transform.LookAt(start.transform);
            arr.transform.GetChild(0).transform.localPosition = new Vector3(0,0,0.2f);
        }     
    }

    public void ArrowPoint(GameObject arrowParent, GameObject arrowPoint, GameObject startNode)
    {
        GameObject arr = Instantiate(arrow, arrowParent.transform);
        arr.transform.position = arrowPoint.transform.position;
        arr.transform.LookAt(startNode.transform);
    }
}
