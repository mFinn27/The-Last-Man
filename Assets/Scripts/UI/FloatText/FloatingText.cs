using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeedY = 1.5f;
    public float thoiGianSongMacDinh = 0.8f;

    private TextMeshPro text;
    private float randomXSpeed;
    private float thoiGianSongHienTai;
    private float fontMacDinh;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        fontMacDinh = text.fontSize;
    }

    public void Setup(float dame, bool chiMang)
    {
        text.text = dame.ToString();
        text.color = chiMang ? Color.yellow : Color.white;

        text.fontSize = fontMacDinh;
        if (chiMang)
        {
            text.fontSize = fontMacDinh * 1.5f;
        }

        randomXSpeed = Random.Range(-1f, 1f);
        thoiGianSongHienTai = thoiGianSongMacDinh;
    }

    void Update()
    {
        transform.position += new Vector3(randomXSpeed, moveSpeedY, 0) * Time.deltaTime;
        transform.rotation = Quaternion.identity;
        thoiGianSongHienTai -= Time.deltaTime;

        if (thoiGianSongHienTai <= 0)
        {
            FloatingTextManager.Instance.ReturnText(gameObject);
        }
    }
}