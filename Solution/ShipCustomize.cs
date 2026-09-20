using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Shipwright.Solution;

public class ShipCustomize : MonoBehaviour
{
    public static readonly List<ShipCustomize> m_instances = new();

    private GameObject m_customize = null!;
    private GameObject m_shipTentBeam = null!;
    private GameObject m_shipTent = null!;
    private GameObject m_shipTentHolders1 = null!;
    private GameObject m_shipTentHolders2 = null!;
    private GameObject m_traderLamp = null!;
    private GameObject m_storage = null!;
    private readonly List<GameObject> m_crates = new();
    private readonly List<GameObject> m_shields = new();
    public void Awake()
    {
        var objects = transform.Find("ship/visual/Customize");
        if (objects == null) return;
        m_customize = objects.gameObject;
        var tentBeam = objects.Find("ShipTen2_beam");
        if (tentBeam == null) return;
        m_shipTentBeam = tentBeam.gameObject;
        var tent = objects.Find("ShipTen2 (1)");
        if (tent == null) return;
        m_shipTent = tent.gameObject;
        var holders1 = objects.Find("ShipTentHolders");
        if (holders1 == null) return;
        m_shipTentHolders1 = holders1.gameObject;
        var holders2 = objects.Find("ShipTentHolders (1)");
        if (holders2 == null) return;
        m_shipTentHolders2 = holders2.gameObject;
        var lamp = objects.Find("TraderLamp");
        if (lamp == null) return;
        m_traderLamp = lamp.gameObject;
        var storage = objects.Find("storage");
        if (storage == null) return;
        m_storage = storage.gameObject;
        foreach (Transform obj in m_storage.transform)
        {
            if (obj.name.StartsWith("Shield"))
            {
                m_shields.Add(obj.gameObject);
            }
            else
            {
                m_crates.Add(obj.gameObject);
            }
        }
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        DisableAll();
        
        m_instances.Add(this);
        
        SetCustomize(ShipwrightPlugin._useShipCustomize.Value is ShipwrightPlugin.Toggle.On);
        SetTent(ShipwrightPlugin._useShipTent.Value is ShipwrightPlugin.Toggle.On);
        SetLamp(ShipwrightPlugin._useTraderLamp.Value is ShipwrightPlugin.Toggle.On);
        SetStorage(ShipwrightPlugin._useStorage.Value is ShipwrightPlugin.Toggle.On);
        SetShields(ShipwrightPlugin._useShields.Value is ShipwrightPlugin.Toggle.On);
    }

    public void SetCustomize(bool enable)
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        m_customize.SetActive(enable);
    }

    public void SetTent(bool enable)
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        m_shipTentBeam.SetActive(enable);
        m_shipTent.SetActive(enable);
        m_shipTentHolders1.SetActive(enable);
        m_shipTentHolders2.SetActive(enable);
    }

    public void SetLamp(bool enable)
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        m_traderLamp.SetActive(enable);
    }

    public void SetStorage(bool enable)
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        foreach (var item in m_crates) item.SetActive(enable);
    }

    public void SetShields(bool enable)
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        foreach (var item in m_shields) item.SetActive(enable);
    }

    public void DisableAll()
    {
        if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
        SetTent(false);
        SetLamp(false);
        SetStorage(false);
        SetShields(false);
    }

    public void OnDestroy() => m_instances.Remove(this);
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZNetScene_Awake_Patch
    {
        private static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;
            if (ShipwrightPlugin.m_balrondShipyardInstalled) return;
            var vikingShip = __instance.GetPrefab("VikingShip");
            if (!vikingShip) return;
            vikingShip.AddComponent<ShipCustomize>();
        }
    }
}