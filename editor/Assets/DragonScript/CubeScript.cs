using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public Node _node;

    private void OnMouseDown()
    {
        Debug.Log("click cube");
        if (_node.type == NodeType.Input || _node.type == NodeType.Output) return;
        DragonEventHandler.sInstance.InitSelectNode(gameObject);
    }
}
