using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObject : MonoBehaviour
{
    public const string GrassGrayAmountPropName = "_GrayAmount";
    public const float GrassGrayAmountValueUnGray = 0;
    public const float GrassGrayAmountValueGray = 1;

    // 毎フレームの描画を許可するか
    public bool allowUpdateDraw = true;
    // 動いていない時でも描画を実行するか
    public bool allowNoMoveDraw = false;
    public float drawRadius = 1;
    public float drawAmount = 1;

    [SerializeField]
    private int detectCount = 10;
    [SerializeField]
    private bool unGray = false;

    private int grayableObjectLayerMask = 0;

    private Vector3 beforePosition = Vector3.zero;

    private Transform myTransform = null;
    private Collider[] grayableBuffer = null;
    private Collider[] grayableGrassBuffer = new Collider[20];

    private void Awake()
    {
        grayableBuffer = new Collider[detectCount];
        grayableObjectLayerMask = LayerMask.GetMask(LayerMaskUtil.GrayableLayerName, LayerMaskUtil.GrayableNonCollisionLayerName);
        myTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!allowUpdateDraw) return;
        
        if (allowNoMoveDraw)
        {
            // allowNoMoveDrawがtrueの場合は、動いていない時も更新する
            DrawToGrayableObject();
        }
        else if (myTransform.position != beforePosition)
        {
            // 処理負荷を減らすため、動いている時のみ更新する
            DrawToGrayableObject();
            beforePosition = myTransform.position;
        }
    }

    /// <summary>
    /// 灰色に出来るオブジェクトを灰色にする
    /// </summary>
    public void DrawToGrayableObject()
    {
        Physics.OverlapSphereNonAlloc(transform.position, drawRadius, grayableBuffer, grayableObjectLayerMask);
        foreach (Collider grayableCollider in grayableBuffer)
        {
            if (grayableCollider == null) continue;

            GrayableObject grayableObject = grayableCollider.GetComponent<GrayableObject>();
            if (unGray)
            {
                grayableObject.DrawUnGray(transform.position, drawRadius, drawAmount);
            }
            else
            {
                grayableObject.DrawGray(transform.position, drawRadius, drawAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, drawRadius);
    }
}
