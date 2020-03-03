using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnit : Piece
{
    public override bool hasAttack()
    {
        return false;
    }

    private int gruntDmg(int targx)
    {
        int dmg = 0;
        for(int add = -1; add < 2; add+=2)
        {
            for (int y = posY - 1, x = targx + add; ; y--, x += add)
            {
                if (x >= 0 && x < 8 && y >= 0 && y < 8)
                {
                    Square square = gameMaster.grid[y][x];
                    if(square.piece != null)
                    {
                        var grunt = square.piece as Grunt;
                        if ( grunt != null)
                        {
                            dmg += grunt.attackPower;
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return dmg;
    }

    private int tankDmg(int x)
    {
        int dmg = 0;

        for(int y = posY - 1; y > 0; y--)
        {
            Square square = gameMaster.grid[y][x];
            if (square.piece != null)
            {
                var tank = square.piece as Tank;
                if (tank != null)
                {
                    dmg += tank.attackPower;
                }
                break;
            }
        }

        return dmg;
    }

    private int jumpshipDmg(int x)
    {
        int dmg = 0;

        (int x, int y)[] jmpShipAttkRange = { (x - 1, posY), (x + 1, posY), (x, posY - 1)};
        foreach (var pos in jmpShipAttkRange)
        {
            if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
            {
                var jmpship = gameMaster.grid[pos.y][pos.x].piece as Jumpship;
                if (jmpship != null)
                {
                    dmg += jmpship.attackPower;
                }
            }
        }
        return dmg;
    }

    private int calculateDmgOnPos(int x)
    {
        int dmg = 0;
        dmg += gruntDmg(x);
        dmg += tankDmg(x);
        dmg += jumpshipDmg(x);
        return dmg;
    }

    public override void calculateMoves()
    {
        int mindamage = calculateDmgOnPos(posX);
        int newX = posX;
        for(int x = posX - 1; x <= posX + 1; x+=2)
        {
            if (x >= 0 && x < 8 && gameMaster.grid[posY][x].piece == null)
            {
                int newdmg = calculateDmgOnPos(x);
                if(newdmg < mindamage || Mathf.Abs(4-x) < Mathf.Abs(4 - newX))
                {
                    mindamage = newdmg;
                    newX = x;
                }
            }

        }
        changePos(newX, posY);

    }
}
