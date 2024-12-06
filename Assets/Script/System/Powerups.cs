using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public enum buffTier
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }
    private MagazineSystem magSys;
    private PlayerControl player;
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private int magIncreaseAmount;
    [SerializeField] private int hpIncreaseAmount;
    [SerializeField] private float timeToActivate;
    [SerializeField] private float bulletSpeedUpAmount;
    [SerializeField] private float moveSpeedUpAmount;
    [SerializeField] private float reloadTimeReduce;
    bool isIn;

    void Update()
    {
        if(isIn) timeToActivate -= Time.deltaTime;
        if(timeToActivate <= 0) Gacha();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            targetPlayer = coll.gameObject;
            isIn = true;
            magSys = targetPlayer.GetComponent<MagazineSystem>();
            player = targetPlayer.GetComponent<PlayerControl>();
        }
    }

    private void OnTriggerExit(Collider coll) 
    {
        isIn = false;
        timeToActivate = 3f;
    }

    private void Gacha()
    {
        
            int x = Random.Range(0, 3);
            if(x == 0)  MagIncrease();
            if(x == 1)  BulletSpeedIncrease();
            if(x == 2)  MoveSpeedIncrease();
            if(x == 3)  ReloadTimeDecrease();
       
    }    

    //Tier 1
    public void MagIncrease()
    {
        magSys.maxMag += magIncreaseAmount;
    }

    public void BulletSpeedIncrease()
    {
        player.bulletSpeed += bulletSpeedUpAmount;
    }

    public void MoveSpeedIncrease()
    {
        player.speed += moveSpeedUpAmount;
    }

    public void ReloadTimeDecrease()
    {
        magSys.reloadTime -= reloadTimeReduce;
    }

    //Tier 2
    public void BulletSizeUp()
    {

    }

    public void AttackSpeedUp()
    {

    }

    public void BuffCollectingUp()
    {

    }

    //Tier 3
    

    //Tier 4

}
