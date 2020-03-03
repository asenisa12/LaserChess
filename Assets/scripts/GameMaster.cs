using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    private bool _playerTurn = true;
    public bool playerTurn
    {
        get { return _playerTurn; }
        set
        {
            if(!haveWinner)
            {

                _playerTurn = value;
                turnLabel.text = (_playerTurn) ? "Player" : "AI";
            }
        }
    }
    public Text turnLabel;
    public Text winnerLabel;
    public bool playerhasAttack = false;

    private bool haveWinner = false;

    private int playerMoves = 0;

    List<Piece> playerUnits = new List<Piece>();
    List<Piece> enemyUnits = new List<Piece>();


    private EnemyAi enemyAi;
    public List<List<Square>> grid;
    public GameObject squarePrefab;
    public GameObject playerProjectilePrefab;


    //player units prefabs
    public GameObject gruntPrefab;
    public GameObject jumpshipPrefab;
    public GameObject tankPrefab;

    //ai units prefabs
    public GameObject aiPrefab;
    public GameObject dronePrefab;
    public GameObject dreadnoughtPrefab;
    public GameObject commandUnitPrefab;


    private List<Projectile> projectiles = new List<Projectile>();
    private int projectilesToDestroy = 0;

    public Piece selectedUnit = null;
    public List<(int,int)> selectedUnitMoves = new List<(int,int)>();

    public List<(int, int)> targetedUnits = new List<(int, int)>();


    public bool isPlayerTurn()
    {
        return playerTurn;
    }

    public void indicateWinner(string winner)
    {
        haveWinner = true;
        playerTurn = false;
        winnerLabel.text = winner;
    }

    private void createBoard()
    {
        grid = new List<List<Square>>();
        for (int y = 0; y < 8; y++)
        {
            grid.Add(new List<Square>());
            for (int x = 0; x < 8; x++)
            {
                GameObject nSquare = Instantiate(squarePrefab);
                grid[y].Add(nSquare.GetComponent<Square>());
                grid[y][x].init(x, y, this);
            }
        }
    }

    public void createProjectile(Piece attacker, Square target)
    {
        projectiles.Add(Instantiate(playerProjectilePrefab).GetComponent<Projectile>());
        projectiles.Last().init(attacker, target, this);
        projectilesToDestroy++;
    }

    public void destroyProjectile(GameObject projectileGO)
    {
        if (projectileGO != null)
        {
            Destroy(projectileGO);
            if (--projectilesToDestroy == 0)
            {
                projectiles.Clear();
            }
        }
    }

    public bool projectileExists()
    {
        return projectiles.Count > 0;
    }

    public void destroyPiece(Square square)
    {
        if (square.piece == null)
            return;

        if(square.piece.type == Piece.Type.AI)
        {
            enemyAi.removeDrone(square.piece);
            if(enemyAi.commandUnits.Count == 0)
            {
                indicateWinner("Player Wins");
            }
        }
        else if (square.piece.type == Piece.Type.Player)
        {
            var itemToRemove = playerUnits.Single(r => (r.sameSquare(square.piece)));
            Destroy(itemToRemove.gameObject);
            playerUnits.Remove(itemToRemove);

            if(playerUnits.Count == 0)
            {
                indicateWinner("Ai wins");
            }
        }
        square.piece = null;
    }


    public void endPlayerTurn()
    {
        if(playerTurn)
        {
            selectedUnit = null;
            clearTargetedUnits();
            clearSelectedMoves();
            playerhasAttack = false;
            startAiTurn();
        }
    }

    private void startAiTurn()
    {
        foreach (var unit in playerUnits)
        {
            unit.Moved = false;
        }
        playerMoves = 0;
        playerTurn = false;
        enemyAi.doMoves();
    }

    public void indicatePlayerMove()
    {
        playerMoves++;
        selectedUnit = null;
        clearTargetedUnits();
        clearSelectedMoves();
        playerhasAttack = false;

        if (playerMoves == playerUnits.Count)
        {
            startAiTurn();
        }
    }

    public void clearSelectedMoves()
    {
        foreach(var pos in selectedUnitMoves)
        {
            grid[pos.Item2][pos.Item1].clearColor();
        }
        
        selectedUnitMoves.Clear();
    }

    public void clearTargetedUnits()
    {
        foreach (var pos in targetedUnits)
        {
            grid[pos.Item2][pos.Item1].clearColor();
            grid[pos.Item2][pos.Item1].hasTarget = false;
        }
        targetedUnits.Clear();
    }

    public Square GetSquare(int x, int y)
    {
        return grid[y][x];
    }

    private void testLevel()
    {
        instantitatePiece(7, 4, gruntPrefab, playerUnits, true);
        instantitatePiece(3, 6, dronePrefab, enemyAi.drones, false);
        instantitatePiece(3, 4, tankPrefab, playerUnits, true);
        //instantitatePiece(3, 6, jumpshipPrefab, playerUnits, true);
        instantitatePiece(3, 7, commandUnitPrefab, enemyAi.commandUnits, false);

    }

    private void createEasyLevel()
    {
        for (int i = 2; i < 6; i++) instantitatePiece(i, 1, gruntPrefab, playerUnits, true);
        instantitatePiece(1, 0, tankPrefab, playerUnits, true);
        instantitatePiece(6, 0, tankPrefab, playerUnits, true);
        instantitatePiece(4, 0, jumpshipPrefab, playerUnits, true);
        instantitatePiece(3, 0, jumpshipPrefab, playerUnits, true);

        for (int i = 2; i < 6; i++) instantitatePiece(i, 5, dronePrefab, enemyAi.drones, false);

        instantitatePiece(4, 7, commandUnitPrefab, enemyAi.commandUnits, false);
        instantitatePiece(1, 5, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(6, 5, dreadnoughtPrefab, enemyAi.drones, false);
    }


    private void createMediumLevel()
    {
        
        instantitatePiece(2, 0, jumpshipPrefab, playerUnits, true);
        instantitatePiece(5, 0, jumpshipPrefab, playerUnits, true);
        instantitatePiece(4, 0, jumpshipPrefab, playerUnits, true);
        instantitatePiece(3, 0, jumpshipPrefab, playerUnits, true);


        for (int i = 0; i < 8; i++) instantitatePiece(i, 1, gruntPrefab, playerUnits, true);
        
        instantitatePiece(0, 0, tankPrefab, playerUnits, true);
        instantitatePiece(7, 0, tankPrefab, playerUnits, true);

        instantitatePiece(1, 0, tankPrefab, playerUnits, true);
        instantitatePiece(6, 0, tankPrefab, playerUnits, true);



        for (int i = 1; i < 7; i++) instantitatePiece(i, 5, dronePrefab, enemyAi.drones, false);
        for (int i = 1; i < 7; i++) instantitatePiece(i, 6, dronePrefab, enemyAi.drones, false);

       
        instantitatePiece(7, 6, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(7, 7, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(0, 6, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(0, 7, dreadnoughtPrefab, enemyAi.drones, false);
        
        instantitatePiece(4, 7, commandUnitPrefab, enemyAi.commandUnits, false);
        instantitatePiece(3, 7, commandUnitPrefab, enemyAi.commandUnits, false);
    }

    private void createHardLevel()
    {
        for (int i = 2; i < 6; i++) instantitatePiece(i, 1, gruntPrefab, playerUnits, true);
        instantitatePiece(1, 0, tankPrefab, playerUnits, true);
        instantitatePiece(6, 0, tankPrefab, playerUnits, true);
        instantitatePiece(4, 0, jumpshipPrefab, playerUnits, true);
        instantitatePiece(3, 0, jumpshipPrefab, playerUnits, true);

        for (int i = 1; i < 7; i++) instantitatePiece(i, 5, dronePrefab, enemyAi.drones, false);
        for (int i = 1; i < 7; i++) instantitatePiece(i, 6, dronePrefab, enemyAi.drones, false);


        instantitatePiece(7, 6, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(7, 7, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(0, 6, dreadnoughtPrefab, enemyAi.drones, false);
        instantitatePiece(0, 7, dreadnoughtPrefab, enemyAi.drones, false);

        instantitatePiece(4, 7, commandUnitPrefab, enemyAi.commandUnits, false);
        instantitatePiece(3, 7, commandUnitPrefab, enemyAi.commandUnits, false);

    }

    private void instantitatePiece(int x, int y, GameObject prefab, List<Piece> unitList, bool player)
    {
        if (grid[y][x].piece == null)
        {
            GameObject unit = Instantiate(prefab);
            unit.transform.position = grid[y][x].transform.position;
            unit.transform.localScale = grid[y][x].gameObject.transform.localScale;

            grid[y][x].piece = unit.GetComponent<Piece>();
            grid[y][x].piece.setup(x, y, this, player);
            unitList.Add(grid[y][x].piece);

        }

    }
    
    void Start()
    {
        selectedUnit = null;
        selectedUnitMoves = new List<(int, int)>();

        enemyAi = Instantiate(aiPrefab).GetComponent<EnemyAi>();
        enemyAi.seGMref(this);

        createBoard();
        var difficultyLevel = PlayerPrefs.GetString("difficultyLevel", "Test");

        if (difficultyLevel == "Test")
        {
            testLevel();
        }
        else if (difficultyLevel == "Easy")
        {
            createEasyLevel();
        }
        else if (difficultyLevel == "Medium")
        {
            createMediumLevel();
        }
        else if (difficultyLevel == "Hard")
        {
            createHardLevel();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
