using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;

using Random = UnityEngine.Random;

#pragma warning disable IDE0051 // Remove unused private members

public class robotProgramming : MonoBehaviour
{
    public KMBombModule module;
    public KMBombInfo bomb;
    public KMAudio bombAudio;
    static int ModuleIdCounter = 1;
    int ModuleId;
    bool moduleSolved;
    public KMColorblindMode colorblind;
    bool colorblindActive;
    public TextMesh[] colorblindRobotTexts;
    public TextMesh colorblindLEDText;
    public GameObject colorblindOtherText;

    //Buttons
    public KMSelectable startButton;
    public KMSelectable resetButton;
    public KMSelectable[] arrowButtons;
    public KMSelectable[] blockButtons;

    //Maze
    private readonly string[] maze = new string[9]; //to make my life easier, the coordinates for reading off the maze are in reverse order (y, x)
    private static readonly string[] mazeTops = new string[16]
    {
        "XrXyXgXbX|X...X...X|XXX.X.X.X|X...X.X.X|X.XXXXX.X|X.......X",
        "XrXyXgXbX|X.......X|X.XX.XX.X|X.X...X.X|XXX.X.XXX|X...X...X",
        "XrXyXgXbX|X.......X|XXX.XXX.X|X.X...X.X|X.XXX.XXX|X.......X",
        "XrXyXgXbX|X.......X|X.X.XXX.X|X.X.....X|XXX.X.XXX|X...X...X",
        "XrXyXgXbX|X...X...X|X.XXX.XXX|X.X.....X|X.X.XXX.X|X.....X.X",
        "XrXyXgXbX|X.....X.X|X.XXX.X.X|X.X...X.X|XXX.XXX.X|X.......X",
        "XrXyXgXbX|X.X.X.X.X|X.X.X.X.X|X.......X|X.XXXXX.X|X.....X.X",
        "XrXyXgXbX|X.X...X.X|X.X.XXX.X|X.......X|X.X.X.X.X|X.X.X.X.X",
        "XrXyXgXbX|X.X...X.X|X.X.X.X.X|X...X...X|X.XXXXX.X|X.......X",
        "XrXyXgXbX|X.X...X.X|X.XXX.X.X|X.......X|XXX.XXXXX|X.......X",
        "XrXyXgXbX|X...X...X|X.X.X.X.X|X.X...X.X|X.X.XXX.X|X...X...X",
        "XrXyXgXbX|X.X...X.X|X.X.X.X.X|X...X...X|X.XXXXX.X|X...X...X",
        "XrXyXgXbX|X...X.X.X|X.XXX.X.X|X.......X|XXX.X.XXX|X...X...X",
        "XrXyXgXbX|X.......X|XX.XXX.XX|X...X...X|X.X.X.X.X|X.X...X.X",
        "XrXyXgXbX|X.X.....X|X.X.XXX.X|X...X...X|X.XXXXX.X|X.X.....X",
        "XrXyXgXbX|X.....X.X|X.XXX.X.X|X.X...X.X|X.X.XXX.X|X.X.....X"
    };
    private static readonly string[] mazeBottoms = new string[16]
    {
        "X.X.XXX.X|X.X.X...X|XXXXXXXXX",
        "X.XXXXX.X|X.......X|XXXXXXXXX",
        "X.X.XXX.X|X.X.....X|XXXXXXXXX",
        "X.X.XXXXX|X.......X|XXXXXXXXX",
        "X.X.XXXXX|X.......X|XXXXXXXXX",
        "X.X.X.XXX|X.......X|XXXXXXXXX",
        "X.X.X.XXX|X.......X|XXXXXXXXX",
        "X.X.X.X.X|X...X...X|XXXXXXXXX",
        "X.X.XXX.X|X.X.....X|XXXXXXXXX",
        "XXX.XXX.X|X.......X|XXXXXXXXX",
        "X.XXX.XXX|X.X.....X|XXXXXXXXX",
        "X.X.X.X.X|X...X...X|XXXXXXXXX",
        "X.XXX.X.X|X.......X|XXXXXXXXX",
        "X.XXX.XXX|X.......X|XXXXXXXXX",
        "X.X.XXX.X|X.X...x.X|XXXXXXXXX",
        "X.X.X.X.X|X...X...X|XXXXXXXXX"
    };
    public Sprite[] topHalfSprites;
    public Sprite[] bottomHalfSprites;
    public SpriteRenderer topHalfRenderer;
    public SpriteRenderer bottomHalfRenderer;
    int topIndex;
    int bottomIndex;
    private static readonly Vector2Int[] goalPositions = new Vector2Int[4] { new Vector2Int(7, 0), new Vector2Int(5, 0), new Vector2Int(1, 0), new Vector2Int(3, 0) }; //sorted by color (blue, green, red, yellow)

