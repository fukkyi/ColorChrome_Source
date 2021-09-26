using System.Collections.Generic;
using UnityEngine;

public class LayerMaskUtil
{
    /// <summary>
    /// デフォルトのレイヤー
    /// </summary>
    public static readonly int DefaultLayerNumber = 0;
    public static readonly string DefaultLayerName = "Default";
    /// <summary>
    /// 無彩色に出来るオブジェクトのレイヤー
    /// </summary>
    public static readonly int GrayableLayerNumber = 6;
    public static readonly string GrayableLayerName = "Grayable";
    /// <summary>
    /// プレイヤーのレイヤー
    /// </summary>
    public static readonly int PlayerLayerNumber = 7;
    public static readonly string PlayerLayerName = "Player";
    /// <summary>
    /// 敵のレイヤー
    /// </summary>
    public static readonly int EnemyLayerNumber = 8;
    public static readonly string EnemyLayerName = "Enemy";
    /// <summary>
    /// 無彩色に出来るオブジェクトのレイヤー
    /// </summary>
    public static readonly int GrayableGrassLayerNumber = 11;
    public static readonly string GrayableGrassLayerName = "GrayableGrass";
    /// <summary>
    /// プレイヤー攻撃判定のレイヤー
    /// </summary>
    public static readonly int PlayerAttackLayerNumber = 12;
    public static readonly string PlayerAttackLayerName = "PlayerAttackZone";
    /// <summary>
    /// 敵攻撃判定のレイヤー
    /// </summary>
    public static readonly int EnemyAttackLayerNumber = 13;
    public static readonly string EnemyAttackLayerName = "EnemyAttackZone";
    /// <summary>
    /// 衝突しない無彩色に出来るオブジェクトのレイヤー
    /// </summary>
    public static readonly int GrayableNonCollisionLayerNumber = 14;
    public static readonly string GrayableNonCollisionLayerName = "Grayable_NonCollision";
    /// <summary>
    /// Terrainのレイヤー
    /// </summary>
    public static readonly int TerrainLayerNumber = 17;
    public static readonly string TerrainLayerName = "Terrain";
    /// <summary>
    /// 死亡した敵のレイヤー
    /// </summary>
    public static readonly int DeadEnemyLayerNumber = 18;
    public static readonly string DeadEnemyLayerName = "DeadEnemy";

    private static readonly string[] GroundLayers = { "Default", GrayableLayerName };

    /// <summary>
    /// 地形のレイヤーに反応するレイヤーマスクを取得する
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskGrounds()
    {
        return LayerMask.GetMask(GroundLayers);
    }

    /// <summary>
    /// 地形のレイヤーと敵に反応するレイヤーマスクを取得する
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskGroundsAndEnemy()
    {
        return ~LayerMask.GetMask(PlayerLayerName, PlayerAttackLayerName, EnemyAttackLayerName, GrayableGrassLayerName);
    }

    /// <summary>
    /// 足音を鳴らすオブジェクトのみに反応するレイヤーマスクを取得する
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskFootstepsObject()
    {
        return LayerMask.GetMask(TerrainLayerName, DefaultLayerName);
    }
}
