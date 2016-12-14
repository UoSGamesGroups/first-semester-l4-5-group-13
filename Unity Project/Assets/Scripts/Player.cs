using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    public static int artifactCount = 0;
    public static int difficulty = 0;

    public Image heartImage;
    public Sprite[] heartSprites;

    public Image artifactImage;
    public Sprite[] artifactImages;

    public GameObject fadeInObject;
    SpriteRenderer spRend;

    public Text scoreCount;

    [Range(0, 3)]
    public int playerHealth;
    public bool invincible;
    SpriteRenderer tempRed;

    public bool hasArtifact;

    private bool facingRight;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.3f;
    float accelerationTimeAirborne = 0.1f;
    float accelerationTimeGrounded = 0.075f;
    public float moveSpeed = 5;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 2;
    public float wallStickTime = 0.25f;
    float timeToWallUnstick;

    float minJumpVelocity;
    float maxJumpVelocity;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Animator animator;

    void Start()
    {
        invincible = false;
        tempRed = gameObject.GetComponent<SpriteRenderer>();
        scoreCount.text = artifactCount.ToString();
        spRend = fadeInObject.GetComponent<SpriteRenderer>();
        spRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        StartCoroutine(FadeTo(0.0f, 2.5f));

        playerHealth = 3;
        hasArtifact = false;

        facingRight = false;
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        heartImage.sprite = heartSprites[playerHealth];

        if(hasArtifact)
        {
            artifactImage.sprite = artifactImages[1];
            if(!facingRight)
            {
                animator.Play("TopHatIdleBLeft");
            }
            else
            {
                animator.Play("TopHatIdleBRight");
            }

            StartCoroutine(FadeTo(1.0f, 2.5f));
            StartCoroutine(Wait(2.5f, 1));
        }
        else if(playerHealth < 1)
        {
            StartCoroutine(FadeTo(1.0f, 2.5f));
            StartCoroutine(Wait(2.5f, 0));
        }
        else
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            bool wallSliding = false;
            if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;

                if(velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if(timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if(input.x != wallDirX && input.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(wallSliding)
                {
                    if(wallDirX == input.x)
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    else if(input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else
                    {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }
                if(controller.collisions.below)
                {
                    velocity.y = maxJumpVelocity;
                }
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                if(velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);

            if(controller.collisions.above || controller.collisions.below)
            {
                if(controller.collisions.slidingDownMaxSlope)
                {
                    velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                }
                else
                {
                    velocity.y = 0;
                }
            }

            if(controller.collisions.below && (velocity.y == 0))
            {
                if(input.x < 0)
                {
                    facingRight = false;
                    animator.Play("TopHatWalkLeft");
                }
                if(input.x > 0)
                {
                    facingRight = true;
                    animator.Play("TopHatWalkRight");
                }
                if(input.x == 0)
                {
                    if(!facingRight)
                    {
                        animator.Play("TopHatIdleLeft");
                    }
                    else
                    {
                        animator.Play("TopHatIdleRight");
                    }
                }
            }
            if(controller.collisions.left && !controller.collisions.below && velocity.y < 0)
            {
                animator.Play("TopHatWallSlideLeft");
            }
            if(controller.collisions.right && !controller.collisions.below && velocity.y < 0)
            {
                animator.Play("TopHatWallSlideRight");
            }
            if(!controller.collisions.below && (!controller.collisions.left && !controller.collisions.right))
            {
                if(velocity.y > 0)
                {
                    if(input.x < 0)
                    {
                        facingRight = false;
                        animator.Play("TopHatJumpLeft");
                    }
                    if(input.x > 0)
                    {
                        facingRight = true;
                        animator.Play("TopHatJumpRight");
                    }
                    if(input.x == 0)
                    {
                        if(!facingRight)
                        {
                            animator.Play("TopHatJumpLeft");
                        }
                        else
                        {
                            animator.Play("TopHatJumpRight");
                        }
                    }
                }
                if(velocity.y < 0)
                {
                    if(input.x < 0)
                    {
                        facingRight = false;
                        animator.Play("TopHatDescentLeft");
                    }
                    if(input.x > 0)
                    {
                        facingRight = true;
                        animator.Play("TopHatDescentRight");

                    }
                    if(input.x == 0)
                    {
                        if(!facingRight)
                        {
                            animator.Play("TopHatDescentLeft");
                        }
                        else
                        {
                            animator.Play("TopHatDescentRight");
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Artifact"))
        {
            collision.gameObject.SetActive(false);
            hasArtifact = true;
            artifactCount++;
            difficulty++;
        }
        else if(collision.gameObject.CompareTag("Harmful"))
        {
            playerHealth--;
            var tempRed = gameObject.GetComponent<SpriteRenderer>();
            tempRed.color = new Color(1.0f, 0.5f, 0.5f, 1f);
            Invoke("resetInvulnerability", 2);
            Invoke("resetColor", 2);
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = spRend.color.a;
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            spRend.color = newColor;
            yield return null;
        }
    }

    IEnumerator Wait(float time, int scene)
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(scene);
    }

    void resetInvulnerability()
    {
        invincible = false;
    }

    void resetColor()
    {
        tempRed.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}