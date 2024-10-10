using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private CharacterController cc;
    private MagazineSystem magSys;
    [SerializeField] private float speed = 5f;
    public float bulletSpeed = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform nozzle;
    [SerializeField] private GameObject tankHead;
    [SerializeField] public int hp;
    
    private Vector3 move;
    private Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        Transform okeh = FindAnyObjectByType<Spawnpoint>().GetComponent<Spawnpoint>().GetPos((int) OwnerClientId);
        transform.position = new Vector3(okeh.position.x, okeh.position.y, okeh.position.z);
        magSys = GetComponent<MagazineSystem>();
    }

    public override void OnNetworkSpawn()
    {

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
        Dead(); 
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
        bool shoot = GetComponent<MagazineSystem>().shootable;
        if (Input.GetMouseButtonDown(0))
        {
            if(shoot)
            {
                GameObject bullet = Instantiate(bulletPrefab, nozzle.position, nozzle.rotation);
                bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
                magSys.currMag -= 1;
            }
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

    private void Dead()
    {
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }
}
