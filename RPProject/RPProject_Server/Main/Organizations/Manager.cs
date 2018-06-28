    using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;

namespace server.Main.Organizations
{
    public class Manager : BaseScript
    {
        public static Manager Instance;

        public const int OrganizationCost = 50000;

        public Dictionary<string,Organization> Organizations = new Dictionary<string,Organization>();

        private List<InviteInfo> _pendingInvites = new List<InviteInfo>();

        public Manager()
        {
            Instance = this;
            Load();
            EventHandlers["Organizations:Create"] += new Action<Player, string, string>(CreateOrganizationRequest);
            EventHandlers["Organizations:Delete"] += new Action<Player, string>(DeleteOrganizationRequest);
            EventHandlers["Organizations:UpdatePerms"] += new Action<Player, string, string, bool, bool, bool, bool>(UpdatePermsRequest);
            EventHandlers["Organizations:Invite"] += new Action<Player, string, int>(InvitePlayerRequest);
            EventHandlers["Organizations:Kick"] += new Action<Player, string, string>(KickPlayerRequest);
            EventHandlers["Organizations:Deposit"] += new Action<Player, string, int>(DepositRequest);
            EventHandlers["Organizations:Withdraw"] += new Action<Player, string, int>(WithdrawRequest);
            EventHandlers["Organizations:Accept"] += new Action<Player, string>(AcceptRequest);
            EventHandlers["Organizations:Decline"] += new Action<Player, string>(DeclineRequest);
            EventHandlers["Organizations:Leave"] += new Action<Player, string>(LeaveRequest);
        }

        private void LeaveRequest([FromSource] Player player, string s)
        {
            if (Organizations.ContainsKey(s))
            {
                Organizations[s].RemoveMember(player);
                LoadCharactersOrganizations(player);
            }
        }

        private void AcceptRequest([FromSource] Player player, string s)
        {
            for (int i = 0; i < _pendingInvites.Count; i++)
            {
                Debug.WriteLine(_pendingInvites[i].Organization);
                if (_pendingInvites[i].Organization != s  || _pendingInvites[i].Recipient != player.Name) continue;
                _pendingInvites.Remove(_pendingInvites[i]);
                Organizations[s].AddMember(player);
                UpdateClientsInvites(player);
            }
        }

        private void DeclineRequest([FromSource] Player player, string s)
        {

            for (int i = 0; i < _pendingInvites.Count; i++)
            {
                if (_pendingInvites[i].Organization != s || _pendingInvites[i].Recipient != player.Name) continue;
                _pendingInvites.Remove(_pendingInvites[i]);
                UpdateClientsInvites(player);
                Utility.Instance.SendChatMessage(player, "[Organizations]", "You have declined the invite for " + s + "!", 150, 25, 25);
            }
        }

