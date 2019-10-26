using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;
    public float FadeStep = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        StartCoroutine(FadeControl());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FadeControl()
    {
        var fadeAmount = 0f;
        var color = _material.color;

        color.a = fadeAmount;
        _renderer.material.color = color;

        for (float f = 0f; f <= 1.01; f += FadeStep)
        {
            color.a = f;
            _renderer.material.color = color;
            yield return new WaitForSeconds(0.00001f / GameManager.GameSpeed);
        }
    }
}
