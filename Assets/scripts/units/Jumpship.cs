using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpship : Piece
{
    private void markSquareIfMovable(int x, int y)
    {
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            if(gameMaster.grid[y][x].piece == null)
            {
                gameMaster.grid[y][x].markMovableSquare();
            }
        }
    }

    private void attackAtSquare(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Piece piece = gameMaster.grid[y][x].piece;
            if (piece != null && piece.type == Type.AI)
                gameMaster.createProjectile(this, gameMaster.grid[y][x]);
        }
    }
    public override void calculateMoves()
    {
        gameMaster.grid[posY][posX].markMovableSquare();
        markSquareIfMovable(posX + 1, posY + 2);
        markSquareIfMovable(posX - 1, posY + 2);

        markSquareIfMovable(posX + 2, posY + 1);
        markSquareIfMovable(posX + 2, posY - 1);

        markSquareIfMovable(posX + 1, posY - 2);
        markSquareIfMovable(posX - 1, posY - 2);

        markSquareIfMovable(posX - 2, posY + 1);
        markSquareIfMovable(posX - 2, posY - 1);
    }

    public override bool hasAttack()
    {
        (int, int)[] attckPos = { (posX - 1, posY), (posX + 1, posY), (posX, posY - 1), (posX, posY + 1) };
        foreach (var pos in attckPos)
            attackAtSquare(pos.Item1, pos.Item2);

        return false;
    }

        // Update is called once per frame
   void Update()
   {
        
   }
}
