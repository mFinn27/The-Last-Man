using UnityEngine;
using System.Collections.Generic;

public class AutoAim : MonoBehaviour
{
    [Header("Stats Set Up")]
    public float tamDanh = 7f;
    public LayerMask enemyLayer;
    [SerializeField] private float thoiGianMoiLanQuet = 0.1f;

    public Transform mucTieuHienTai { get; private set; }

    private List<Collider2D> ketQuaDanhSachEnemy = new List<Collider2D>();
    private ContactFilter2D fil;

    void Start()
    {
        fil = new ContactFilter2D();
        fil.SetLayerMask(enemyLayer);
        fil.useLayerMask = true;
        fil.useTriggers = true;

        InvokeRepeating(nameof(FindClosestEnemy), 0f, thoiGianMoiLanQuet);
    }

    private void FindClosestEnemy()
    {
        int count = Physics2D.OverlapCircle(transform.position, tamDanh, fil, ketQuaDanhSachEnemy);

        float khoangCachGanNhat = Mathf.Infinity;
        Transform mucTieuGanNhat = null;

        for (int i = 0; i < count; i++)
        {
            float khoangCach = (ketQuaDanhSachEnemy[i].transform.position - transform.position).sqrMagnitude;
            if (khoangCach < khoangCachGanNhat)
            {
                khoangCachGanNhat = khoangCach;
                mucTieuGanNhat = ketQuaDanhSachEnemy[i].transform;
            }
        }
        mucTieuHienTai = mucTieuGanNhat;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamDanh);
    }
}