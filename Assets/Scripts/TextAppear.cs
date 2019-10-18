using UnityEngine;

public class TextAppear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void ShowText()
    {
        gameObject.SetActive(true);
    }
}
