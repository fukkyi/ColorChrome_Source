using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazingAssets.TerrainToMesh;

public class TerrainMeshController : MonoBehaviour
{
    [SerializeField]
    private string detailMeshName = "Detail Mesh";

    [SerializeField]
    private bool setDetailMesh = false;

    [ContextMenu("ResetTerrainMesh")]
    private void ResetTerrainMesh()
    {
        int terrainCount = transform.childCount;
        Transform[] terrains = new Transform[terrainCount];
        // 子オブジェクトのテラインを全て取得する
        for(int i = 0; i < terrainCount; i++)
        {
            terrains[i] = transform.GetChild(i);
        }
        // テライン毎に処理を行う
        foreach (Transform terrain in terrains)
        {
            Transform meshParent = terrain.GetChild(0);

            int meshCount = meshParent.childCount;
            Transform[] meshes = new Transform[meshCount];
            // 分割されたメッシュを取得する
            for(int i = 0; i < meshCount; i++)
            {
                meshes[i] = meshParent.GetChild(i);
            }
            // メッシュに対する処理を行う
            foreach (Transform mesh in meshes)
            {
                mesh.gameObject.AddComponent<GrayableObject>();
                mesh.gameObject.layer = LayerMaskUtil.GrayableLayerNumber;
                // DetailMeshが存在しない場合は先に進める
                if (setDetailMesh || mesh.childCount == 0) continue;

                Transform detailLayerParent = mesh.GetChild(0);

                int detailLayerCount = detailLayerParent.childCount;
                Transform[] detailLayers = new Transform[detailLayerCount];
                // DetailMeshLayerを取得する
                for(int i = 0; i < detailLayerCount; i++)
                {
                    detailLayers[i] = detailLayerParent.GetChild(i);
                }
                // DetailMeshLayerに対する処理を行う
                foreach(Transform detailLayer in detailLayers)
                {
                    int detailCount = detailLayer.childCount;
                    Transform[] details = new Transform[detailCount];
                    // DetailMeshを取得する
                    for(int i = 0; i < detailCount; i++)
                    {
                        details[i] = detailLayer.GetChild(i);
                    }
                    // DetailMeshに対する処理を行う
                    foreach (Transform detail in details)
                    {
                        detail.gameObject.layer = LayerMaskUtil.GrayableGrassLayerNumber;
                        SphereCollider addCollider = detail.gameObject.AddComponent<SphereCollider>();
                        addCollider.center = Vector3.zero;
                        addCollider.radius = 0.2f;
                        addCollider.isTrigger = true;
                    }
                }
            }
        }
    }

    private void SetGrayableGrassToDetailMesh()
    {
        
    }
}
