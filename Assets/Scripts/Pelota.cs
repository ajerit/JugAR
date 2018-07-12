using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota : MonoBehaviour {

	public float velocidadInicial = 30f;
	private Rigidbody rb;
	public bool pelotaEnMov = false;
	[SerializeField]
    private Vector3 initialVelocity = new Vector3(0,0,0);

	private Vector3 lastFrameVelocity;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.velocity = initialVelocity;
	}
	
	// Update is called once per frame
	void Update () {

		lastFrameVelocity = rb.velocity;
	}

	void OnCollisionEnter(Collision coll)
	{
		Bounce(coll.contacts[0].normal);
	}

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
		rb.velocity = direction * speed;
	}
}
