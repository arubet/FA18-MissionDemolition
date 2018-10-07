using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Singleton

    //fields set in the Unity Inspector pane
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    //This is a property (that is, a method masquerading as a field)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //When _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - LastPoint).magnitude < minDist)
        {
            return;
        }
        if (points.Count == 0)
        {
            //If this is the launch point...
            Vector3 launchPos = pt - Slingshot.LAUNCH_POS;
            Vector3 launchPosDiff = pt - launchPos;
            // ... it adds an extra it of line to aid aiming later
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            //Normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1,LastPoint);
            line.enabled = true;
        }
    }

    //Returns the location of the most recently added point
    public Vector3 LastPoint
    {
        get
        {
            if (points == null)
            {
                //if there are no points, returns Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            //if there is no poi,search fo one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                } else {
                    return; // Return if we did not find a poi
                }
            }else{ 
                return;
            }
        }
            // If there is a poi,it's loc is added every FixedUpdate
            AddPoint();
            if (FollowCam.POI == null)
            {
                poi = null;
            }
        }
    }
