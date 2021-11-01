using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;
    private Animator anim;
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;

    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //reset the move delta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //swap sprite direction , going right left
        if(moveDelta.x > 0) {
            // transform.localScale = Vector3.one;
            transform.localScale = originalSize;
        }
        else if(moveDelta.x < 0) {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y , originalSize.z);
        }

        //Add push vector, if any
        moveDelta += pushDirection;

        if (moveDelta.x == 0 & moveDelta.y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else {
            anim.SetBool("isRunning", true);
        }

        //Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0 , new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor","Blocking"));
        if (hit.collider == null) {
            //make this thing move
            transform.Translate(0 , moveDelta.y * Time.deltaTime , 0);
        }
        
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0 , new Vector2(moveDelta.x , 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor","Blocking"));
        if (hit.collider == null) {
            //make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime ,  0 , 0);
        }
    }
}
