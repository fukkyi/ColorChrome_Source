using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Terrain))]
public class TerrainFootstepRinger : FootstepRinger
{
    private const string GrassLayerName = "CC_grass";
    private const string RoadLayerName = "CC_road";
    private const string DirtLayerName = "CC_dirt";
    private const string SnowLayerName = "CC_snow";
    private const string StoneLayerName = "CC_rock";
    private const string SnowRockLayerName = "CC_snowrock";
    private const string IceLayerName = "CC_ice";

    private Terrain terrain = null;

    private TerrainData terrainData = null;
    private float[] splatmap = new float[0];

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    public override void PlayFootsteps(Vector2 textureCoord, Vector3? playPosition = null)
    {
        base.PlayFootsteps(textureCoord, playPosition);
        PlayFootstepsByTerrainLayer(textureCoord, playPosition);
    }

    /// <summary>
    /// 現在の足元のTerrainレイヤーから足音を鳴らす
    /// </summary>
    /// <param name="textureCoord"></param>
    /// <param name="playPosition"></param>
    private void PlayFootstepsByTerrainLayer(Vector2 textureCoord, Vector3? playPosition = null)
    {
        float[,,] alphaMaps = terrainData.GetAlphamaps(Mathf.FloorToInt(textureCoord.x * terrainData.alphamapWidth), Mathf.FloorToInt(textureCoord.y * terrainData.alphamapHeight), 1, 1);
        int layerCount = terrainData.alphamapLayers;

        // レイヤーが何もない場合は処理しない
        if (layerCount <= 0) return;
        // 三番目の配列を取り出す
        Array.Resize(ref splatmap, layerCount);
        for (int i = 0; i < layerCount; i++)
        {
            splatmap[i] = alphaMaps[0, 0, i];
        }
        // 一番含まれているレイヤー
        int maxLayerIndex = Array.IndexOf(splatmap, Mathf.Max(splatmap));
        // レイヤーの名前を取得する
        TerrainLayer[] terrainLayers = terrainData.terrainLayers;
        string terrainLayerName = terrainLayers[maxLayerIndex].name;
        // レイヤーによって足音を変える
        switch(terrainLayerName)
        {
            case GrassLayerName:
                PlayFootstepsSound(FootStepType.Grass, playPosition);
                break;
            case RoadLayerName:
            case DirtLayerName:
                PlayFootstepsSound(FootStepType.Gravel, playPosition);
                break;
            case StoneLayerName:
                PlayFootstepsSound(FootStepType.Stone, playPosition);
                break;
            case SnowLayerName:
                PlayFootstepsSound(FootStepType.Snow, playPosition);
                break;
            case SnowRockLayerName:
            case IceLayerName:
                PlayFootstepsSound(FootStepType.Ice, playPosition);
                break;
        }
    }
}
