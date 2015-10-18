using UnityEngine;
using System.Collections;

public class SoldierControl : MonoBehaviour {

	public float speed = 1.0f;
	public float runningSpeed = 3.0f;
	public float gravity = 20.0f;
	public bool alwaysRunning = false;

	private Animator animator;
	private CharacterController controller;

	private string lastDirection = "front";
	private string currentDirection = "front";
	private float lastInputX = 0;
	private float lastInputY = 0;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");

		if (inputX != 0 || inputY != 0) {	
			currentDirection = getDirection (inputX, inputY);

			if (currentDirection != lastDirection) {
				RotateCharacter (currentDirection);
			}

			SetCharacterAnimation (inputX, inputY);
			MoveCharacter (inputX, inputY);
		} else {
			StopCharacterAnimation ();
		}

		lastDirection = currentDirection;
	}

	bool IsRunning () {
		return Input.GetKey (KeyCode.LeftShift) || alwaysRunning;
	}

	void MoveCharacter (float inputX, float inputY) {
		Vector3 moveDirection = Vector3.zero;

		if (controller.isGrounded) {
			moveDirection = new Vector3(0, 0, 1);
			moveDirection = transform.TransformDirection(moveDirection);
			if (IsRunning()) {
				moveDirection *= runningSpeed;
			} else {
				moveDirection *= speed;
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	string getDirection (float inputX, float inputY) {
		string direction = lastDirection;

		if (inputX < 0) {
			direction = "left";
		} else if (inputX > 0) {
			direction = "right";
		}

		if (inputY > 0) {
			direction = "back";
		} else if (inputY < 0) {
			direction = "front";
		}

		return direction;
	}

	void RotateCharacter (string direction) {
		Quaternion rotation = Quaternion.identity;

		switch (direction) {
		case "left":
			rotation.eulerAngles = new Vector3 (0, 0, 0);
			transform.rotation = rotation;
			break;
		case "right":
			rotation.eulerAngles = new Vector3 (0, 180, 0);
			transform.rotation = rotation;
			break;
		case "front":
			rotation.eulerAngles = new Vector3 (0, 270, 0);
			transform.rotation = rotation;
			break;
		case "back":
			rotation.eulerAngles = new Vector3 (0, 90, 0);
			transform.rotation = rotation;
			break;
		}
			
	}
		
	void SetCharacterAnimation (float inputX, float inputY) {
		if (inputX != 0 || inputY != 0) {
			if (IsRunning()) {
				animator.SetBool ("running", true);
				animator.SetBool ("walking", false);
			} else {
				animator.SetBool ("walking", true);
				animator.SetBool ("running", false);
			}
		}
	}

	void StopCharacterAnimation () {
		animator.SetBool ("walking", false);
		animator.SetBool ("running", false);
	}

}
