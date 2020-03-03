using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    public List<Piece> drones = new List<Piece>();
    public List<Piece> commandUnits = new List<Piece>();

    private int commandUnitsMoved = 0;
    private int dronesMoved = 0;
    GameMaster gm;

    public void Awake()
    {
        drones = new List<Piece>();
        commandUnits = new List<Piece>();
    }

    public void seGMref(GameMaster gm)
    {
        this.gm = gm;
    }

    public void removeDrone(Piece drone)
    {

        List<Piece> units = ((drone as CommandUnit) != null) ? commandUnits : drones;

        if ((drone as CommandUnit) != null) Debug.Log("Command unit"); else Debug.Log("drone");
        for (int i = 0; i <units.Count; i++)
        {
            if(units[i].sameSquare(drone))
            {
                Destroy(units[i].gameObject);
                units.RemoveAt(i);
                break;
            }
        }
    }


    public void doMoves()
    {

        if(commandUnits.Count == 0)
        {
            gm.indicateWinner("Player Wins");
            return;
        }

        if (gm.projectileExists())
        {
            Invoke("doMoves", 0.5f);
            return;
        }

        if(drones.Count >= dronesMoved + 1)
        {
            Piece curDrone =  drones[dronesMoved++]; 
            curDrone.calculateMoves();
            curDrone.hasAttack();

            if(curDrone.posY == 0)
            {
                gm.indicateWinner("AI Wins");
            }
            
        }
        else if(commandUnits.Count >= commandUnitsMoved + 1)
        {
            commandUnits[commandUnitsMoved++].calculateMoves();
        }
        else
        {
            dronesMoved = 0;
            commandUnitsMoved = 0;
            gm.playerTurn = true;
            return;
        }
        Invoke("doMoves", 0.5f);
    }
}
