using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : ParticleObject
{
    public float bulletLifeTime = 1;

    protected Rigidbody myRb = null;
    protected Collider bulletCollider = null;

    protected bool isCollisioned = false;
    protected int attackPower = 0;

    protected AudioSource magicBulletSound = null;
    protected List<GameObject> damagedObjectList = new List<GameObject>();

    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();

        myRb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();

        lifeTime = bulletLifeTime;
    }

    protected void Start()
    {
        magicBulletSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeTime();
    }

    public override void EnableObject()
    {
        base.EnableObject();
        bulletCollider.enabled = true;
    }

    public override void DisableObject()
    {
        base.DisableObject();
        damagedObjectList.Clear();
    }

    /// <summary>
    /// ���@�e�𔭎˂���
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="attackPower"></param>
    /// <param name="lifeTime"></param>
    public void Shot(Vector3 position, Vector3 velocity, int attackPower, float lifeTime = 1.0f)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(velocity);

        this.attackPower = attackPower;
        this.myRb.velocity = velocity;
        this.lifeTime = lifeTime;

        if (magicBulletSound != null)
        {
            magicBulletSound.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionAction(collision);

        particle.Stop();
        myRb.velocity = Vector3.zero;
        bulletCollider.enabled = false;
        isCollisioned = true;
    }

    protected override void OnTimeOut()
    {
        base.OnTimeOut();
        if (!isCollisioned)
        {
            // �����������Ă��Ȃ��ꍇ�ɁA���ł����ꏊ�Ƀp�[�e�B�N���𐶐�����
            PlayCollsionParticle();
        }
    }

    /// <summary>
    /// �Փ˂����ۂ̏���
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionAction(Collision collision)
    {
        if (magicBulletSound != null)
        {
            magicBulletSound.Stop();
        }
        // �Փ˂����ꏊ�Ƀp�[�e�B�N���𐶐�����
        PlayCollsionParticle();
    }

    /// <summary>
    /// �Փ˂����ۂɏo��p�[�e�B�N���𐶐�����
    /// </summary>
    protected virtual void PlayCollsionParticle() { }
}
