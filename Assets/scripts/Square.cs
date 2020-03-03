using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private GameMaster gm = null;
    public SpriteRenderer sr;
    private Color markedColor = Color.red;
    private Color hoverColor = Color.green;
    private Color defaultColor = Color.white;

    public Piece piece = null;


    public int X;
    public int Y;
    public int size;
    public bool hasTarget = false;

    public int marked = 1;
    public int getSize()
    {
        return size;
    }

    public void init(int X, int Y, GameMaster gmObj)
    {
        this.X = X;
        this.Y = Y;
        sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = Resources.Load<Sprite>(((X + Y) % 2 == 0)?"steel_square":"sand_square");
        size = Screen.height / 8 - Screen.height / 64;
        transform.localScale = new Vector3((float)size / sr.sprite.rect.width, (float)size / sr.sprite.rect.height, 1);

        gm = gmObj;
        transform.position = new Vector3(X * ((float)size / 100 + 0.05f), Y * ((float)size / 100 + 0.05f), 0);
        defaultColor = Color.white;
        sr.color = defaultColor;
    }

    
    public void clearColor()
    {
        sr.color = defaultColor;
    }

    public void markForShoot()
    {
        hasTarget = true;
        gm.targetedUnits.Add((X, Y));
        sr.color = new Color(0.8f, 0.5f, 0.5f);
    }

    public void markMovableSquare()
    {
        if(piece == null || piece.sameSquare(gm.selectedUnit))
        {
           gm.selectedUnitMoves.Add((X, Y));
           sr.color = new Color(0,1,0);
        }
    }


    private void selectPlayer()
    {
        gm.selectedUnit = piece;
        piece.calculateMoves();
       // gm.playerhasAttack = piece.hasAttack();
    }

    private void doPlayerMoves()
    {
        gm.clearSelectedMoves();
        gm.clearTargetedUnits();
        gm.selectedUnit.changePos(X, Y);
        piece.Moved = true;
        gm.playerhasAttack = piece.hasAttack();
        if (!gm.playerhasAttack)
        {
            gm.indicatePlayerMove();
        }
    }


    private void playerActions()
    {

        if(gm.selectedUnit == null && (piece != null && piece.type == Piece.Type.Player && !piece.Moved))
        {
            selectPlayer();
        }
        else if(gm.playerhasAttack && hasTarget)
        {
            gm.createProjectile(gm.selectedUnit, this);
            gm.indicatePlayerMove();
        }
        else if (gm.selectedUnitMoves.Contains((X, Y)))
        {
            doPlayerMoves();
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            try
            {
                if (gm.isPlayerTurn())
                {
                    playerActions();
                } 
            }
            catch (NullReferenceException)
            {}
        }
    }


    void Awake()
    {
        
    }

}
