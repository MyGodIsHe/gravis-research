using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNode : MonoBehaviour
{
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
                    print( " object is clicked by mouse");
                    print(hit.transform.name);
                }
            
            }
            else //if(ray.transform == null)
            {
                node = null;
            }
        }
    }
    
}