    //Robot Characteristics
    public enum RobotColor
    {
        Blue, Green, Red, Yellow
    }
    public enum Shape
    {
        Triangle, Square, Hexagon, Circle
    }
    public enum Type
    {
        ROB, HAL, R2D2, Fender
    }
    private readonly RobotColor[] robotColors = new RobotColor[4] { RobotColor.Blue, RobotColor.Green, RobotColor.Red, RobotColor.Yellow };
    private readonly Shape[] robotShapes = new Shape[4] { Shape.Triangle, Shape.Square, Shape.Hexagon, Shape.Circle };
    private readonly Robot[] robots = new Robot[4];
    private readonly Robot[] sortedRobots = new Robot[4];
    private Type[] robotTypes;

    //Robot Visuals
    public GameObject[] robotObjects;
    readonly GameObject[] sortedRobotObjects = new GameObject[4]; //makes moving the visuals WAY easier if sorted by color
    public Material[] robotMaterials;
    public Mesh[] robotMeshes;

    //Moving/Handling Input
    public enum Direction
    {
        Up, Right, Down, Left
    }
    readonly List<string> inputNames = new List<string>();
    //Striking
    bool willStrike;
    bool lastR2D2Behavior;
    int lastSerialCharacterIndex;
    //R2D2
    bool R2D2actsLikeHAL;
    bool initialR2D2Behavior; //Reset
    //Fender
    int serialCharacterIndex;
    int initialCharacterIndex; //Reset
    readonly string[] placeNames = new string[6] { "1st", "2nd", "3rd", "4th", "5th", "6th" }; //used for logging
    //Animation
    readonly Queue<AnimationRequest> animationQueue = new Queue<AnimationRequest>();
    bool currentlyAnimating;

    //Showing inputs
    //LED
    List<RobotColor> notBlockedColors = new List<RobotColor> { RobotColor.Blue, RobotColor.Green, RobotColor.Red, RobotColor.Yellow };
    int currentColorIndex;
    int initialColorIndex;
    public Renderer LedRenderer;
    public Material[] unlitColorMaterials;
    //Small Display
    public TextMesh displayText;
    public Sprite[] shapeSprites;
    private static readonly Color[] displayColors = new Color[4] { new Color(58 / 255f, 88 / 255f, 1), new Color(0, 1, 0), new Color(1, 0, 0), new Color(1, 1, 0) };
    public SpriteRenderer[] displayShapeRenderers;
    public GameObject displayShapesObject;

    void Awake()
    {
        ModuleId = ModuleIdCounter++;

        for (int i = 0; i < 4; i++)
        {
            int dummy = i; //i need this because "i" is changing constantly and i think that makes it not work
            arrowButtons[dummy].OnInteract += delegate () { arrowButtonPressed((Direction) dummy); return false; };
            blockButtons[dummy].OnInteract += delegate () { blockButtonPressed((RobotColor) dummy); return false; };
        }
        startButton.OnInteract += delegate () { handleStart(); return false; };
        resetButton.OnInteract += delegate () { willStrike = false; handleReset(); return false; };

        colorblindActive = colorblind.ColorblindModeActive;
    }

