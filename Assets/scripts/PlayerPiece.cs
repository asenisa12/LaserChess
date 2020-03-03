using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public abstract class PlayerPiece : Piece
{
    protected int maxMoves = 1;

    protected bool moveEnded(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8 &&
            Abs(x - posX) <= maxMoves && Abs(y - posY) <= maxMoves &&
            gameMaster.grid[y][x].piece == null)
        {
            gameMaster.grid[y][x].markMovableSquare();
            return false;
        }

        return true;
    }

    protected bool markForAttack(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Square square = gameMaster.grid[y][x];
            if (square.piece != null)
            {
                if (square.piece.type == Type.AI)
                {
                    square.markForShoot();
                }
                return true;
            }
            return false;
        }
        return true;
    }
}
