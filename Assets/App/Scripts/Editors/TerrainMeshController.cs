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
        // �q�I�u�W�F�N�g�̃e���C����S�Ď擾����
        for(int i = 0; i < terrainCount; i++)
        {
            terrains[i] = transform.GetChild(i);
        }
        // �e���C�����ɏ������s��
        foreach (Transform terrain in terrains)
        {
            Transform meshParent = terrain.GetChild(0);

            int meshCount = meshParent.childCount;
            Transform[] meshes = new Transform[meshCount];
            // �������ꂽ���b�V�����擾����
            for(int i = 0; i < meshCount; i++)
            {
                meshes[i] = meshParent.GetChild(i);
            }
            // ���b�V���ɑ΂��鏈�����s��
            foreach (Transform mesh in meshes)
            {
                mesh.gameObject.AddComponent<GrayableObject>();
                mesh.gameObject.layer = LayerMaskUtil.GrayableLayerNumber;
                // DetailMesh�����݂��Ȃ��ꍇ�͐�ɐi�߂�
                if (setDetailMesh || mesh.childCount == 0) continue;

                Transform detailLayerParent = mesh.GetChild(0);

                int detailLayerCount = detailLayerParent.childCount;
                Transform[] detailLayers = new Transform[detailLayerCount];
                // DetailMeshLayer���擾����
                for(int i = 0; i < detailLayerCount; i++)
                {
                    detailLayers[i] = detailLayerParent.GetChild(i);
                }
                // DetailMeshLayer�ɑ΂��鏈�����s��
                foreach(Transform detailLayer in detailLayers)
                {
                    int detailCount = detailLayer.childCount;
                    Transform[] details = new Transform[detailCount];
                    // DetailMesh���擾����
                    for(int i = 0; i < detailCount; i++)
                    {
                        details[i] = detailLayer.GetChild(i);
                    }
                    // DetailMesh�ɑ΂��鏈�����s��
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
