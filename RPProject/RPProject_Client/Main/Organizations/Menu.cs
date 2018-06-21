using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Organizations
{
    public class Menu : BaseScript
    {
        private UIMenu _menu = null;


        private UIMenu _inviteMenu = null;
        private List<string> _invites = new List<string>();

        private UIMenu _myOrgsMenu = null;
        private List<Organization> _myOrgs = new List<Organization>();

        public static Menu Instance;

        public Menu()
        {
            Instance = this;
            SetupEvents();
            SetupNativeUi();
        }

        private void AcceptInvite(string name)
        {
            TriggerServerEvent("Organizations:Accept", name);
        }

        private void DeclineInvite(string name)
        {
            TriggerServerEvent("Organizations:Decline", name);
        }

        private void SetupEvents()
        {
            EventHandlers["Organizations:RefreshInvites"] += new Action<List<dynamic>>(RefreshInvites);
            EventHandlers["Organizations:RefreshOrgs"] += new Action<List<dynamic>>(RefreshOrganizations);
        }

        private void SetupNativeUi()
        {
            _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Organizations", new PointF(5, Screen.Height / 2));

            var createButton = new UIMenuItem("Create ( 50,000$! )");
            _menu.AddItem(createButton);
            _menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == createButton)
                {
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                    Utility.Instance.KeyboardInput("Organization Name" ,"", 300, s =>
                        {
                            Utility.Instance.KeyboardInput("Organization Description", "", 2000, s1 =>
                            {
                                TriggerServerEvent("Organizations:Create", s, s1);
                            });
                        } );
                }
            };

            _inviteMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(_menu, "Invites", new PointF(5, Screen.Height / 2));

            _myOrgsMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(_menu, "My Organizations", new PointF(5, Screen.Height / 2));

            InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
        }

        private void RefreshInvites(List<dynamic> invites)
        {
            Debug.WriteLine("ESTSETESTSETAWETWAETRWAETRAWERAWERAWE");
            _inviteMenu.Clear();
            _invites.Clear();
            foreach (var invite in invites)
            {
                _invites.Add(Convert.ToString(invite));
            }

            foreach (var invite in _invites)
            {
                var menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(_inviteMenu,"Invitation To "+invite, new PointF(5, Screen.Height / 2));
                var acceptButton = new UIMenuItem("Accept!");
                var declineButton = new UIMenuItem("Decline!");
                menu.AddItem(acceptButton);
                menu.AddItem(declineButton);
                menu.OnItemSelect += (sender, item, index) =>
                {
                    if (item == acceptButton)
                    {
                        AcceptInvite(invite);
                    }
                    else if (item == declineButton)
                    {
                        DeclineInvite(invite);
                    }
                };
            }
        }

        private void RefreshOrganizations(List<dynamic> orgInfo)
        {
            _myOrgsMenu.Clear();
            _myOrgs.Clear();
            Debug.WriteLine("Refreshing");
            foreach (var info in orgInfo)
            {
             Debug.Write(info.name);   
                _myOrgs.Add(new Organization(info.name, info.invite, info.promote, info.deposit, info.withdrawl, info.members, info.bank));    
            }

            foreach (var org in _myOrgs)
            {
                var menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(_myOrgsMenu ,org.Name, new PointF(5, Screen.Height / 2));

                var DeleteMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, "~r~Delete (Only works if your the owner) ", new PointF(5, Screen.Height / 2));
                var YesButtonDelete = new UIMenuItem("~r~Yes!");
                var NoButtonDelete = new UIMenuItem("~g~No!");
                DeleteMenu.AddItem(YesButtonDelete);
                DeleteMenu.AddItem(NoButtonDelete);
                DeleteMenu.OnItemSelect += (sender, item, index) =>
                {
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                    if (item == YesButtonDelete)
                    {
                        TriggerServerEvent("Organizations:Delete", org.Name);
                    }
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                };

                var LeaveMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, "~r~Leave Organization!", new PointF(5, Screen.Height / 2));
                var YesButtonLeave = new UIMenuItem("~r~Yes!");
                var NoButtonLeave = new UIMenuItem("~g~No!");
                LeaveMenu.AddItem(YesButtonLeave);
                LeaveMenu.AddItem(NoButtonLeave);
                LeaveMenu.OnItemSelect += (sender, item, index) =>
                {
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                    if (item == YesButtonLeave)
                    {
                        TriggerServerEvent("Organizations:Leave", org.Name);
                    }
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                };

                if (org.CanInvite)
                {
                    var inviteButton = new UIMenuItem("~b~Invite Closest Player!");
                    menu.AddItem(inviteButton);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();   
                    menu.OnItemSelect += (sender, item, index) =>
                    {
                        if (item == inviteButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.GetClosestPlayer(out var output);
                            if (output.Dist < 5)
                            {
                                TriggerServerEvent("Organizations:Invite", org.Name, API.GetPlayerServerId(output.Pid));
                            }
                            else
                            {
                                Utility.Instance.SendChatMessage("[Organizations]", "No player is close enough to invite!", 150, 25, 25);
                            }
                        }
                    };
                }

                if (org.CanPromote)
                {
                    var permsMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, "Members",new PointF(5, Screen.Height / 2));
                    foreach (var member in org.Members)
                    {
                        Debug.WriteLine(member);
                        UIMenu memberMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(permsMenu, member, new PointF(5, Screen.Height / 2));
                        var depositSlider = new UIMenuSliderItem("Can Deposit. ~g~Yes~w~/~r~No", new List<dynamic>() { 0, 1 }, 0);
                        var withdrawlSlider = new UIMenuSliderItem("Can Withdrawl. ~g~Yes~w~/~r~No", new List<dynamic>() { 0, 1 }, 0);
                        var inviteSlider = new UIMenuSliderItem("Can Invite. ~g~Yes~w~/~r~No", new List<dynamic>() { 0, 1 }, 0);
                        var promoteSlider = new UIMenuSliderItem("Can Promote. ~g~Yes~w~/~r~No", new List<dynamic>() { 0, 1 }, 0);
                        var kickButton = new UIMenuItem("~r~Kick!");
                        var saveButton = new UIMenuItem("~g~Save Perms!");
                        memberMenu.AddItem(kickButton);
                        memberMenu.AddItem(depositSlider);
                        memberMenu.AddItem(withdrawlSlider);
                        memberMenu.AddItem(inviteSlider);
                        memberMenu.AddItem(promoteSlider);
                        memberMenu.AddItem(saveButton);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                        memberMenu.OnItemSelect += (sender, item, index) =>
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            if (item == saveButton)
                            {
                                var canDeposit = depositSlider.Index == 0 ? true : false;
                                var canWithdraw = withdrawlSlider.Index == 0 ? true : false;
                                var canInvite = inviteSlider.Index == 0 ? true : false;
                                var canPromote = promoteSlider.Index == 0 ? true : false;
                                TriggerServerEvent("Organizations:UpdatePerms", org.Name, member, canInvite, canPromote, canDeposit, canWithdraw);
                            }

                            if (item == kickButton)
                            {
                                TriggerServerEvent("Organizations:Kick", org.Name,  member);
                            }
                        };
                    }   
                }

                var balanceButton  = new UIMenuItem("~g~Organization Bank Balance : "+org.Bank);
                menu.AddItem(balanceButton);

                if (org.CanDeposit)
                {
                    var depositButton = new UIMenuItem("~y~Deposit Into Organization Bank!");
                    menu.AddItem(depositButton);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    menu.OnItemSelect += (sender, item, index) =>
                    {
                        if (item == depositButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput("Amount you are depositing", "", 10, s =>
                            {
                                if (Int32.TryParse(s, out var result))
                                {
                                    TriggerServerEvent("Organizations:Deposit", org.Name, result);
                                }
                                else
                                {
                                    Utility.Instance.SendChatMessage("[Organizations]", "Invalid amount!", 150, 25, 25);
                                }
                            });
                        }
                    };
                }

                if (org.CanWithdrawl)
                {
                    var withdrawButton = new UIMenuItem("~o~Withdraw From Organization Bank!");
                    menu.AddItem(withdrawButton);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    menu.OnItemSelect += (sender, item, index) => {
                        if (item == withdrawButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput("Amount you are withdrawing", "", 10, s =>
                            {
                                if (Int32.TryParse(s, out var result))
                                {
                                    TriggerServerEvent("Organizations:Withdraw", org.Name, result);
                                }
                                else
                                {
                                    Utility.Instance.SendChatMessage("[Organizations]", "Invalid amount!", 150, 25, 25);
                                }
                            });
                        }
                    };
                }
            }
        }
    }
}
