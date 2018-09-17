using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float movementSensitivity = 1;
	public float heightMovementInfluenceSensitivity = 1; //one is no influence. 
	public float mouseSensitivity = 1;
	public float mousePanSensitivity = 1;
	public bool invertScrollDirection = true;
	public float scrollSensitivity = 1;
	public float CameraHeightFloor = 16;
	public float CameraHeightCeiling = 128;

	public Vector3 scrollDirectionVector = new Vector3(1, -1, 1).normalized;

	private Vector3 leftRightMovement = new Vector3(0.707f, 0, -0.707f);
	private Vector3 forwardBackwardsMovement = new Vector3(0.707f, 0, 0.707f);

	private InputManager mouseControl;

	// Use this for initialization
	void Start () {
		scrollDirectionVector.Normalize();
		mouseControl = GameObject.Find("InputManager").GetComponent<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		float heightInfluence = 1 + Mathf.Lerp(0, heightMovementInfluenceSensitivity * (CameraHeightCeiling - CameraHeightFloor), (transform.position.y - CameraHeightFloor) / (CameraHeightCeiling - CameraHeightFloor));

		transform.position += (leftRightMovement * horizontal + forwardBackwardsMovement * vertical) * movementSensitivity * heightInfluence * Time.deltaTime;

		float scrollAmmount = Input.GetAxis("Mouse ScrollWheel");

		if (mouseControl.mouseOverUI)
			scrollAmmount = 0;

		float scrollInversionDirection = (invertScrollDirection) ? 1 : -1;
		Vector3 scrollMovement = scrollDirectionVector * scrollInversionDirection * scrollAmmount * scrollSensitivity * transform.position.y;
		if (scrollMovement.y + transform.position.y > CameraHeightFloor && scrollMovement.y + transform.position.y < CameraHeightCeiling)
			transform.position += scrollMovement;

		if (true || Input.GetMouseButton(1))
		{
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = -Input.GetAxis("Mouse Y");

			//transform.Rotate(mouseY * mouseSensitivity, mouseX * mouseSensitivity, 0);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.z = 0;

			transform.rotation = Quaternion.Euler(rot);
		}

		if (Input.GetMouseButton(2))
		{
			float mouseX = -Input.GetAxis("Mouse X");
			float mouseY = -Input.GetAxis("Mouse Y");

			transform.position += (leftRightMovement * mouseX + forwardBackwardsMovement * mouseY) * mousePanSensitivity;
		}
	}
}
