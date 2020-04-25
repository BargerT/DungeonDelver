using UnityEngine;

public class Tile : MonoBehaviour
{
    public Doorway[] doorways;
    public MeshCollider meshCollider;

    public Bounds TileBounds
    {
        get { return meshCollider.bounds; }
    }
}
