using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform wall;
    [SerializeField] private Collider tank;
    [SerializeField] private float duration = 3;

    private Vector3 direction;
    private Vector3 lastVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
        Destroy(gameObject, duration); //durasi hidup bullet
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContact(0).normal); //mantul
            rb.velocity = direction * bulletSpeed;
        }
    }
}