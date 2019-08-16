///*
// * Copyright (c) 2018 Razeware LLC
// * 
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// * 
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// *
// * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
// * distribute, sublicense, create a derivative work, and/or sell copies of the 
// * Software in any work that is designed, intended, or marketed for pedagogical or 
// * instructional purposes related to programming, coding, application development, 
// * or information technology.  Permission for such use, copying, modification,
// * merger, publication, distribution, sublicensing, creation of derivative works, 
// * or sale is expressly withheld.
// *    
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// * THE SOFTWARE.
// */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector : MonoBehaviour
{

    public GameObject attackLocationPrefab;
    public GameObject highlightPrefab;

    private GameObject highlightTile;
    private GameObject attackingPiece;

    private List<Vector2Int> attackLocations;
    private List<GameObject> locationHighlights;

    private UIManager uimanager;

    public AudioSource audioSource;
    public AudioClip[] Clips;

    public GameObject highlightTile1;
    public GameObject highlightTile2;
    public GameObject highlightTile3;
    public GameObject highlightTile4;

    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        enabled = false;
        highlightTile = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile.SetActive(false);
        highlightTile1 = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile1.SetActive(false);
        highlightTile2 = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile2.SetActive(false);
        highlightTile3 = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile3.SetActive(false);
        highlightTile4 = Instantiate(highlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        highlightTile4.SetActive(false);
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
            highlightTile1.SetActive(false);
            highlightTile2.SetActive(false);
            highlightTile3.SetActive(false);
            highlightTile4.SetActive(false);

            //highlight selected grid
            highlightTile.SetActive(true);
            TacticsPiece thisPiece = attackingPiece.GetComponent<TacticsPiece>();
            if (thisPiece.name == "Mage")
            {
                List<Vector2Int> affectedArea = new List<Vector2Int>();
                affectedArea.Add(new Vector2Int(grid.x + 1, grid.y));
                affectedArea.Add(new Vector2Int(grid.x - 1, grid.y));
                affectedArea.Add(new Vector2Int(grid.x, grid.y + 1));
                affectedArea.Add(new Vector2Int(grid.x, grid.y - 1));
                affectedArea.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);
                if (affectedArea.Count == 2)
                {
                    highlightTile1.transform.position = Geometry.PointFromGrid(affectedArea[0]);
                    highlightTile2.transform.position = Geometry.PointFromGrid(affectedArea[1]);
                    highlightTile1.SetActive(true);
                    highlightTile2.SetActive(true);
                    highlightTile3.SetActive(false);
                    highlightTile4.SetActive(false);

                }
                else
                {
                    if (affectedArea.Count == 3)
                    {
                        highlightTile1.transform.position = Geometry.PointFromGrid(affectedArea[0]);
                        highlightTile2.transform.position = Geometry.PointFromGrid(affectedArea[1]);
                        highlightTile3.transform.position = Geometry.PointFromGrid(affectedArea[2]);
                        highlightTile1.SetActive(true);
                        highlightTile2.SetActive(true);
                        highlightTile3.SetActive(true);
                        highlightTile4.SetActive(false);
                    }
                    if (affectedArea.Count == 4)
                    {
                        highlightTile1.transform.position = Geometry.PointFromGrid(new Vector2Int(grid.x + 1, grid.y));
                        highlightTile2.transform.position = Geometry.PointFromGrid(new Vector2Int(grid.x - 1, grid.y));
                        highlightTile3.transform.position = Geometry.PointFromGrid(new Vector2Int(grid.x, grid.y + 1));
                        highlightTile4.transform.position = Geometry.PointFromGrid(new Vector2Int(grid.x, grid.y - 1));
                        highlightTile1.SetActive(true);
                        highlightTile2.SetActive(true);
                        highlightTile3.SetActive(true);
                        highlightTile4.SetActive(true);
                    }
                }



            }
            highlightTile.transform.position = Geometry.PointFromGrid(grid);

            //when mouse button is clicked, calls GameManager to move the piece to new tile
            if (Input.GetMouseButtonDown(0))
            {
                //if player clicks on a grid that is not legal
                if (!attackLocations.Contains(grid))
                {
                    //exit
                    return;
                }

                GameManager.instance.Attack(attackingPiece, grid);
                GameManager.instance.Rotate(attackingPiece, grid);
                attackingPiece.GetComponent<Animator>().SetTrigger("attack");
                TacticsPiece pieceObject = attackingPiece.GetComponent<TacticsPiece>();
                switch (pieceObject.name)
                {
                    case "Archer":
                        audioSource.clip = Clips[0];
                        audioSource.Play();
                        break;
                    case "Knight":
                    case "Swordman":
                        audioSource.clip = Clips[1];
                        audioSource.Play();
                        break;
                    case "Priest":
                        audioSource.clip = Clips[2];
                        audioSource.Play();
                        break;
                }

                Debug.Log("Exit Attack State");
                highlightTile.SetActive(false);
                if (thisPiece.name == "Mage")
                {
                    highlightTile1.SetActive(false);
                    highlightTile2.SetActive(false);
                    highlightTile3.SetActive(false);
                    highlightTile4.SetActive(false);
                }
                ExitAttackState();
            }
        }
        else
        {
            highlightTile.SetActive(false);
        }
    }

    IEnumerator Delay() 
    {
        yield return new WaitForSeconds(60);
    }

    //stores the piece being moved and enables movement state
    public void EnterAttackState(GameObject piece)
    {
        attackingPiece = piece;
        enabled = true;

        //get list of legal locations
        attackLocations = GameManager.instance.AttacksForPiece(attackingPiece);

        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in attackLocations)
        {
            GameObject highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    //from movement to tile
    public void ExitAttackState()
    {
        enabled = false;
        highlightTile.SetActive(false);

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        uimanager.UpdateState("Face");
        NewDirectionSelector direction = GetComponent<NewDirectionSelector>();
        Debug.Log("Enter Direction State");
        direction.EnterDirectionState(attackingPiece);
    }
}