using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMouvement : MonoBehaviour
{
    public float verticalFall;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float maxY;

    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        float velX = horizontalSpeed * Input.GetAxis("Horizontal");
        float velY = verticalSpeed * Input.GetAxis("Vertical") - verticalFall;

        if (this.transform.position.y > this.maxY)
        {
            velY = verticalSpeed * Input.GetAxis("Vertical") - verticalFall * 10;
        }
        rb.velocity = new Vector2(velX,velY);
    }
}
