using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEventHandler : MonoBehaviour
{
    public GameObject m_nodePrefab;
    public GameObject m_nodeObject;
    public GameObject selectedNodeGameobject;
    public GameObject m_warningTxt;

    public Material m_defaultMaterial;
    public Material m_selectMaterial;

    public Node selectedNode;
    public string selectedGStream;

    public bool loaded;
    public bool selected;

    public static DragonEventHandler sInstance { get; private set; }
    DragonEventHandler()
    {
        sInstance = this;
    }

    void Start()
    {
        loaded = false;
        selected = false;
    }

    void Update()
    {
        CreateCube();
    }

    public void InitSelectNode(GameObject f_node)
    {
        selectedNodeGameobject = f_node;
        GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("node");
        for(int i=0; i< nodeObjects.Length; i++)
        {
            nodeObjects[i].GetComponent<MeshRenderer>().material = m_defaultMaterial;
        }
        selectedNodeGameobject.GetComponent<MeshRenderer>().material = m_selectMaterial;
        selectedNode = f_node.GetComponent<CubeScript>()._node;
        selected = true;
        Debug.Log(selectedNodeGameobject.transform.position);
    }

    public void CreateCube()
    {
        if (!loaded) return;

        if(Input.GetKeyDown(KeyCode.N))
        {
            if(!selected)
            {
                m_warningTxt.SetActive(true);
                StartCoroutine(Disappear(m_warningTxt));
                return;
            }

            AddNewNode();
        }
    }

    public void AddNewNode()
    {

    }

    private IEnumerator Disappear(GameObject f_textObj)
    {
        yield return new WaitForSeconds(3);
        f_textObj.SetActive(false);
    }

}