/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ChessBoard board;

    public GameObject blueKnight;
    public GameObject blueSwordman;
    public GameObject blueArcher;
    public GameObject blueMage;
    public GameObject bluePriest;

    public GameObject redKnight;
    public GameObject redSwordman;
    public GameObject redArcher;
    public GameObject redMage;
    public GameObject redPriest;

    private GameObject[,] pieces;

    private Player blue;
    private Player red;
    public Player player1;
    public Player player2;

    private UIManager uimanager;

    public GameObject fire;

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        pieces = new GameObject[8, 8];
        blue = new Player("blue", true);
        red = new Player("red", false);       
        player1 = blue;
        player2 = red;
        InitialSetup();
        fire.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void InitialSetup()
    {
        //Blue Team
        AddPiece(blueMage, blue, 2, 0);
        AddPiece(bluePriest, blue, 3, 0);
        AddPiece(blueMage, blue, 4, 0);
        AddPiece(blueArcher, blue, 0, 1);
        AddPiece(blueKnight, blue, 1, 1);
        AddPiece(blueSwordman, blue, 2, 1);
        AddPiece(blueKnight, blue, 3, 1);
        AddPiece(blueSwordman, blue, 4, 1);
        AddPiece(blueKnight, blue, 5, 1);
        AddPiece(blueArcher, blue, 6, 1);

        //Red Team
        AddPiece(redArcher, red, 1, 6);
        AddPiece(redKnight, red, 2, 6);
        AddPiece(redSwordman, red, 3, 6);
        AddPiece(redKnight, red, 4, 6);
        AddPiece(redSwordman, red, 5, 6);
        AddPiece(redKnight, red, 6, 6);
        AddPiece(redArcher, red, 7, 6);
        AddPiece(redMage, red, 3, 7);
        AddPiece(redPriest, red, 4, 7);
        AddPiece(redMage, red, 5, 7);
    }

    //For initialising set up: add chess pieces to board
    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, player, col, row);
        player.myPieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPieceForMovement(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
        uimanager.UpdateState("");
    }

    //convert grid position to gameobject position of piece
    public GameObject PieceAtGrid(Vector2Int grid)
    {
        if (grid.x > 7 || grid.y > 7 || grid.x < 0 || grid.y < 0)
        {
            return null;
        }
        return pieces[grid.x, grid.y];
    }

    //convert gameobject position to grid position of piece
    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    //
    public bool isFriendlyPiece(Vector2Int gridPoint)
    {
        //get piece
        GameObject piece = PieceAtGrid(gridPoint);

        //if no piece
        if (piece == null) {
            return false;
        }

        //if piece belongs to opponent
        if (player2.myPieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public bool PieceBelongToCurrentPlayer(GameObject piece)
    {
        return player1.myPieces.Contains(piece);
    }

    //move piece
    public void Move(GameObject piece, Vector2Int grid)
    {

        //get start grid position
        Vector2Int startGrid = GridForPiece(piece);
        //replace start grid position with new grid position
        pieces[startGrid.x, startGrid.y] = null;
        pieces[grid.x, grid.y] = piece;
        StartCoroutine(board.MovePiece(piece, grid));
        //board.MovePiece(piece, grid);
    }

    public void Attack(GameObject pieceObject, Vector2Int grid)
    {
        TacticsPiece piece = pieceObject.GetComponent<TacticsPiece>();
        List<Vector2Int> affectedArea = new List<Vector2Int>();
        Vector2Int startGrid = GridForPiece(pieceObject);
        int direction = GetAttackDirection(startGrid, grid);
        piece.castAttack();

        if (piece.attackType == 0)
        {
            affectedArea.Add(grid);
            
        }
        if (piece.attackType == 1)
        {
            
            if (grid.x == startGrid.x)
            {
                if (startGrid.y < grid.y)
                {
                    for (int i = startGrid.y+1; i < startGrid.y + 7; i++)
                    {
                        affectedArea.Add(new Vector2Int(startGrid.x, i));
                    }
                }
                else
                {
                    for (int i = startGrid.y - 6; i < startGrid.y; i++)
                    {
                        if (i > 0)
                        {
                            affectedArea.Add(new Vector2Int(startGrid.x, i));
                        }
                    }
                }
            }
            else
            {
                if (grid.y == startGrid.y)
                {
                    if (startGrid.x < grid.x)
                    {
                        for (int i = startGrid.x + 1; i < startGrid.x + 7; i++)
                        {
                            affectedArea.Add(new Vector2Int(i, startGrid.y));
                        }
                    }
                    else
                    {
                        for (int i = startGrid.x - 6; i < startGrid.x; i++)
                        {
                            if (i > 0)
                            {
                                affectedArea.Add(new Vector2Int(i, startGrid.y));
                            }
                        }
                    }                    
                }
            }

        }
        if (piece.attackType == 2)
        {
            affectedArea.Add(new Vector2Int(grid.x + 1, grid.y));
            affectedArea.Add(new Vector2Int(grid.x - 1, grid.y));
            affectedArea.Add(new Vector2Int(grid.x, grid.y));
            affectedArea.Add(new Vector2Int(grid.x, grid.y + 1));
            affectedArea.Add(new Vector2Int(grid.x, grid.y - 1));

            //spawnFire(grid);
        }

        if (piece.attackType == 3)
        {
            foreach (GameObject friendlyPiece in player1.myPieces)
            {
                affectedArea.Add(GridForPiece(friendlyPiece));
            }
            
        }

        affectedArea.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);
        foreach (Vector2Int area in affectedArea)
        {
            bool isFriendly = false;
            GameObject enemy = pieces[area.x, area.y];
            foreach (GameObject stuff in player1.myPieces)
            {
                if (enemy == stuff){
                    isFriendly = true;
                    if (piece.attackType == 3)
                    {
                        TacticsPiece target = enemy.GetComponent<TacticsPiece>();
                        target.receiveAttack(piece.attack, piece.surehit, direction, true);
                    }
                }
                
            }
            if (enemy != null && !isFriendly) 
            {
                TacticsPiece target = enemy.GetComponent<TacticsPiece>();
                target.receiveAttack(piece.attack, piece.surehit, direction, false);
                //Debug.Log("enemy health" + target.health);
                ///Debug.Log("enemy location" + GridForPiece(enemy));
                if (target.health <= 0)
                {
                    player2.myPieces.Remove(enemy);
                }
                if (piece.attackType == 2)
                {
                    StartCoroutine(spawnFire(enemy));
                }
            }
        }
        //Debug.Log(player2.myPieces.Count);
        if (player2.myPieces.Count == 0)
        {
            EndGame(false);
        }
    }

    public IEnumerator spawnFire(GameObject target)
    {
        var fireObject = Instantiate(fire, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), Quaternion.identity);
        fireObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //float transInRate = 0.0f;
        float scale = 0.25f;
        float time = 0.0f;
        while (time < 4.5f)
        {
            yield return new WaitForSeconds(0.5f);
            time += 0.5f;
        }
        float transRate = 0.0f;
        while (transRate < 2.4f)
        {
            Debug.Log(transRate);
            fireObject.transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 0, Time.deltaTime * transRate);
            transRate += Time.deltaTime / 1.0f;
        }
        Destroy(fireObject.gameObject);

        //fire.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        //fire.SetActive(false);
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        //get character
        TacticsPiece piece = pieceObject.GetComponent<TacticsPiece>();
        //get location of character
        Vector2Int grid = GridForPiece(pieceObject);
        //get list of legal locations
        List<Vector2Int> locations = piece.MoveLocations(grid);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(isFriendlyPiece);
        locations.RemoveAll(isEnemyPiece);

        return locations;
    }

    public List<Vector2Int> DirectionsForPiece(GameObject pieceObject)
    {
        //get character
        TacticsPiece piece = pieceObject.GetComponent<TacticsPiece>();
        //get location of character
        Vector2Int grid = GridForPiece(pieceObject);
        //get list of legal locations
        List<Vector2Int> locations = piece.FaceDirections(grid);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        return locations;
    }

    public bool isEnemyPiece(Vector2Int gridPoint)
    {
        //get piece
        GameObject piece = PieceAtGrid(gridPoint);


        //if piece belongs to opponent
        if (player2.myPieces.Contains(piece))
        {
            return true;
        }

        return false;
    }

    public List<Vector2Int> AttacksForPiece(GameObject pieceObject)
    {
        //get character
        TacticsPiece piece = pieceObject.GetComponent<TacticsPiece>();
        //get location of character
        Vector2Int grid = GridForPiece(pieceObject);
        List<Vector2Int> locations = new List<Vector2Int>();
        //get list of legal locations
        if (piece.attackType == 3)
        {
           foreach (GameObject friendlyPiece in player1.myPieces)
           {
                locations.Add(GridForPiece(friendlyPiece));
           }
            piece.cooldown += 5;
        }
        
        else
        {
            locations = piece.AttackLocations(grid);
        }
        

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        return locations;
    }

    public void NextPlayer()
    {
        foreach (GameObject piece in player1.myPieces)
        {
            if (piece != null)
            {
                TacticsPiece currentPiece = piece.GetComponent<TacticsPiece>();
                if (currentPiece.cooldown > 0)
                {
                    currentPiece.cooldown -= 1;
                }
            }
            
        }

        uimanager.turn = !uimanager.turn;
        uimanager.UpdatePanel();
        uimanager.DisplayButton();
        Player tempPlayer = player1;
        player1 = player2;
        player2 = tempPlayer;
    }

    public int GetAttackDirection(Vector2Int startGrid, Vector2Int grid)
    {
        if (startGrid.x == grid.x)
        {
            if (startGrid.y > grid.y)
            {
                return 3;
            }
            return 1;
        }

        if (startGrid.x > grid.x)
        {
            return 2;
        }
        return 4;
            
      
    }

    public void Rotate(GameObject piece, Vector2Int grid)
    {
        TacticsPiece currentPiece = piece.GetComponent<TacticsPiece>();
        Debug.Log(currentPiece.direction);
        Vector2Int pieceLocation = GridForPiece(piece);

        int direction;

        if (grid.x == pieceLocation.x)
        {
            if (grid.y > pieceLocation.y)
            {
                direction = 1;
            }
            else
            {
                direction = 3;
            }
            piece.transform.Rotate(0, 90f * (currentPiece.direction - direction), 0);
        }
        else
        {
            if (grid.x > pieceLocation.x)
            {
                direction = 4;
            }
            else
            {
                direction = 2;
            }
            piece.transform.Rotate(0, 90f * (currentPiece.direction - direction), 0);
        }
        currentPiece.direction = direction;
        Debug.Log("current dir" + direction); 

    }

    public void EndGame(bool isSurrender)
    {
        if (isSurrender)
        {
            uimanager.DisplayEndScreen(player2.playerName);
        }
        else
        {
            uimanager.DisplayEndScreen(player1.playerName);
        }
        audioSource.clip = audioClip;
        audioSource.Play();
        Destroy(board.GetComponent<TileSelector>());
        Destroy(board.GetComponent<MoveSelector>());
        Destroy(board.GetComponent<AttackSelector>());
    }
}
