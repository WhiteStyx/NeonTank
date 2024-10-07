using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private MagazineSystem magSys;
    private PlayerControl player;
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private int magIncreaseAmount;
    [SerializeField] private float speedUpAmount;
    [SerializeField] private int hpIncreaseAmount;

    void Start()
    {
        magSys = targetPlayer.GetComponent<MagazineSystem>();
        player = targetPlayer.GetComponent<PlayerControl>();
    }

    private void MagIncrease()
    {
        magSys.maxMag += magIncreaseAmount;
    }

    private void BulletSpeedIncrease()
    {
        player.bulletSpeed += speedUpAmount;
    }

    private void MaxHPIncrease()
    {
        player.hp += hpIncreaseAmount;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            targetPlayer = coll.gameObject;
        }
    }
}
