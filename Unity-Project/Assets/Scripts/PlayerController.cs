using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;

	private bool jump = false;
	private bool grounded = false;
	private bool moveLeft = false;
	private bool moveRight = false;

	private Rigidbody2D rigBod;

	void Awake()
	{
		rigBod = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		grounded = Physics2D.Linecast(transform.position,
			groundCheck.position,
			1 << LayerMask.NameToLayer("Ground"));

		if(Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			moveLeft = true;
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			moveRight = true;
		}
	}

	void FixedUpdate()
	{
		if(moveLeft && (rigBod.velocity.x > -maxSpeed))
		{
			rigBod.AddForce(Vector2.left * moveForce);
			moveLeft = false;
		}

		if(moveRight && (rigBod.velocity.x < maxSpeed))
		{
			rigBod.AddForce(Vector2.right * moveForce);
			moveRight = false;
		}

		if(jump)
		{
			rigBod.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}
}