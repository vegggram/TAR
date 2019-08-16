using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    //default chessboard material
    public Material defaultMaterial;

    //highlighted chessboard material on mouse selection
    public Material movementMaterial;

    public AudioSource audioSource;
    public AudioClip[] Clips;

    //add piece to chessboard
    public GameObject AddPiece(GameObject piece, Player player, int col, int row)
    {
        Vector2Int grid = Geometry.GridValue(col, row);
        //Quartenion identity menas "no rotation/ aligned with world/parent axes"
        if (player.playerName == "red")
        {
            GameObject startPiece = Instantiate(piece, Geometry.PointFromGrid(grid), Quaternion.Euler(0, 180f, 0), gameObject.transform);
            return startPiece;
        }
        else
        {
            GameObject startPiece = Instantiate(piece, Geometry.PointFromGrid(grid), Quaternion.identity, gameObject.transform);
            return startPiece;
        }
    }

    //remove piece from chessboard when piece runs out of HP
    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

    //select piece for movement/attack
    public void SelectPieceForMovement(GameObject piece)
    {
        MeshRenderer rend = piece.GetComponentInChildren<MeshRenderer>();
        rend.material = movementMaterial;
    }

    //deselect piece for movement/attack
    public void DeselectPiece(GameObject piece)
    {
        MeshRenderer rend = piece.GetComponentInChildren<MeshRenderer>();
        rend.material = defaultMaterial;
    }

    //move piece on select
    public IEnumerator MovePiece(GameObject piece, Vector2Int grid)
    {
        //piece.transform.position = Geometry.PointFromGrid(grid);
        TacticsPiece pieceObject = piece.GetComponent<TacticsPiece>();
        piece.GetComponent<Animator>().SetTrigger("move");
        Debug.Log(pieceObject.name);
        if (pieceObject.name == "Knight") audioSource.clip = Clips[1];
        else
        {
            audioSource.clip = Clips[0];
        }
        audioSource.Play();
        float transRate = 0.0f;
        while (transRate < 2.4f)
        {
            piece.transform.position = Vector3.Lerp(piece.transform.position, Geometry.PointFromGrid(grid), Time.deltaTime * transRate);
            transRate += Time.deltaTime / 1.0f;
            yield return null;
        }
        piece.GetComponent<Animator>().SetTrigger("idle");
        audioSource.Stop();
    }
}