using System.Collections.Generic;
using UnityEngine;

public class DontDestroyTag : MonoBehaviour
{

    public GameObject[] FindGameObjectsWithSameName(string name)
    {
        GameObject[] allObjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> likeNames = new List<GameObject>();
        foreach (GameObject obj in allObjs)
        {
            if (obj.name == name)
            {
                likeNames.Add(obj);
            }
        }
        return likeNames.ToArray();
    }

    private void Awake()
    {
        GameObject[] gamobjects = FindGameObjectsWithSameName(gameObject.name);
        if (gamobjects.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
