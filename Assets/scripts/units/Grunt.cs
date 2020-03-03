using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : PlayerPiece
{
    public override bool hasAttack()
    {
        for (int addy = -1; addy < 2; addy +=2)
        {
            for (int addx = -1; addx < 2; addx +=2)
            {
                for (int y = posY + addy, x = posX + addx;
                    !markForAttack(x, y); y += addy, x += addx);
            }
        }
        return gameMaster.targetedUnits.Count > 0;
    }

    public override void calculateMoves()
    {
        gameMaster.grid[posY][posX].markMovableSquare();
        moveEnded(posX + 1, posY);
        moveEnded(posX - 1, posY);
        moveEnded(posX, posY + 1);
        moveEnded(posX, posY - 1);
    }
}
