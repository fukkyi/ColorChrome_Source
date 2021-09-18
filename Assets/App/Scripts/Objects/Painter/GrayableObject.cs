using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrayableObject : MonoBehaviour
{
    public int MaxGrayAmount { get; private set; } = 0;
    public int CurrentGrayAmount { get; private set; } = 0;

    private static readonly Color paintColor = Color.green;
    private static readonly Color unPaintColor = Color.black;
    private static readonly Color eraseColor = Color.white;
    /// <summary>
    /// 灰色になった量を計算する際のミップマップレベル
    /// この値が小さいと計算精度が上がるが処理が重くなる
    /// </summary>
    private static readonly int CalcMipMapLevel = 6;
    private static readonly int MaxGrayAmountOfPixel = 255;

    [SerializeField]
    private Material positionMapper = null;
    [SerializeField]
    private Material extender = null;
    [SerializeField]
    private Material painter = null;

    [SerializeField]
    private string mainTexturePropertyName = "_MainTex";
    [SerializeField]
    private string targetPropertyName = "_GrayMap";

    private TerrainDetailPainter detailPainter = null;

    private Material material = null;
    private Texture2D sourceTexture = null;

    private Renderer targetRenderer = null;
    private Mesh targetMesh = null;

    private RenderTexture positionMap = null;
    private RenderTexture destTexture = null;

    private bool isReadingTexture = false;

    private void Reset()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        detailPainter = GetComponentInParent<TerrainDetailPainter>();

        targetRenderer = GetComponent<Renderer>();
        targetMesh = GetComponent<MeshFilter>().mesh;

        material = GetComponent<Renderer>().material;
        sourceTexture = (Texture2D)material.GetTexture(targetPropertyName);
        // sourceTexture = CreateGrayMapTexture(material.GetTexture(mainTexturePropertyName), unPaintColor);

        int width = sourceTexture.width;
        int height = sourceTexture.height;

        positionMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf)
        {
            anisoLevel = 0,
            autoGenerateMips = false,
            filterMode = FilterMode.Point,
        };

        destTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
        {
            anisoLevel = 0,
            autoGenerateMips = false,
            useMipMap = true,
        };

        Graphics.Blit(sourceTexture, destTexture);

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture(targetPropertyName, destTexture);
        targetRenderer.SetPropertyBlock(block);

        CalcTotalGrayAmount(destTexture);

        UpdatePositionMap();
    }

    private void UpdatePositionMap()
    {
        positionMapper.SetPass(0);
        Graphics.SetRenderTarget(positionMap);
        Graphics.DrawMeshNow(targetMesh, Matrix4x4.identity);

        UpdateRenderTexture(positionMap, extender);
    }

    public void DrawGray(Vector3 drawPosition, float paintRadius, float paintAmount)
    {
        painter.SetColor("_Color", paintColor);
        Draw(drawPosition, paintRadius, paintAmount);
        // DetailPainterがある場合はDetailも更新する
        if (detailPainter != null)
        {
            detailPainter.DrawDetail(drawPosition, paintRadius);
        }

        StartClacGrayAmount(destTexture);
    }

    public void DrawUnGray(Vector3 drawPosition, float paintRadius, float paintAmount)
    {
        // 白で減算することでテクスチャの色を黒に戻す
        painter.SetColor("_Color", eraseColor);
        Draw(drawPosition, paintRadius, -paintAmount);
        // DetailPainterがある場合はDetailも更新する
        if (detailPainter != null)
        {
            detailPainter.DrawDetail(drawPosition, paintRadius, true);
        }
    }

    private void Draw(Vector3 drawPosition, float paintRadius, float paintAmount)
    {
        painter.SetTexture("_PositionMap", positionMap);
        painter.SetMatrix("_ObjectToWorld", transform.localToWorldMatrix);
        painter.SetVector("_Pos_Rad", new Vector4(drawPosition.x, drawPosition.y, drawPosition.z, paintRadius));
        painter.SetFloat("_PaintAmount", paintAmount * Time.deltaTime);

        UpdateRenderTexture(destTexture, painter);
    }

    private void UpdateRenderTexture(RenderTexture texture, Material useMaterial)
    {
        RenderTexture temporary = RenderTexture.GetTemporary(texture.descriptor);
        Graphics.Blit(texture, temporary);
        Graphics.Blit(temporary, texture, useMaterial);
        RenderTexture.ReleaseTemporary(temporary);
    }

    /// <summary>
    /// 灰色になった量の最大値を計算する
    /// </summary>
    /// <param name="grayMap"></param>
    private void CalcTotalGrayAmount(Texture grayMap)
    {
        int calcWidth = grayMap.width >> CalcMipMapLevel;
        int calcHeight = grayMap.height >> CalcMipMapLevel;

        MaxGrayAmount = 0;

        for (int y = 0; y < calcHeight; y++)
        {
            for(int x = 0; x < calcWidth; x++)
            {
                MaxGrayAmount += MaxGrayAmountOfPixel;
            }
        }
    }

    /// <summary>
    /// 現在の灰色になった量を計算する処理をスタートさせる
    /// </summary>
    /// <param name="grayMap"></param>
    public void StartClacGrayAmount(RenderTexture grayMap)
    {
        // 既に計算処理が走っている場合は計算しない
        if (isReadingTexture) return;

        StartCoroutine(AsyncClacGrayAmount(grayMap));
    }

    /// <summary>
    /// どのくらい灰色になったか非同期で計算する
    /// </summary>
    /// <param name="grayMap"></param>
    /// <returns></returns>
    private IEnumerator AsyncClacGrayAmount(RenderTexture grayMap)
    {
        isReadingTexture = true;
        // 灰色判定用のテクスチャのミップマップを動的生成する
        grayMap.GenerateMips();

        // GPUを使って灰色判定用のテクスチャの色情報を非同期で読み取る
        AsyncGPUReadbackRequest request = AsyncGPUReadback.Request(grayMap, CalcMipMapLevel);
        while (!request.done)
        {
            yield return null;
        }
        // 読み取った色情報をNativeArrayで取得する
        Unity.Collections.NativeArray<Color32> buffer = request.GetData<Color32>();

        // 現在の灰色になった量を計算する
        CurrentGrayAmount = 0;
        for(int i = 0; i < buffer.Length; i++)
        {
            CurrentGrayAmount += buffer[i].g;
        }
        // 読み取った色情報を解放する
        buffer.Dispose();

        isReadingTexture = false;
    }
}
