using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : BaseSceneManager<GameSceneUIManager>
{
    [SerializeField]
    private HpGauge playerHPGauge = null;
    public HpGauge PlayerHPGauge { get { return playerHPGauge; } }

    [SerializeField]
    private InkGauge inkGauge = null;
    public InkGauge InkGauge { get { return inkGauge; } }

    [SerializeField]
    private MiniMapUI miniMap = null;
    public MiniMapUI MiniMap { get { return miniMap; } }

    [SerializeField]
    private ReticleUI reticle = null;
    public ReticleUI Reticle { get { return reticle; } }

    [SerializeField]
    private MissionUI missionList = null;
    public MissionUI MissionList { get { return missionList; } }

    [SerializeField]
    private BossHPGauge bossHPGauge = null;
    public BossHPGauge BossHPGauge { get { return bossHPGauge; } }

    [SerializeField]
    private ItemGauge itemGauge = null;
    public ItemGauge ItemGauge { get { return itemGauge; } }

    [SerializeField]
    private DestinationUI destinationIcon = null;
    public DestinationUI DestinationIcon { get { return destinationIcon; } }
}
