using UnityEngine;
using TMPro;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    public enum GunType { Pistol, Rifle, Shotgun }
    public GunType currentGun = GunType.Pistol;

    [Header("Gun Stats")]
    // Each index = Pistol, Rifle, Shotgun
    public float[] fireRates = { 0.2f, 0.4f, 0.8f };
    public int[] damages = { 1, 2, 4 };
    public int[] ammo = { 60, 40, 20 };
    public int[] maxAmmo = { 60, 40, 20 };
    public int[] bulletCounts = { 1, 1, 3 };   // shotgun fires 3
    public float[] spreadAngles = { 0f, 0f, 15f }; // shotgun spread

    [Header("UI")]
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI ammoText;

    void Awake() { instance = this; }

    void Update()
    {
        // Q = previous gun, E = next gun
        if (UnityEngine.InputSystem.Keyboard.current.qKey.wasPressedThisFrame)
            SwitchGun(-1);
        if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            SwitchGun(1);

        UpdateUI();
    }

    void SwitchGun(int dir)
    {
        int count = System.Enum.GetValues(typeof(GunType)).Length;
        int next = ((int)currentGun + dir + count) % count;
        currentGun = (GunType)next;
    }

    public float GetFireRate() => fireRates[(int)currentGun];
    public int GetDamage() => damages[(int)currentGun];
    public int GetBulletCount() => bulletCounts[(int)currentGun];
    public float GetSpread() => spreadAngles[(int)currentGun];

    public bool HasAmmo()
    {
        return ammo[(int)currentGun] > 0;
    }

    public void UseAmmo()
    {
        ammo[(int)currentGun]--;
        if (ammo[(int)currentGun] < 0)
            ammo[(int)currentGun] = 0;
    }

    public void AddAmmo(int amount)
    {
        int i = (int)currentGun;
        ammo[i] = Mathf.Min(ammo[i] + amount, maxAmmo[i]);
    }

    void UpdateUI()
    {
        if (gunNameText != null)
            gunNameText.text = currentGun.ToString().ToUpper();

        if (ammoText != null)
            ammoText.text = ammo[(int)currentGun] +
                            " / " + maxAmmo[(int)currentGun];
    }
}