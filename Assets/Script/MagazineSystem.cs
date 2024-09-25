using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSystem : MonoBehaviour
{
    [SerializeField] private int maxMag;
    [SerializeField] private int magSize;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform nozzle;
    bool reloading;
    bool shootable;

    void Start()
    {
        magSize = maxMag;
        shootable = true;
        reloading = false;
    }

    void Update()
    {
        Check();
        Reload();
        Shoot();
    }

    private void Check()
    {
        if(magSize<=0)  shootable = false;
        if(reloading) shootable = false;
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(!reloading)
            {
                reloading = true;
                StartCoroutine(StartReload());
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(shootable)
            {
                Instantiate(bullet, nozzle.position, nozzle.rotation);
                magSize -= 1;
            }
            else
            {
                if(!reloading)
                {
                    reloading = true;
                    StartCoroutine(StartReload());
                }
            }
        }
    }

    IEnumerator StartReload()
    {
        shootable = false;
        yield return new WaitForSeconds(reloadTime);
        magSize = maxMag;
        shootable = true;
        reloading = false;
    }
}
