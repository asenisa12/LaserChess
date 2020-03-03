using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private float speed = 0.15f;
    private Square target;
    private GameMaster gm;
    private int damage;
    bool initialized = false;


    public void init(Piece attacker, Square target, GameMaster gameMaster)
    {
        initialized = true;
        this.target = target;
        gm = gameMaster;
        damage = attacker.attackPower;
        transform.position = attacker.transform.position;

        Vector3 vectorToTarget = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
    }
    
    void Update()
    {
        if(initialized)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
            if(transform.position == target.transform.position)
            {
                if(target.piece != null)
                {
                    target.piece.Hitpoints -= damage;
                    if(target.piece.Hitpoints <= 0 )
                    {
                        gm.destroyPiece(target);
                    }
                }

                gm.destroyProjectile(gameObject);
            }
        }
        
    }
}
