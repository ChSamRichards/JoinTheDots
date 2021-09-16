using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{

    public int rows = 5;
    public int cols = 4;
    public float cellsize = 1;
    private bool _mouseState;
    public GameObject Target,Target2;
    public Vector2 mousePos;
    public Vector2 endPos,endpos2;
    public Material mat;
    public Vector2 startMousePosition;
    private LineRenderer lineRenderer;
    private int currLines = 0;
    private int lines = 0;
    private float xLimit, yLimit;
    public GameObject[,] arr;
    float upAngle, rightAngle, leftAngle, downAngle;
    public List<GameObject> swipeup = new List<GameObject>();
    public List<GameObject> swipedown = new List<GameObject>();
    public List<GameObject> swipeleft = new List<GameObject>();
    public List<GameObject> swiperight = new List<GameObject>();
    public List<GameObject> finalbox = new List<GameObject>();
    public List<Vector2> setpositions = new List<Vector2>();
    public List<Vector2> setposinitial = new List<Vector2>();
    public GameObject GameOver;
    public int r, c;
    public enum Swipe { None, Up, Down, Left, Right };
    RaycastHit2D rayHit;
    public int flag = 0;

    public List<Vector2> originalpos = new List<Vector2>();
    public TextMesh text00, text01, text02, text10, text11, text12,  text20, text21, text22, text30, text31, text32;
    // Use this for initialization
    void Start()
    {
        arr = new GameObject[5, 4];
        CreateGrid();
    }
    void Update()
    {
        rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {

            

            if (rayHit.collider.CompareTag("circle"))
            {
                Target = rayHit.collider.gameObject;
                CheckGameObject();
                startMousePosition = Target.transform.position;
                if (lineRenderer == null)
                {
                  //  Debug.Log(startMousePosition);

                   if ((startMousePosition.y <= 3f) && (startMousePosition.y >= -3f) && (startMousePosition.x <= 2.3f) && (startMousePosition.x >= -2.3f))
                   {
                        ////Creating line
                        createLine();
                        

                   }

                }
                    lineRenderer.SetPosition(0, startMousePosition);
             
            }
 
        }
        
        if (Input.GetMouseButtonUp(0) && lineRenderer)
        {
           
            if (rayHit.collider.CompareTag("circle"))
            {
               //storing endposition and end target
                Target2 = rayHit.collider.gameObject;
                endpos2 = Target2.transform.position;
                //checking length
                var points = new Vector3[lineRenderer.positionCount]; var count = lineRenderer.GetPositions(points);

                if (count >= 2)
                {
                    var length = 0f;

                    
                    var start = points[0];

                    
                    for (var i = 1; i < count; i++)
                    {
                        // get the current position
                        var end = points[i];
                       
                        length += Vector2.Distance(start, end);
                        
                        start = end;
                    }
                    Debug.Log("length = " + length);
                }



                if ((r < 5 && c < 4) && (r >= 0 && c >= 0))
                {

                    if ((mousePos.y < 3.1f) && (mousePos.y > -3.1f) && (mousePos.x < 2.4f) && (mousePos.x > -2.4f))
                    {

                        endPos = mousePos;

                        if (setposinitial.Contains(arr[r, c].transform.position))
                        {
                            setpositions.Add(arr[r, c].transform.position);
                        }
                        if(setpositions.Contains(endpos2))
                        {
                            setposinitial.Add(endpos2);
                        }
                        //if the direction is upside
                        if (SwipeDirections() == Swipe.Up && swipeup.Contains(Target) == false && swipedown.Contains(arr[r - 1, c]) == false)
                        {
                            lineRenderer.SetPosition(1, new Vector2(Target.transform.position.x, Target.transform.position.y + cellsize));
                            setpositions.Add(new Vector2(Target.transform.position.x, Target.transform.position.y + cellsize));
                            setposinitial.Add(new Vector2(startMousePosition.x, startMousePosition.y));
                            swipeup.Add(Target);
                            if (r > 0)
                            {

                                if (finalbox.Contains(arr[r, c]) == false)
                                    finalbox.Add(arr[r, c]);

                                if (finalbox.Contains(arr[r - 1, c]) == false)
                                    finalbox.Add(arr[r - 1, c]);

                                upAngle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r - 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                                Debug.Log(upAngle);

                                SwipeUpCheck();
                            }


                        }
                        //if direction is downards
                        else if (SwipeDirections() == Swipe.Down && swipedown.Contains(Target) == false && swipeup.Contains(arr[r + 1, c]) == false)
                        {
                            lineRenderer.SetPosition(1, new Vector2(Target.transform.position.x, Target.transform.position.y - cellsize));
                            setpositions.Add(new Vector2(Target.transform.position.x, Target.transform.position.y - cellsize));
                            setposinitial.Add(new Vector2(startMousePosition.x, startMousePosition.y));
                            swipedown.Add(Target);
                            if (r < 4)
                            {

                                if (finalbox.Contains(arr[r, c]) == false)
                                    finalbox.Add(arr[r, c]);


                                if (finalbox.Contains(arr[r + 1, c]) == false)
                                    finalbox.Add(arr[r + 1, c]);

                                downAngle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                                Debug.Log(downAngle);


                                SwipeDownCheck();
                            }


                        }
                        //if direction is leftside
                        else if (SwipeDirections() == Swipe.Left && swipeleft.Contains(Target) == false && swiperight.Contains(arr[r, c - 1]) == false)
                        {
                            lineRenderer.SetPosition(1, new Vector2(Target.transform.position.x - cellsize, Target.transform.position.y));
                            setpositions.Add(new Vector2(Target.transform.position.x - cellsize, Target.transform.position.y));
                            setposinitial.Add(new Vector2(startMousePosition.x, startMousePosition.y));
                            swipeleft.Add(Target);
                            if (c > 0)
                            {


                                if (finalbox.Contains(arr[r, c]) == false)
                                    finalbox.Add(arr[r, c]);

                                if (finalbox.Contains(arr[r, c - 1]) == false)
                                    finalbox.Add(arr[r, c - 1]);

                                leftAngle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c - 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                                Debug.Log(leftAngle);
                                // finalbox.Add(arr[r, c]);
                                SwipeLeftCheck();
                            }


                        }

                        //if direction is rightside
                        else if (SwipeDirections() == Swipe.Right && swiperight.Contains(Target) == false && swipeleft.Contains(arr[r, c + 1]) == false)
                        {
                            lineRenderer.SetPosition(1, new Vector2(Target.transform.position.x + cellsize, Target.transform.position.y));
                            setpositions.Add(new Vector2(Target.transform.position.x + cellsize, Target.transform.position.y));
                            setposinitial.Add(new Vector2(startMousePosition.x, startMousePosition.y));
                            swiperight.Add(Target);
                            if (c < 3)
                            {


                                if (finalbox.Contains(arr[r, c]) == false)
                                    finalbox.Add(arr[r, c]);

                                if (finalbox.Contains(arr[r, c + 1]) == false)
                                    finalbox.Add(arr[r, c + 1]);

                                rightAngle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c + 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);
                                Debug.Log(rightAngle);

                                //  finalbox.Add(arr[r, c]);
                                SwipeRightCheck();
                            }
                        }

                        else
                        {
                            Destroy(GameObject.Find("Line" + currLines));
                        }

                        lineRenderer = null;
                        currLines++;
                    }
                    else
                    {
                        lineRenderer.SetPosition(1, startMousePosition);
                        Destroy(GameObject.Find("Line" + currLines));
                    }
                }
            }
            else
            {
                lineRenderer.SetPosition(1, startMousePosition);
                Destroy(GameObject.Find("Line" + currLines));
            }

        }
        if (Input.GetMouseButton(0) && lineRenderer)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));

        }


        


       

    }
    //checking al adajacent sides if direction is upside
    private void SwipeUpCheck()
    {
        if (c == 3)//last column
        {
            
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r - 1, c]) && finalbox.Contains(arr[r - 1, c - 1]) && finalbox.Contains(arr[r, c - 1]))//leftbox
            {
                if(setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position) && setpositions.Contains(arr[r - 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position) && setposinitial.Contains(arr[r - 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position))
                    {

                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r - 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r - 1, c - 1].transform.position.y, arr[r - 1, c].transform.position.x - arr[r - 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c - 1].transform.position.y - arr[r, c - 1].transform.position.y, arr[r - 1, c - 1].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c - 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);

                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("UpLeft");
                            LoadCheckText();
                           // LoadTextUp(r,c);
                        }
                    }
                }

                
            }

        }
        else if (c == 0)//first column
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r - 1, c]) && finalbox.Contains(arr[r - 1, c + 1]) && finalbox.Contains(arr[r, c + 1]))//Rightbox
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position) && setpositions.Contains(arr[r - 1, c + 1].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position) && setposinitial.Contains(arr[r - 1, c + 1].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position))
                    {
                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r - 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r - 1, c + 1].transform.position.y, arr[r - 1, c].transform.position.x - arr[r - 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c + 1].transform.position.y - arr[r, c + 1].transform.position.y, arr[r - 1, c + 1].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c + 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("UpRight"); LoadCheckText();
                           // LoadTextUp(r,c);
                        }
                    }
                }

            }
        }
        else if ((c != 3) && (c != 0))
        {
            if ((finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r - 1, c]) && finalbox.Contains(arr[r - 1, c + 1]) && finalbox.Contains(arr[r, c + 1])) ||
            (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r - 1, c]) && finalbox.Contains(arr[r - 1, c - 1]) && finalbox.Contains(arr[r, c - 1])))//rightbox and left box
            {
                if ((setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position) && setpositions.Contains(arr[r - 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position)) ||
                    (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position) && setpositions.Contains(arr[r - 1, c + 1].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position)))
                {
                    if ((setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position) && setposinitial.Contains(arr[r - 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position)) ||
                    (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position) && setposinitial.Contains(arr[r - 1, c + 1].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position)))
                    {



                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r - 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r - 1, c - 1].transform.position.y, arr[r - 1, c].transform.position.x - arr[r - 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c - 1].transform.position.y - arr[r, c - 1].transform.position.y, arr[r - 1, c - 1].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c - 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);


                        float upangle1 = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r - 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle1 = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r - 1, c + 1].transform.position.y, arr[r - 1, c].transform.position.x - arr[r - 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle1 = Mathf.Abs(Mathf.Atan2(arr[r - 1, c + 1].transform.position.y - arr[r, c + 1].transform.position.y, arr[r - 1, c + 1].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle1 = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c + 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);


                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("UpRight11111111111111111111111111"); LoadCheckText();
                        }
                        else if (upangle1 + leftangle1 + rightangle1 + downagnle1 == 360)
                        {
                            Debug.Log("UpRight2222222222222222222222222222");  LoadCheckText();
                        }
                    }
                }
            }
        }








    }
    //to display text after box is formed
    private void LoadCheckText()
    {
        if ((setposinitial.Contains(originalpos[0]) && setposinitial.Contains(originalpos[1]) && setposinitial.Contains(originalpos[4]) && setposinitial.Contains(originalpos[5])) &&
            (setpositions.Contains(originalpos[0]) && setpositions.Contains(originalpos[1]) && setpositions.Contains(originalpos[4]) && setpositions.Contains(originalpos[5])))
        {
            text00.text = "1"; flag++;
            //setposinitial.Remove(originalpos[0]); setposinitial.Remove(originalpos[1]); setposinitial.Remove(originalpos[4]); setposinitial.Remove(originalpos[5]);
            //setpositions.Remove(originalpos[0]); setpositions.Remove(originalpos[1]); setpositions.Remove(originalpos[4]); setpositions.Remove(originalpos[5]);
        }
        if ((setposinitial.Contains(originalpos[1]) && setposinitial.Contains(originalpos[2]) && setposinitial.Contains(originalpos[5]) && setposinitial.Contains(originalpos[6])) &&
           (setpositions.Contains(originalpos[1]) && setpositions.Contains(originalpos[2]) && setpositions.Contains(originalpos[5]) && setpositions.Contains(originalpos[6])))
        {
            text01.text = "1"; flag++;

            //setposinitial.Remove(originalpos[1]); setposinitial.Remove(originalpos[2]); setposinitial.Remove(originalpos[5]); setposinitial.Remove(originalpos[6]);
            //setpositions.Remove(originalpos[1]); setpositions.Remove(originalpos[2]); setpositions.Remove(originalpos[5]); setpositions.Remove(originalpos[6]);
        }
        if ((setposinitial.Contains(originalpos[2]) && setposinitial.Contains(originalpos[3]) && setposinitial.Contains(originalpos[6]) && setposinitial.Contains(originalpos[7])) &&
          (setpositions.Contains(originalpos[2]) && setpositions.Contains(originalpos[3]) && setpositions.Contains(originalpos[6]) && setpositions.Contains(originalpos[7])))
        {
            text02.text = "1"; flag++;

            //setposinitial.Remove(originalpos[2]); setposinitial.Remove(originalpos[3]); setposinitial.Remove(originalpos[6]); setposinitial.Remove(originalpos[7]);
            //setpositions.Remove(originalpos[2]); setpositions.Remove(originalpos[3]); setpositions.Remove(originalpos[6]); setpositions.Remove(originalpos[7]);
        }

        if ((setposinitial.Contains(originalpos[4]) && setposinitial.Contains(originalpos[5]) && setposinitial.Contains(originalpos[8]) && setposinitial.Contains(originalpos[9])) &&
           (setpositions.Contains(originalpos[4]) && setpositions.Contains(originalpos[5]) && setpositions.Contains(originalpos[8]) && setpositions.Contains(originalpos[9])))
        {
            text10.text = "1"; flag++;

            //setposinitial.Remove(originalpos[4]); setposinitial.Remove(originalpos[5]); setposinitial.Remove(originalpos[8]); setposinitial.Remove(originalpos[9]);
            //setpositions.Remove(originalpos[4]); setpositions.Remove(originalpos[5]); setpositions.Remove(originalpos[8]); setpositions.Remove(originalpos[9]);
        }

        if ((setposinitial.Contains(originalpos[5]) && setposinitial.Contains(originalpos[6]) && setposinitial.Contains(originalpos[9]) && setposinitial.Contains(originalpos[10])) &&
           (setpositions.Contains(originalpos[5]) && setpositions.Contains(originalpos[6]) && setpositions.Contains(originalpos[9]) && setpositions.Contains(originalpos[10])))
        {
            text11.text = "1"; flag++;

            //setposinitial.Remove(originalpos[5]); setposinitial.Remove(originalpos[6]); setposinitial.Remove(originalpos[9]); setposinitial.Remove(originalpos[10]);
            //setpositions.Remove(originalpos[5]); setpositions.Remove(originalpos[6]); setpositions.Remove(originalpos[9]); setpositions.Remove(originalpos[10]);
        }

        if ((setposinitial.Contains(originalpos[6]) && setposinitial.Contains(originalpos[7]) && setposinitial.Contains(originalpos[10]) && setposinitial.Contains(originalpos[11])) &&
           (setpositions.Contains(originalpos[6]) && setpositions.Contains(originalpos[7]) && setpositions.Contains(originalpos[10]) && setpositions.Contains(originalpos[11])))
        {
            text12.text = "1"; flag++;
            //setposinitial.Remove(originalpos[6]); setposinitial.Remove(originalpos[7]); setposinitial.Remove(originalpos[10]); setposinitial.Remove(originalpos[11]);
            //setpositions.Remove(originalpos[6]); setpositions.Remove(originalpos[7]); setpositions.Remove(originalpos[10]); setpositions.Remove(originalpos[11]);
        }

        if ((setposinitial.Contains(originalpos[8]) && setposinitial.Contains(originalpos[9]) && setposinitial.Contains(originalpos[12]) && setposinitial.Contains(originalpos[13])) &&
           (setpositions.Contains(originalpos[8]) && setpositions.Contains(originalpos[9]) && setpositions.Contains(originalpos[12]) && setpositions.Contains(originalpos[13])))
        {
            text20.text = "1"; flag++;
            //setposinitial.Remove(originalpos[8]); setposinitial.Remove(originalpos[9]); setposinitial.Remove(originalpos[12]); setposinitial.Remove(originalpos[13]);
            //setpositions.Remove(originalpos[8]); setpositions.Remove(originalpos[9]); setpositions.Remove(originalpos[12]); setpositions.Remove(originalpos[13]);

        }

        if ((setposinitial.Contains(originalpos[9]) && setposinitial.Contains(originalpos[10]) && setposinitial.Contains(originalpos[13]) && setposinitial.Contains(originalpos[14])) &&
           (setpositions.Contains(originalpos[9]) && setpositions.Contains(originalpos[10]) && setpositions.Contains(originalpos[13]) && setpositions.Contains(originalpos[14])))
        {
            text21.text = "1"; flag++;

            //setposinitial.Remove(originalpos[9]); setposinitial.Remove(originalpos[10]); setposinitial.Remove(originalpos[13]); setposinitial.Remove(originalpos[14]);
            //setpositions.Remove(originalpos[9]); setpositions.Remove(originalpos[10]); setpositions.Remove(originalpos[13]); setpositions.Remove(originalpos[14]);
        }

        if ((setposinitial.Contains(originalpos[10]) && setposinitial.Contains(originalpos[11]) && setposinitial.Contains(originalpos[14]) && setposinitial.Contains(originalpos[15])) &&
           (setpositions.Contains(originalpos[10]) && setpositions.Contains(originalpos[11]) && setpositions.Contains(originalpos[14]) && setpositions.Contains(originalpos[15])))
        {
            text22.text = "1"; flag++;
            //setposinitial.Remove(originalpos[10]); setposinitial.Remove(originalpos[11]); setposinitial.Remove(originalpos[14]); setposinitial.Remove(originalpos[15]);
            //setpositions.Remove(originalpos[10]); setpositions.Remove(originalpos[11]); setpositions.Remove(originalpos[14]); setpositions.Remove(originalpos[15]);
        }

        if ((setposinitial.Contains(originalpos[12]) && setposinitial.Contains(originalpos[13]) && setposinitial.Contains(originalpos[16]) && setposinitial.Contains(originalpos[17])) &&
           (setpositions.Contains(originalpos[12]) && setpositions.Contains(originalpos[13]) && setpositions.Contains(originalpos[16]) && setpositions.Contains(originalpos[17])))
        {
            text30.text = "1"; flag++;
            //setposinitial.Remove(originalpos[12]); setposinitial.Remove(originalpos[13]); setposinitial.Remove(originalpos[16]); setposinitial.Remove(originalpos[17]);
            //setpositions.Remove(originalpos[12]); setpositions.Remove(originalpos[13]); setpositions.Remove(originalpos[16]); setpositions.Remove(originalpos[17]);
        }

        if ((setposinitial.Contains(originalpos[13]) && setposinitial.Contains(originalpos[14]) && setposinitial.Contains(originalpos[17]) && setposinitial.Contains(originalpos[18])) &&
           (setpositions.Contains(originalpos[13]) && setpositions.Contains(originalpos[14]) && setpositions.Contains(originalpos[17]) && setpositions.Contains(originalpos[18])))
        {
            text31.text = "1"; flag++;
            //setposinitial.Remove(originalpos[13]); setposinitial.Remove(originalpos[14]); setposinitial.Remove(originalpos[17]); setposinitial.Remove(originalpos[18]);
            //setpositions.Remove(originalpos[13]); setpositions.Remove(originalpos[14]); setpositions.Remove(originalpos[17]); setpositions.Remove(originalpos[18]);
        }

        if ((setposinitial.Contains(originalpos[14]) && setposinitial.Contains(originalpos[15]) && setposinitial.Contains(originalpos[18]) && setposinitial.Contains(originalpos[19])) &&
           (setpositions.Contains(originalpos[14]) && setpositions.Contains(originalpos[15]) && setpositions.Contains(originalpos[18]) && setpositions.Contains(originalpos[19])))
        {
            text32.text = "1"; flag++;
            //setposinitial.Remove(originalpos[14]); setposinitial.Remove(originalpos[15]); setposinitial.Remove(originalpos[18]); setposinitial.Remove(originalpos[19]);
            //setpositions.Remove(originalpos[14]); setpositions.Remove(originalpos[15]); setpositions.Remove(originalpos[18]); setpositions.Remove(originalpos[19]);
        }







    }



    //to check all adjacent sides if swiped down

    private void SwipeDownCheck()
    {
        if (c == 3)//last column
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c - 1]) && finalbox.Contains(arr[r, c - 1]))//leftbox
            {
                if   (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position))
                    {


                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c - 1].transform.position.y - arr[r, c - 1].transform.position.y, arr[r + 1, c - 1].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r + 1, c - 1].transform.position.y, arr[r + 1, c].transform.position.x - arr[r + 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c - 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Downright"); LoadCheckText();
                        }
                    }
                }

            }

        }
        else if (c == 0)//first column
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c + 1]) && finalbox.Contains(arr[r, c + 1]))//Rightbox
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c + 1].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position))
                  
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c + 1].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position))

                    {

                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c + 1].transform.position.y - arr[r, c + 1].transform.position.y, arr[r + 1, c + 1].transform.position.x - arr[r, c - +1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c + 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r + 1, c + 1].transform.position.y, arr[r + 1, c].transform.position.x - arr[r + 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Downright2"); LoadCheckText();
                        }
                    }
                }

            }
        }
        else if (c != 3 && c != 0)
        {
            if ((finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c + 1]) && finalbox.Contains(arr[r, c + 1])) ||
            (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c - 1]) && finalbox.Contains(arr[r, c - 1])))//rightbox and left box
            {
                if ((setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c + 1].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position)) ||
                   (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position)))
                {
                    if ((setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c + 1].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position)) ||
                  (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position)))
                    {


                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c - 1].transform.position.y - arr[r, c - 1].transform.position.y, arr[r + 1, c - 1].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r + 1, c - 1].transform.position.y, arr[r + 1, c].transform.position.x - arr[r + 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c - 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);


                        float upangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c + 1].transform.position.y - arr[r, c + 1].transform.position.y, arr[r + 1, c + 1].transform.position.x - arr[r, c - +1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle1 = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r, c].transform.position.y, arr[r, c + 1].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle1 = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r + 1, c + 1].transform.position.y, arr[r + 1, c].transform.position.x - arr[r + 1, c + 1].transform.position.x) * 180 / Mathf.PI);

                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Downright111111111111111111111111111111111"); LoadCheckText();
                        }
                        else if (upangle1 + leftangle1 + rightangle1 + downagnle1 == 360)
                        {
                            Debug.Log("Downright22222222222222222222222222222"); LoadCheckText();
                        }
                    }
                }

            }
        }


    }
    //to check all adjacent sides if swiped left
    private void SwipeLeftCheck()
    {

        if (r == 4)//last row
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c - 1]) && finalbox.Contains(arr[r - 1, c - 1]) && finalbox.Contains(arr[r - 1, c]))//leftup
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position) && setpositions.Contains(arr[r - 1, c - 1].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position) && setposinitial.Contains(arr[r - 1, c - 1].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position))
                    {

                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r - 1, c - 1].transform.position.y, arr[r, c - 1].transform.position.x - arr[r - 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c - 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c - 1].transform.position.y - arr[r - 1, c].transform.position.y, arr[r - 1, c - 1].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Leftup1"); LoadCheckText();
                        }
                    }
                }

            }

        }
        else if (r == 0)//first row
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c - 1]) && finalbox.Contains(arr[r, c - 1]))//leftdown
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position))
                    {

                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r + 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c - 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r + 1, c - 1].transform.position.y, arr[r, c - 1].transform.position.x - arr[r + 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c - 1].transform.position.y - arr[r + 1, c].transform.position.y, arr[r + 1, c - 1].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Leftup2"); LoadCheckText();
                        }
                    }
                }

            }
        }
        else if (r != 4 && r != 0)
        {
            if ((finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c - 1]) && finalbox.Contains(arr[r - 1, c - 1]) && finalbox.Contains(arr[r - 1, c])) ||
                (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r + 1, c]) && finalbox.Contains(arr[r + 1, c - 1]) && finalbox.Contains(arr[r, c - 1])))//upanddownbox\
            {
                if ((setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position) && setpositions.Contains(arr[r - 1, c - 1].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position)) ||
                   (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position) && setpositions.Contains(arr[r + 1, c - 1].transform.position) && setpositions.Contains(arr[r, c - 1].transform.position)))
                {
                    if ((setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position) && setposinitial.Contains(arr[r - 1, c - 1].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position)) ||
                   (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position) && setposinitial.Contains(arr[r + 1, c - 1].transform.position) && setposinitial.Contains(arr[r, c - 1].transform.position)))
                    {


                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r - 1, c - 1].transform.position.y, arr[r, c - 1].transform.position.x - arr[r - 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c - 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r + 1, c].transform.position.y, arr[r, c].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c - 1].transform.position.y - arr[r - 1, c].transform.position.y, arr[r - 1, c - 1].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);


                        float upangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r + 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle1 = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c - 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float downagnle1 = Mathf.Abs(Mathf.Atan2(arr[r, c - 1].transform.position.y - arr[r + 1, c - 1].transform.position.y, arr[r, c - 1].transform.position.x - arr[r + 1, c - 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c - 1].transform.position.y - arr[r + 1, c].transform.position.y, arr[r + 1, c - 1].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);



                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Lefttttttttttttttttttttttttttttt11111111111"); LoadCheckText();
                        }
                        else if (upangle1 + leftangle1 + rightangle1 + downagnle1 == 360)
                        {
                            Debug.Log("Lefttttttttttttttttttttttttttttt22222222222"); LoadCheckText();
                        }
                    }
                }
            }
        }

    }
    // to check al adjacent sides if swiped right
    private void SwipeRightCheck()
    {
        if (r == 4)//last row
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c + 1]) && finalbox.Contains(arr[r - 1, c + 1]) && finalbox.Contains(arr[r - 1, c]))//Rightup
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position) && setpositions.Contains(arr[r - 1, c + 1].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position))

                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position) && setposinitial.Contains(arr[r - 1, c + 1].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position))

                    {


                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r - 1, c + 1].transform.position.y, arr[r, c + 1].transform.position.x - arr[r - 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c + 1].transform.position.y - arr[r - 1, c].transform.position.y, arr[r - 1, c + 1].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r - 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c + 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {

                            Debug.Log("Rightup1"); LoadCheckText();
                        }
                    }
                }

            }

        }
        else if (r == 0)//first row
        {
            if (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c + 1]) && finalbox.Contains(arr[r + 1, c + 1]) && finalbox.Contains(arr[r + 1, c]))//Rightdown
            {
                if (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position) && setpositions.Contains(arr[r + 1, c + 1].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position))
                {
                    if (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position) && setposinitial.Contains(arr[r + 1, c + 1].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position))
                    {

                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r + 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r + 1, c + 1].transform.position.y - arr[r + 1, c].transform.position.y, arr[r + 1, c + 1].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r + 1, c + 1].transform.position.y, arr[r, c + 1].transform.position.x - arr[r + 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c + 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);
                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("rightup2"); LoadCheckText();
                        }

                    }


                }
            }
        }
        else if (r != 4 && r != 0)
        {
            if ((finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c + 1]) && finalbox.Contains(arr[r - 1, c + 1]) && finalbox.Contains(arr[r - 1, c])) ||
            (finalbox.Contains(arr[r, c]) && finalbox.Contains(arr[r, c + 1]) && finalbox.Contains(arr[r + 1, c + 1]) && finalbox.Contains(arr[r + 1, c]))) //upanddownbox
            {
                if ((setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position) && setpositions.Contains(arr[r - 1, c + 1].transform.position) && setpositions.Contains(arr[r - 1, c].transform.position)) ||
                  (setpositions.Contains(arr[r, c].transform.position) && setpositions.Contains(arr[r, c + 1].transform.position) && setpositions.Contains(arr[r + 1, c + 1].transform.position) && setpositions.Contains(arr[r + 1, c].transform.position)))
                {
                    if ((setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position) && setposinitial.Contains(arr[r - 1, c + 1].transform.position) && setposinitial.Contains(arr[r - 1, c].transform.position)) ||
                       (setposinitial.Contains(arr[r, c].transform.position) && setposinitial.Contains(arr[r, c + 1].transform.position) && setposinitial.Contains(arr[r + 1, c + 1].transform.position) && setposinitial.Contains(arr[r + 1, c].transform.position)))
                    {




                        float upangle = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r - 1, c + 1].transform.position.y, arr[r, c + 1].transform.position.x - arr[r - 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float leftangle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c + 1].transform.position.y - arr[r - 1, c].transform.position.y, arr[r - 1, c + 1].transform.position.x - arr[r - 1, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle = Mathf.Abs(Mathf.Atan2(arr[r - 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r - 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float rightangle = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c + 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);

                        float upangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c].transform.position.y - arr[r, c].transform.position.y, arr[r + 1, c].transform.position.x - arr[r, c].transform.position.x) * 180 / Mathf.PI);
                        float leftangle1 = Mathf.Abs(Mathf.Atan2(arr[r + 1, c + 1].transform.position.y - arr[r + 1, c].transform.position.y, arr[r + 1, c + 1].transform.position.x - arr[r + 1, c].transform.position.x) * 180 / Mathf.PI);
                        float downagnle1 = Mathf.Abs(Mathf.Atan2(arr[r, c + 1].transform.position.y - arr[r + 1, c + 1].transform.position.y, arr[r, c + 1].transform.position.x - arr[r + 1, c + 1].transform.position.x) * 180 / Mathf.PI);
                        float rightangle1 = Mathf.Abs(Mathf.Atan2(arr[r, c].transform.position.y - arr[r, c + 1].transform.position.y, arr[r, c].transform.position.x - arr[r, c + 1].transform.position.x) * 180 / Mathf.PI);


                        if (upangle + leftangle + downagnle + rightangle == 360)
                        {
                            Debug.Log("Lefttttttttttttttttttttttttttttt11111111111"); LoadCheckText();
                        }
                        else if (upangle1 + leftangle1 + rightangle1 + downagnle1 == 360)
                        {
                            Debug.Log("Lefttttttttttttttttttttttttttttt22222222222"); LoadCheckText();
                        }
                    }

                }

            }
        }




    }

    // r and c are postioins of game objects
    private void CheckGameObject()
    {
       
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                if(arr[i,j] == Target)
                {
                    r = i;
                    c = j;
                    Debug.Log(r + ", " + c);
                }
            }
        }
    }
    //creating line
    private void createLine()
    {
        lineRenderer = new GameObject("Line" + currLines).AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.material = mat;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.numCapVertices = 50;
    }
    // to check which direction did we sipe
    private Swipe SwipeDirections()
    {
        Swipe direction = Swipe.None;
        Vector2 currentSwipe = endPos - startMousePosition;
        float angle = (Mathf.Atan2(currentSwipe.y, currentSwipe.x) * (180 / Mathf.PI));

        if (angle > 45f && angle < 135f)
        {
            
            direction = Swipe.Up;
        }
        else if (angle < -45f && angle > -135f)
        {
            direction = Swipe.Down;
        }
        else if (angle < -135f || angle > 135f)
        {
            direction = Swipe.Left;
        }
        else if (angle > -45f && angle < 45f)
        {
            direction = Swipe.Right;
        }
        else
        {
            direction = Swipe.None;
        }
        

        

        return direction;
       
    }

    //creating grid of rows and columns
    private void CreateGrid()
    {
        GameObject Rcircle = (GameObject)Instantiate(Resources.Load("Circle"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
              
                GameObject circle = Instantiate(Rcircle, transform);
                
                float Xpos = col * cellsize;
                float Ypos = row * (-cellsize);
                circle.transform.position = new Vector2(Xpos, Ypos);
                arr[row, col] = circle;
               

            }

        }
        
        Destroy(Rcircle);
        float width = cols * cellsize;
        float height = rows * cellsize;
        transform.position = new Vector2(-width / 2 + cellsize / 2, height / 2 - cellsize / 2);
        for (int i = 0; i < 20; i++)
        {
            originalpos[i] = new Vector2(this.gameObject.transform.GetChild(i).gameObject.transform.position.x, this.gameObject.transform.GetChild(i).gameObject.transform.position.y);
                
        }










    }
    public void BackGame()
    {
        GameOver.SetActive(true);
    }
    public void Replay()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }



}