    void Start()
    {
        topIndex = Random.Range(0, 16);
        bottomIndex = Random.Range(0, 16);
        string[] topHalf = mazeTops[topIndex].Split('|');
        string[] bottomHalf = mazeBottoms[bottomIndex].Split('|');
        string logMaze = "";

        topHalfRenderer.sprite = topHalfSprites[topIndex];
        bottomHalfRenderer.sprite = bottomHalfSprites[bottomIndex];
        displayText.text = "[AWAITING INPUT]\n" + (topIndex + 1) + " " + (bottomIndex + 1);

        for (int i = 0; i < 6; i++) //assign top half of the maze
        {
            maze[i] = topHalf[i];
            logMaze += topHalf[i] + "\n";
        }
        for (int i = 6; i < 9; i++) //assign bottom half
        {
            maze[i] = bottomHalf[i - 6];
            logMaze += bottomHalf[i - 6] + (i == 8 ? "" : "\n");
        }
        LogMsg("The maze numbers are " + (topIndex + 1) + " and " + (bottomIndex + 1) + ".");
        LogMsg("The resulting maze:\n" + logMaze);

        robotColors.Shuffle();
        robotShapes.Shuffle();

        int caseNumber = 0; //this number increments by 4 if the first rule in the chart applies, by 2 if the second rule applies, and by 1 if the 3rd rule applies

        for (int i = 0; i < 4; i++)
        {
            robotObjects[i].GetComponent<Renderer>().material = robotMaterials[(int) robotColors[i]];
            robotObjects[i].GetComponent<MeshFilter>().mesh = robotMeshes[(int) robotShapes[i]];

            displayShapeRenderers[i].color = displayColors[(int) robotColors[i]];
            displayShapeRenderers[i].sprite = shapeSprites[(int) robotShapes[i]];

            if (robotColors[i] == RobotColor.Yellow && robotShapes[i] == Shape.Hexagon) //second rule
                caseNumber += 2;
        }

        if (robotColors[0] == RobotColor.Red || robotColors[0] == RobotColor.Green) //first rule
            caseNumber += 4;
        if (robotShapes[2] == Shape.Triangle) //third rule
            caseNumber += 1;
        switch (caseNumber)
        {
            default:
            case 0:
                robotTypes = new Type[4] { Type.ROB, Type.HAL, Type.R2D2, Type.Fender };
                break;
            case 1:
                robotTypes = new Type[4] { Type.HAL, Type.Fender, Type.ROB, Type.R2D2 };
                break;
            case 2:
                robotTypes = new Type[4] { Type.HAL, Type.ROB, Type.Fender, Type.R2D2 };
                break;
            case 3:
                robotTypes = new Type[4] { Type.R2D2, Type.Fender, Type.HAL, Type.ROB };
                break;
            case 4:
                robotTypes = new Type[4] { Type.Fender, Type.ROB, Type.HAL, Type.R2D2 };
                break;
            case 5:
                robotTypes = new Type[4] { Type.HAL, Type.R2D2, Type.Fender, Type.ROB };
                break;
            case 6:
                robotTypes = new Type[4] { Type.Fender, Type.HAL, Type.R2D2, Type.ROB };
                break;
            case 7:
                robotTypes = new Type[4] { Type.R2D2, Type.ROB, Type.Fender, Type.HAL };
                break;
        }

        //Sorting the robots based on color to make moving them easier
        int[] initialXPositions = new int[4] { 1, 3, 5, 7 };
        for (int i = 0; i < 4; i++)
        {
            robots[i] = new Robot(robotColors[i], robotShapes[i], robotTypes[i], initialXPositions[i], 7);
            sortedRobots[(int) robotColors[i]] = robots[i];
            sortedRobotObjects[(int) robotColors[i]] = robotObjects[i];
        }

        LogMsg("The robots in the initial order are: " + LogRobots(robots[0]) + ", " + LogRobots(robots[1]) + ", " + LogRobots(robots[2]) + ", " + LogRobots(robots[3]) + ".");
        LogMsg("The robot types are: " + robotTypes[0].ToString() + ", " + robotTypes[1].ToString() + ", " + robotTypes[2].ToString() + ", " + robotTypes[3].ToString() + ".");

        if (colorblindActive)
            setupColorblind();

        LogMsgSilent(maze.Join("\n"));
    }

