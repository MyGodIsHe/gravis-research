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
        if((Vector4)DeselectColor == Vector4.zero)
        {
            DeselectColor = mesh.material.color;
        }
        mesh.material.color = DeselectColor;
    }
    private void OnEnable() {
        ClickNode.instance.OnNodeSelected += ChangeColorOnSelected;
        ClickNode.instance.OnNodeDeselected += ChangeColorOnDeselected;
    }

    void ChangeColorOnSelected()
    {
        if(ClickNode.instance.node == this.gameObject)
        mesh.material.color = SelectColor;
        else
        mesh.material.color = DeselectColor;
    }
    
    void ChangeColorOnDeselected()
    {
        if(ClickNode.instance.node != this.gameObject)
        mesh.material.color = DeselectColor;
    }

    /*void Update()
    {
        if(ClickNode.instance.node == this.gameObject)
        {
            mesh.material.color = SelectColor;
        }
        else
        {
            mesh.material.color = DeselectColor;
        }
    }*/
}
