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

    /*public static void AlignNodes(List<GameObject> nodes)
    {
        // set levels
        var startNode = GetStartNode(nodes);
        var levels = new Dictionary<Node, int>
        {
            [startNode] = 0
        };
        var future = new HashSet<Node>
        {
            startNode
        };
        var pass = new HashSet<Node>();
        while (future.Count != 0)
        {
            var newFuture = new HashSet<Node>();
            foreach (var node in future)
            {
                var level = levels[node];
                foreach (var input_node in node.inputs)
                {
                    if (!pass.Contains(input_node))
                    {
                        levels[input_node] = level - 1;
                        newFuture.Add(input_node);
                    }
                }
                foreach (var output_node in node.outputs)
                {
                    if (!pass.Contains(output_node))
                    {
                        levels[output_node] = level + 1;
                        newFuture.Add(output_node);
                    }
                }
                pass.Add(node);
            }
            future = newFuture;
        }
    }*/
}
