using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
//    Direction currentDir;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float t;
    Rigidbody2D rb;

    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;

    public float walkSpeed = 3f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    Coroutine moveCoroutine;
    Vector2 move;

    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);
        animator.SetBool("isMoving", input != Vector2.zero);

        rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);

        if (!isMoving && input != Vector2.zero)
        {
            moveCoroutine = StartCoroutine(Move(transform));
        }
        else if (input == Vector2.zero && moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            isMoving = false;
        }

        if (!isMoving)
        {
            
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                input.y = 0;
            else
                input.x = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);


    /*            if(input.x < 0)
                {
                    currentDir = Direction.West;
                }

                if(input.x > 0)
                {
                    currentDir = Direction.East;
                }

                if(input.y < 0)
                {
                    currentDir = Direction.South;
                }

                if(input.y > 0)
                {
                    currentDir = Direction.North;
                }
    */
    /*           switch(currentDir)
                {
                    case Direction.North:
                        gameObject.GetComponent<SpriteRenderer>().sprite = northSprite;
                        break;
    
                    case Direction.East:
                        gameObject.GetComponent<SpriteRenderer>().sprite = eastSprite;
                        break;
    
                    case Direction.South:
                        gameObject.GetComponent<SpriteRenderer>().sprite = southSprite;
                        break;
    
                    case Direction.West:
                        gameObject.GetComponent<SpriteRenderer>().sprite = westSprite;
                        break;
                }
    */
               StartCoroutine(Move(transform));
            }
        }
    }

    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;

        endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y), startPos.z);

        while (t < 1f)
        {
            t += Time.deltaTime * walkSpeed;
            entity.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
    }

    void FixedUpdate() 
    {
    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    if (input != Vector2.zero) {
        rb.MovePosition(rb.position + input * walkSpeed * Time.fixedDeltaTime);
    }
    }
}

    


enum Direction
{
    North,
    East,
    South,
    West
}
