using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    
    public MeshRenderer mesh;
    public Color SelectColor;
    public Color DeselectColor;
    
    private void Awake() {
        mesh = gameObject.GetComponent<MeshRenderer>();
        if((Vector4)DeselectColor == new Vector4(0,0,0,0))
        {
            DeselectColor = mesh.material.color;
        }
    }
    
    void Update()
    {
        if(ClickNode.instance.node == this.gameObject)
        {
            mesh.material.color = SelectColor;
        }
        else
        {
            mesh.material.color = DeselectColor;
        }
    }
}
