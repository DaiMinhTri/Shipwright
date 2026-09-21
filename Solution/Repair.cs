using HarmonyLib;
using UnityEngine;

namespace Shipwright.Solution;

public static class Repair
{
    public static bool m_isSecondary;

    private static (string material, int amount) GetShipConfig(Piece piece)
    {
        return piece.m_name switch
        {
            "$ship_karve" => (ShipwrightPlugin._karveMaterial.Value, ShipwrightPlugin._karveMaterialAmount.Value),
            "$ship_longship" => (ShipwrightPlugin._longshipMaterial.Value, ShipwrightPlugin._longshipMaterialAmount.Value),
            "$ship_longship_ashlands" => (ShipwrightPlugin._drakkarMaterial.Value, ShipwrightPlugin._drakkarMaterialAmount.Value),
            _ => (ShipwrightPlugin._defaultMaterial.Value, ShipwrightPlugin._defaultMaterialAmount.Value),
        };
    }

    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
    private static class Humanoid_StartAttack_Patch
    {
        private static bool Prefix(Humanoid __instance, bool secondaryAttack, ref bool __result)
        {
            if (__instance is not Player player) return true;
            ItemDrop.ItemData? toolItem = player.m_rightItem;
            if (toolItem == null) return true;
            m_isSecondary = secondaryAttack;
            if (!IsCorrectTool(toolItem))
            {
                ResetDrawTime(toolItem);
                return true;
            }
            __result = false;
            if (secondaryAttack && ShipwrightPlugin._canDeconstruct.Value is ShipwrightPlugin.Toggle.Off) return false;
            Piece? hoveringPiece = player.m_hoveringPiece;
            if (hoveringPiece == null)
            {
                player.Message(MessageHud.MessageType.Center, "$msg_missinghoverpiece");
                return false;
            }
            if (hoveringPiece.m_name == "$ship_raft") return false;
            if (!hoveringPiece.TryGetComponent(out WearNTear component)) return false;
            var currentHealth = component.m_nview.GetZDO().GetFloat(ZDOVars.s_health, component.m_health);

            if (!secondaryAttack)
            {
                if (currentHealth >= component.m_health)
                {
                    ResetDrawTime(toolItem);
                    return false;
                }

                if (!CanRepair(player, hoveringPiece))
                {
                    ResetDrawTime(toolItem);
                    return false;
                }
            }
            if (!IsLoaded(toolItem)) return false;

            if (!secondaryAttack)
            {
                if (!UseMaterial(player, hoveringPiece)) return false;
                var repairAmount = component.m_health * ShipwrightPlugin._repairAmount.Value;
                var newHealth = Mathf.Clamp(currentHealth + repairAmount, 1f, component.m_health);
                RepairAmount(component, newHealth);
                player.Message(MessageHud.MessageType.TopLeft, Localization.instance.Localize("$msg_shiphealth: " + $" {(int)newHealth}/{(int)component.m_health}"));
            }
            else
            {
                DeconstructShip(hoveringPiece, component, player);
            }

            UseTool(hoveringPiece, player, toolItem);
            __result = true;
            return false;
        }
    }

    public static void UpdateHammerDraw()
    {
        if (!Player.m_localPlayer) return;
        var toolItem = Player.m_localPlayer.m_rightItem;
        if (toolItem == null) return;
        if (!IsCorrectTool(toolItem)) return;
        if (Player.m_localPlayer is { m_attack: false, m_attackHold: false, m_secondaryAttack: false, m_secondaryAttackHold: false }) ResetDrawTime(toolItem);
    }

    private static bool IsCorrectTool(ItemDrop.ItemData toolItem) => toolItem.m_shared.m_name == "$item_hammerbucket";
    public static float GetRepairDrawPercentage(ItemDrop.ItemData tool, bool secondary) => Mathf.Clamp01(tool.m_shared.m_attack.m_attackDrawPercentage /
        (secondary ? ShipwrightPlugin._deconstructDuration.Value : ShipwrightPlugin._repairDuration.Value));
    public static void ResetDrawTime(ItemDrop.ItemData tool) => tool.m_shared.m_attack.m_attackDrawPercentage = 0f;

