using UnityEngine;

public static class Tools
{
    public static Vector2 GetDirectionVector(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return Vector2.up;
            case Directions.Right:
                return Vector2.right;
            case Directions.Down:
                return Vector2.down;
            case Directions.Left:
                return Vector2.left;
            case Directions.UpRight:
                return new Vector2(1, 1); // Diagonal up-right
            case Directions.UpLeft:
                return new Vector2(-1, 1); // Diagonal up-left
            case Directions.DownRight:
                return new Vector2(1, -1); // Diagonal down-right
            case Directions.DownLeft:
                return new Vector2(-1, -1); // Diagonal down-left
            // Add cases for any other directions you need
            default:
                return Vector2.zero;
        }
    }
}
public enum Directions
{
    Up,
    Right,
    Down,
    Left,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft,
    InPlace
}