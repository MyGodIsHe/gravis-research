using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClickNode : MonoBehaviour
{
    public event Action OnNodeSelected = () => { };
    public event Action OnNodeDeselected = () => { };
    
    public static ClickNode instance;
    public GameObject node;
    
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit  hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.name != null)
                {
                    node = hit.transform.gameObject;
                    OnNodeSelected.Invoke();
                }       
            
            }
            else
            {
                node = null;
                OnNodeDeselected.Invoke();
            }
        }
    }
    
}
