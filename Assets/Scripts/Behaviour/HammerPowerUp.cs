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
        Destroy(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    public override void Activate()
    {
        Instantiate(gameObject);
    }
}
