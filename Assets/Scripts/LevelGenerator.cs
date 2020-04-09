using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width;
    public int height;
    [Tooltip("Minimum area for BSP.")]
    public float minSplitArea;
    [Tooltip("Smallest prorportion of width/height that will be used for splitting.")]
    public float cutMin;
    [Tooltip("Largest prorportion of width/height that will be used for splitting.")]
    public float cutMax;
    [Tooltip("Int value that shifts the rooms inward from their respective partition.")]
    public int roomShrink;
    [Tooltip("Scale value applied to walls")]
    public float wallScale;
    public GameObject camera;
    public GameObject wall;
    public GameObject floor;
    public GameObject exit;
    [Tooltip("Enemy array for random enemy generations.")]
    public GameObject[] enemies;
    public int enemySpawnChance;
    

    private GameObject player;
    private List<Node> leafList;
    private int[,] map;

    // Node class for use in BSP tree
    private class Node
    {
        // Coordinates of the two corners of the space
        public Vector2 bottomLeft;
        public Vector2 topRight;

        // Left and right nodes
        public Node left;
        public Node right;

        public Node(float blX, float blY, float trX, float trY)
        {
            bottomLeft = new Vector2(blX, blY);
            topRight = new Vector2(trX, trY);

            left = null;
            right = null;
        }

        // Returns the midpoint of this node (based on bottomLeft and topRight)
        public Vector2 GetMidpoint()
        {
            return new Vector2((int)((bottomLeft.x + topRight.x) / 2), (int)((bottomLeft.y + topRight.y) / 2));
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        leafList = new List<Node>();
        map = new int[width, height];
        player = PlayerController.instance.currentEnemy.gameObject;

        Node head = new Node(0, 0, width, height);
        SplitGenerate(head);

        

        foreach (Node leaf in leafList)
        {
            Vector3[] points = new Vector3[4];

            // Shrink down room coordinates to not fully fill the partition
            points[0] = new Vector2(leaf.bottomLeft.x + roomShrink, leaf.bottomLeft.y + roomShrink);
            points[1] = new Vector2(leaf.topRight.x - roomShrink, leaf.topRight.y - roomShrink);

            points[2] = new Vector2(points[1].x, points[0].y);
            points[3] = new Vector2(points[0].x, points[1].y);

            // Adjust map to correctly represent rooms
            for (int x = (int)leaf.bottomLeft.x + roomShrink; x < leaf.topRight.x - roomShrink; x++)
            {
                for (int y = (int)leaf.bottomLeft.y + roomShrink; y < leaf.topRight.y - roomShrink; y++)
                {
                    map[x, y] = 1;
                }
            }
        }

        Node firstRoom = leafList[0];
        for (int x = (int)firstRoom.bottomLeft.x + roomShrink; x < firstRoom.topRight.x - roomShrink; x++)
        {
            for (int y = (int)firstRoom.bottomLeft.y + roomShrink; y < firstRoom.topRight.y - roomShrink; y++)
            {
                map[x, y] = 3;
            }
        }



        // Place player and exit
        player.transform.position = leafList[0].GetMidpoint() * wallScale;
        camera.transform.position = leafList[0].GetMidpoint() * wallScale;
        Instantiate(exit, leafList[leafList.Count - 1].GetMidpoint() * wallScale, Quaternion.identity);



        ConnectRooms(head, map);

        // Build walls
        for (int x = 1; x < map.GetUpperBound(0); x++)
        {
            for (int y = 1; y < map.GetUpperBound(1); y++)
            { 
                if (IsWall(map, x, y))
                {
                    // Build Wall
                    Vector2 pos = new Vector2(x * wallScale, y * wallScale);
                    Vector3 scale = new Vector3(wallScale, wallScale);
                    Instantiate(wall, pos, Quaternion.identity).transform.localScale = scale;
                }
                else if (IsSurroundedBy(map, x, y, 1))
                {
                    // Place Enemy
                    if (Random.Range(0, 100) < enemySpawnChance)
                    {
                        map[x, y] = 2;
                        Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(x * wallScale, y * wallScale), Quaternion.identity);
                    }
                }

                if (map[x,y] > 0)
                {
                    var newFloor = Instantiate(floor, new Vector2(x, y) * wallScale, Quaternion.identity);
                    newFloor.transform.localScale = newFloor.transform.localScale * wallScale;
                }
                
            }
        }
    }

    // Checks if x and y are within the bounds of map
    private bool IsValidCoordinate(int[,] map, int x, int y)
    {
        if (x - 1 < 0 || x + 1 >= map.GetUpperBound(0) || y - 1 < 0 || y + 1 >= map.GetUpperBound(1))
            return false;
        else
            return true;
    }

    // Test if map[x, y] == check and is within bounds of map
    private bool IsEqual(int[,] map, int x, int y, int check)
    {   
        // Check equality
        if (IsValidCoordinate(map, x, y) && map[x, y] == check)
            return true;
        else
            return false;
    }

    // Returns whether or not a respective (x, y) coordinate on map should be a wall
    private bool IsWall(int[,] map, int x, int y)
    { 
        if (map[x, y] == 0 && (!IsEqual(map, x - 1, y, 0) || !IsEqual(map, x - 1, y + 1, 0) || !IsEqual(map, x, y + 1, 0) || !IsEqual(map, x + 1, y + 1, 0) 
            || !IsEqual(map, x + 1, y, 0) || !IsEqual(map, x + 1, y - 1, 0) || !IsEqual(map, x, y - 1, 0) || !IsEqual(map, x - 1, y - 1, 0)))
            return true;
        else
            return false;
    }

    // Returns true if (x, y) is surrounded by check, else false
    private bool IsSurroundedBy(int[,] map, int x, int y, int check)
    {
        if (map[x, y] == 1 && map[x - 1, y] == check && map[x - 1, y + 1] == check && map[x, y + 1] == check && map[x + 1, y + 1] == check && map[x + 1, y] == check 
            && map[x + 1, y - 1] == check && map[x, y - 1] == check && map[x - 1, y - 1] == check)
            return true;
        else
            return false;
    }

    // Sets adjacent index of map[x, y] to target
    private void FillAdjacent(int[,] map, int x, int y, int target)
    {
        int temp = map[x, y];
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y -1; j <= y + 1; j++)
            {
                map[i, j] = target;
            }
        }

        map[x, y] = temp;
    }

    // Generates the BSP tree
    private void SplitGenerate(Node target)
    {
        // Base Case (check if area is above the minimum to continue splitting
        float width = target.topRight.x - target.bottomLeft.x;
        float height = target.topRight.y - target.bottomLeft.y;
        if (width * height < minSplitArea)
        {
            leafList.Add(target);
            return;
        }

        // Too narrow in width
        if (width > 2.5 * height)
        {
            CutVertical(target);
        }
        // Too narrow in height
        else if (height > 2.5 * width)
        {
            CutHorizontal(target);
        }
        // Random split
        else
        {
            if (Random.Range(0, 2) == 0)
                CutVertical(target);
            else
                CutHorizontal(target);
        }
        

        // Recursive case
        SplitGenerate(target.left);
        SplitGenerate(target.right);
    }

    // Cut a Node with a vertical line
    private void CutVertical(Node target)
    {
        int cutPointX = (int)Mathf.Lerp(target.bottomLeft.x, target.topRight.x, Random.Range(cutMin, cutMax));
        target.left = new Node(target.bottomLeft.x, target.bottomLeft.y, cutPointX, target.topRight.y);
        target.right = new Node(cutPointX, target.bottomLeft.y, target.topRight.x, target.topRight.y);
    }

    // Cut a Node with a horizontal line
    private void CutHorizontal(Node target)
    {
        int cutPointY = (int)Mathf.Lerp(target.bottomLeft.y, target.topRight.y, Random.Range(cutMin, cutMax));
        target.left = new Node(target.bottomLeft.x, cutPointY, target.topRight.x, target.topRight.y);
        target.right = new Node(target.bottomLeft.x, target.bottomLeft.y, target.topRight.x, cutPointY);
    }

    // Connects all rooms on the map
    private void ConnectRooms(Node target, int[,] map)
    {
        // Base case
        if (target.left == null || target.right == null)
        {
            return;
        }

        // Recursive case
        ConnectRooms(target.left, map);
        ConnectRooms(target.right, map);

        // Get midpoints and attempt to connect them
        Vector2 midpointLeft = target.left.GetMidpoint();
        Vector2 midpointRight = target.right.GetMidpoint();

        while (midpointLeft != midpointRight)
        {
            if (midpointLeft.x < midpointRight.x)
            {
                midpointLeft.x += 1;
                map[(int) midpointLeft.x, (int) midpointLeft.y - 1] = 1;
            }
            else if (midpointLeft.x > midpointRight.x)
            {
                midpointLeft.x -= 1;
                map[(int) midpointLeft.x, (int) midpointLeft.y - 1] = 1;
            }
            else if (midpointLeft.y < midpointRight.y)
            {
                midpointLeft.y += 1;
                map[(int)midpointLeft.x - 1, (int)midpointLeft.y] = 1;
            }
            else if (midpointLeft.y > midpointRight.y)
            {
                midpointLeft.y -= 1;
                map[(int) midpointLeft.x - 1, (int) midpointLeft.y] = 1;
            }

            map[(int) midpointLeft.x, (int) midpointLeft.y] = 1;
        }
    }
}
