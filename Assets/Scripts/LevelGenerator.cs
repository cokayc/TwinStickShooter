using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float minSplitArea;
    public float minRoomArea;
    public float hallwayOffset;
    public int roomShrink;
    public GameObject wall;

    private List<Node> leafList;
    private int[,] map;

    private class Node
    {
        public Vector2 bottomLeft;
        public Vector2 topRight;

        public Vector2 roomBottomLeft;
        public Vector2 roomTopRight;

        public Node left;
        public Node right;

        public List<GameObject> walls;

        public Node(float blX, float blY, float trX, float trY)
        {
            walls = new List<GameObject>();

            bottomLeft = new Vector2(blX, blY);
            topRight = new Vector2(trX, trY);

            left = null;
            right = null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        leafList = new List<Node>();
        map = new int[width, height];

        Node head = new Node(0, 0, width, height);
        SplitGenerate(head);

        //TODO: Tidy up (non repeating code, better var names)
        foreach (Node leaf in leafList)
        {
            // Generate two points of the room in the split section
            //TODO: Generate in a smarter way (keeps generating until a room matches the area)
            float area = 0;
            Vector3[] points = new Vector3[4];

            /*
            //TODO: Random generation of coordinates
            int count = 0;
            while (area < minRoomArea)
            {
                if (count >= 100)
                {
                    points[0] = new Vector2(leaf.bottomLeft.x, leaf.bottomLeft.y);
                    points[1] = new Vector2(leaf.topRight.x, leaf.topRight.y);
                    break;
                }
                
                points[0] = new Vector2(Mathf.Lerp(leaf.bottomLeft.x, leaf.topRight.x, Random.Range(0f, 1f)), Mathf.Lerp(leaf.bottomLeft.y, leaf.topRight.y, Random.Range(0f, 1f)));
                points[1] = new Vector2(Mathf.Lerp(leaf.bottomLeft.x, leaf.topRight.x, Random.Range(0f, 1f)), Mathf.Lerp(leaf.bottomLeft.y, leaf.topRight.y, Random.Range(0f, 1f)));

                area = Mathf.Abs(points[0].x - points[1].x) * Mathf.Abs(points[0].y - points[1].y);
                count++;
            }
            */

            // Shrink down room coordinates to not fully fill the partition
            points[0] = new Vector2(leaf.bottomLeft.x + roomShrink, leaf.bottomLeft.y + roomShrink);
            points[1] = new Vector2(leaf.topRight.x - roomShrink, leaf.topRight.y - roomShrink);

            points[2] = new Vector2(points[1].x, points[0].y);
            points[3] = new Vector2(points[0].x, points[1].y);

            for (int x = (int)leaf.bottomLeft.x + roomShrink; x < leaf.topRight.x - roomShrink; x++)
            {
                for (int y = (int)leaf.bottomLeft.y + roomShrink; y < leaf.topRight.y - roomShrink; y++)
                {
                    map[x, y] = 1;
                }
            }

            /*
            // Build walls
            leaf.walls.Add(BuildWall2D(points[0], points[2]));
            leaf.walls.Add(BuildWall2D(points[2], points[1]));
            leaf.walls.Add(BuildWall2D(points[1], points[3]));
            leaf.walls.Add(BuildWall2D(points[3], points[0]));
            */

            float[] xvals = new float[4];
            float[] yvals = new float[4];

            for (int i = 0; i < points.Length; i++)
            {
                xvals[i] = points[i].x;
                yvals[i] = points[i].y;
            }


            leaf.roomBottomLeft = new Vector2(Mathf.Min(xvals), Mathf.Min(yvals));
            leaf.roomTopRight = new Vector2(Mathf.Max(xvals), Mathf.Max(yvals));

            // Testing for correct splitting
            //Instantiate(wall, leaf.bottomLeft, Quaternion.identity);
            //Instantiate(wall, leaf.topRight, Quaternion.identity);
            //Debug.Log("Left: " + leaf.bottomLeft.x + ", " + leaf.bottomLeft.y + " Right: " + leaf.topRight.x + ", " + leaf.topRight.y);
        }

        ConnectRooms(head, map);

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (IsWall(map, x, y))
                {
                    BuildWallMap(x, y);
                }
            }
        }

        
    }

    private bool IsWall(int[,] map, int x, int y)
    { 
        if (map[x, y] == 0 && ((x - 1 >= 0 && map[x - 1, y] == 1) || (y + 1 < map.GetUpperBound(1) && map[x, y + 1] == 1) || (x + 1 < map.GetUpperBound(0) && map[x + 1, y] == 1) || (y - 1 >= 0 && map[x, y - 1] == 1)))
            return true;
        else
            return false;

    }

    private void BuildWallMap(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        Instantiate(wall, pos, Quaternion.identity);
    }

    private void BuildWall(Vector3 p1, Vector3 p2)
    {
        Vector3 between = p2 - p1;
        float distance = between.magnitude;
        Vector3 midPoint = p1 + (between / 2);

        var obj = Instantiate(wall, midPoint, Quaternion.identity);
        obj.transform.localScale = new Vector3(1, 1, distance);
        obj.transform.LookAt(p2);
    }

    private GameObject BuildWall2D(Vector3 p1, Vector3 p2)
    {
        Vector3 between = p2 - p1;
        float distance = between.magnitude;
        Vector3 midPoint = p1 + (between / 2);

        var obj = Instantiate(wall, midPoint, Quaternion.identity);
        obj.transform.localScale = new Vector3(1, distance, 1);
        float angle = Mathf.Atan2(between.y, between.x) * Mathf.Rad2Deg + 90;
        obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        return obj;
    }

    private void SplitGenerate(Node target)
    {
        // Base Case
        float width = target.topRight.x - target.bottomLeft.x;
        float height = target.topRight.y - target.bottomLeft.y;
        if (width * height < minSplitArea)
        {
            leafList.Add(target);
            return;
        }

        if (Random.Range(0 , 2) == 0)
        {
            // Vertical
            int cutPointX = (int) Mathf.Lerp(target.bottomLeft.x, target.topRight.x, Random.Range(.25f, .75f));
            target.left = new Node(target.bottomLeft.x, target.bottomLeft.y, cutPointX, target.topRight.y);
            target.right = new Node(cutPointX, target.bottomLeft.y, target.topRight.x, target.topRight.y);

        }
        else
        {
            // Horizontal
            int cutPointY = (int) Mathf.Lerp(target.bottomLeft.y, target.topRight.y, Random.Range(.25f, .75f));
            target.left = new Node(target.bottomLeft.x, cutPointY, target.topRight.x, target.topRight.y);
            target.right = new Node(target.bottomLeft.x, target.bottomLeft.y, target.topRight.x, cutPointY);
        }

        SplitGenerate(target.left);
        SplitGenerate(target.right);
    }

    private void ConnectRooms(Node target, int[,] map)
    {
        if (target.left == null || target.right == null)
        {
            return;
        }

        ConnectRooms(target.left, map);
        ConnectRooms(target.right, map);
        Debug.Log(target.left.walls.Count);
        Debug.Log(target.right.walls.Count);

        Vector2 midpointLeft = new Vector2( (int) ((target.left.bottomLeft.x + target.left.topRight.x) / 2), (int) ((target.left.bottomLeft.y + target.left.topRight.y) / 2));
        Vector2 midpointRight = new Vector2( (int) ((target.right.bottomLeft.x + target.right.topRight.x) / 2), (int) ((target.right.bottomLeft.y + target.right.topRight.y) / 2));

        while (midpointLeft != midpointRight)
        {
            if (midpointLeft.x < midpointRight.x)
            {
                midpointLeft.x += 1;
            }
            else if (midpointLeft.x > midpointRight.x)
            {
                midpointLeft.x -= 1;
            }
            else if (midpointLeft.y < midpointRight.y)
            {
                midpointLeft.y += 1;
            }
            else if (midpointLeft.y > midpointRight.y)
            {
                midpointLeft.y -= 1;
            }

            map[(int) midpointLeft.x, (int) midpointLeft.y] = 1;
        }

        //Instantiate(wall, midpointLeft, Quaternion.identity);
        //Instantiate(wall, midpointRight, Quaternion.identity).GetComponent<SpriteRenderer>().color = Color.blue;


        /*
        // Find midpoints of both rooms
        Vector2 midpointLeft = new Vector2((target.left.bottomLeft.x + target.left.topRight.x) / 2, (target.left.bottomLeft.y + target.left.topRight.y) / 2);
        Vector2 midpointRight = new Vector2((target.right.bottomLeft.x + target.right.topRight.x) / 2, (target.right.bottomLeft.y + target.right.topRight.y) / 2);
        Vector2 direction = midpointRight - midpointLeft;
        direction.Normalize();

        
        RaycastHit2D[] hits = Physics2D.RaycastAll(midpointLeft, direction, Mathf.Infinity, LayerMask.GetMask("Wall"));
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        Debug.Log(hits.Length);
        int wallIndex = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(target.right.walls.Contains(hits[i].collider.gameObject));
            Debug.Log(hits[i].collider.transform.position);
            if (target.right.walls.Contains(hits[i].collider.gameObject))
            {
                wallIndex = i;
                break;
            }
        }
        Debug.Log(wallIndex);

        Instantiate(wall, midpointLeft, Quaternion.identity);
        Instantiate(wall, midpointRight, Quaternion.identity).GetComponent<SpriteRenderer>().color = Color.blue;
        var newWall = BuildWall2D(hits[wallIndex - 1].transform.position, hits[wallIndex].transform.position);
        //BuildWall2D(midpointLeft, midpointRight);
        target.walls.Add(newWall);
        target.walls.AddRange(target.left.walls);
        target.walls.AddRange(target.right.walls);
        */




        /*
        foreach (GameObject wall in target.left.walls) 
        {
            RaycastHit2D[] hits = new RaycastHit2D[4];
            hits[0] = Physics2D.Raycast(wall.transform.position, Vector2.up);
            hits[1] = Physics2D.Raycast(wall.transform.position, Vector2.right);
            hits[2] = Physics2D.Raycast(wall.transform.position, Vector2.down);
            hits[3] = Physics2D.Raycast(wall.transform.position, Vector2.left);

            for (int i = 0; i < 4; i ++)
            {
                if (hits[i].collider != null && target.right.walls.Contains(hits[i].collider.gameObject))
                {
                    Debug.Log("success");
                    var newWall = BuildWall2D(wall.transform.position, hits[i].point);
                    target.walls.Add(newWall);
                    target.walls.AddRange(target.left.walls);
                    target.walls.AddRange(target.right.walls);
                    return;
                }
            }
        }
        
        target.walls.AddRange(target.left.walls);
        target.walls.AddRange(target.right.walls);
        */

        /*
        // Search for a place where a wall can connect to rooms
        for (float i = target.left.roomBottomLeft.x + hallwayOffset; i < target.left.roomTopRight.x; i += .5f)
        {
            Vector2 top = new Vector2(i, target.left.roomTopRight.y);
            RaycastHit2D hitUp = Physics2D.Raycast(top, Vector2.up);
            Vector2 bottom = new Vector2(i, target.left.roomBottomLeft.y);
            RaycastHit2D hitDown = Physics2D.Raycast(bottom, Vector2.down);

            if (hitUp.collider != null)
            {
                BuildWall2D(top, new Vector2(i, hitUp.collider.transform.position.y));
                target.roomBottomLeft = target.left.roomBottomLeft;
                target.roomTopRight = target.right.roomTopRight;
                return;
            }
            else if (hitDown.collider != null)
            {
                BuildWall2D(bottom, new Vector2(i, hitDown.collider.transform.position.y));
                target.roomBottomLeft = target.right.roomBottomLeft;
                target.roomTopRight = target.left.roomTopRight;
                return;
            }
        }

        for (float i = target.left.roomBottomLeft.y + hallwayOffset; i < target.left.roomTopRight.y; i += .5f)
        {
            Vector2 left = new Vector2(target.left.roomBottomLeft.x, i);
            RaycastHit2D hitLeft = Physics2D.Raycast(left, Vector2.up);
            Vector2 right = new Vector2(target.left.roomTopRight.x, i);
            RaycastHit2D hitRight = Physics2D.Raycast(right, Vector2.down);

            if (hitLeft.collider != null)
            {
                BuildWall2D(left, new Vector2(i, hitLeft.collider.transform.position.y));
                target.roomBottomLeft = target.right.roomBottomLeft;
                target.roomTopRight = target.left.roomTopRight;
                return;
            }
            else if (hitRight.collider != null)
            {
                BuildWall2D(right, new Vector2(i, hitRight.collider.transform.position.y));
                target.roomBottomLeft = target.left.roomBottomLeft;
                target.roomTopRight = target.right.roomTopRight;
                return;
            }
        }
        */

        Debug.Log("Failed to find connection");
    }
}
