using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private readonly List<Transform> tiles = new List<Transform>();

    private Camera mainCamera;
    private float scrollSpeed;
    private float tileWidth;
    private float tileSpacing;

    public static void SetupScene()
    {
        CreateLayer("Background", 0.65f);
        CreateLayer("Ground", 2.4f);
    }

    private static void CreateLayer(string sourceName, float speed)
    {
        if (GameObject.Find(sourceName + " Parallax") != null)
        {
            return;
        }

        GameObject sourceObject = GameObject.Find(sourceName);
        if (sourceObject == null || !sourceObject.TryGetComponent(out SpriteRenderer sourceRenderer))
        {
            return;
        }

        GameObject layerObject = new GameObject(sourceName + " Parallax");
        ParallaxBackground layer = layerObject.AddComponent<ParallaxBackground>();
        layer.Initialize(sourceRenderer, speed);

        sourceRenderer.enabled = false;
    }

    private void Initialize(SpriteRenderer sourceRenderer, float speed)
    {
        mainCamera = Camera.main;
        scrollSpeed = speed;
        tileWidth = sourceRenderer.bounds.size.x;
        tileSpacing = tileWidth - 0.04f;

        int tileCount = CalculateTileCount();
        float startX = GetCameraLeftEdge() - tileSpacing;

        for (int i = 0; i < tileCount; i++)
        {
            GameObject tile = new GameObject(sourceRenderer.gameObject.name + " Tile " + i);
            tile.transform.SetParent(transform);
            tile.transform.position = new Vector3(
                startX + tileSpacing * i,
                sourceRenderer.transform.position.y,
                sourceRenderer.transform.position.z);
            tile.transform.localScale = sourceRenderer.transform.lossyScale;

            SpriteRenderer tileRenderer = tile.AddComponent<SpriteRenderer>();
            CopyRenderer(sourceRenderer, tileRenderer);

            tiles.Add(tile.transform);
        }
    }

    private void Update()
    {
        if (tiles.Count == 0)
        {
            return;
        }

        float moveDistance = scrollSpeed * Time.deltaTime;
        float leftEdge = GetCameraLeftEdge() - tileSpacing;
        float rightMostX = GetRightMostTileX();

        foreach (Transform tile in tiles)
        {
            tile.position += Vector3.left * moveDistance;

            if (tile.position.x <= leftEdge)
            {
                rightMostX += tileSpacing;
                tile.position = new Vector3(rightMostX, tile.position.y, tile.position.z);
            }
        }
    }

    private int CalculateTileCount()
    {
        float cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        return Mathf.CeilToInt(cameraWidth / tileSpacing) + 4;
    }

    private float GetCameraLeftEdge()
    {
        return mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
    }

    private float GetRightMostTileX()
    {
        float rightMostX = tiles[0].position.x;

        foreach (Transform tile in tiles)
        {
            if (tile.position.x > rightMostX)
            {
                rightMostX = tile.position.x;
            }
        }

        return rightMostX;
    }

    private static void CopyRenderer(SpriteRenderer source, SpriteRenderer target)
    {
        target.sprite = source.sprite;
        target.color = source.color;
        target.material = source.sharedMaterial;
        target.sortingLayerID = source.sortingLayerID;
        target.sortingOrder = source.sortingOrder;
        target.flipX = source.flipX;
        target.flipY = source.flipY;
        target.drawMode = source.drawMode;
        target.size = source.size;
        target.maskInteraction = source.maskInteraction;
        target.spriteSortPoint = source.spriteSortPoint;
    }
}
