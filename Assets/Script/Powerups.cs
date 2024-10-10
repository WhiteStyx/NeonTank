using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private MagazineSystem magSys;
    private Player player;
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private int magIncreaseAmount;
    [SerializeField] private float speedUpAmount;
    [SerializeField] private int hpIncreaseAmount;
    [SerializeField] private float timeToActivate;
    bool isIn;

    void Start()
    {

    }

    void Update()
    {
        if(isIn)    timeToActivate -= Time.deltaTime;
        if(timeToActivate < 0)
        {
            isIn = false;
            timeToActivate = 3f;
            Gacha();
        }
    }

    private void MagIncrease()
    {
        magSys.maxMag += magIncreaseAmount;
        Debug.Log("0");
    }

    private void BulletSpeedIncrease()
    {
        player.bulletSpeed += speedUpAmount;
        Debug.Log("1");
    }

    private void MaxHPIncrease()
    {
        player.hp += hpIncreaseAmount;
        Debug.Log("2");
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            targetPlayer = coll.gameObject;
            isIn = true;
            magSys = targetPlayer.GetComponent<MagazineSystem>();
            player = targetPlayer.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider coll) 
    {
        isIn = false;
        timeToActivate = 3f;
    }

    private void Gacha()
    {
        int x = Random.Range(0,2);
        switch(x)
        {
            case 0:
                MagIncrease();
                break;
            case 1:
                BulletSpeedIncrease();
                break;
            case 2:
                MaxHPIncrease();
                break;
        }
    }    
}
