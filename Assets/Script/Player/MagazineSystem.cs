using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSystem : MonoBehaviour
{
    [SerializeField] public int maxMag;
    [SerializeField] public int currMag;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform nozzle;
    bool reloading;
    public bool shootable;

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

    IEnumerator StartReload()
    {
        shootable = false;
        yield return new WaitForSeconds(reloadTime);
        currMag = maxMag;
        shootable = true;
        reloading = false;
    }
}
