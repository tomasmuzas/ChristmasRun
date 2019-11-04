using UnityEngine;

public class HammerPowerUp : PowerUpSpawn
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            GameManager.Instance.MainCharacter.transform.position.x,
            transform.position.y,
            transform.position.z);
    }

    void OnCollisionEnter(Collision other)
    {
        DestroyGameObject(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        DestroyGameObject(other.gameObject);
    }

    private void DestroyGameObject(GameObject gameObject)
    {
        var smoothDestroy = gameObject.GetComponent<SmoothDestroy>();
        if (smoothDestroy)
        {
            smoothDestroy.StartDestroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public override GameObject Activate()
    {
        return Instantiate(gameObject);
    }
}
