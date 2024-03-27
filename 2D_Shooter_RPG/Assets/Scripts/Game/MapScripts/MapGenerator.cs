using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    Dictionary<int, GameObject> groundTileset;
    Dictionary<int, GameObject> groundTileGroups;

    Dictionary<int, GameObject> treeTileset;
    Dictionary<int, GameObject> treeTileGroups;

    Dictionary<int, GameObject> bushTileset;
    Dictionary<int, GameObject> bushTileGroups;
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    [SerializeField] private GameObject _groundPrefab0;
    [SerializeField] private GameObject _groundPrefab1;
    [SerializeField] private GameObject _groundPrefab2;
    [SerializeField] private GameObject _groundPrefab3;
    [SerializeField] private GameObject _groundPrefab4;
    [SerializeField] private GameObject _groundPrefab5;
    [SerializeField] private GameObject _groundPrefab6;
    [SerializeField] private GameObject _groundPrefab7;

    [SerializeField] private GameObject _treePrefab0;
    [SerializeField] private GameObject _treePrefab1;
    [SerializeField] private GameObject _treePrefab2;

    [SerializeField] private GameObject _bushPrefab0;
    [SerializeField] private GameObject _bushPrefab1;
    [SerializeField] private GameObject _bushPrefab2;
    [SerializeField] private GameObject _bushPrefab3;
    [SerializeField] private GameObject _bushPrefab4;
    [SerializeField] private GameObject _bushPrefab5;
    
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeigth;
    [SerializeField] private float _noiseScale;
    [SerializeField] private int _octaves;
    [SerializeField][Range(0,1)] private float _persistance;
    [SerializeField] private float _lacunarity;

    [SerializeField] private int _seed;
    [SerializeField] private Vector2 _offset;

    void Start()
    {
        CreateGroundTileset();
        CreateGroundTileGroups();
        CreateTreeTileset();
        CreateTreeTileGroups();
        CreateBushTileset();
        CreateBushTileGroups();
        GenerateMap();
    }

    void CreateGroundTileset()
    {
        groundTileset = new Dictionary<int, GameObject>
        {
            { 0, _groundPrefab0 },
            { 1, _groundPrefab1 },
            { 2, _groundPrefab2 },
            { 3, _groundPrefab3 },
            { 4, _groundPrefab4 },
            { 5, _groundPrefab5 },
            { 6, _groundPrefab6 },
            { 7, _groundPrefab7 }
        };
    }

    void CreateGroundTileGroups()
    {
        groundTileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in groundTileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            groundTileGroups.Add(prefab_pair.Key, tile_group);
        }
    }
    void CreateTreeTileset()
    {
        treeTileset = new Dictionary<int, GameObject>
        {
            { 0, _treePrefab0 },
            { 1, _treePrefab1 },
            { 2, _treePrefab2 }
        };
    }

    void CreateTreeTileGroups()
    {
        treeTileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in treeTileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            treeTileGroups.Add(prefab_pair.Key, tile_group);
        }
    }

    void CreateBushTileset()
    {
        bushTileset = new Dictionary<int, GameObject>
        {
            { 0, _bushPrefab0 },
            { 1, _bushPrefab1 },
            { 2, _bushPrefab2 },
            { 3, _bushPrefab3 },
            { 4, _bushPrefab4 },
            { 5, _bushPrefab5 }
        };
    }

    void CreateBushTileGroups()
    {
        bushTileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in bushTileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            bushTileGroups.Add(prefab_pair.Key, tile_group);
        }
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            _mapWidth, 
            _mapHeigth, 
            _seed, 
            _noiseScale, 
            _octaves, 
            _persistance, 
            _lacunarity, 
            _offset);

        MapDisplay.CreateTiles(_mapWidth, _mapHeigth, _seed, ref groundTileset, ref groundTileGroups, ref treeTileset, ref treeTileGroups, ref bushTileset, ref bushTileGroups, ref tile_grid, noiseMap);
    }

    void OnValidate()
    {
        if (_mapWidth < 1){
            _mapWidth = 1;
        }

        if (_mapHeigth < 1){
            _mapHeigth = 1;
        }

        if (_lacunarity < 1){
            _lacunarity = 1;
        }

        if (_octaves < 0){
            _mapHeigth = 0;
        }
    }
}
