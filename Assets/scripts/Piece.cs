using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    protected SpriteRenderer sr;

    public GameMaster gameMaster;

    private GameObject healthBar;
    private float healthbarDefaultSize;
    public int posX;
    public int posY;
    private bool moved;
    public bool Moved
    {
        get { return moved; }
        set
        {
            gameMaster.grid[posY][posX].sr.color = (value) ? new Color(0.8f, 0.8f, 0.8f,1) : Color.white;
            moved = value;
        }
    }

    public int startingHP;
    private int hitpoints;
    public int attackPower;

    public int Hitpoints
    {
        get { return hitpoints; }
        set
        {
            hitpoints = value;
            var sr = healthBar.GetComponent<SpriteRenderer>();
            var size = sr.size;
            sr.size = new Vector2(healthbarDefaultSize * (float)hitpoints / (float)startingHP, size.y);
        }
    }

    public enum Type
    {
        AI,
        Player
    }
    public Type type;

    public bool sameSquare(Piece p)
    {
        if (p == null) return false;

        return p.posX == posX && p.posY == posY;
    }

    public void setup(int x, int y, GameMaster gMaster, bool player)
    {
        posX = x;
        posY = y;

        gameMaster = gMaster;
        hitpoints = startingHP;

        type = (player) ? Type.Player : Type.AI;
        sr = gameObject.GetComponent<SpriteRenderer>();

        healthBar = Instantiate(Resources.Load<GameObject>("HealthBar"));
        healthBar.transform.localScale = gameObject.transform.localScale*3/4f;
        healthBar.transform.position = new Vector3(0.0f, sr.bounds.extents.y * (7.0f / 9.0f), 0.0f) + transform.position;
        healthBar.transform.parent = transform;
        healthbarDefaultSize = healthBar.GetComponent<SpriteRenderer>().size.x;
    }

    public abstract void calculateMoves();

    public abstract bool hasAttack();

    public void changePos(int x, int y)
    {
        gameMaster.grid[posY][posX].clearColor();
        gameMaster.grid[posY][posX].piece = null;
        posX = x;
        posY = y;
        gameObject.transform.position = gameMaster.GetSquare(x, y).transform.position;
        gameMaster.grid[y][x].piece = this;

    }

}
