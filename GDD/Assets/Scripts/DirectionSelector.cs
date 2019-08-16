/////*
//// * Copyright (c) 2018 Razeware LLC
//// * 
//// * Permission is hereby granted, free of charge, to any person obtaining a copy
//// * of this software and associated documentation files (the "Software"), to deal
//// * in the Software without restriction, including without limitation the rights
//// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// * copies of the Software, and to permit persons to whom the Software is
//// * furnished to do so, subject to the following conditions:
//// * 
//// * The above copyright notice and this permission notice shall be included in
//// * all copies or substantial portions of the Software.
//// *
//// * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
//// * distribute, sublicense, create a derivative work, and/or sell copies of the 
//// * Software in any work that is designed, intended, or marketed for pedagogical or 
//// * instructional purposes related to programming, coding, application development, 
//// * or information technology.  Permission for such use, copying, modification,
//// * merger, publication, distribution, sublicensing, creation of derivative works, 
//// * or sale is expressly withheld.
//// *    
//// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//// * THE SOFTWARE.
//// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class DirectionSelector : MonoBehaviour
//{

//    public Button frontButton, leftButton, backButton, rightButton;

//    private GameObject facingPiece;

//    private int newDirection;
//    private List<Button> buttons;

//    // Start is called before the first frame update
//    void Start()
//    {
//        enabled = false;
//        Vector2Int pieceLocation = GameManager.instance.GridForPiece(facingPiece);

//        Button front = Instantiate(frontButton, facingPiece.transform.position, Quaternion.Euler(0f, 0, 0));
//        Button back = Instantiate(backButton, facingPiece.transform.position, Quaternion.Euler(90f, 0, 0));
//        Button left = Instantiate(leftButton, facingPiece.transform.position, Quaternion.Euler(180f, 0, 0));
//        Button right = Instantiate(rightButton, facingPiece.transform.position, Quaternion.Euler(270f, 0, 0));
//        buttons.Add(front);
//        buttons.Add(left);
//        buttons.Add(back);
//        buttons.Add(right);
//    }

//    // Update is called once per frame
//    void Update()
//    {


//        if (EventSystem.current.currentSelectedGameObject.name == "frontButton")
//        {
//            newDirection = 1;
//        }

//        if (EventSystem.current.currentSelectedGameObject.name == "leftButton")
//        {
//            newDirection = 2;
//        }

//        if (EventSystem.current.currentSelectedGameObject.name == "backButton")
//        {
//            newDirection = 3;
//        }

//        if (EventSystem.current.currentSelectedGameObject.name == "rightButton")
//        {
//            newDirection = 4;
//        }

//        GameManager.instance.Rotate(facingPiece, newDirection);
//        Debug.Log("Exit Direction State");
//        ExitDirectionState();
//    }

//    //stores the piece being moved and enables movement state
//    public void EnterDirectionState(GameObject piece)
//    {
//        facingPiece = piece;
//        enabled = true;
//    }

//    //from movement to tile
//    private void ExitDirectionState()
//    {
//        enabled = false;
//        GameManager.instance.DeselectPiece(facingPiece);
//        facingPiece = null;

//        GameManager.instance.NextPlayer();

//        TileSelector selector = GetComponent<TileSelector>();
//        Debug.Log("Enter Tile State");
//        selector.EnterTileState();

//        foreach (Button b in buttons)
//        {
//            Destroy(b);
//        }
//    }
//}