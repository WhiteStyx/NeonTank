using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private CharacterController cc;
    private MagazineSystem magSys;
    public PlayerInput playerControls;
    PlayerInput.PlayerActions p_input;
    [SerializeField] private float speed = 5f;
    public float bulletSpeed = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform nozzle;
    [SerializeField] private GameObject tankHead;
    [SerializeField] public int hp;
    
    private Vector3 move;
    private Vector3 direction;
    Vector3 moveDirection = Vector3.zero;

    void Awake()
    {
        playerControls = new PlayerInput();
        p_input = playerControls.Player;
    }

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
        Move(p_input.Move.ReadValue<Vector2>());
        Rotate();
        Shoot();
        Dead(); 
    }

    private void OnEnable()
    {
        p_input.Enable();
    }

    private void OnDisable()
    {
        p_input.Disable();
    }

    private void Move(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        cc.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        Debug.Log(input);
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
