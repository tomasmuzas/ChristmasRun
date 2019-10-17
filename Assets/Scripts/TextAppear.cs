using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        EventManager.OnObjectCollision += ShowText;
    }

    void OnDisable()
    {
        EventManager.OnObjectCollision -= ShowText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowText()
    {
        gameObject.SetActive(true);
    }
}
