using UnityEngine;

public class LaserMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 pos = this.gameObject.transform.position;

        if (pos.x == 10 && pos.y == 0)
        {
            rb.AddForce(new Vector2(-DifController.instance.getVelocity(), 0));
        }else if (pos.x == -10 && pos.y == 0)
        {
            rb.AddForce(new Vector2(DifController.instance.getVelocity(), 0));
        }
        else if (pos.x == 0 && pos.y == 10)
        {
            rb.AddForce(new Vector2(0, -DifController.instance.getVelocity()));
        }
        else
        {
            rb.AddForce(new Vector2(0, DifController.instance.getVelocity()));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FimLaser"))
        {
            Destroy(this.gameObject);
        }
    }
}
