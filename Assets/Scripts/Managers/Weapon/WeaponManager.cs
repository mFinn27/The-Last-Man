using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("--- TÚI ĐỒ VŨ KHÍ ---")]
    public int maxSlot = 5;
    public List<WeaponData> danhSachVuKhi = new List<WeaponData>();

    [Header("--- RENDER VẬT LÝ ---")]
    public Transform weaponPivot;
    public float khoangCachXepSung = 0.8f;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.vuKhiKhoiDauDangChon != null)
        {
            ThuMuaVuKhi(GameManager.Instance.vuKhiKhoiDauDangChon);
        }

        if (PlayerStats.Instance != null && PlayerStats.Instance.dataNhanVat != null)
        {
            var dsMacDinh = PlayerStats.Instance.dataNhanVat.danhSachVuKhiChoPhepChon;
            if (dsMacDinh != null && dsMacDinh.Count > 0)
            {
                foreach (var vk in dsMacDinh)
                {
                    ThuMuaVuKhi(vk);
                }
            }
        }
        CapNhatVuKhiTrenNguoi();
    }

    public bool ThuMuaVuKhi(WeaponData vuKhiMoi)
    {
        if (danhSachVuKhi.Count < maxSlot)
        {
            danhSachVuKhi.Add(vuKhiMoi);
            CapNhatVuKhiTrenNguoi();
            return true;
        }
        else
        {
            for (int i = 0; i < danhSachVuKhi.Count; i++)
            {
                if (KiemTraTheGhep(danhSachVuKhi[i], vuKhiMoi))
                {
                    danhSachVuKhi[i] = danhSachVuKhi[i].vuKhiCapTiepTheo;
                    CapNhatVuKhiTrenNguoi();
                    return true;
                }
            }
            return false;
        }
    }

    public void BanVuKhi(int indexSlot)
    {
        if (indexSlot >= 0 && indexSlot < danhSachVuKhi.Count)
        {
            PlayerStats.Instance.vangHienTai += danhSachVuKhi[indexSlot].giaBan;
            danhSachVuKhi.RemoveAt(indexSlot);
            CapNhatVuKhiTrenNguoi();
        }
    }

    public bool ThuGhepThuCong(int indexSlot)
    {
        WeaponData vuKhiDangChon = danhSachVuKhi[indexSlot];
        for (int i = 0; i < danhSachVuKhi.Count; i++)
        {
            if (i != indexSlot && KiemTraTheGhep(vuKhiDangChon, danhSachVuKhi[i]))
            {
                danhSachVuKhi[indexSlot] = vuKhiDangChon.vuKhiCapTiepTheo;
                danhSachVuKhi.RemoveAt(i);
                CapNhatVuKhiTrenNguoi();
                return true;
            }
        }
        return false;
    }

    public bool KiemTraTheGhep(WeaponData a, WeaponData b)
    {
        return a.tenMatHang == b.tenMatHang && a.capDo == b.capDo && a.vuKhiCapTiepTheo != null;
    }

    public bool CoTheGhepKhong(int indexSlot)
    {
        WeaponData vuKhiDangChon = danhSachVuKhi[indexSlot];
        for (int i = 0; i < danhSachVuKhi.Count; i++)
        {
            if (i != indexSlot && KiemTraTheGhep(vuKhiDangChon, danhSachVuKhi[i])) return true;
        }
        return false;
    }

    private void CapNhatVuKhiTrenNguoi()
    {
        foreach (Transform child in weaponPivot) { Destroy(child.gameObject); }

        int tongSoVuKhi = danhSachVuKhi.Count;
        float gocChia = 360f / (tongSoVuKhi > 0 ? tongSoVuKhi : 1);

        AutoAim aim = GetComponentInParent<AutoAim>();
        PlayerMovement move = GetComponentInParent<PlayerMovement>();

        for (int i = 0; i < tongSoVuKhi; i++)
        {
            WeaponData data = danhSachVuKhi[i];
            if (data != null && data.weaponPrefab != null)
            {
                GameObject sungMoi = Instantiate(data.weaponPrefab, weaponPivot);

                RangedWeapon r = sungMoi.GetComponentInChildren<RangedWeapon>();
                if (r != null) r.Setup(data, aim, move);

                ThrustWeapon t = sungMoi.GetComponentInChildren<ThrustWeapon>();
                if (t != null) t.Setup(data, aim, move);

                ArcMeleeWeapon a = sungMoi.GetComponentInChildren<ArcMeleeWeapon>();
                if (a != null) a.Setup(data, aim, move);

                float goc = i * gocChia;
                float radian = goc * Mathf.Deg2Rad;
                Vector3 viTriTuongDoi = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * khoangCachXepSung;
                sungMoi.transform.localPosition = viTriTuongDoi;
            }
        }

        if (ShopUI.Instance != null && ShopUI.Instance.panelShop.activeInHierarchy)
            ShopUI.Instance.CapNhatGiaoDienKhoVuKhi();
    }
}