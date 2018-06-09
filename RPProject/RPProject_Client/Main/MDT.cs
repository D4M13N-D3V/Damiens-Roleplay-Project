using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main
{
    public class MDT : BaseScript
    {

        public static MDT Instance;
        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(459.74072265625f, -989.10797119141f, 24.914863586426f),
            new Vector3(1853.1198730469f, 3690.1137695313f, 34.267044067383f),
            new Vector3(-449.55703735352f, 6012.3388671875f, 31.716371536255f)
        };
        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public MDT()
        {
            Instance = this;
            GarageCheck();
            DrawMarkers();
        }


        private async void DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(255, 0, 0, 255));
                    }
                }
                await Delay(0);
            }
        }

        private async void GarageCheck()
        {
            while (true)
            {
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (!MenuRestricted && dist < 1.5f || Game.PlayerPed.IsInPoliceVehicle && Game.PlayerPed.SeatIndex == VehicleSeat.Driver || Game.PlayerPed.SeatIndex == VehicleSeat.Passenger)
                    {
                        _menuOpen = true;
                    }
                }
                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        InteractionMenu.Instance._interactionMenu, "Police Computer", "Access your police computer.");
                    var _paperworkMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        _menu, "Paperwork", "Warrants, Arrests, Bolos");
                    var bookingButton = new UIMenuItem("Submit Crime Paperwork", "Submit your booking paperwork.");
                    var ticketButton = new UIMenuItem("Submit Ticket Paperwork", "Submit your ticket paperwork.");
                    var warrantButton = new UIMenuItem("Submit Warrant", "Submit your warrant paperwork.");
                    var removeWarrantButton = new UIMenuItem("Remove Warrant", "Submit your warrant paperwork.");
                    var boloButton = new UIMenuItem("Submit Bolo", "Submit your bolo paperwork.");
                    var removeBoloButton = new UIMenuItem("Remove Bolo", "Submit your bolo paperwork.");

                    var _searchMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        _menu, "Database", "Search Warrants, Civlians, Vehicles, Arrests");
                    var warrantSearchButton = new UIMenuItem("Search For Warrants By Name", "Search the NCIC Database for a matching person for warrants.");
                    var boloSearchButton = new UIMenuItem("Search For Warrants By Plate", "Search the NCIC Database for a matching bolo.");
                    var civlianSearchButton = new UIMenuItem("Search For Civlian By Name", "Search the NCIC Database for a matching person for criminal information.");
                    var bankStatementSearchButton = new UIMenuItem("Request Bank Statement From Banks", "Request bank statement from the banks. Requires a warrant.");
                    var plateVehicleSearchButton = new UIMenuItem("Search For Vehicle By Plate", "Search the DMV Database for a vehicle with a matching plate number.");
                    var ownerVehicleSearchSearchButton = new UIMenuItem("Search For Vehicle By Owner", "Search the DMV Database for a vehicle with a matching registered owner.");
                    var modelVehicleSearchButton = new UIMenuItem("Search For BOLO By Make/Model", "Search the DMV Database for a vehicle with a make/model.");


                    _paperworkMenu.AddItem(bookingButton);
                    _paperworkMenu.AddItem(ticketButton);
                    _paperworkMenu.AddItem(warrantButton);
                    _paperworkMenu.AddItem(removeWarrantButton);
                    _paperworkMenu.AddItem(boloButton);
                    _paperworkMenu.AddItem(removeBoloButton);
                    _paperworkMenu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        if (selectedItem == bookingButton)
                        {
                            BookingPaperworkFunctionality();
                        }
                        else if (selectedItem == ticketButton)
                        {
                            TicketPaperworkFunctionality();
                        }
                        else if (selectedItem == warrantButton)
                        {
                            WarrantPaperworkFunctionality();
                        }
                        else if (selectedItem == removeWarrantButton)
                        {
                            WarrantRemovalPaperworkFunctionality();
                        }
                        else if (selectedItem == boloButton)
                        {
                            BoloPaperworkFunctionality();
                        }
                        else if (selectedItem == removeBoloButton)
                        {
                            BoloRemovalPaperworkFunctionality();
                        }
                    };

                    _searchMenu.AddItem(civlianSearchButton);
                    _searchMenu.AddItem(bankStatementSearchButton);
                    _searchMenu.AddItem(plateVehicleSearchButton);
                    _searchMenu.AddItem(ownerVehicleSearchSearchButton);
                    _searchMenu.AddItem(modelVehicleSearchButton);
                    _searchMenu.AddItem(warrantSearchButton);
                    _searchMenu.AddItem(boloSearchButton);
                    _searchMenu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        if (selectedItem == ownerVehicleSearchSearchButton)
                        {
                            OwnerVehicleSearchFunctionality();
                        }
                        else if (selectedItem == plateVehicleSearchButton)
                        {
                            PlateVehicleSearchFunctionality();
                        }
                        else if (selectedItem == bankStatementSearchButton)
                        {
                            BankStatementFunctionality();
                        }
                        else if (selectedItem == modelVehicleSearchButton)
                        {
                            ModelVehicleSearchFunctionality();
                        }
                        else if (selectedItem == boloSearchButton)
                        {
                            BoloSearchFunctionality();
                        }
                        else if (selectedItem == warrantSearchButton)
                        {
                            WarrantSearchFunctio();
                        }
                        else if (selectedItem == civlianSearchButton)
                        {
                            CivilianSearchFunctionality();
                        }
                    };

                    _menuCreated = true;
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                else if (!_menuOpen && _menuCreated)
                {
                    _menuCreated = false;
                    if (_menu.Visible)
                    {
                        _menu.Visible = false;
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        InteractionMenu.Instance._interactionMenu.Visible = true;
                    }

                    var i = 0;
                    foreach (var item in InteractionMenu.Instance._interactionMenu.MenuItems)
                    {
                        if (item == _menu.ParentItem)
                        {
                            InteractionMenu.Instance._interactionMenu.RemoveItemAt(i);
                            break;
                        }
                        i++;
                    }
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                await Delay(0);
            }
        }

        private void CivilianSearchFunctionality()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Utility.Instance.KeyboardInput("The first name of the person you are searching to find information on.", "", 10,
                delegate (string firstname)
                {
                    Utility.Instance.KeyboardInput("The last name of the person you are searching to find information on.", "", 10,
                        delegate (string lastname)
                        {
                            TriggerServerEvent("CivlianSearch",firstname,lastname);
                        });
                });
        }

        private void BankStatementFunctionality()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Utility.Instance.KeyboardInput("The id of the player you are checking the bank statement of.", "", 10,
                delegate (string id)
                {
                    int ID;
                    if (int.TryParse(id, out ID))
                    {
                        TriggerServerEvent("BankStatementRequest", ID);
                    }
                });
        }

        private void BoloSearchFunctionality()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Utility.Instance.KeyboardInput("The plate of the vehicle you are checking to see if there is a BOLO.","",10,
                delegate(string plate)
                {
                    TriggerServerEvent("BoloSearch",plate);
                });
        }

        private void WarrantSearchFunctio()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Utility.Instance.KeyboardInput("The name of the of the person you are checking to see if there is a warrant for.", "", 10,
                delegate (string name)
                {
                    TriggerServerEvent("WarrantSearch",name);
                });
        }

        private void OwnerVehicleSearchFunctionality()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Utility.Instance.KeyboardInput("The first name of the person your checking vehicles for.", "John", 100,
                delegate (string first) {
                    Utility.Instance.KeyboardInput("The Last name of the person your checking vehicles for.", "John", 100,
                        delegate (string last)
                        {
                            TriggerServerEvent("VehicleSearchByOwner", first, last);

                        });
                });
            _menu.Visible = true; ;
        }

        private void PlateVehicleSearchFunctionality()
        {
            Utility.Instance.KeyboardInput("The plate to search for.", "", 100,
                delegate (string plate)
                {
                    TriggerServerEvent("VehicleSearchByPlate", plate);

                });
        }

        private void ModelVehicleSearchFunctionality()
        {
            Utility.Instance.KeyboardInput("The model/make to search for.", "", 100,
                delegate (string model)
                {
                    TriggerServerEvent("VehicleSearchByModel", model);

                });
        }

        private void TicketPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("ID of the player that you are trying to book", "", 6, delegate (string id)
            {
                Utility.Instance.KeyboardInput("The amount of money to fine the suspect.", "", 6, delegate (string fineamount)
                {
                    Utility.Instance.KeyboardInput("The charges the suspect is being charged with.", "", 5000, delegate (string charges)
                    {
                        int fineAmount;
                        if (!int.TryParse(fineamount, out fineAmount))
                        {
                            fineAmount = 1;
                        }
                        int ID;
                        if (!int.TryParse(id, out ID))
                        {
                            ID = 1;
                        }
                        TriggerServerEvent("TicketPaperwork", ID, charges, fineAmount );
                    });
                });
            });
        }


        private void BookingPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("ID of the player that you are trying to book", "", 6, delegate (string id)
            {
                Utility.Instance.KeyboardInput("The amount of jail time you are going to send the suspect to jail for.", "", 6, delegate (string jailtime)
                {
                    Utility.Instance.KeyboardInput("The amount of money to fine the suspect.", "", 6, delegate (string fineamount)
                    {
                        Utility.Instance.KeyboardInput("The charges the suspect is being charged with.", "", 5000, delegate (string charges)
                        {
                            int jailTime;
                            if (!int.TryParse(jailtime, out jailTime))
                            {
                                jailTime = 1;
                            }
                            int fineAmount;
                            if (!int.TryParse(fineamount, out fineAmount))
                            {
                                fineAmount = 1;
                            }
                            int ID;
                            if (!int.TryParse(id, out ID))
                            {
                                ID = 1;
                            }
                            TriggerServerEvent("ArrestPaperwork", ID, jailTime, fineAmount, charges);
                        });
                    });
                });
            });
        }

        private void WarrantPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("Enter the name who the warrant is for.", "", 5000, delegate (string name)
            {
                Utility.Instance.KeyboardInput("Enter the reason for the warrant.", "", 5000, delegate (string reason)
                {
                    Utility.Instance.KeyboardInput("Enter the evidence for the warrant", "", 5000,
                        delegate (string evidence)
                        {
                            Utility.Instance.KeyboardInput("Enter notes for the warrant", "", 5000, delegate (string notes)
                            {
                                TriggerServerEvent("WarrantPaperwork", name, reason, evidence, notes);
                            });
                        });
                });
            });
        }

        private void WarrantRemovalPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("Enter the warrant number for the warrant you want to removed.", "", 10,
                delegate (string warrantId)
                {
                    int warrantID;
                    if (!int.TryParse(warrantId, out warrantID))
                    {
                        warrantID = -1;
                    }
                    TriggerServerEvent("WarrantRemovalPaperwork", warrantID);
                });
        }

        private void BoloPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("Enter the plate the bolo is for.", "", 5000, delegate (string name)
            {
                Utility.Instance.KeyboardInput("Enter the reason for the bolo.", "", 5000, delegate (string reason)
                {
                    Utility.Instance.KeyboardInput("Enter the evidence for the bolo", "", 5000,
                        delegate (string evidence)
                        {
                            Utility.Instance.KeyboardInput("Enter description of the vehicle for the bolo", "", 5000, delegate (string notes)
                            {
                                TriggerServerEvent("BoloPaperwork", name, reason, evidence, notes);
                            });
                        });
                });
            });
        }

        private void BoloRemovalPaperworkFunctionality()
        {
            Utility.Instance.KeyboardInput("Enter the bolo number for the warrant you want to removed.", "", 10,
                delegate (string warrantId)
                {
                    int warrantID;
                    if (!int.TryParse(warrantId, out warrantID))
                    {
                        warrantID = -1;
                    }
                    TriggerServerEvent("BoloRemovalPaperwork", warrantID);
                });
        }

    }
}
