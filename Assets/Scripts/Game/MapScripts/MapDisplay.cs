using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{       
    public static void CreateTiles(
        int mapWidth,
        int mapHeight,
        int seed,
        ref Dictionary<int, GameObject> groundTileset,
        ref Dictionary<int, GameObject> groundTileGroups,
        ref Dictionary<int, GameObject> treeTileset,
        ref Dictionary<int, GameObject> treeTileGroups,
        ref Dictionary<int, GameObject> bushTileset,
        ref Dictionary<int, GameObject> bushTileGroups,
        ref List<List<GameObject>> tile_grid,
        float [,] noiseMap)
    {

    float chanceForTree = 0.01f;
    float chanceForBush = 0.1f;
    int tile_id;
    float plantRNG;

    System.Random prng = new System.Random(seed);

        for (int x = 0; x < mapWidth; x++)
        {
            tile_grid.Add(new List<GameObject>());

            for (int y = 0; y < mapHeight; y++)
            {
                tile_id = MapNoiseValueToTileIndex(noiseMap[x,y],groundTileset.Count);                
                CreateTile(groundTileset, groundTileGroups, ref tile_grid, tile_id, x, y, mapWidth, mapHeight);     
                plantRNG = (float)prng.NextDouble();

                if (plantRNG < chanceForTree)
                {   
                    tile_id = MapNoiseValueToTileIndex(noiseMap[x,y],treeTileset.Count);
                    CreateTile(treeTileset, treeTileGroups, ref tile_grid, tile_id, x, y, mapWidth, mapHeight);
                }
                else if (plantRNG < chanceForBush)
                {   
                    tile_id = MapNoiseValueToTileIndex(noiseMap[x,y],bushTileset.Count);
                    CreateTile(bushTileset, bushTileGroups, ref tile_grid, tile_id, x, y, mapWidth, mapHeight);
                }
            }
        }
    }

    private static int MapNoiseValueToTileIndex(float noiseValue, int tilesetCount)
    {
        noiseValue = Mathf.Clamp01(noiseValue);
        float scaledValue = noiseValue * tilesetCount;
        if (scaledValue >= tilesetCount)
            {
                scaledValue = tilesetCount-1;
            }
        int tileIndex = Mathf.FloorToInt(scaledValue);

        return tileIndex;
    }

    private static void CreateTile(
        Dictionary<int, GameObject> tileset,
        Dictionary<int, GameObject> tile_groups, 
        ref List<List<GameObject>> tile_grid, 
        int tile_id,
        int x,
        int y,
        int mapWidth,
        int mapHeight)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3((x*4)-mapWidth*2,(y*4)-mapHeight*2,0);

        tile_grid[x].Add(tile);
    }
}
