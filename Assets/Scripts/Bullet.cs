using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 2.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
