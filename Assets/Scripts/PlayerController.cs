using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
       
    public float moveSpeed;
    public float climbSpeed;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider2D;
    private Vector2 movement = new Vector2();

    private bool isByLadder = false;
    private bool isClimbing = false;

    public ArrayList floorObjects;
    private float originalGravityScale;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalGravityScale = rigidBody.gravityScale;
        floorObjects = new ArrayList(GameObject.FindGameObjectsWithTag("Floor"));

    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        movement.x = moveHorizontal * moveSpeed;

        if (isByLadder && moveVertical != 0.0) {
            EnableClimbing();
        }
        if (isClimbing) {
            movement.y = moveVertical * climbSpeed;
        }
        rigidBody.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Console.WriteLine("Entering trigger with tag " + collision.gameObject.tag);
        if(collision.gameObject.tag == "Ladder")
        {
            isByLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Console.WriteLine("Exiting trigger with tag " + collision.gameObject.tag);
        if(collision.gameObject.tag == "Ladder") {
            DisableClimbing();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && isClimbing == true)
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void EnableClimbing() 
    {
        isClimbing = true;
        rigidBody.gravityScale = 0.0f;
        EnableFloorCollisions(false);
    }

    private void DisableClimbing() 
    {
        isByLadder = false;
        isClimbing = false;
        rigidBody.gravityScale = originalGravityScale;
        EnableFloorCollisions(true);
    }

    private void EnableFloorCollisions(bool enable) {
        foreach (GameObject floor in floorObjects)
        {
            Physics2D.IgnoreCollision(boxCollider2D, floor.GetComponent<BoxCollider2D>(), !enable);
        }
    }
}