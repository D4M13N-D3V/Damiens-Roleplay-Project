using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.EmergencyServices;
using client.Main.EmergencyServices.Police;
using client.Main.Users.Inventory;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Criminal.Drugs
{
    public class DrugSelling : BaseScript
    {
        public DrugSelling Instance;

        private bool _isSelling = false;
        private Random _random = new Random();

        private List<string> _callMessages = new List<string>()
        {
            "This guy over here is trying to sell me ",
            "Theres someone over here trying to sell ",
            "Some wierdo is trying to sell ",
            "Theres a drug dealer over here i think hes selling "
        };

        public DrugSelling()
        {
            Instance = this;
            API.DecorRegister("HasBoughtDrugs", 2);
            EventHandlers["StartSellingDrugs"] += new Action(DrugToggle);
            DrugSellingLogic();
        }

        private async Task DrugSellingLogic()
        {
            while (true)
            {
                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    var ped = Utility.Instance.ClosestPed.Handle;
                    var hasBought = API.DecorGetBool(ped, "HasBoughtDrugs");
                    if (API.DoesEntityExist(ped) && hasBought == false)
                    {
                        var randomChance = _random.Next(3);
                        if (randomChance == 0)
                        {
                            foreach (var drug in Enum.GetValues(typeof(DrugTypes)).Cast<DrugTypes>())
                            {
                                if (InventoryUI.Instance.HasItem(Convert.ToString(drug)) > 0)
                                {
                                    if (Police.Instance.CopCount >= 1)
                                    {
                                        Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                                        await Delay(1500);
                                        Game.PlayerPed.Task.ClearAll();
                                        TriggerServerEvent("SellItemByName", Convert.ToString(drug));
                                        API.DecorSetBool(ped, "HasBoughtDrugs", true);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Utility.Instance.SendChatMessage("[Drugs]","This person does not want to buy your drugs!",255,0,0);
                            API.DecorSetBool(ped, "HasBoughtDrugs", true);
                            API.SetPedScream(ped);
                            if (randomChance == 2)
                            {
                                TriggerEvent("911CallClientAnonymous", _callMessages[_random.Next(0, _callMessages.Count)]);
                            }
                        }
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage("[Drugs]", "Not enough police on.", 255, 0, 0);
                    }
                }
                await Delay(0);
            }
        }

        private async void DrugToggle()
        {
            if (_isSelling)
            {
                _isSelling = false;
                Game.PlayerPed.Task.ClearAll();
                await Delay(1000);
                Game.PlayerPed.Task.ClearAll();
                await Delay(1000);
                Game.PlayerPed.Task.ClearAll();
            }
            else
            {
                _isSelling = true;
                StartSellingDrugs();
            }
        }

        private async Task StartSellingDrugs()
        {
            await Delay(30000);
            DrugSellingAnim();
            while (_isSelling)
            {
                if (!API.IsPedUsingScenario(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD") && !API.IsPedUsingScenario(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER"))
                {
                    API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD", 0, true);
                }

                var ped = Utility.Instance.ClosestPed.Handle;
                var hasBought = API.DecorGetBool(ped, "HasBoughtDrugs");
                if (API.DoesEntityExist(ped) && hasBought == false)
                {
                    var randomChance = _random.Next(3);
                    if (randomChance == 0)
                    {
                        foreach (var drug in Enum.GetValues(typeof(DrugTypes)).Cast<DrugTypes>())
                        {
                            if (InventoryUI.Instance.HasItem(Convert.ToString(drug)) > 0)
                            {
                                if (Police.Instance.CopCount < 1)
                                {
                                    Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                                    await Delay(1500);
                                    Game.PlayerPed.Task.ClearAll();
                                    TriggerServerEvent("SellItemByName", Convert.ToString(drug));
                                    API.DecorSetBool(ped, "HasBoughtDrugs", true);
                                    break;
                                }
                                else
                                {
                                    Utility.Instance.SendChatMessage("[Drugs]", "Not enough police on.", 255, 0, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        API.DecorSetBool(ped, "HasBoughtDrugs", true);
                        API.SetPedScream(ped);
                        if (randomChance == 2)
                        {
                            TriggerEvent("911CallClientAnonymous", _callMessages[_random.Next(0, _callMessages.Count)]);
                        }
                    }
                }
                await Delay(1500);
            }
            Game.PlayerPed.Task.ClearAll();
            Game.PlayerPed.Task.ClearAll();
        }

        private void DrugSellingAnim()
        {
            var rdmAnim = _random.Next(2);
            if (rdmAnim == 0)
            {
                API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER", 0, true);
            }
            else
            {
                API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD", 0, true);
            }
        }
    }
}
