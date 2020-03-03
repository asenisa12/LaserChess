using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : PlayerPiece
{
    public override bool hasAttack()
    {
        for (int add = -1; add < 2; add+=2)
        {
            for (int y = posY + add; !markForAttack(posX, y); y += add);
            for (int x = posX + add; !markForAttack(x, posY); x += add) ;
        }
       
        return gameMaster.targetedUnits.Count > 0;
    }

    public override void calculateMoves()
    {
        gameMaster.grid[posY][posX].markMovableSquare();
        for (int addy = -1; addy < 2; addy++)
        {
            for (int addx = -1; addx < 2; addx++)
            {
                if(addx != 0  || addy != 0)
                {
                    for (int y = posY + addy, x = posX + addx; 
                        !moveEnded(x, y); y+=addy, x+=addx);
                }
            }
        }
    }

    void Start()
    {
        startingHP = 5;
        attackPower = 2;
        maxMoves = 3;    
    }
}
