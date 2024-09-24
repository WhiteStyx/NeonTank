using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cube : NetworkBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform nozzle;
    [SerializeField] private GameObject tankHead;
    
    private Vector3 move;
    private Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(20, 20, 20);
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        Move();
        Rotate();
        Shoot();
    }

    private void Move()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        cc.Move(move * speed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 mousePosition = GetMousePosition();
        direction = mousePosition - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);

        tankHead.transform.right = direction * -1;
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, nozzle.position, nozzle.rotation);
        }
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitinfo, Mathf.Infinity))
        {
            return hitinfo.point;
        }
        return Vector3.zero;
    }
}
