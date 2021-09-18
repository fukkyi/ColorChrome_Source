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
        // Detail�f�[�^�͊����߂�Ȃ����߁A�v���C�I�����Ƀv���O�����Ŋ����߂�
        EditorApplication.playModeStateChanged += OnEditorStoped;
        #endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// Detail�f�[�^���v���C�O�̏�ԂɊ����߂�
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
    /// Detail���C���[��؂�ւ��đ��̐F��ς���
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
        // Terrain�̑傫���̏���Rect�Ŏ���
        Rect terrainRect = new Rect(terrainPos, terrainSize);

        Vector2 minPosPoint = drawObjPoint + Vector2.left * drawRadius + Vector2.down * drawRadius;
        Vector2 maxPosPoint = drawObjPoint + Vector2.right * drawRadius + Vector2.up * drawRadius;
        // ���W����Terrain�ł̈ʒu�̔䗦���擾����
        Vector2 minPosRatio = Rect.PointToNormalized(terrainRect, minPosPoint);
        Vector2 maxPosRatio = Rect.PointToNormalized(terrainRect, maxPosPoint);
        // �䗦����DetailMap�ł̈ʒu���v�Z����
        int detailMinXPos = (int)(terrain.terrainData.detailWidth * minPosRatio.x);
        int detailMinYPos = (int)(terrain.terrainData.detailHeight * minPosRatio.y);
        int detailMaxXPos = (int)(terrain.terrainData.detailWidth * maxPosRatio.x);
        int detailMaxYPos = (int)(terrain.terrainData.detailHeight * maxPosRatio.y);
        int detailWidth = detailMaxXPos - detailMinXPos;
        int detailHeight = detailMaxYPos - detailMinYPos;
        // ����DetailMap�͈̔͊O�܂Ŏw�肳�ꂻ���ł���΁A�͈͓��܂ŃN�����v����
        if (detailMinXPos + detailWidth > terrain.terrainData.detailWidth)
        {
            detailWidth -= (detailMinXPos + detailWidth) - terrain.terrainData.detailWidth;
        }
        if (detailMinXPos + detailWidth > terrain.terrainData.detailWidth)
        {
            detailWidth -= (detailMinXPos + detailWidth) - terrain.terrainData.detailWidth;
        }

        // �Ȃ���DetailMap��1������Y,2������X,�������Ă���̂ŁA������width��height���t�ɂ��邱�ƂőΏ�
        int[,] grassDetailMap = terrain.terrainData.GetDetailLayer(detailMinXPos, detailMinYPos, detailHeight, detailWidth, defaultGrassLayer);
        int[,] grayDetailMap = terrain.terrainData.GetDetailLayer(detailMinXPos, detailMinYPos, detailHeight, detailWidth, grayGrassLayer);
        // �������z��Ȃ̂�GetLength()�ŗv�f�����擾����
        for (int y = 0; y < grassDetailMap.GetLength(1); y++)
        {
            for (int x = 0; x < grassDetailMap.GetLength(0); x++)
            {
                if (unGray)
                {
                    // �D�F�̑����Ȃ��ꍇ�͏��������Ȃ�
                    if (grayDetailMap[x, y] == 0) continue;
                }
                else
                {
                    // �F�����鑐���Ȃ��ꍇ�͏��������Ȃ�
                    if (grassDetailMap[x, y] == 0) continue;
                }

                Vector2 detailPos = Vector2.right * (detailMinXPos + x) + Vector2.up * (detailMinYPos + y);
                // �v�Z���Ă�����W���h�锼�a�ɓ����Ă��Ȃ��ꍇ�͏��������Ȃ�
                if (!CheckDetailPositionWithinRadius(detailPos, drawPos, terrainRect, drawRadius)) continue;

                if (unGray)
                {
                    // �D�F���猳�ɖ߂�
                    grassDetailMap[x, y] = grayDetailMap[x, y];
                    grayDetailMap[x, y] = 0;
                }
                else
                {
                    // �D�F�ɂ���
                    grayDetailMap[x, y] = grassDetailMap[x, y];
                    grassDetailMap[x, y] = 0;
                }
            }
        }

        terrain.terrainData.SetDetailLayer(detailMinXPos, detailMinYPos, grayGrassLayer, grayDetailMap);
        terrain.terrainData.SetDetailLayer(detailMinXPos, detailMinYPos, defaultGrassLayer, grassDetailMap);
    }

    /// <summary>
    /// Detail�p�̍��W�œh�锼�a�ɓ����Ă��邩���肷��
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