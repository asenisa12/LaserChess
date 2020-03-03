using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dreadnought : Piece
{

    private List<(int x, int y)> getNeighbours(int cur_x, int cur_y, bool hasPiece = false)
    {
        List<(int x, int y)> neighbours = new List<(int x, int y)>();

        for (int y = cur_y - 1; y <= cur_y +1; y += 1)
        {
            if (y >= 0 && y < 8)
            {
                for (int x = cur_x - 1; x <= cur_x + 1; x += 1)
                {
                    if(x==posX && y == posY)
                        continue;

                    if (x >= 0 && x < 8)
                    {
                        Square sq = gameMaster.grid[y][x];
                        if (!(sq.piece != null && sq.piece.type == Type.AI))
                            neighbours.Add((x, y));
                    }
                }
            }
        }
        return neighbours;
    }

    public override bool hasAttack()
    {

        foreach (var neigh in getNeighbours(posX, posY))
        {
            if (squareHasPlayer(neigh.x, neigh.y))
            {
                gameMaster.createProjectile(this, gameMaster.grid[neigh.y][neigh.x]);
            }
        }
        return false;
    }

    private bool squareHasPlayer(int x, int y)
    {
        Piece p = gameMaster.grid[y][x].piece;
        if(p != null && p.type == Type.Player)
            return true;

        return false;
    }
    

    public override void calculateMoves()
    {
        Dictionary<(int x, int y), (int x, int y)> path = new Dictionary<(int x, int y), (int x, int y)>();
        Queue<(int x, int y)> q = new Queue<(int x, int y)>();

        q.Enqueue((posX, posY));
        path[(posX, posY)] = (posX, posY);

        while(q.Count > 0)
        {
            var curPos = q.Dequeue();
            foreach (var neigh in getNeighbours(curPos.x, curPos.y))
            {
                if (squareHasPlayer(neigh.x, neigh.y))
                {
                    var pos = curPos;
                    while(path[pos] != (posX, posY))
                    {
                        //gameMaster.grid[pos.y][pos.x].sr.color = Color.grey;
                        pos = path[pos];
                    }
                    changePos(pos.x, pos.y);
                    return;
                }

                if (!path.ContainsKey(neigh))
                {
                    path[neigh] = curPos;
                    q.Enqueue(neigh);
                }
            }
        }

        //in case the dreadnought is surrounded only by ai's
        (int x, int y) targetPos = (posX, posY);
        foreach (var neigh in getNeighbours(posX, posY))
        {
            if(neigh.y < targetPos.y)
            {
                targetPos = neigh;
            }
        }
        
        changePos(targetPos.x, targetPos.y);

    }
}
