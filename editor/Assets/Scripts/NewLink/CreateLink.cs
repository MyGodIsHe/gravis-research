using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLink : MonoBehaviour
{
    private GraphManager graphManager;

    public GameObject nodeContextMenu;
    [SerializeField]private GameObject firstNode;
    [SerializeField]private GameObject secondNode;
    public bool createLinkClicked = false;

    public void ContextMenuOpen()
    {
        nodeContextMenu.SetActive(true);
    }

    public void ContextMenuClose()
    {
        nodeContextMenu.SetActive(false);
    }

    private void Awake() {
        graphManager = GraphManager.Get();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit  hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform != null 
                /*&& hit.transform.gameObject != ClickNode.instance.node*/ 
                && createLinkClicked 
                && !nodeContextMenu.activeSelf)
                {
                    secondNode = hit.transform.gameObject;
                    graphManager.LineTo(firstNode, secondNode);
                    createLinkClicked = false;
                    firstNode = null;
                    secondNode = null;
                }
                else if (firstNode == null && createLinkClicked == false)
                {
                    firstNode = ClickNode.instance.node;
                }
                else if (hit.transform == null)
                {
                    ContextMenuClose();
                }
            }
        }
    

        if(firstNode != null && !createLinkClicked)
        {
            ContextMenuOpen();
        }
        else
        {
            ContextMenuClose();
        }
    }

    public void CreateArrowLink()
    {
        createLinkClicked = true;
        //firstNode = ClickNode.instance.node;
    }
}
