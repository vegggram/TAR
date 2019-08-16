using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chess piece use point values(x,y,z values) - Vector3 to move/attack
//Chess board contains grid values(col, row values) - Vector2
//Hence, need to convert between point values and grid values

public class Geometry : MonoBehaviour
{
    //converting col and row values to usable grid values
    static public Vector2Int GridValue(int col, int row)
    {
        Vector2Int grid = new Vector2Int(col, row);
        return grid;
    }

    //converting selected grid values to point values
    //chess pieces move in x-z axes direction
    static public Vector3 PointFromGrid(Vector2Int grid)
    {
        float x = -3.5f + 1.0f * grid.x;
        float z = -3.5f + 1.0f * grid.y;
        Vector3 point = new Vector3(x, 0, z);
        return point;
    }

    //converting selected point values to grid values
    static public Vector2Int GridFromPoint(Vector3 point)
    {
        int col = Mathf.FloorToInt(4.0f + point.x);
        int row = Mathf.FloorToInt(4.0f + point.z);
        Vector2Int grid = new Vector2Int(col, row);
        return grid;
    }
}
