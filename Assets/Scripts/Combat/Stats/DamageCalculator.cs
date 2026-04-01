using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(float dameVuKhi, float tiLeChiMangVuKhi, float satThuongChiMangVuKhi, out bool coChiMang)
    {
        float bonusDame = PlayerStats.Instance != null ? PlayerStats.Instance.GetDamage() : 0f;
        float bonusTiLeChiMang = PlayerStats.Instance != null ? PlayerStats.Instance.GetCritChance() : 0f;
        float bonusSatThuongChiMang = PlayerStats.Instance != null ? PlayerStats.Instance.GetCritDamage() : 0f;

        float tiLeChiMangCuoiCung = Mathf.Clamp01(tiLeChiMangVuKhi + bonusTiLeChiMang);
        coChiMang = Random.value <= tiLeChiMangCuoiCung;

        float dameCuoiCung = dameVuKhi + bonusDame;

        if (coChiMang)
        {
            float satThuongChiMangCuoiCung = satThuongChiMangVuKhi + bonusSatThuongChiMang;
            dameCuoiCung *= satThuongChiMangCuoiCung;
        }

        return dameCuoiCung;
    }

    public static float CalculateAttackSpeed(float tocDoDanhVuKhi, WeaponData data = null)
    {
        float bonusTocDoDanh = PlayerStats.Instance != null ? PlayerStats.Instance.GetAttackSpeed() : 0f;
        float tocDoDanhCuoi = tocDoDanhVuKhi * (1f + bonusTocDoDanh);

        if (data != null && data.coGioiHanTocDoDanh)
        {
            return Mathf.Min(tocDoDanhCuoi, data.tocDoDanhToiDa);
        }

        return Mathf.Min(tocDoDanhCuoi, 3.5f);
    }

    public static float CalculateLifeSteal(float hutMauVuKhi)
    {
        float bonusHutMau = PlayerStats.Instance != null ? PlayerStats.Instance.GetLifeSteal() : 0f;
        float hutMauCuoi = hutMauVuKhi + bonusHutMau;
        return Mathf.Min(hutMauCuoi, 0.4f);
    }
}