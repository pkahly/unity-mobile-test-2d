using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    public MazeSpec mazeSpec;

    private WorldSpace[,] world;
    private SpriteRenderer spriteRenderer;
    private WorldGenerator worldGenerator;

    private Position lastMazeClickPos;
    private bool isGameOver = false;

    void Start()
    {
        // Generate the maze and create world array
        worldGenerator = new WorldGenerator();
        world = worldGenerator.GenerateWorld(mazeSpec);

        // Create a Sprite Renderer
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;

        DrawMaze();

        ResizeCamera(worldGenerator.GetZLength());
    }

    void Update()
    {
        // No updates after winning
        if (isGameOver)
        {
            return;
        }

        // Get Screen Position
        Vector2 screenPos;
        if (Input.touchCount == 1)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            screenPos = Input.mousePosition;
        }
        else
        {
            return;
        }

        // Convert to maze coords
        Position mazePos = ConvertToMazePos(screenPos);
        if (lastMazeClickPos.x == mazePos.x && lastMazeClickPos.y == mazePos.y)
        {
            return;
        }
        lastMazeClickPos = mazePos;

        // Convert to World coords and mark as a path
        worldGenerator.TryAddPath(mazePos);

        // Check if the Game is Won
        if (worldGenerator.IsMazeSolved())
        {
            Debug.Log("WON!");
            isGameOver = true;
        }

        // Redraw Maze
        DrawMaze();
    }

    void DrawMaze()
    {
        // Refresh the world to pick up maze changes
        world = worldGenerator.RebuildWorld();

        // Create Maze Texture
        Color[] pixels = new Color[worldGenerator.GetXLength() * worldGenerator.GetZLength()];
        for (int x = 0; x < worldGenerator.GetXLength(); x++)
        {
            for (int z = 0; z < worldGenerator.GetZLength(); z++)
            {
                int pixelIndex = z * worldGenerator.GetXLength() + x;
                if (world[x, z].type == WorldSpace.Type.floor)
                {
                    pixels[pixelIndex] = Color.white;
                }
                else if (world[x, z].type == WorldSpace.Type.wall)
                {
                    pixels[pixelIndex] = Color.black;
                }
                else if (world[x, z].type == WorldSpace.Type.path)
                {
                    pixels[pixelIndex] = Color.green;
                }
            }
        }

        Texture2D texture = new Texture2D(worldGenerator.GetXLength(), worldGenerator.GetZLength());
        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        // Create Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);

        // Add to Scene
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = new Vector3(0, 0, 0);
    }

    void ResizeCamera(int worldHeight)
    {
        Camera.main.orthographicSize = worldHeight / 2.0f;
    }

    Position ConvertToMazePos(Vector2 screenPos)
    {
        // Convert to Unity coordinates
        Vector2 unityPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Convert to local sprite coordinates
        Vector2 spritePos = spriteRenderer.transform.InverseTransformPoint(unityPos);

        // Translate to WorldSpace array indicies
        float spriteXSize = spriteRenderer.sprite.bounds.extents.x;
        float spriteYSize = spriteRenderer.sprite.bounds.extents.y;
        Vector2 worldPos = new Vector2(spritePos.x + spriteXSize, spritePos.y + spriteYSize);

        // Convert to Maze indicies
        return new Position() { x = worldGenerator.ConvertToMazeCoord(worldPos.x), y = worldGenerator.ConvertToMazeCoord(worldPos.y) };
    }
}
