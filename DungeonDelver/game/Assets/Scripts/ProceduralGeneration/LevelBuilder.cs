using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    public Tile startTilePrefab, endTilePrefab;
    public List<Tile> tilePrefabs = new List<Tile>();
    public Vector2 iterationRange = new Vector2(10, 15);

    List<Doorway> availableDoorways = new List<Doorway>();

    StartTile startTile;
    EndTile endTile;
    List<Tile> placedTiles = new List<Tile>();
    public bool levelFinished = false;
    private bool courStarted = false;
    private int prevTile = -1;

    LayerMask roomLayerMask;

    private void Start()
    {
        roomLayerMask = LayerMask.GetMask("Room");
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name.CompareTo("TileBuilder") == 0 && !courStarted)
        {
            StartCoroutine("GenerateLevel");
            courStarted = true;
        } 
        if (levelFinished)
        {
            levelFinished = false;
            StopCoroutine("GenerateLevel");
            ResetLevelGenerator();
        }
    }

    IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        // Place Start Tile
        PlaceStartTile();
        yield return interval;

        // Random iterations
        int iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);
        for(int i = 0; i < iterations; i++)
        {
            // Place random tile from list
            if (i + 1 == iterations)
            {
                PlaceTile(true);
            }
            else
            {
                PlaceTile(false);
            }
            yield return interval;
        }

        // Place end tile
        PlaceEndTile();
        
        yield return levelFinished;
    }

    void AddDoorwaysToList(Tile tile, ref List<Doorway> list)
    {
        foreach(Doorway doorway in tile.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, doorway);
        }
    }

    void PlaceStartTile()
    {
        // Instantiate Tile
        startTile = Instantiate(startTilePrefab) as StartTile;
        startTile.transform.parent = this.transform;

        // Get Doorways from current room and add them randomly to the available doorway list
        AddDoorwaysToList(startTile, ref availableDoorways);

        // Position room
        startTile.transform.position = Vector3.zero;
        startTile.transform.rotation = Quaternion.identity;
    }

    void PlaceTile(bool final)
    {
        // Instantiate Tile
        Tile currentTile;
        if (final)
        {
            currentTile = Instantiate(tilePrefabs[tilePrefabs.Count - 1]) as Tile;
        } else
        {
            int tileSelection = Random.Range(0, tilePrefabs.Count - 1);
            while(tileSelection == prevTile)
            {
                tileSelection = Random.Range(0, tilePrefabs.Count - 1);
            }

            prevTile = tileSelection;
            currentTile = Instantiate(tilePrefabs[tileSelection]) as Tile;
        }

        currentTile.transform.parent = this.transform;

        // Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentTileDoorways = new List<Doorway>();
        AddDoorwaysToList(currentTile, ref currentTileDoorways);

        // Get Doorways from current tile and add them randomly to the list of avialable doorways
        AddDoorwaysToList(currentTile, ref availableDoorways);

        bool tilePlaced = false;

        // Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            foreach (Doorway currentDoorway in currentTileDoorways)
            {
                // Position Tile
                PositionTileAtDoorway(ref currentTile, currentDoorway, availableDoorway);

                // Check room overlaps
                if(CheckTileOverlap(currentTile))
                {
                    continue;
                }

                tilePlaced = true;

                // Add room to list for cleanup later
                placedTiles.Add(currentTile);

                // Remove doorways
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                // Exit Loop if tile has been placed
                break;
            }

            // Exit loop if tile has been placed
            if(tilePlaced)
            {
                break;
            }
        }

        // Tile could not be placed so restart the generator
        if(!tilePlaced)
        {

            Destroy(currentTile.gameObject);
            ResetLevelGenerator();
        }
    }

    void PositionTileAtDoorway(ref Tile tile, Doorway tileDoorway, Doorway targetDoorway)
    {
        // Reset tile position and rotation
        tile.transform.position = Vector3.zero;
        tile.transform.rotation = Quaternion.identity;

        // Rotate tile to match previous doorway orientation
        Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
        Vector3 tileDoorwayEuler = tileDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(tileDoorwayEuler.y, targetDoorwayEuler.y);
        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        tileDoorway.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);

        // Position tile
        Vector3 tilePositionOffset = tileDoorway.transform.position - tile.transform.position;
        tile.transform.position = targetDoorway.transform.position - tilePositionOffset;
    }

    bool CheckTileOverlap(Tile tile)
    {
        Bounds bounds = tile.TileBounds;
        bounds.Expand(-0.1f);

        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, tile.transform.rotation, roomLayerMask);
        if(colliders.Length > 0)
        {
            // Ignore collisions with current room
            foreach(Collider c in colliders)
            {
                if(c.transform.parent.gameObject.Equals(tile.gameObject))
                {
                    continue;
                } else
                {
                    Debug.LogError("Overlap detected");
                    return true;
                }
            }
        }
        return false;
    }

    void PlaceEndTile()
    {
        // Instantiate tile
        endTile = Instantiate(endTilePrefab) as EndTile;
        endTile.transform.parent = this.transform;

        // Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        Doorway doorway = endTile.doorways[0];

        bool tilePlaced = false;

        // Try all available doorways
        foreach(Doorway availableDoorway in allAvailableDoorways)
        {
            // Position tile
            Tile tile = (Tile)endTile;
            PositionTileAtDoorway(ref tile, doorway, availableDoorway);

            // Check if tile overlaps
            if(CheckTileOverlap(endTile))
            {
                continue;
            }

            tilePlaced = true;

            // Remove doorways
            doorway.gameObject.SetActive(false);
            availableDoorways.Remove(doorway);

            availableDoorway.gameObject.SetActive(false);
            availableDoorways.Remove(availableDoorway);

            // Exit loop if tile has been placed
            break;
        }

        // Tile could not be placed so restart the generator
        if (!tilePlaced)
        {
            Destroy(endTile.gameObject);
            ResetLevelGenerator();
        }
    }

    void ResetLevelGenerator()
    {
        // Delete all rooms
        if(startTile)
        {
            Destroy(startTile.gameObject);
        }

        if(endTile)
        {
            Destroy(endTile.gameObject);
        }

        foreach(Tile tile in placedTiles)
        {
            Destroy(tile.gameObject);
        }

        // Clear lists
        placedTiles.Clear();
        availableDoorways.Clear();

        // Reset coroutine
        StartCoroutine("GenerateLevel");
    }
}
