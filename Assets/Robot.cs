using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot {
    public robotProgramming.RobotColor Color;
    public robotProgramming.Shape Shape;
    public robotProgramming.Type Type;
    public Vector2Int Position;
    public Vector2Int InitialPosition; //used for resetting pressing reset button
    public Vector2Int LastPosition; //used for resetting by strike

    public Robot(robotProgramming.RobotColor color, robotProgramming.Shape shape, robotProgramming.Type type, int xpos, int ypos)
    {
        Color = color;
        Shape = shape;
        Type = type;
        Position = new Vector2Int(xpos, ypos);
        InitialPosition = Position;
        LastPosition = Position;
    }
    public Robot(Vector2Int pos)
    {
        Position = pos;
        InitialPosition = Position;
        LastPosition = Position;
    }
}
