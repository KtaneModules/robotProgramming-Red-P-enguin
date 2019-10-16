using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class robotProgrammingScript : MonoBehaviour
{
    public KMBombModule module;
    public KMBombInfo bomb;
    public KMAudio audio;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool moduleSolved = false;

    public KMSelectable[] buttons;
    public KMSelectable[] arrowButtons;
    public Renderer led;
    public Material[] ledMats;
    public Material[] regMats;
    public SpriteRenderer topDisplay;
    public SpriteRenderer bottomDisplay;
    public Sprite[] mazes;
    public TextMesh commandDisplay;
    public TextMesh topText;
    public TextMesh bottomText;
    public Renderer cube;
    public Renderer cylinder;
    public Renderer triangle;
    public Renderer hexagon;
    public GameObject[] robotsObjects;
    public Renderer[] robots;
    public AudioClip[] sounds;

    public float[] xPos;
    public int[] startingCoords;

    private int index = 0;
    private int ledIndex = -1;
    private int whichRobotNum = 0;
    private int calcNum = 0;

    private List<int> colorsTaken = new List<int>();
    private List<int> posTaken = new List<int>();
    private List<int> whichRobots = new List<int>();
    private List<int> robotOrder = new List<int>();
    private List<int> conditionsTrue = new List<int>();
    private List<int> colorsBlocked = new List<int>();

    private List<int> movement = new List<int>();
    private List<int> colorMovement = new List<int>();
    private List<int> pyshicalMovement = new List<int>();
    private List<int> coordinates = new List<int>();
    private List<int> stuckCoordinates = new List<int>();
    private List<string> pressedButtons = new List<string>();

    private bool r2d2movement = false;
    private int fenderMovement = 0;
    private int r2d2tracker = 0;
    private int fenderTracker = 0;

    private string maze;
    private List<string> tops = new List<string>();
    private List<string> bottoms = new List<string>();

    string[] validbtns = { "left", "up", "right", "down", "l", "u", "r", "d" };
    //tops
    private string T1 = "X1X2X3X4X" +
                       "XOOOXOOOX" +
                       "XXXOXOXOX" +
                       "XOOOXOXOX" +
                       "XOXXXXXOX" +
                       "XOOOOOOOX";
    private string T2 = "X1X2X3X4X" +
                       "XOOOOOOOX" +
                       "XOXXOXXOX" +
                       "XOXOOOXOX" +
                       "XXXOXOXXX" +
                       "XOOOXOOOX";
    private string T3 = "X1X2X3X4X" +
                       "XOOOOOOOX" +
                       "XXXOXXXOX" +
                       "XOXOOOXOX" +
                       "XOXXXOXXX" +
                       "XOOOOOOOX";
    private string T4 = "X1X2X3X4X" +
                       "XOOOOOOOX" +
                       "XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XXXOXOXXX" +
                       "XOOOXOOOX";
    private string T5 = "X1X2X3X4X" +
                       "XOOOXOOOX" +
                       "XOXXXOXXX" +
                       "XOXOOOOOX" +
                       "XOXOXXXOX" +
                       "XOOOOOXOX";
    private string T6 = "X1X2X3X4X" +
                       "XOOOOOXOX" +
                       "XOXXXOXOX" +
                       "XOXOOOXOX" +
                       "XXXOXXXOX" +
                       "XOOOOOOOX";
    private string T7 = "X1X2X3X4X" +
                       "XOXOXOXOX" +
                       "XOXOXOXOX" +
                       "XOOOOOOOX" +
                       "XOXXXXXOX" +
                       "XOOOOOXOX";
    private string T8 = "X1X2X3X4X" +
                       "XOXOOOXOX" +
                       "XOXOXXXOX" +
                       "XOOOOOOOX" +
                       "XOXOXOXOX" +
                       "XOXOXOXOX";
    private string T9 = "X1X2X3X4X" +
                       "XOXOOOXOX" +
                       "XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XOXXXXXOX" +
                       "XOOOOOOOX";
    private string T10 = "X1X2X3X4X" +
                       "XOXOOOXOX" +
                       "XOXXXOXOX" +
                       "XOOOOOOOX" +
                       "XXXOXXXXX" +
                       "XOOOOOOOX";
    private string T11 = "X1X2X3X4X" +
                       "XOOOXOOOX" +
                       "XOXOXOXOX" +
                       "XOXOOOXOX" +
                       "XOXOXXXOX" +
                       "XOOOXOOOX";
    private string T12 = "X1X2X3X4X" +
                       "XOXOOOXOX" +
                       "XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XOXXXXXOX" +
                       "XOOOXOOOX";
    private string T13 = "X1X2X3X4X" +
                       "XOOOXOXOX" +
                       "XOXXXOXOX" +
                       "XOOOOOOOX" +
                       "XXXOXOXXX" +
                       "XOOOXOOOX";
    private string T14 = "X1X2X3X4X" +
                       "XOOOOOOOX" +
                       "XXOXXXOXX" +
                       "XOOOXOOOX" +
                       "XOXOXOXOX" +
                       "XOXOOOXOX";
    private string T15 = "X1X2X3X4X" +
                       "XOXOOOOOX" +
                       "XOXOXXXOX" +
                       "XOOOXOOOX" +
                       "XOXXXXXOX" +
                       "XOXOOOOOX";
    private string T16 = "X1X2X3X4X" +
                       "XOOOOOXOX" +
                       "XOXXXOXOX" +
                       "XOXOOOXOX" +
                       "XOXOXXXOX" +
                       "XOXOOOOOX";

    //bottoms
    private string B1 ="XOXOXXXOX" +
                       "XOXOXOOOX" +
                       "XXXXXXXXX";
    private string B2 ="XOXXXXXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B3 ="XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B4 ="XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B5 ="XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B6 ="XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B7 ="XOXOXOXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B8 ="XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";
    private string B9 ="XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B10 ="XXXOXXXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B11 ="XOXXXOXXX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B12 ="XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";
    private string B13 ="XOXXXOXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B14 ="XOXXXOXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B15 ="XOXOXXXOX" +
                       "XOXOOOXOX" +
                       "XXXXXXXXX";
    private string B16 ="XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";

    void Awake()
    {
        ModuleId = ModuleIdCounter++;
        foreach (KMSelectable button in arrowButtons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
    }

    void Start()
    {
        coordinates.Add(64);
        coordinates.Add(66);
        coordinates.Add(68);
        coordinates.Add(70);
        //intialization
        pressedButtons.Add("");
        pressedButtons.Add("");
        stuckCoordinates.Add(0);
        stuckCoordinates.Add(0);
        stuckCoordinates.Add(0);
        stuckCoordinates.Add(0);
        whichRobots.Add(0);
        whichRobots.Add(0);
        whichRobots.Add(0);
        whichRobots.Add(0);
        tops.Add(T1);
        tops.Add(T2);
        tops.Add(T3);
        tops.Add(T4);
        tops.Add(T5);
        tops.Add(T6);
        tops.Add(T7);
        tops.Add(T8);
        tops.Add(T9);
        tops.Add(T10);
        tops.Add(T11);
        tops.Add(T12);
        tops.Add(T13);
        tops.Add(T14);
        tops.Add(T15);
        tops.Add(T16);
        bottoms.Add(B1);
        bottoms.Add(B2);
        bottoms.Add(B3);
        bottoms.Add(B4);
        bottoms.Add(B5);
        bottoms.Add(B6);
        bottoms.Add(B7);
        bottoms.Add(B8);
        bottoms.Add(B9);
        bottoms.Add(B10);
        bottoms.Add(B11);
        bottoms.Add(B12);
        bottoms.Add(B13);
        bottoms.Add(B14);
        bottoms.Add(B15);
        bottoms.Add(B16);
        index = UnityEngine.Random.Range(0, 16);
        topDisplay.sprite = mazes[index];
        topText.text = "" + (index + 1);
        maze = tops[index];
        index = UnityEngine.Random.Range(0, 16);
        bottomDisplay.sprite = mazes[index + 16];
        maze = maze + bottoms[index];
        topText.text = topText.text + " " + (index + 1);
        moduleStriked();
        colorRobot();
    }

    void moduleStriked()
    {
        moduleSolved = false;
        calcNum = 0;
        colorsBlocked.Clear();
        movement.Clear();
        colorMovement.Clear();
        pyshicalMovement.Clear();
        return;
    }

    void colorRobot()
    {
        index = UnityEngine.Random.Range(0, 4);
        if (!colorsTaken.Contains(index))
        {
            colorsTaken.Add(index);
            if (colorsTaken.Count < 4)
            {
                colorRobot();
            }
            else
            {
                posRobot();
            }
        }
        else
        {
            colorRobot();
        }
    }

    void posRobot()
    {
        index = UnityEngine.Random.Range(0, 4);
        if (!posTaken.Contains(index))
        {
            posTaken.Add(index);
            whichRobots[index] = whichRobotNum;
            whichRobotNum++;
            if (posTaken.Count < 4)
            {
                posRobot();
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    robotsObjects[i].transform.localPosition = new Vector3(xPos[posTaken[i]], 0.0175f, -0.03f);
                    if(colorsTaken[i] == 0)
                    {
                        coordinates[3] = startingCoords[i];
                    }
                    if (colorsTaken[i] == 1)
                    {
                        coordinates[2] = startingCoords[i];
                    }
                    if (colorsTaken[i] == 2)
                    {
                        coordinates[0] = startingCoords[i];
                    }
                    if (colorsTaken[i] == 3)
                    {
                        coordinates[1] = startingCoords[i];
                    }
                    robots[i].material = regMats[colorsTaken[posTaken[i]]];
                }
                robotFinder();
            }
        }
        else
        {
            posRobot();
        }
    }

    void robotFinder()
    {
        // Red or Green in first pos
        if (colorsTaken[0] == 1 || colorsTaken[0] == 2)
        {
            DebugMsg("Red or Lime is in first position. First condition applies.");
            conditionsTrue.Add(1);
        }
        else
        {
            DebugMsg("Red or Lime is not in first position. First condition doesn't apply.");
            conditionsTrue.Add(0);
        }
        // Hexagon is yellow
        if (colorsTaken[posTaken[3]] == 3)
        {
            DebugMsg("Hexagon is yellow. Second condition applies.");
            conditionsTrue.Add(1);
        }
        else
        {
            DebugMsg("Hexagon is not yellow. Second condition doesn't apply.");
            conditionsTrue.Add(0);
        }
        // Triangle in 3rd pos
        if (posTaken[2] == 2)
        {
            DebugMsg("Triangle is in third position. Third condition applies.");
            conditionsTrue.Add(1);
        }
        else
        {
            DebugMsg("Triangle is not in third position. Third condition doesn't apply.");
            conditionsTrue.Add(0);
        }

        //calculations
        if (conditionsTrue[0] != 1)
        {
            if (conditionsTrue[1] != 1)
            {
                if (conditionsTrue[2] != 1)
                {
                    robotOrder.Add(0);
                    robotOrder.Add(1);
                    robotOrder.Add(2);
                    robotOrder.Add(3);
                }
                else
                {
                    robotOrder.Add(1);
                    robotOrder.Add(3);
                    robotOrder.Add(0);
                    robotOrder.Add(2);
                }
            }
            else
            {
                if (conditionsTrue[2] != 1)
                {
                    robotOrder.Add(1);
                    robotOrder.Add(0);
                    robotOrder.Add(3);
                    robotOrder.Add(2);
                }
                else
                {
                    robotOrder.Add(2);
                    robotOrder.Add(3);
                    robotOrder.Add(1);
                    robotOrder.Add(0);
                }
            }
        }
        else
        {
            if (conditionsTrue[1] != 1)
            {
                if (conditionsTrue[2] != 1)
                {
                    robotOrder.Add(3);
                    robotOrder.Add(0);
                    robotOrder.Add(1);
                    robotOrder.Add(2);
                }
                else
                {
                    robotOrder.Add(1);
                    robotOrder.Add(2);
                    robotOrder.Add(3);
                    robotOrder.Add(0);
                }
            }
            else
            {
                if (conditionsTrue[2] != 1)
                {
                    robotOrder.Add(3);
                    robotOrder.Add(1);
                    robotOrder.Add(2);
                    robotOrder.Add(0);
                }
                else
                {
                    robotOrder.Add(2);
                    robotOrder.Add(0);
                    robotOrder.Add(3);
                    robotOrder.Add(1);
                }
            }
        }
        PickLEDcolor();
    }

    void PickLEDcolor()
    {
        for (int i = 0; i < 4; i++)
        {
            stuckCoordinates[i] = coordinates[i];
        }
        ledIndex++;
        if (ledIndex > 3)
        {
            ledIndex = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            if (colorsTaken[i] == ledIndex)
            {
                if (colorsTaken[i] == 0)
                {
                    if (isStuck(9, 3) && isStuck(-9, 3) && isStuck(1, 3) && isStuck(-1, 3))
                    {
                        PickLEDcolor();
                    }
                }
                if (colorsTaken[i] == 1)
                {
                    if (isStuck(9, 2) && isStuck(-9, 2) && isStuck(1, 2) && isStuck(-1, 2))
                    {
                        PickLEDcolor();
                    }
                }
                if (colorsTaken[i] == 2)
                {
                    if (isStuck(9, 0) && isStuck(-9, 0) && isStuck(1, 0) && isStuck(-1, 0))
                    {
                        PickLEDcolor();
                    }
                }
                if (colorsTaken[i] == 3)
                {
                    if (isStuck(9, 1) && isStuck(-9, 1) && isStuck(1, 1) && isStuck(-1, 1))
                    {
                        PickLEDcolor();
                    }
                }
            }
        }
        if (!colorsBlocked.Contains(ledIndex))
        {
            led.material = ledMats[ledIndex];
        }
        else if (colorsBlocked.Contains(0) && colorsBlocked.Contains(1) && colorsBlocked.Contains(2) && colorsBlocked.Contains(3))
        {
            led.material = ledMats[4];
        }
        else
        {
            PickLEDcolor();
        }
        return;
    }

    private bool isStuck(int stuckCoordNum, int whichCoord)
    {
        for (int i = 0; i < 4; i++)
        {
            if (stuckCoordinates[i] < 0)
            {
                stuckCoordinates[i] = 0;
            }
        }
        for (int i = 0; i < movement.Count(); i++)
        {
            if (maze[stuckCoordinates[colorMovement[i]]] != 'X')
            {
                if (movement[calcNum] == 0)
                {
                    stuckCoordinates[colorMovement[i]] = stuckCoordinates[colorMovement[i]] + 9;
                }
                else if (movement[calcNum] == 1)
                {
                    stuckCoordinates[colorMovement[i]] = stuckCoordinates[colorMovement[i]] - 1;
                }
                else if (movement[calcNum] == 2)
                {
                    stuckCoordinates[colorMovement[i]] = stuckCoordinates[colorMovement[i]] - 9;
                }
                else if (movement[calcNum] == 3)
                {
                    stuckCoordinates[colorMovement[i]] = stuckCoordinates[colorMovement[i]] + 1;
                }
            }
        }
        if (stuckCoordinates[whichCoord] + stuckCoordNum == stuckCoordinates[0] || stuckCoordinates[whichCoord] + stuckCoordNum == stuckCoordinates[1] || stuckCoordinates[whichCoord] + stuckCoordNum == stuckCoordinates[2] || stuckCoordinates[whichCoord] + stuckCoordNum == stuckCoordinates[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void buttonPressed(KMSelectable pressedButton)
    {
        bottomText.text = "";
        commandDisplay.text = "";
        pressedButton.AddInteractionPunch();
        if (moduleSolved)
        {
            return;
        }
        else
        {
            if (arrowButtons.Contains(pressedButton))
            {
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                pressedButtons.Add(pressedButton.name);
                for (int i = 0; i < 4; i++)
                {
                    commandDisplay.text = "" + pressedButtons[pressedButtons.Count() - 3] + " " + pressedButtons[pressedButtons.Count() - 2] + " " + pressedButtons[pressedButtons.Count() - 1];
                    if (colorsTaken[i] == ledIndex)
                    {
                        if (colorsTaken[i] == 0)
                        {
                            colorMovement.Add(3);
                        }
                        if (colorsTaken[i] == 1)
                        {
                            colorMovement.Add(2);
                        }
                        if (colorsTaken[i] == 2)
                        {
                            colorMovement.Add(0);
                        }
                        if (colorsTaken[i] == 3)
                        {
                            colorMovement.Add(1);
                        }
                        pyshicalMovement.Add(i);
                        PickLEDcolor();
                        if (robotOrder[i] == 0)
                        {
                            DebugMsg("You are controlling R.O.B.");
                            if (pressedButton == arrowButtons[0])
                            {
                                movement.Add(0);
                            }
                            if (pressedButton == arrowButtons[1])
                            {
                                movement.Add(1);
                            }
                            if (pressedButton == arrowButtons[2])
                            {
                                movement.Add(2);
                            }
                            if (pressedButton == arrowButtons[3])
                            {
                                movement.Add(3);
                            }
                        }
                        else if (robotOrder[i] == 1)
                        {
                            DebugMsg("You are controlling HAL.");
                            if (pressedButton == arrowButtons[0])
                            {
                                movement.Add(2);
                            }
                            if (pressedButton == arrowButtons[1])
                            {
                                movement.Add(3);
                            }
                            if (pressedButton == arrowButtons[2])
                            {
                                movement.Add(0);
                            }
                            if (pressedButton == arrowButtons[3])
                            {
                                movement.Add(1);
                            }
                        }
                        else if (robotOrder[i] == 2)
                        {
                            DebugMsg("You are controlling R2D2.");
                            if (r2d2movement == false)
                            {
                                DebugMsg("R2D2 will act like R.O.B.");
                                r2d2movement = true;
                                if (pressedButton == arrowButtons[0])
                                {
                                    movement.Add(0);
                                }
                                if (pressedButton == arrowButtons[1])
                                {
                                    movement.Add(1);
                                }
                                if (pressedButton == arrowButtons[2])
                                {
                                    movement.Add(2);
                                }
                                if (pressedButton == arrowButtons[3])
                                {
                                    movement.Add(3);
                                }
                            }
                            else
                            {
                                r2d2movement = false;
                                DebugMsg("R2D2 will act like HAL.");
                                if (pressedButton == arrowButtons[0])
                                {
                                    movement.Add(2);
                                }
                                if (pressedButton == arrowButtons[1])
                                {
                                    movement.Add(3);
                                }
                                if (pressedButton == arrowButtons[2])
                                {
                                    movement.Add(0);
                                }
                                if (pressedButton == arrowButtons[3])
                                {
                                    movement.Add(1);
                                }
                            }
                        }
                        else
                        {
                            DebugMsg("You are controlling Fender.");
                            if (char.IsDigit(bomb.GetSerialNumber()[fenderMovement]))
                            {
                                DebugMsg("Fender will act like R.O.B.");
                                if (pressedButton == arrowButtons[0])
                                {
                                    movement.Add(0);
                                }
                                if (pressedButton == arrowButtons[1])
                                {
                                    movement.Add(1);
                                }
                                if (pressedButton == arrowButtons[2])
                                {
                                    movement.Add(2);
                                }
                                if (pressedButton == arrowButtons[3])
                                {
                                    movement.Add(3);
                                }
                            }
                            else
                            {
                                DebugMsg("Fender will act like HAL.");
                                if (pressedButton == arrowButtons[0])
                                {
                                    movement.Add(2);
                                }
                                if (pressedButton == arrowButtons[1])
                                {
                                    movement.Add(3);
                                }
                                if (pressedButton == arrowButtons[2])
                                {
                                    movement.Add(0);
                                }
                                if (pressedButton == arrowButtons[3])
                                {
                                    movement.Add(1);
                                }
                            }
                            fenderMovement++;
                            if (fenderMovement == 6)
                            {
                                fenderMovement = 0;
                            }
                        }
                        i = 4;
                    }
                }
            }
            else if(pressedButton == buttons[1])
            {
                moduleStriked();
                strikedDisplay();
            }
            else if (pressedButton == buttons[0])
            {
                moduleSolved = true;
                calculatingMovement();
            }
            else if (pressedButton == buttons[2])
            {
                colorsBlocked.Add(0);
                if(ledIndex == 0)
                {
                    PickLEDcolor();
                }
            }
            else if (pressedButton == buttons[3])
            {
                colorsBlocked.Add(1);
                if (ledIndex == 1)
                {
                    PickLEDcolor();
                }
            }
            else if (pressedButton == buttons[4])
            {
                colorsBlocked.Add(2);
                if (ledIndex == 2)
                {
                    PickLEDcolor();
                }
            }
            else if (pressedButton == buttons[5])
            {
                colorsBlocked.Add(3);
                if (ledIndex == 3)
                {
                    PickLEDcolor();
                }
            }
        }
    }

    void calculatingMovement()
    {
        if (calcNum == movement.Count())
        {
            calculatingEnd();
        }
        else
        {
            if (robotOrder[pyshicalMovement[calcNum]] == 2)
            {
                r2d2tracker++;
            }
            if (robotOrder[pyshicalMovement[calcNum]] == 3)
            {
                fenderTracker++;
                if (fenderTracker == 6)
                {
                    fenderTracker = 0;
                }
            }
            if (movement[calcNum] == 0)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 9;
            }
            else if (movement[calcNum] == 1)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 1;
            }
            else if (movement[calcNum] == 2)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 9;
            }
            else if (movement[calcNum] == 3)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 1;
            }
            duringCheck();
        }
    }

    void duringCheck()
    {
        if (maze[coordinates[colorMovement[calcNum]]] == 'X' || coordinates[0] == coordinates[1] || coordinates[0] == coordinates[2] || coordinates[0] == coordinates[3] || coordinates[1] == coordinates[2] || coordinates[1] == coordinates[3] || coordinates[2] == coordinates[3])
        {
            if (movement[calcNum] == 0)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 9;
            }
            else if (movement[calcNum] == 1)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 1;
            }
            else if (movement[calcNum] == 2)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 9;
            }
            else if (movement[calcNum] == 3)
            {
                coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 1;
            }
            if (robotOrder[pyshicalMovement[calcNum]] == 2 && r2d2tracker > 0)
            {
                r2d2tracker = r2d2tracker - 1;
                if (r2d2tracker % 2 == 0)
                {
                    r2d2movement = false;
                }
                else
                {
                    r2d2movement = true;
                }
            }
            else if (robotOrder[pyshicalMovement[calcNum]] == 3)
            {
                if (fenderTracker > 0)
                {
                    fenderTracker--;
                }
                fenderMovement = fenderTracker;
            }
            moduleSolved = false;
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot crashed. Module striked.");
            DebugMsg("ERROR: Successful movements: " + calcNum);
            commandDisplay.text = "CRASHED";
            GetComponent<KMBombModule>().HandleStrike();
            Invoke("moduleStriked", 1);
            moduleStriked();
            strikedDisplay();
        }
        else if (coordinates[colorMovement[calcNum]] < 0)
        {
            moduleSolved = false;
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot outside of boundries. Module striked.");
            commandDisplay.text = "OOB";
            GetComponent<KMBombModule>().HandleStrike();
            Invoke("moduleStriked", 1);
            strikedDisplay();
        }
        else
        {
            mover();
        }
    }

    void mover()
    {
        if (movement[calcNum] == 0)
        {
            robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.x, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.z - .01f);
        }
        else if (movement[calcNum] == 1)
        {
            robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.x - .01f, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.z);
        }
        else if (movement[calcNum] == 2)
        {
            robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.x, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.z + .01f);
        }
        else if (movement[calcNum] == 3)
        {
            robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.x + .01f, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[pyshicalMovement[calcNum]]].transform.localPosition.z);
        }
        calcNum++;
        Invoke("calculatingMovement",.2f);
    }

    void calculatingEnd()
    {
        if (maze[coordinates[0]] == '1' && maze[coordinates[1]] == '2' && maze[coordinates[2]] == '3' && maze[coordinates[3]] == '4')
        {
            audio.PlaySoundAtTransform("solve", transform);
            DebugMsg("Program run successfully. Module solved.");
            GetComponent<KMBombModule>().HandlePass();
        }
        else
        {
            moduleSolved = false;
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Out of commands. Module striked.");
            commandDisplay.text = "OOC";
            GetComponent<KMBombModule>().HandleStrike();
            Invoke("moduleStriked", 1);
            strikedDisplay();
        }
    }

    void strikedDisplay()
    {
        commandDisplay.text = "" + regMats[colorsTaken[0]].name[0] + regMats[colorsTaken[1]].name[0] + regMats[colorsTaken[2]].name[0] + regMats[colorsTaken[3]].name[0] + " " + (fenderMovement + 1);
        if(r2d2movement == false)
        {
            bottomText.text = "R.O.B";
        }
        else
        {
            bottomText.text = "HAL";
        }
    }

    public string TwitchHelpMessage = "Use !{0} press left to press the left button. (Valid buttons are left, right, up, down). Use !{0} block blue/red/green/yellow to block that color. Use !{0} start to start the program. Use !{0} reset to reset the program.";
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        var parms = cmd.ToLowerInvariant().Split(new[] { ' ' });

        if (parms.Length == 2 && parms[0].ToLower() == "block")
        {
            if (parms[1].ToLower() == "red" || parms[1].ToLower() == "r")
            {
                yield return null;
                yield return new KMSelectable[] { buttons[4] };
            }
            else if (parms[1].ToLower() == "yellow" || parms[1].ToLower() == "y")
            {
                yield return null;
                yield return new KMSelectable[] { buttons[5] };
            }
            else if (parms[1].ToLower() == "green" || parms[1].ToLower() == "g")
            {
                yield return null;
                yield return new KMSelectable[] { buttons[3] };
            }
            else if (parms[1].ToLower() == "blue" || parms[1].ToLower() == "b")
            {
                yield return null;
                yield return new KMSelectable[] { buttons[2] };
            }
            else
            {
                yield break;
            }
        }
        else if (parms.Length == 1 && parms[0].ToLower() == "start")
        {
            yield return null;
            yield return new KMSelectable[] { buttons[0] };
        }
        else if (parms.Length == 1 && parms[0].ToLower() == "reset")
        {
            yield return null;
            yield return new KMSelectable[] { buttons[1] };
        }
        else
        {
            string[] parameters = cmd.Split(' ');
            var buttonsToPress = new List<KMSelectable>();
            foreach (string parm in parameters)
            {
                if (parm.ToLower() == "l" || parm.ToLower() == "left")
                {
                    buttonsToPress.Add(arrowButtons[1]);
                }
                else  if (parm.ToLower() == "d" || parm.ToLower() == "down")
                {
                    buttonsToPress.Add(arrowButtons[0]);
                }
                else if (parm.ToLower() == "u" || parm.ToLower() == "up")
                {
                    buttonsToPress.Add(arrowButtons[2]);
                }
                else if (parm.ToLower() == "r" || parm.ToLower() == "right")
                {
                    buttonsToPress.Add(arrowButtons[3]);
                }
                else
                {
                    yield break;
                }
            }
            yield return null;
            yield return buttonsToPress;
        }
    }

    void DebugMsg(string msg)
    {
        Debug.LogFormat("[Robot Programming #{0}] {1}", ModuleId, msg);
    }
}
