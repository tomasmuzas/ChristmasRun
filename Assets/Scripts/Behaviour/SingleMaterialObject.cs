using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMaterialObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var material = GetComponent<Renderer>().material;
        ChangeChildrenMaterial(material);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeChildrenMaterial(Material material)
    {
        var children = GetComponentsInChildren<Renderer>();
        foreach (var rend in children)
        {
            rend.materials = new[]{material};
        }
    }
}