    //button presses
    void arrowButtonPressed(Direction direction)
    {
        if (notBlockedColors.Count == 0 || currentlyAnimating || moduleSolved)
            return;

        inputNames.Add(direction.ToString());

        int index = (int) notBlockedColors[currentColorIndex];
        string colorName = notBlockedColors[currentColorIndex].ToString().ToLower();
        string directionName = direction.ToString().ToLower();
        Robot currentRobot = sortedRobots[index];
        Debug.LogFormat("[Robot Programming #{0}] The {1} robot, {2}, has received a{3} {4} press.", ModuleId, colorName, currentRobot.Type.ToString(), directionName[0].EqualsAny("aeiou") ? "n" : "", directionName);

        //modifying direction according to robot's type
        Direction outputDirection = new Direction();
        switch (currentRobot.Type)
        {
            case Type.ROB:
                outputDirection = direction;
                break;
            case Type.HAL:
                outputDirection = (Direction) (((int) direction + 2) % 4);
                break;
            case Type.R2D2:
                if (R2D2actsLikeHAL)
                {
                    outputDirection = (Direction) (((int) direction + 2) % 4);
                }
                else
                {
                    outputDirection = direction;
                }

                Debug.LogFormat("[Robot Programming #{0}] R2D2 will act like {1} for this turn.", ModuleId, R2D2actsLikeHAL ? "HAL" : "ROB");
                R2D2actsLikeHAL = !R2D2actsLikeHAL;
                break;
            case Type.Fender:
                string serialNumber = bomb.GetSerialNumber();

                string serialBorderedCharacter = "";
                for (int i = 0; i < 6; i++)
                {
                    if (i == serialCharacterIndex)
                    {
                        serialBorderedCharacter += "[" + serialNumber[i] + "]";
                        continue;
                    }
                    serialBorderedCharacter += serialNumber[i];
                }

                bool isADigit = char.IsDigit(serialNumber[serialCharacterIndex]);
                if (isADigit)
                {
                    outputDirection = direction;
                }
                else
                {
                    outputDirection = (Direction) (((int) direction + 2) % 4);
                }

                Debug.LogFormat("[Robot Programming #{0}] The {1} character in the serial number ({2}) is a {3}, so Fender will act like {4} for this turn.", ModuleId, placeNames[serialCharacterIndex], serialBorderedCharacter, isADigit ? "digit" : "letter", isADigit ? "ROB" : "HAL");
                serialCharacterIndex = (serialCharacterIndex + 1) % 6;
                break;
        }

        //moving the robot
        switch (outputDirection)
        {
            case Direction.Up:
                currentRobot.Position.y--;
                break;
            case Direction.Right:
                currentRobot.Position.x++;
                break;
            case Direction.Down:
                currentRobot.Position.y++;
                break;
            case Direction.Left:
                currentRobot.Position.x--;
                break;
        }
        LogMsg("The " + colorName + " robot will move " + outputDirection.ToString().ToLower() + ". Its new coordinates are " + (currentRobot.Position.x + 1) + ", " + (9 - currentRobot.Position.y) + ".");

        bool crashes = isRobotColliding(currentRobot, !willStrike); //!willStrike makes it only log the first crash
        if (!willStrike) //prevents extraneous moves from being animated/counted for the most recent behavior, because it'll strike before it gets to the move anyway
        {
            animationQueue.Enqueue(new AnimationRequest(index, outputDirection, crashes));
            if (!crashes)
            {
                currentRobot.LastPosition = currentRobot.Position;
                lastR2D2Behavior = R2D2actsLikeHAL;
                lastSerialCharacterIndex = serialCharacterIndex;
            }
        }
        if (crashes)
        {
            willStrike = true;
        }

        //shift forward color by one and keep shifting if robot is stuck, completely stopping if all robots are stuck
        bool decidedRobot = false;
        for (int i = currentColorIndex + 1; i < currentColorIndex + 1 + notBlockedColors.Count; i++)
        {
            if (!isRobotStuck(sortedRobots[(int) notBlockedColors[i % notBlockedColors.Count]]))
            {
                decidedRobot = true;
                currentColorIndex = i % notBlockedColors.Count;
                break;
            }
        }
        if (!decidedRobot)
        {
            LogMsg("All robots are blocked or stuck, so no more moves can be made.");
            notBlockedColors.Clear(); //this is ok to clear because at that point no moves can be made anyway
        }

        updateLed();
        updateDisplay();
    }

