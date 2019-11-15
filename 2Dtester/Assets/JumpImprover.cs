using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpImprover : MonoBehaviour
{
    public float fallMiltiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y < 0) // we are falling
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMiltiplier -1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