    private static bool CanRepair(Player player, Piece piece)
    {
        if (!HasMaterial(player, piece)) return false;
        if (!player.HaveStamina(ShipwrightPlugin._staminaCost.Value)) return false;
        if (player.InAttack() && player.HaveQueuedChain()) return false;
        if (player.InDodge() || !player.CanMove() || player.IsKnockedBack() || player.IsStaggering() || player.InMinorAction()) return false;

        return true;
    }

    private static bool HasMaterial(Player player, Piece piece)
    {
        var (matName, matAmount) = GetShipConfig(piece);
        if (matAmount == 0) return true;
        ItemDrop mat = GetUseMaterial(matName);
        var name = mat.m_itemData.m_shared.m_name;
        if (!player.GetInventory().HaveItem(name))
        {
            player.Message(MessageHud.MessageType.Center, "$msg_missingmat: " + $" {name}");
            return false;
        }

        var playerAmount = player.GetInventory().CountItems(name);
        if (playerAmount < matAmount)
        {
            player.Message(MessageHud.MessageType.Center, "$msg_missingmat: " + $" {matAmount}x {name}");
            return false;
        }

        return true;
    }

    private static bool UseMaterial(Player player, Piece piece)
    {
        var (matName, matAmount) = GetShipConfig(piece);
        if (matAmount == 0) return true;
        if (!HasMaterial(player, piece)) return false;
        ItemDrop mat = GetUseMaterial(matName);
        var name = mat.m_itemData.m_shared.m_name;
        player.GetInventory().RemoveItem(name, matAmount);
        return true;
    }

    private static ItemDrop GetUseMaterial(string materialName)
    {
        GameObject material = ZNetScene.instance.GetPrefab(materialName);
        var mat = !material ? ZNetScene.instance.GetPrefab("Wood") : material;
        return mat.GetComponent<ItemDrop>();
    }

    private static void DeconstructShip(Piece piece, WearNTear component, Player player)
    {
        float returnAmount = ShipwrightPlugin._deconstructReturnAmount.Value;

        if (returnAmount > 0f && piece.m_resources != null)
        {
            foreach (var req in piece.m_resources)
            {
                if (req?.m_resItem == null) continue;
                int baseAmount = req.m_amount;
                int dropAmount = Mathf.CeilToInt(baseAmount * returnAmount);
                if (dropAmount > 0)
                {
                    var prefab = req.m_resItem.gameObject;
                    player.GetInventory().AddItem(prefab, dropAmount);
                }
            }
        }

        component.Destroy();
    }

    private static void UseTool(Piece hoveringPiece, Player player, ItemDrop.ItemData toolItem)
    {
        player.FaceLookDirection();
        player.m_zanim.SetTrigger(toolItem.m_shared.m_attack.m_attackAnimation);

        var transform = hoveringPiece.transform;
        if (ShipwrightPlugin._usePlaceEffects.Value is ShipwrightPlugin.Toggle.On) hoveringPiece.m_placeEffect.Create(transform.position, transform.rotation);
        player.UseStamina(ShipwrightPlugin._staminaCost.Value);
        var transform1 = player.transform;
        toolItem.m_shared.m_triggerEffect.Create(transform1.position, transform1.rotation);
        if (ShipwrightPlugin._useDurability.Value is ShipwrightPlugin.Toggle.Off) return;
        toolItem.m_durability -= toolItem.m_shared.m_useDurabilityDrain;
    }

    private static void RepairAmount(WearNTear component, float amount)
    {
        component.m_nview.GetZDO().Set(ZDOVars.s_health, amount);
        component.m_nview.InvokeRPC(nameof(WearNTear.RPC_HealthChanged), amount);
    }

    private static bool IsLoaded(ItemDrop.ItemData tool)
    {
        tool.m_shared.m_attack.m_attackDrawPercentage += Time.deltaTime * tool.m_quality;
        if (GetRepairDrawPercentage(tool, m_isSecondary) < 1f) return false;
        tool.m_shared.m_attack.m_attackDrawPercentage = 0f;
        return true;
    }
}
