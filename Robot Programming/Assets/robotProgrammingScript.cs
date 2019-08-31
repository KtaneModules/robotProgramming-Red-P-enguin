﻿using System;
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
    public Renderer cube;
    public Renderer cylinder;
    public Renderer triangle;
    public Renderer hexagon;
    public GameObject[] robotsObjects;
    public Renderer[] robots;
    public AudioClip[] sounds;

    public float[] xPos;

    private int index = 0;
    private int ledIndex = 0;
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
    private List<int> coordinates = new List<int>();
    private List<string> pressedButtons = new List<string>();

    private bool r2d2movement = false;
    private int fenderMovement = 0;

    private string maze;
    private List<string> tops = new List<string>();
    private List<string> bottoms = new List<string>();
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
    private string B1 = "XXXOXXXOX" +
                       "XOOOXOOOX" +
                       "XXXOXXXXX" +
                       "XOOOOOOOX" +
                       "XOXOXXXOX" +
                       "XOXOXOOOX" +
                       "XXXXXXXXX";
    private string B2 = "XXXOXOXXX" +
                       "XOOOXOOOX" +
                       "XOXXXXXOX" +
                       "XOOOOOOOX" +
                       "XOXXXXXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B3 = "XOXXXOXXX" +
                       "XOOOOOOOX" +
                       "XOXXXXXXX" +
                       "XOOOXOOOX" +
                       "XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B4 = "XXXOXOXOX" +
                       "XOOOXOXOX" +
                       "XOXXXXXOX" +
                       "XOXOOOOOX" +
                       "XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B5 = "XOXOXXXXX" +
                       "XOXOOOXOX" +
                       "XOXXXOXOX" +
                       "XOXOOOOOX" +
                       "XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B6 = "XOXXXOXOX" +
                       "XOXOOOXOX" +
                       "XOXXXOXOX" +
                       "XOXOOOOOX" +
                       "XOXOXXXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B7 = "XXXOXXXOX" +
                       "XOOOXOXOX" +
                       "XOXXXOXOX" +
                       "XOXOXOOOX" +
                       "XOXOXOXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B8 = "XXXOXXXOX" +
                       "XOOOOOXOX" +
                       "XOXXXOXOX" +
                       "XOXOXOXOX" +
                       "XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";
    private string B9 = "XOXXXOXOX" +
                       "XOOOXOXOX" +
                       "XOXXXOXXX" +
                       "XOOOOOOOX" +
                       "XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B10 = "XXXOXXXOX" +
                       "XOOOOOOOX" +
                       "XOXXXOXXX" +
                       "XOOOOOOOX" +
                       "XXXOXXXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B11 = "XXXOXOXOX" +
                       "XOOOXOXOX" +
                       "XOXXXXXOX" +
                       "XOOOOOOOX" +
                       "XOXXXOXXX" +
                       "XOXOOOOOX" +
                       "XXXXXXXXX";
    private string B12 = "XXXOXOXXX" +
                       "XOOOXOOOX" +
                       "XOXXXXXOX" +
                       "XOXOOOXOX" +
                       "XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";
    private string B13 = "XOXXXOXOX" +
                       "XOOOOOXOX" +
                       "XXXOXOXXX" +
                       "XOOOXOXOX" +
                       "XOXXXOXOX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B14 = "XXXOXOXXX" +
                       "XOXOXOOOX" +
                       "XOXOXXXOX" +
                       "XOOOXOOOX" +
                       "XOXXXOXXX" +
                       "XOOOOOOOX" +
                       "XXXXXXXXX";
    private string B15 = "XOXOXXXOX" +
                       "XOXOOOOOX" +
                       "XOXXXOXXX" +
                       "XOXOOOOOX" +
                       "XOXOXXXOX" +
                       "XOXOOOXOX" +
                       "XXXXXXXXX";
    private string B16 = "XXXOXOXXX" +
                       "XOOOOOOOX" +
                       "XOXXXXXOX" +
                       "XOXOOOXOX" +
                       "XOXOXOXOX" +
                       "XOOOXOOOX" +
                       "XXXXXXXXX";

    void Awake()
    {
        ModuleId = ModuleIdCounter++;
        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
        foreach (KMSelectable button in arrowButtons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
    }

    void Start()
    {
        //intialization
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
        maze = tops[index];
        index = UnityEngine.Random.Range(0, 16);
        bottomDisplay.sprite = mazes[index + 16];
        maze = maze + bottoms[index];
        moduleStriked();
        colorRobot();
    }

    void moduleStriked()
    {
        r2d2movement = false;
        fenderMovement = 0;
        calcNum = 0;
        colorsBlocked.Clear();
        movement.Clear();
        colorMovement.Clear();
        pressedButtons.Clear();
        coordinates.Clear();
        coordinates.Add(100);
        coordinates.Add(102);
        coordinates.Add(104);
        coordinates.Add(106);
        //commandDisplay.text = "";
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
                    robotsObjects[i].transform.localPosition = new Vector3(xPos[posTaken[i]], 0.0175f, -0.03475f);
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
        ledIndex = UnityEngine.Random.Range(0, 4);
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
    }

    void buttonPressed(KMSelectable pressedButton)
    {
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
                if (pressedButtons.Count() == 1)
                {
                    commandDisplay.text = pressedButtons[0];
                }
                else if (pressedButtons.Count() == 2)
                {
                    commandDisplay.text = pressedButtons[0] + " " + pressedButtons[1];
                }
                else
                {
                    commandDisplay.text = pressedButtons[pressedButtons.Count() - 3] + " " + pressedButtons[pressedButtons.Count() - 2] + " " + pressedButtons[pressedButtons.Count() - 1];
                }
                for (int i = 0; i < 4; i++)
                {
                    if (colorsTaken[i] == ledIndex)
                    {
                        colorMovement.Add(i);
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
            else if (pressedButton == buttons[0])
            {
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                moduleStriked();
                PickLEDcolor();
            }
            else if (pressedButton == buttons[1])
            {

                calculatingMovement();
            }
            else
            {
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                if (pressedButton == buttons[2])
                {
                    colorsBlocked.Add(2);
                    if (led.material = ledMats[2])
                    {
                        PickLEDcolor();
                    }
                }
                else if (pressedButton == buttons[3])
                {
                    colorsBlocked.Add(3);
                    if (led.material = ledMats[3])
                    {
                        PickLEDcolor();
                    }
                }
                else if (pressedButton == buttons[4])
                {
                    colorsBlocked.Add(1);
                    if (led.material = ledMats[1])
                    {
                        PickLEDcolor();
                    }
                }
                else
                {
                    colorsBlocked.Add(0);
                    if (led.material = ledMats[0])
                    {
                        PickLEDcolor();
                    }
                }
            }
        }
    }

    void calculatingMovement()
    {
        /*/if (movement.Count() == 0)
        {
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot not at goal. Module striked.");
            GetComponent<KMBombModule>().HandleStrike();
            moduleStriked();
            return;
        }/*/
        if (calcNum == movement.Count())
        {
            calculatingEnd();
            return;
        }
        if (coordinates[colorMovement[calcNum]] < 0)
        {
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot outside of boundries. Module striked.");
            GetComponent<KMBombModule>().HandleStrike();
            moduleStriked();
            return;
        }
        if (movement[calcNum] == 0)
        {
            coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 9;
            robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.x, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.z - .01f);
        }
        else if (movement[calcNum] == 1)
        {
            coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 1;
            robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.x - .01f, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.z);
        }
        else if (movement[calcNum] == 2)
        {
            coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] - 9;
            robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.x, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.z + .01f);
        }
        else if (movement[calcNum] == 3)
        {
            coordinates[colorMovement[calcNum]] = coordinates[colorMovement[calcNum]] + 1;
            robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition = new Vector3(robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.x + .01f, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.y, robotsObjects[whichRobots[colorMovement[calcNum]]].transform.localPosition.z);
        }
        Invoke("duringCheck", .75f);
    }

    void duringCheck()
    {
        if (maze[coordinates[colorMovement[calcNum]]] == 'X' || coordinates[0] == coordinates[1] || coordinates[0] == coordinates[2] || coordinates[0] == coordinates[3] || coordinates[1] == coordinates[2] || coordinates[1] == coordinates[3] || coordinates[2] == coordinates[3])
        {
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot crashed. Module striked.");
            DebugMsg("Successful movements: " + calcNum);
            GetComponent<KMBombModule>().HandleStrike();
            for (int i = 0; i < 4; i++)
            {
                robotsObjects[i].transform.localPosition = new Vector3(xPos[posTaken[i]], 0.0175f, -0.03475f);
            }
            moduleStriked();
        }
        else
        {
            calcNum++;
            calculatingMovement();
        }
    }

    void calculatingEnd()
    {
        commandDisplay.text = "" + maze[coordinates[0]] + maze[coordinates[1]] + maze[coordinates[2]] + maze[coordinates[3]];
        if (maze[coordinates[0]] == '1' && maze[coordinates[1]] == '2' && maze[coordinates[2]] == '3' && maze[coordinates[3]] == '4')
        {
            audio.PlaySoundAtTransform("solve", transform);
            DebugMsg("Program run successfully. Module solved.");
            GetComponent<KMBombModule>().HandlePass();
        }
        else
        {
            audio.PlaySoundAtTransform("strike", transform);
            DebugMsg("ERROR: Robot not at goal. Module striked.");
            GetComponent<KMBombModule>().HandleStrike();
            for (int i = 0; i < 4; i++)
            {
                robotsObjects[i].transform.localPosition = new Vector3(xPos[posTaken[i]], 0.0175f, -0.03475f);
            }
            moduleStriked();
        }
    }

    public string TwitchHelpMessage = "Use !{0} press left to press the left button. (Valid buttons are left, right, up, down). Use !{0} block blue/red/green/yellow to block that color. Use !{0} start to start the program. Use !{0} reset to reset the program.";
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });

        if (parts.Length == 2 && parts[0] == "press")
        {
            if (parts[1].ToLower() == "down")
            {
                yield return null;
                buttonPressed(arrowButtons[0]);
            }
            else if (parts[1].ToLower() == "left")
            {
                yield return null;
                buttonPressed(arrowButtons[1]);
            }
            else if (parts[1].ToLower() == "up")
            {
                yield return null;
                buttonPressed(arrowButtons[2]);
            }
            else if (parts[1].ToLower() == "right")
            {
                yield return null;
                buttonPressed(arrowButtons[3]);
            }
        }
        else if (parts.Length == 2 && parts[0] == "block")
        {
            if (parts[1].ToLower() == "red")
            {
                yield return null;
                buttonPressed(buttons[2]);
            }
            else if (parts[1].ToLower() == "yellow")
            {
                yield return null;
                buttonPressed(buttons[3]);
            }
            else if (parts[1].ToLower() == "green")
            {
                yield return null;
                buttonPressed(buttons[4]);
            }
            else if (parts[1].ToLower() == "blue")
            {
                yield return null;
                buttonPressed(buttons[5]);
            }
        }
        else if (parts.Length == 1 && parts[0] == "start")
        {
            yield return null;
            buttonPressed(buttons[1]);
        }
        else if (parts.Length == 1 && parts[0] == "reset")
        {
            yield return null;
            buttonPressed(buttons[0]);
        }
        else
        {
            yield break;
        }
    }

    void DebugMsg(string msg)
    {
        Debug.LogFormat("[Robot Programming #{0}] {1}", ModuleId, msg);
    }
}
