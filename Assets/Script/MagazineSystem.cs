using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSystem : MonoBehaviour
{
    [SerializeField] public int maxMag;
    [SerializeField] private int currMag;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform nozzle;
    bool reloading;
    bool shootable;

    void Start()
    {
        currMag = maxMag;
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
        if(currMag<=0)
        {
            shootable = false;
            if(!reloading)
                {
                    reloading = true;
                    StartCoroutine(StartReload());
                }
        }
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
                currMag -= 1;
            }
        }
    }

    IEnumerator StartReload()
    {
        shootable = false;
        yield return new WaitForSeconds(reloadTime);
        currMag = maxMag;
        shootable = true;
        reloading = false;
    }
}
