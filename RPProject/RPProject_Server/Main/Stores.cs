    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main
{
    public class Stores : BaseScript
    {
        public Stores()
        {
            SetupTwentyFourSevenFood();
            SetupTwentyFourSevenDrink();
            SetupTwentyFourSevenCounter();
            SetupAmmunationStores();
            SetupPoliceStore();
            SetupEMSStore();
            SetupHardwareStore();
        }

        public async void SetupTwentyFourSevenDrink()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("Monster", "A monster energy drink", 0, 4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Redbull", "A can of energy drink that taste like mooose piss.", 0,
                4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Mtn-Dew-Kickstart",
                "A can of orange flavored kickstart energy drink.", 0, 4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Mtn-Dew", "A tooth rotting bottle of delicious mountain dew.", 0, 3,
                1, false);
            ItemManager.Instance.DynamicCreateItem("Lemonade", "A refreshing bottle lemonade.", 0, 3, 1, false);
            ItemManager.Instance.DynamicCreateItem("Pink-Lemonade", "A pink lemonade drink.", 0, 3, 1, false);
            ItemManager.Instance.DynamicCreateItem("Coke", "A cold coke.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("Pepsi", "A cold pepsi.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("Sprite", "A tasty clear soda.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("Juice", "A box of juice.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("Water", "A bottle of water.", 0, 1, 1, false);
        }

        public async void SetupTwentyFourSevenFood()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("SlimJim", "A stick of pure dried out meat.", 0, 5, 1, false);
            ItemManager.Instance.DynamicCreateItem("BeefJerky", "Bag of strips of seasoned dried out meat.", 0, 5, 1,
                false);
            ItemManager.Instance.DynamicCreateItem("PorkRinds", "A bag of pork rinds.", 0, 4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Cheetos", "A bag of cheetos.", 0, 4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Doritos", "A bag of doritos.", 0, 4, 1, false);
            ItemManager.Instance.DynamicCreateItem("Pistachios", "A bag of pistachios", 0, 3, 1, false);
            ItemManager.Instance.DynamicCreateItem("Doughnut", "A singular delicious doughnut.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("GummyBears", "A bag of gummy bears.", 0, 2, 1, false);
            ItemManager.Instance.DynamicCreateItem("IceCream", "A little tub of ice cream.", 0, 1, 1, false);
            ItemManager.Instance.DynamicCreateItem("Chocolate-Bar", "A bar of chocolate.", 0, 1, 1, false);
        }

        public async void SetupTwentyFourSevenCounter()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("Cigarette", "A pack of ciggirates.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Bobby-Pins",
                "Bobby pin that you can use for basic lock picking, has a low success rate.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Chew", "A can of chewing tobacco.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Dip", "A can ofdip.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Bandages", "Basic bandages.", 0, 10, 1, false);
        }

        public async void SetupAmmunationStores()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("Binoculars", "A pair of functional binoculars.", 0, 500, 1, false);
            ItemManager.Instance.DynamicCreateItem("SNS Pistol", "A firearm.", 0, 1200, 10, false);
            ItemManager.Instance.DynamicCreateItem("Pistol .50", "A firearm.", 0, 2500, 35, false);
            ItemManager.Instance.DynamicCreateItem("Pistol", "A firearm.", 0, 1500, 15, false);
            ItemManager.Instance.DynamicCreateItem("Combat Pistol", "A firearm.", 0, 2500, 20, false);
            ItemManager.Instance.DynamicCreateItem("Heavy Pistol", "A firearm.", 0, 2500, 30, false);
            ItemManager.Instance.DynamicCreateItem("Single Action Revolver", "A firearm.", 4500, 35, 1, false);
            ItemManager.Instance.DynamicCreateItem("Double Action Revolver", "A firearm.", 3500, 35, 1, false);
            ItemManager.Instance.DynamicCreateItem("Pump Shotgun", "A firearm.", 1200, 10, 50, false);
            ItemManager.Instance.DynamicCreateItem("Hunting Rifle", "A firearm.", 1200, 10, 50, false);
            ItemManager.Instance.DynamicCreateItem("Shotgun Ammo", "Ammunition.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Pistol Ammo", "Ammunition.", 0, 10, 1, false);
            ItemManager.Instance.DynamicCreateItem("Rifle Ammo", "Ammunition.", 0, 10, 1, false);
        }

        public async void SetupPoliceStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("Binoculars(P)", "A pair of functional binoculars engraved with SASP on the side.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Riot Shield(P)", "A riot shield with SASP engraved on it..", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Hobblecuffs(P)", "A pair of handcuffs and ankel braclets with SASP engraved on them.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Handcuffs(P)", "A pair of handcuffs with SASP engraved on them", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Tazer(P)", "A tazer with SASP marked on the side.", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Nighstick(P)", "A nighstick with SASP marked on the side.", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Combat Pistol(P)", "A glock with SASP engraved on the slide.", 0, 0, 20, true);
            ItemManager.Instance.DynamicCreateItem("Pump Shotgun(P)", "A shotgun with SASP engraved on the slide.", 0, 0, 100, true);
            ItemManager.Instance.DynamicCreateItem("Carbine Rifle(P)", "A carbine rifle with SASP engraved on the slide.", 0, 0, 120, true);
            ItemManager.Instance.DynamicCreateItem("Fingerprint Scanner(P)", "A police issued digital fingerprint scanner to retrieve peoples identitys.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Spike Strips(P)", "Police issued spike strips for disabling vehicles tires.", 0, 0, 50, true);
            ItemManager.Instance.DynamicCreateItem("Police Lock Tool(P)", "A tool issued to police to unlock vehicles.", 0, 0, 50, true);
            ItemManager.Instance.DynamicCreateItem("Bandages(P)", "Bandages for minor healing.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("Scuba Gear(P)", "Scuba gear for diving.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Medical Supplies(P)", "Medical supplies for reviving someone", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Pain Killers(P)", "Bottle of pain killers to help post injury problem.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("First Aid Kit(P)", "First aid kit to replenish all health.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("GSR Kit(P)", "Kit to check if someone has fired a weapon recently.", 0, 0, 1, false);
        }

        public async void SetupEMSStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Bandages(EMS)", "Bandages for minor healing.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("Scuba Gear(EMS)", "Scuba gear for diving.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Medical Supplies(EMS)", "Medical supplies for reviving someone", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Pain Killers(EMS)", "Bottle of pain killers to help post injury problem.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("First Aid Kit(EMS)", "First aid kit to replenish all health.", 0, 0, 1, false);
        }

        public async void SetupHardwareStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Scuba Gear", "Scuba gear for diving.", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Lockpick", "Lockpick that you can use to break into things.", 0, 0, 5, true);
            ItemManager.Instance.DynamicCreateItem("Knife", "A weapon.", 25, 25, 5, true);
            ItemManager.Instance.DynamicCreateItem("Hammer", "A weapon.", 25, 25, 5, true);
            ItemManager.Instance.DynamicCreateItem("Fireaxe", "A weapon.", 60, 60, 15, true);
            ItemManager.Instance.DynamicCreateItem("Crowbar", "A weapon.", 20, 20, 5, true);
            ItemManager.Instance.DynamicCreateItem("Bottle", "A weapon.", 5, 5, 5, true);
            ItemManager.Instance.DynamicCreateItem("Dagger", "A weapon.", 40, 40, 10, true);
            ItemManager.Instance.DynamicCreateItem("Hatchet", "A weapon.", 60, 60, 10, true);
            ItemManager.Instance.DynamicCreateItem("Machete", "A weapon.", 60, 60, 10, true);
            ItemManager.Instance.DynamicCreateItem("Pool Cue", "A weapon.", 60, 60, 8, true);
            ItemManager.Instance.DynamicCreateItem("Wrench", "A weapon.", 30, 30, 20, true);
            ItemManager.Instance.DynamicCreateItem("Switchblade", "A weapon.", 25, 25, 5, true);
            ItemManager.Instance.DynamicCreateItem("Brass Knuckles", "A weapon.", 40, 40, 5, true);
        }
    }
}