    void blockButtonPressed(RobotColor color)
    {
        if (!notBlockedColors.Contains(color) || currentlyAnimating || moduleSolved)
            return;

        if (currentColorIndex > notBlockedColors.IndexOf(color))
        {
            //LogMsgSilent("Current color index greater than index of blocked color. Shifting down.");
            currentColorIndex--;
        }
        notBlockedColors.Remove(color);
        if (currentColorIndex > notBlockedColors.Count - 1)
        {
            //LogMsgSilent("Current color index greater than length of list. Setting to 0.");
            currentColorIndex = 0;
        }

        inputNames.Add("Block " + color.ToString());
        LogMsg(sortedRobots[(int) color].Type.ToString() + " (" + color.ToString() + ") has been blocked.");

        updateLed();
        updateDisplay();
    }

    //handle visuals
    void updateLed()
    {
        if (notBlockedColors.Count <= 0 || currentColorIndex == 4)
        {
            LedRenderer.material = unlitColorMaterials[4];

            if (colorblindActive)
                colorblindLEDText.text = "";

            return;
        }

        LedRenderer.material = unlitColorMaterials[(int) notBlockedColors[currentColorIndex]];

        if (colorblindActive)
            setColorblindText(colorblindLEDText, notBlockedColors[currentColorIndex]);
    }

    void updateDisplay()
    {
        displayShapesObject.SetActive(false);

        displayText.text = "";
        for (int i = Mathf.Max(0, inputNames.Count - 3); i < inputNames.Count; i++)
        {
            if (inputNames[i][0] == 'B') //If it's a block action
            {
                string colorHex = "";
                switch (inputNames[i].Split(' ')[1])
                {
                    case "Blue":
                        colorHex = "#3A58FF";
                        break;
                    case "Green":
                        colorHex = "#00FF00";
                        break;
                    case "Red":
                        colorHex = "#FF0000";
                        break;
                    case "Yellow":
                        colorHex = "#FFFF00";
                        break;
                }

                displayText.text += "<color=" + colorHex + ">Block</color>";
            }
            else
                displayText.text += inputNames[i];

            if (i < inputNames.Count - 1)
                displayText.text += "\n";
        }
    }

    //handle other things
    void handleStart()
    {
        if (inputNames.Count <= 0 || currentlyAnimating || moduleSolved)
            return;
        LogMsg("Running the program.");
        StartCoroutine(AnimateInstructions());
    }

    void handleReset()
    {
        if (currentlyAnimating || moduleSolved)
            return;

        resetVariables(false);
    }

