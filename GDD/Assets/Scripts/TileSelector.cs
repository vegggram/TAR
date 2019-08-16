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

public class TileSelector : MonoBehaviour
{
    //store transparent overlay for tile and prefab that you are pointing at
    public GameObject highlightPrefab;
    private GameObject highlightTile;
    private UIManager uimanager;

    // Start is called before the first frame update
    void Start()
    {
        //turns initial row and column for highlighted tile into a point
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Vector2Int grid = Geometry.GridValue(0, 0);
        Vector3 point = Geometry.PointFromGrid(grid);
        //create highlighted gameObject from prefab
        //Quartenion identity menas "no rotation/ aligned with world/parent axes"
        highlightTile = Instantiate(highlightPrefab, point, Quaternion.identity, gameObject.transform);
        //deactivated highlighting first
        highlightTile.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //use raycasting to get the selected grid from mouse input
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if raycast/mouse input found, select grid
        //Physics.Raycast checks if ray intersects with any physics colliders on chessboard
        if(Physics.Raycast(raycast, out hit))
        {
            //turn intersection point into grid
            Vector3 point = hit.point;
            Vector2Int grid = Geometry.GridFromPoint(point);

            //highlight selected grid
            highlightTile.SetActive(true);
            highlightTile.transform.position = Geometry.PointFromGrid(grid);

            //check if mouse button is down
            //if mouse button is down, highlight tile
            if (Input.GetMouseButtonDown(0))
            {
                //select the piece at grid to highlight
                GameObject selectedPiece = GameManager.instance.PieceAtGrid(grid);
                TacticsPiece thisPiece = selectedPiece.GetComponent<TacticsPiece>();
                //check if selected piece belongs to player
                Debug.Log("cooldown : " + thisPiece.cooldown);
                if (GameManager.instance.PieceBelongToCurrentPlayer(selectedPiece))
                {
                    if (thisPiece.cooldown == 0)
                    {
                        GameManager.instance.SelectPiece(selectedPiece);
                        //tile to movement
                        uimanager.ShowDescription(thisPiece, true);
                        ExitTileState(selectedPiece);
                    }

                    //highlight selected piece
                    
                    else 
                    {
                        thisPiece.showCoolDown();
                        
                    }
                    uimanager.ShowDescription(thisPiece, true);
                }
                else
                {
                    uimanager.ShowDescription(thisPiece, false);
                }

            }
        }
        //else if raycast/mouse input not found, highlight remains deactivated
        else
        {
            highlightTile.SetActive(false);
        }
    }

    //Select tile for movement
    public void EnterTileState()
    {
        enabled = true;
    }

    //Selecting tile to moving piece
    private void ExitTileState(GameObject movingPiece)
    {
        enabled = false;
        highlightTile.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        Debug.Log("Enter Move State");
        uimanager.UpdateState("Move");
        move.EnterMovementState(movingPiece);
    }

}
