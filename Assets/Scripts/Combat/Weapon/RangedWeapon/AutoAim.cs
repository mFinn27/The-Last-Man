using UnityEngine;
using System.Collections.Generic;

public class AutoAim : MonoBehaviour
{
    [Header("--- RADAR TOÀN BẢN ĐỒ ---")]
    public float banKinhQuet = 20f;
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
        int count = Physics2D.OverlapCircle(transform.position, banKinhQuet, fil, ketQuaDanhSachEnemy);

        float khoangCachGanNhat = Mathf.Infinity;
        Transform mucTieuGanNhat = null;

        for (int i = 0; i < count; i++)
        {
            Collider2D col = ketQuaDanhSachEnemy[i];
            if (col == null || !col.gameObject.activeInHierarchy) continue;

            if (!col.CompareTag("Enemy")) continue;

            float khoangCach = (col.transform.position - transform.position).sqrMagnitude;
            if (khoangCach < khoangCachGanNhat)
            {
                khoangCachGanNhat = khoangCach;
                mucTieuGanNhat = col.transform;
            }
        }
        mucTieuHienTai = mucTieuGanNhat;
    }
}