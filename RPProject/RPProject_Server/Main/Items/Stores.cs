using System.Threading.Tasks;
using CitizenFX.Core;

namespace server.Main.Items
{
    public class Stores : BaseScript
    {
        public Stores()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupTwentyFourSevenFood();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupTwentyFourSevenDrink();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupTwentyFourSevenCounter();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupAmmunationStores();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupPoliceStore();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupEMSStore();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupHardwareStore();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        public async Task SetupTwentyFourSevenDrink()
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

        public async Task SetupTwentyFourSevenFood()
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

        public async Task SetupTwentyFourSevenCounter()
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
            ItemManager.Instance.DynamicCreateItem("Bandages", "A can ofdip.", 0, 10, 1, false);
        }

        public async Task SetupAmmunationStores()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Binoculars", "A pair of functional binoculars.", 0, 500, 1, false);
            ItemManager.Instance.DynamicCreateItem("SNS Pistol", "A firearm.", 5000, 5000, 10, false, true);
            ItemManager.Instance.DynamicCreateItem("Pistol .50", "A firearm.", 8000, 8000, 15, false, true);
            ItemManager.Instance.DynamicCreateItem("Pistol", "A firearm.", 0, 6000, 6000, false, true);
            ItemManager.Instance.DynamicCreateItem("Combat Pistol", "A firearm.", 7000, 7000, 15, false, true);
            ItemManager.Instance.DynamicCreateItem("Heavy Pistol", "A firearm.", 7500, 7500, 15, false, true);
            ItemManager.Instance.DynamicCreateItem("Single Action Revolver", "A firearm.", 12000, 12000, 35, false, true);
            ItemManager.Instance.DynamicCreateItem("Double Action Revolver", "A firearm.", 15000, 15000, 35, false, true);
            ItemManager.Instance.DynamicCreateItem("Pump Shotgun", "A firearm.", 20000, 20000, 25, false, true);
            ItemManager.Instance.DynamicCreateItem("Hunting Rifle", "A firearm.", 30000, 30000, 25, false,true);
            ItemManager.Instance.DynamicCreateItem("Shotgun Ammo", "Ammunition.", 0, 10, 0, false, true);
            ItemManager.Instance.DynamicCreateItem("Pistol Ammo", "Ammunition.", 0, 10, 0, false, true);
            ItemManager.Instance.DynamicCreateItem("Rifle Ammo", "Ammunition.", 0, 10, 0, false, true);
        }

        public async Task SetupPoliceStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }

            ItemManager.Instance.DynamicCreateItem("Binoculars(P)", "A pair of functional binoculars engraved with SASP on the side.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Body Armor(P)", "Police body armor.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Flares(P)", "Flares.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Flashlight", "Flashlight.", 25, 25, 2, true);
            ItemManager.Instance.DynamicCreateItem("Riot Shield(P)", "A riot shield with SASP engraved on it..", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Hobblecuffs(P)", "A pair of handcuffs and ankel braclets with SASP engraved on them.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Handcuffs(P)", "A pair of handcuffs with SASP engraved on them", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Tazer(P)", "A tazer with SASP marked on the side.", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Nighstick(P)", "A nighstick with SASP marked on the side.", 0, 0, 25, true);
            ItemManager.Instance.DynamicCreateItem("Combat Pistol(P)", "A glock with SASP engraved on the slide.", 0, 0, 20, true);
            ItemManager.Instance.DynamicCreateItem("Pump Shotgun(P)", "A shotgun with SASP engraved on the slide.", 0, 0, 100, true);
            ItemManager.Instance.DynamicCreateItem("Carbine Rifle(P)", "A carbine rifle with SASP engraved on the slide.", 0, 0, 120, true);
            ItemManager.Instance.DynamicCreateItem("Fingerprint Scanner(P)", "A police issued digital fingerprint scanner to retrieve peoples identitys.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Spike Strips(P)", "Police issued spike strips for disabling vehicles tires.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Police Lock Tool(P)", "A tool issued to police to unlock vehicles.", 0, 0, 0, true);
            ItemManager.Instance.DynamicCreateItem("Bandages(P)", "Bandages for minor healing.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("Scuba Gear(P)", "Scuba gear for diving.", 0, 0, 15, true);
            ItemManager.Instance.DynamicCreateItem("Medical Supplies(P)", "Medical supplies for reviving someone", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Pain Killers(P)", "Bottle of pain killers to help post injury problem.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("First Aid Kit(P)", "First aid kit to replenish all health.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("GSR Kit(P)", "Kit to check if someone has fired a weapon recently.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Fishing License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Boar Hunting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Hunting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Deer Hunting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Mountain Lion Hunting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Rabbit Hunting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Diving License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Mining License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Woodcutting License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Boating License", "License.", 0, 0, 0, false);
            ItemManager.Instance.DynamicCreateItem("Trail Pass", "License.", 0, 0, 0, false);
        }

        public async Task SetupEMSStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Bandages(EMS)", "Bandages for minor healing.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("Scuba Gear(EMS)", "Scuba gear for diving.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Binoculars(EMS)", "Scuba gear for diving.", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Medical Supplies(EMS)", "Medical supplies for reviving someone", 0, 0, 1, true);
            ItemManager.Instance.DynamicCreateItem("Pain Killers(EMS)", "Bottle of pain killers to help post injury problem.", 0, 0, 1, false);
            ItemManager.Instance.DynamicCreateItem("First Aid Kit(EMS)", "First aid kit to replenish all health.", 0, 0, 1, false);
        }
        public async Task SetupHardwareStore()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Scuba Gear", "Scuba gear for diving.", 15000, 15000, 25, false);
            ItemManager.Instance.DynamicCreateItem("Lockpick", "Lockpick that you can use to break into things.", 200, 200, 5, false);
            ItemManager.Instance.DynamicCreateItem("Knife", "A weapon.", 25, 25, 5, false);
            ItemManager.Instance.DynamicCreateItem("Hammer", "A weapon.", 25, 25, 5, false);
            ItemManager.Instance.DynamicCreateItem("Fireaxe", "A weapon.", 60, 60, 15, false);
            ItemManager.Instance.DynamicCreateItem("Crowbar", "A weapon.", 20, 20, 5, false);
            ItemManager.Instance.DynamicCreateItem("Bottle", "A weapon.", 5, 5, 5, false);
            ItemManager.Instance.DynamicCreateItem("Dagger", "A weapon.", 40, 40, 10, false);
            ItemManager.Instance.DynamicCreateItem("Hatchet", "A weapon.", 60, 60, 10, false);
            ItemManager.Instance.DynamicCreateItem("Machete", "A weapon.", 60, 60, 10, false);
            ItemManager.Instance.DynamicCreateItem("Pool Cue", "A weapon.", 60, 60, 8, false);
            ItemManager.Instance.DynamicCreateItem("Wrench", "A weapon.", 30, 30, 20, false);
            ItemManager.Instance.DynamicCreateItem("Switchblade", "A weapon.", 25, 25, 5, false);
            ItemManager.Instance.DynamicCreateItem("Brass Knuckles", "A weapon.", 40, 40, 5, false);
            ItemManager.Instance.DynamicCreateItem("Zipties", "Zipties.", 100, 100, 5, false);
        }
    }
}