    void resetVariables(bool struck)
    {
        if (!struck)
            LogMsg("Reset.");
        Debug.LogFormat("<Robot Programming #{0} R2D2 now acts like {1}.", ModuleId, initialR2D2Behavior ? "HAL" : "ROB");
        Debug.LogFormat("<Robot Programming #{0} Fender now starts at the {1} character.", ModuleId, placeNames[initialCharacterIndex]);
        Debug.LogFormat("<Robot Programming #{0} The LED is now {1}.", ModuleId, ((RobotColor) initialColorIndex).ToString());
        inputNames.Clear();

        //reset everything to what it initially was
        animationQueue.Clear();

        R2D2actsLikeHAL = initialR2D2Behavior;
        serialCharacterIndex = initialCharacterIndex;

        notBlockedColors = new List<RobotColor> { RobotColor.Blue, RobotColor.Green, RobotColor.Red, RobotColor.Yellow };
        currentColorIndex = initialColorIndex;
        updateLed();

        for (int i = 0; i < 4; i++)
        {
            sortedRobots[i].Position = sortedRobots[i].InitialPosition;
            Debug.LogFormat("<Robot Programming #{0}> {1} ({2}) is now at {3}, {4}.", ModuleId, sortedRobots[i].Type.ToString(), ((RobotColor) i).ToString(), sortedRobots[i].Position.x + 1, sortedRobots[i].Position.y + 1);
        }

        displayText.text = (struck ? "[ERROR]" : "[RESET]") + "\n R2D2: " + (R2D2actsLikeHAL ? "HAL" : "ROB") + "\nFender: " + placeNames[serialCharacterIndex] + "\n" + (topIndex + 1) + " " + (bottomIndex + 1) + "\n";
        displayShapesObject.SetActive(true);
        willStrike = false;
    }

    void handleStrike(RobotColor ledColor)
    {
        willStrike = true; //ensures willStrike is true (wouldn't be the case if the reason the mod is striking is because the robots aren't in goal positions)
        module.HandleStrike();
        bombAudio.PlaySoundAtTransform("strike", transform);

        //set everything to how it would be when the robot strikes
        initialColorIndex = (int) ledColor;
        initialR2D2Behavior = lastR2D2Behavior;
        initialCharacterIndex = lastSerialCharacterIndex;
        for (int i = 0; i < 4; i++)
        {
            sortedRobots[i].InitialPosition = sortedRobots[i].LastPosition;
        }

        resetVariables(true);
    }

    void handleSolve()
    {
        LogMsg("Solved.");
        module.HandlePass();
        moduleSolved = true;
        bombAudio.PlaySoundAtTransform("solve", transform);

        LedRenderer.material = unlitColorMaterials[4];
        displayText.text = "[SOLVED!]";
    }

    //checks
    bool isRobotColliding(Robot robot, bool logCrashes)
    {
        if (robot.Position.x < 0 || robot.Position.x > 8 || robot.Position.y < 0 || robot.Position.y > 8) //prevents out of bounds (which causes wall checks to error out)
        {
            if (logCrashes)
                LogMsg(robot.Type.ToString() + " (" + robot.Color.ToString() + ") is moving out of bounds. This move will cause the program to crash!");
            return true;
        }

        for (int i = 0; i < 4; i++)
        {
            if (sortedRobots[i].Position == robot.Position && sortedRobots[i] != robot) //if robot is on top of another robot
            {
                if (logCrashes)
                    LogMsg(robot.Type.ToString() + " (" + robot.Color.ToString() + ") is colliding with the " + ((RobotColor) i).ToString() + " robot. This move will cause the program to crash!");
                return true;
            }
        }

        if (maze[robot.Position.y][robot.Position.x] == 'X') //checks if robot is in a wall
        {
            if (logCrashes)
                LogMsg(robot.Type.ToString() + " (" + robot.Color.ToString() + ") is colliding with a wall. This move will cause the program to crash!");
            return true;
        }
        return false;
    }