        public bool Create(Player player, string name, string description)
        {
            name = new string(name.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            description = new string(description.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM ORGS WHERE name='" + name + "'").Result;
            while (data.Read())
            {
                DatabaseManager.Instance.EndQuery(data);
                return false;
            }
            DatabaseManager.Instance.EndQuery(data);
            var tempOrg = new Organization();
            var user = UserManager.Instance.GetUserFromPlayer(player);
            tempOrg.Owner = user.CurrentCharacter.FullName;
            tempOrg.Bank = 0;
            tempOrg.Name = name;
            tempOrg.Description = description;
            DatabaseManager.Instance.Execute("INSERT INTO ORGS (name,json) VALUES('" + name + "','" + JsonConvert.SerializeObject(tempOrg) + "');");
            tempOrg.AddMember(player);
            tempOrg.UpdateMemberPerms(player, true, true, true, true);
            Organizations.Add(tempOrg.Name,tempOrg);
            LoadCharactersOrganizations(player);
            return true;
        }

        public bool Delete(Player player, string name)
        {
            var org = Organizations[name];
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (org.Owner == user.CurrentCharacter.FullName)
            {
                DatabaseManager.Instance.Execute("DELETE FROM ORGS WHERE name='"+name+"';");
                Organizations.Remove(name);
                LoadCharactersOrganizations(player);
                return true;
            }
            return false;
        }

        public async Task Load()
        {
            while (DatabaseManager.Instance == null)
            {
                await Delay(0);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM ORGS;").Result;
            while (data.Read())
            {
                var tmpOrg = JsonConvert.DeserializeObject<Organization>(Convert.ToString(data["json"]));
                Organizations.Add(Convert.ToString(data["name"]),tmpOrg);
            }
            DatabaseManager.Instance.EndQuery(data);
        }

        public List<string> GetOrgsApartOf(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var orgs = new List<string>();
            foreach (var org in Organizations)
            {
                foreach (var member in org.Value.Members)
                {
                    if (member.Character == user.CurrentCharacter.FullName)
                    {
                        orgs.Add(org.Value.Name);
                        break;
                    }
                }
            }
            return orgs;
        }

        public void LoadCharactersOrganizations(Player player)
        {
            var orgs = GetOrgsApartOf(player);
            List<dynamic> info = new List<dynamic>();
            foreach (var org in orgs)
            {
                var organization = Organizations[org];
                var member = organization.GetMemberObjectFromPlayer(player);
                dynamic obj = new ExpandoObject();
                obj.name = organization.Name;
                obj.invite = member.CanInvite;
                obj.promote = member.CanPromote;
                obj.deposit = member.CanDeposit;
                obj.withdrawl = member.CanWithdrawl;
                obj.members = organization.GetMemberNames();
                obj.bank = organization.Bank;
                info.Add(obj);
            }
            TriggerClientEvent(player,"Organizations:RefreshOrgs", info);
        }

        public void CreateOrganizationRequest([FromSource] Player player, string name, string desc)
        {
            Debug.WriteLine(name+" "+desc);
            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Bank) >= OrganizationCost && Create(player, name, desc))
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, OrganizationCost);
                Utility.Instance.SendChatMessage(player, "[Organizations]", "You have created a organization : " + name + "(" + desc + ")", 150, 25, 25);
            }
            else if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Cash) >= OrganizationCost && Create(player, name, desc))
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, OrganizationCost);
                Utility.Instance.SendChatMessage(player, "[Organizations]", "You have created a organization : " + name + "(" + desc + ")", 150, 25, 25);
            }
            else
            {
                Utility.Instance.SendChatMessage(player,"[Organizations]","You do not have enough money. ( Costs "+OrganizationCost+")",150,25,25);
            }
        }

        public void DeleteOrganizationRequest([FromSource] Player player, string name)
        {
            if (Organizations.ContainsKey(name) && Delete(player,name))
            {
                Utility.Instance.SendChatMessage(player, "[Organizations]", "You have deleted the organization.", 150, 25, 25);
            }
            else
            {
                Utility.Instance.SendChatMessage(player,"[Organizations]","This organization does not exist or you are not the owner!",150,25,25);
            }
        }

        public void UpdatePermsRequest([FromSource] Player player, string name, string target, bool canInvite,
            bool canPromote, bool canDeposit, bool canWithdrawl)
        {
            if (Organizations.ContainsKey(name))
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                var org = Organizations[name];
                var members = org.Members.Where(x => x.Character == user.CurrentCharacter.FullName).ToList();
                Member updater = null;
                if (members.Any())
                {
                    updater = members[0];
                }
                if (updater!=null && updater.CanPromote)
                {
                    org.UpdateMemberPerms(target,canInvite,canPromote,canDeposit,canWithdrawl);
                }
            }
        }

        public void InvitePlayerRequest([FromSource] Player player, string name, int id)
        {
            if (Organizations.ContainsKey(name))
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                var org = Organizations[name];
                var members = org.Members.Where(x => x.Character == user.CurrentCharacter.FullName).ToList();
                Member updater = null;
                if (members.Any())
                {
                    updater = members[0];
                }
                if (updater != null && updater.CanInvite)
                {
                    var plyList = new PlayerList();
                    var ply = plyList[id];
                    if (ply != null)
                    {
                        var invite = new InviteInfo(name, player.Name, ply.Name);
                        _pendingInvites.Add(invite);
                        UpdateClientsInvites(ply);
                        RemoveOverTime();
                        async Task RemoveOverTime()
                        {
                            await Delay(60000);
                            _pendingInvites.Remove(invite);
                            UpdateClientsInvites(ply);
                        }
                        Utility.Instance.SendChatMessage(ply, "[Organizations]", "You have recieved a invite to an organization. ( " + org.Name + " : " + org.Description + " ), type /accept " + org.Name + " to join!", 150, 25, 25);
                        Utility.Instance.SendChatMessage(player,  "[Organizations]", "You have invited "+UserManager.Instance.GetUserFromPlayer(ply).CurrentCharacter.FullName+" to  " + org.Name+"!", 150, 25, 25);
                    }
                }
            }
        }

        public void UpdateClientsInvites(Player player)
        {
            List<string> orgs = new List<string>();
            foreach (var invite in _pendingInvites)
            {
                if (invite.Recipient == player.Name)
                {
                    orgs.Add(invite.Organization);
                }
            }
            TriggerClientEvent("Organizations:RefreshInvites", orgs);
        }

        public void KickPlayerRequest([FromSource] Player player, string name, string target)
        {
            if (Organizations.ContainsKey(name))
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                var org = Organizations[name];
                var members = org.Members.Where(x => x.Character == user.CurrentCharacter.FullName).ToList();
                Member updater = null;
                if (members.Any())
                {
                    updater = members[0];
                }
                if (updater != null && updater.CanPromote)
                {
                    foreach (var member in org.Members)
                    {
                        if (member.Character == target)
                        {
                            org.RemoveMember(target);
                            return;
                        }
                    }
                }
            }
        }

        public void DepositRequest([FromSource] Player player, string name, int amount)
        {
            if (Organizations.ContainsKey(name) && amount!=null)
            {
                var org = Organizations[name];
                org.Deposit(player,amount);
            }
        }

        public void WithdrawRequest([FromSource] Player player, string name, int amount)
        {
            if (Organizations.ContainsKey(name) && amount != null)
            {
                var org = Organizations[name];
                org.Withdrawl(player, amount);
            }
        }
    }
}
