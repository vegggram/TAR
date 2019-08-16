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

public class MoveSelector : MonoBehaviour
{

    public GameObject moveLocationPrefab;
    public GameObject highlightPrefab;

    private GameObject highlightTile;
    private GameObject movingPiece;

    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    private UIManager uimanager;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        highlightTile = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile.SetActive(false);
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //use raycasting to get the selected grid from mouse input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if raycast/mouse input found, select grid
        //Physics.Raycast checks if ray intersects with any physics colliders on chessboard
        if (Physics.Raycast(ray, out hit))
        {
            //turn intersection point into grid
            Vector3 point = hit.point;
            Vector2Int grid = Geometry.GridFromPoint(point);

            //highlight selected grid
            highlightTile.SetActive(true);
            highlightTile.transform.position = Geometry.PointFromGrid(grid);

            //when mouse button is clicked, calls GameManager to move the piece to new tile
            if (Input.GetMouseButtonDown(0))
            {

                 //if player clicks on a grid that is not legal
                if (!moveLocations.Contains(grid) && grid != GameManager.instance.GridForPiece(movingPiece))
                {
                    //exit
                    return;
                }

                if (GameManager.instance.PieceAtGrid(grid) == null)
                {
                    GameManager.instance.Move(movingPiece, grid);
                    Debug.Log("Exit Move State");
                    ExitMovementState();
                }
                else
                {
                    if (grid == GameManager.instance.GridForPiece(movingPiece))
                    {
                        GameManager.instance.DeselectPiece(movingPiece);
                        TileSelector selector = GetComponent<TileSelector>();
                        Debug.Log("Enter Tile State");
                        selector.EnterTileState();
                        foreach (GameObject highlight in locationHighlights)
                        {
                            Destroy(highlight);
                        }
                    }
                }
            }
        }
        else
        {
            highlightTile.SetActive(false);
        }

    }

    //stores the piece being moved and enables movement state
    public void EnterMovementState(GameObject piece)
    {
        movingPiece = piece;
        enabled = true;

        //get list of legal locations
        moveLocations = GameManager.instance.MovesForPiece(movingPiece);

        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    //from movement to tile
    public void ExitMovementState()
    {
        enabled = false;
        highlightTile.SetActive(false);

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        AttackSelector attack = GetComponent<AttackSelector>();
        Debug.Log("Enter Attack State");
        uimanager.UpdateState("Attack");
        attack.EnterAttackState(movingPiece);
    }
}