    bool isRobotStuck(Robot robot)
    {
        Vector2Int[] directions = new Vector2Int[4] { new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1) };
        for (int i = 0; i < 4; i++)
        {
            if (!isRobotColliding(new Robot(robot.Position + directions[i]), false))
                return false;
        }
        LogMsg(robot.Type.ToString() + " (" + robot.Color.ToString() + ") cannot move! Skipping its turn.");
        return true;
    }

    bool isModuleSolved()
    {
        for (int i = 0; i < 4; i++)
        {
            if (sortedRobots[i].Position != goalPositions[i])
                return false;
        }
        return true;
    }

    //animation
    IEnumerator AnimateInstructions()
    {
        currentlyAnimating = true;
        int animationsDone = 0;

        while (animationQueue.Count > 0)
        {
            var request = animationQueue.Dequeue();
            GameObject robotObject = sortedRobotObjects[request.RobotIndex];
            Vector3 startPosition = robotObject.transform.localPosition;
            Vector3 endPosition = new Vector3(Mathf.Lerp(-0.07345f, 0.00635f, sortedRobots[request.RobotIndex].Position.x / 8f), 0f, Mathf.Lerp(0.04f, -0.04f, sortedRobots[request.RobotIndex].Position.y / 8f));
            switch (request.Direction)
            {
                case Direction.Up:
                    endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + .01f);
                    break;
                case Direction.Right:
                    endPosition = new Vector3(startPosition.x + 0.009975f, startPosition.y, startPosition.z);
                    break;
                case Direction.Down:
                    endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - .01f);
                    break;
                case Direction.Left:
                    endPosition = new Vector3(startPosition.x - 0.009975f, startPosition.y, startPosition.z);
                    break;
            }

            float t = 0;

            if (request.Crashed) //if the move made the robot crash. WILL strike
            {
                while (t < .5f)
                {
                    t += .05f;
                    robotObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                    yield return new WaitForSeconds(.01f);
                }
                handleStrike((RobotColor) request.RobotIndex);
                while (t > 0)
                {
                    t -= .05f;
                    robotObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                    yield return new WaitForSeconds(.01f);
                }
                currentlyAnimating = false;
                yield break;
            }

            float speed = Mathf.Lerp(.05f, 1f, animationsDone / 40f); //gradually speed up movement
            while (t < 1)
            {
                t += speed;
                robotObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                yield return new WaitForSeconds(.01f);
            }
            animationsDone++;
        }

        if (isModuleSolved())
        {
            handleSolve();
        }
        else
        {
            LogMsg("The program didn't bring all robots to the goal. Strike!");
            handleStrike(notBlockedColors[currentColorIndex]);
        }

        currentlyAnimating = false;
        yield break;
    }

    //colorblind
    void setupColorblind()
    {
        for (int i = 0; i < 4; i++)
        {
            setColorblindText(colorblindRobotTexts[i], robotColors[i]);
            if (robotShapes[i] == Shape.Triangle)
                colorblindRobotTexts[i].transform.localPosition = new Vector3(0f, -0.00192f, -0.0043f);
        }
        setColorblindText(colorblindLEDText, notBlockedColors[currentColorIndex]);
        colorblindOtherText.SetActive(true);
    }

    void setColorblindText(TextMesh text, RobotColor color)
    {
        text.text = "" + color.ToString()[0];
        if (color.EqualsAny(RobotColor.Green, RobotColor.Yellow))
            text.color = Color.black;
        else
            text.color = Color.white;
    }

    void toggleColorblind()
    {
        colorblindActive = !colorblindActive;
        if (colorblindActive)
        {
            for (int i = 0; i < 4; i++)
            {
                setColorblindText(colorblindRobotTexts[i], robotColors[i]);
            }
            setColorblindText(colorblindLEDText, notBlockedColors[currentColorIndex]);
            colorblindOtherText.SetActive(true);
            updateDisplay();
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                colorblindRobotTexts[i].text = "";
            }
            colorblindLEDText.text = "";
            colorblindOtherText.SetActive(false);
            updateDisplay();
        }
    }

    //twitch plays
    enum Input
    {
        Up, Right, Down, Left, Block, Reset, Start, Colorblind
    }
    private readonly string TwitchHelpMessage = @"""!{0} left/right/up/down/l/r/u/d"" to press that button. ""!{0} block red/yellow/green/blue/r/y/g/b"" to block that color. ""!{0} reset"" to reset the program. ""!{0} start"" to start the program. ""!{0} colorblind"" to toggle colorblind.";
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        var commands = cmd.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        Queue<Input> inputs = new Queue<Input>();
        Queue<RobotColor> blocks = new Queue<RobotColor>();
        for (int i = 0; i < commands.Length; i++)
        {
            switch (commands[i])
            {
                case "up":
                case "u":
                    inputs.Enqueue(Input.Up);
                    break;
                case "right":
                case "r":
                    inputs.Enqueue(Input.Right);
                    break;
                case "down":
                case "d":
                    inputs.Enqueue(Input.Down);
                    break;
                case "left":
                case "l":
                    inputs.Enqueue(Input.Left);
                    break;
                case "reset":
                    //inputs.Clear(); //since they will all be ignored by the reset anyway
                    inputs.Enqueue(Input.Reset);
                    break;
                case "start":
                    inputs.Enqueue(Input.Start);
                    i = commands.Length; //exit out of for loop, because further commands will be ignored by the start anyway
                    break;
                case "block":
                    inputs.Enqueue(Input.Block);

                    if (i + 1 >= commands.Length)
                    {
                        yield return @"sendtochaterror ""block"" command not followed by a color.";
                        yield break;
                    }
                    switch (commands[i + 1])
                    {
                        case "blue":
                        case "b":
                            blocks.Enqueue(RobotColor.Blue);
                            break;
                        case "green":
                        case "g":
                            blocks.Enqueue(RobotColor.Green);
                            break;
                        case "red":
                        case "r":
                            blocks.Enqueue(RobotColor.Red);
                            break;
                        case "yellow":
                        case "y":
                            blocks.Enqueue(RobotColor.Yellow);
                            break;
                        default:
                            yield return @"sendtochaterror ""block"" command not followed by a color.";
                            yield break;
                    }

                    i++;
                    break;
                case "colorblind":
                    yield return null;
                    toggleColorblind();
                    break;
                default:
                    yield return $@"sendtochaterror ""{commands[i]}"" is an improper command.";
                    yield break;
            }
        }

        if (inputs.Count > 0)
        {
            yield return null;

            while (currentlyAnimating)
                yield return "trycancel";

            if (!moduleSolved)
                yield return executeCommands(inputs, blocks);
        }
    }

    IEnumerator executeCommands(Queue<Input> inputs, Queue<RobotColor> blockedColors)
    {
        while (inputs.Count > 0)
        {
            Input input = inputs.Dequeue();
            switch (input)
            {
                case Input.Up:
                    arrowButtons[0].OnInteract();
                    break;
                case Input.Right:
                    arrowButtons[1].OnInteract();
                    break;
                case Input.Down:
                    arrowButtons[2].OnInteract();
                    break;
                case Input.Left:
                    arrowButtons[3].OnInteract();
                    break;
                case Input.Reset:
                    resetButton.OnInteract();
                    break;
                case Input.Start:
                    startButton.OnInteract();
                    break;
                case Input.Block:
                    RobotColor blockedColor = blockedColors.Dequeue();
                    blockButtons[(int) blockedColor].OnInteract();
                    break;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    //logging and other functions
    void LogMsg(string msg)
    {
        Debug.LogFormat("[Robot Programming #{0}] {1}", ModuleId, msg);
    }
    void LogMsgSilent(string msg)
    {
        Debug.LogFormat("<Robot Programming #{0}> {1}", ModuleId, msg);
    }
    string LogRobots(Robot robot)
    {
        return $"{robot.Color} {robot.Shape}";
    }
}