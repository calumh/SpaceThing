//Code modified from http://pixelnest.io/tutorials/2d-game-unity/

using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
    public float speed;
    private Vector2 movement;
    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {   
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 4 - Movement per direction
        movement = new Vector2(
          speed * inputX,
          speed * inputY);
        // 5 - Move the game object
        // Move the camera


        // rigidbody2D.velocity = movement;
        /*
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);

        //transform.rotation = rot;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 400 * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rigidbody2D.angularVelocity = 0;
        */


        rigidbody2D.AddForce(movement);
    
        }
}
