using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNode : MonoBehaviour
{
    public Transform selectedNode;
    public GameObject nodePrefab;
    public Material lineMaterial;
    public LoadListener loadListener;
    public List<GameObject> cubeNodes;
    public List<Node> nodes;


    void Update()
    {
        SelectObserver();
        ButtonManager();
    }

    public void SelectObserver()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            selectedNode = hit.transform;
        }
    }

    public void ConnectNodes(GameObject selectedNode, GameObject newNode)
    {
        LineRenderer line = newNode.AddComponent<LineRenderer>();
        line.material = lineMaterial;
        line.widthMultiplier = 0.15f;
        line.SetPosition(line.positionCount - 2, selectedNode.transform.position);
        line.SetPosition(line.positionCount - 1, newNode.transform.position);

    }

    public void ButtonManager()
    {
        if(Input.GetKey(KeyCode.N))
        {
            if(selectedNode != null)
            {
                //точка создания новой ноды
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPosition = Camera.main.WorldToScreenPoint(mousePos);
                mousePos.z = 10;
                GameObject pref = Instantiate(nodePrefab, worldPosition, Quaternion.identity);
                //cubeNodes = FindObjectsWithTag("Cube");
               
                pref.transform.position =  AlignNode();
                foreach(var cubeNode in cubeNodes)
                {
                    //nodes.Add(node);
                }
                //loadListener.AlignNodes();
                //Node.AlignNodesByForceDirected(nodes);
                //
                ConnectNodes(selectedNode.gameObject, pref);
            }
        }
    }

    public Vector3 AlignNode() 
    {
        Vector3 force = dispersionForce();
        var dist = force.magnitude;
        Vector3 pos = new Vector3();
        return pos += force.normalized * 0.2f * (2 - dist);
    }

    private static Vector3 dispersionForce()
    {
        return new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
        );
    }
}
