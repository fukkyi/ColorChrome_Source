using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainDetailPainter : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain = null;

    private int[,] originalDefaultDetailMap = null;
    private int[,] originalGrayDetailMap = null;

    private int defaultGrassLayer = 0;
    private int grayGrassLayer = 1;

    private void Awake()
    {
        TerrainData terrainData = terrain.terrainData;

        originalDefaultDetailMap = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, defaultGrassLayer);
        originalGrayDetailMap = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, grayGrassLayer);

        #if UNITY_EDITOR
        // Detailデータは巻き戻らないため、プレイ終了時にプログラムで巻き戻す
        EditorApplication.playModeStateChanged += OnEditorStoped;
        #endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// Detailデータをプレイ前の状態に巻き戻す
    /// </summary>
    /// <param name="state"></param>
    private void OnEditorStoped(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.ExitingPlayMode) return;

        terrain.terrainData.SetDetailLayer(0, 0, defaultGrassLayer, originalDefaultDetailMap);
        terrain.terrainData.SetDetailLayer(0, 0, grayGrassLayer, originalGrayDetailMap);
    }
#endif

    /// <summary>
    /// Detailレイヤーを切り替えて草の色を変える
    /// </summary>
    /// <param name="drawPos"></param>
    /// <param name="drawRadius"></param>
    /// <param name="unGray"></param>
    public void DrawDetail(Vector3 drawPos, float drawRadius, bool unGray = false)
    {
        Vector3 rawTerrainPos = terrain.GetPosition();
        Vector3 rawTerrainSize = terrain.terrainData.size;
        Vector2 terrainPos = Vector2.right * rawTerrainPos.x + Vector2.up * rawTerrainPos.z;
        Vector2 terrainSize = Vector2.right * rawTerrainSize.x + Vector2.up * rawTerrainSize.z;
        Vector2 drawObjPoint = Vector2.right * drawPos.x + Vector2.up * drawPos.z;
        // Terrainの大きさの情報をRectで持つ
        Rect terrainRect = new Rect(terrainPos, terrainSize);

        Vector2 minPosPoint = drawObjPoint + Vector2.left * drawRadius + Vector2.down * drawRadius;
        Vector2 maxPosPoint = drawObjPoint + Vector2.right * drawRadius + Vector2.up * drawRadius;
        // 座標からTerrainでの位置の比率を取得する
        Vector2 minPosRatio = Rect.PointToNormalized(terrainRect, minPosPoint);
        Vector2 maxPosRatio = Rect.PointToNormalized(terrainRect, maxPosPoint);
        // 比率からDetailMapでの位置を計算する
        int detailMinXPos = (int)(terrain.terrainData.detailWidth * minPosRatio.x);
        int detailMinYPos = (int)(terrain.terrainData.detailHeight * minPosRatio.y);
        int detailMaxXPos = (int)(terrain.terrainData.detailWidth * maxPosRatio.x);
        int detailMaxYPos = (int)(terrain.terrainData.detailHeight * maxPosRatio.y);
        int detailWidth = detailMaxXPos - detailMinXPos;
        int detailHeight = detailMaxYPos - detailMinYPos;
        // もしDetailMapの範囲外まで指定されそうであれば、範囲内までクランプする
        if (detailMinXPos + detailWidth > terrain.terrainData.detailWidth)
        {
            detailWidth -= (detailMinXPos + detailWidth) - terrain.terrainData.detailWidth;
        }
        if (detailMinXPos + detailWidth > terrain.terrainData.detailWidth)
        {
            detailWidth -= (detailMinXPos + detailWidth) - terrain.terrainData.detailWidth;
        }

        // なぜかDetailMapの1次元にY,2次元にX,が入っているので、引数のwidthとheightを逆にすることで対処
        int[,] grassDetailMap = terrain.terrainData.GetDetailLayer(detailMinXPos, detailMinYPos, detailHeight, detailWidth, defaultGrassLayer);
        int[,] grayDetailMap = terrain.terrainData.GetDetailLayer(detailMinXPos, detailMinYPos, detailHeight, detailWidth, grayGrassLayer);
        // 多次元配列なのでGetLength()で要素数を取得する
        for (int y = 0; y < grassDetailMap.GetLength(1); y++)
        {
            for (int x = 0; x < grassDetailMap.GetLength(0); x++)
            {
                if (unGray)
                {
                    // 灰色の草がない場合は処理をしない
                    if (grayDetailMap[x, y] == 0) continue;
                }
                else
                {
                    // 色がある草がない場合は処理をしない
                    if (grassDetailMap[x, y] == 0) continue;
                }

                Vector2 detailPos = Vector2.right * (detailMinXPos + x) + Vector2.up * (detailMinYPos + y);
                // 計算している座標が塗る半径に入っていない場合は処理をしない
                if (!CheckDetailPositionWithinRadius(detailPos, drawPos, terrainRect, drawRadius)) continue;

                if (unGray)
                {
                    // 灰色から元に戻す
                    grassDetailMap[x, y] = grayDetailMap[x, y];
                    grayDetailMap[x, y] = 0;
                }
                else
                {
                    // 灰色にする
                    grayDetailMap[x, y] = grassDetailMap[x, y];
                    grassDetailMap[x, y] = 0;
                }
            }
        }

        terrain.terrainData.SetDetailLayer(detailMinXPos, detailMinYPos, grayGrassLayer, grayDetailMap);
        terrain.terrainData.SetDetailLayer(detailMinXPos, detailMinYPos, defaultGrassLayer, grassDetailMap);
    }

    /// <summary>
    /// Detail用の座標で塗る半径に入っているか判定する
    /// </summary>
    /// <param name="detailPos"></param>
    /// <param name="centerPos"></param>
    /// <param name="terrainRect"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private bool CheckDetailPositionWithinRadius(Vector2 detailPos, Vector3 centerPos, Rect terrainRect, float radius)
    {
        float posRatioX = detailPos.x / terrain.terrainData.detailWidth;
        float posRatioY = detailPos.y / terrain.terrainData.detailHeight;
        Vector2 posRatio = Vector2.right * posRatioX + Vector2.up * posRatioY;
        Vector2 calcRectPoint = Rect.NormalizedToPoint(terrainRect, posRatio);
        Vector3 calcPos = centerPos;
        calcPos.x = calcRectPoint.x;
        calcPos.z = calcRectPoint.y;

        return Vector3.Distance(centerPos, calcPos) <= radius;
    }
}