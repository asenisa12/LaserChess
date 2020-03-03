using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override bool hasAttack()
    {
        for(int y = posY - 1; y >= 0; y--)
        {
            Piece piece = gameMaster.grid[y][posX].piece;
            if (piece != null && piece.type == Type.Player)
            {
                gameMaster.createProjectile(this, gameMaster.grid[y][posX]);
                return true;
            }
            else if(piece != null)
            {
                return false;
            }
        }

        return false;
    }

    public override void calculateMoves()
    {
        if(posY - 1 >=0 && gameMaster.grid[posY - 1][posX].piece == null)
        {
            changePos(posX, posY - 1);
        }
    }

        // Update is called once per frame
    void Update()
    {
        
    }
}
