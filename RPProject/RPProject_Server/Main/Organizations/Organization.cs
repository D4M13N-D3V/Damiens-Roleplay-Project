using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;
using server.Main.Users.CharacterClasses;

namespace server.Main.Organizations
{
    public class Organization
    {
        public string Name;
        public string Description;
        public int Bank;
        public string Owner;
        public List<Member> Members = new List<Member>();

        public List<string> GetMemberNames()
        {
            List<string> memberNames = new List<string>();
            foreach (var member in Members)
            {
                memberNames.Add(member.Character);
            }
            return memberNames;
        }

        public Member GetMemberObjectFromPlayer(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            foreach (var member in Members)
            {
                if (member.Character == user.CurrentCharacter.FullName)
                {
                    return member;
                }
            }
            return null;
        }

        public void AddMember(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var member = new Member(user.CurrentCharacter.FullName,false,false,true,false);
            Utility.Instance.SendChatMessageAll("[Organizations]", "You have been added to " + Name, 150, 25, 25);
            Members.Add(member);
            UpdateDatabase();
        }

        public void Deposit(Player player, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var members = Members.Where(x => x.Character == user.CurrentCharacter.FullName).ToList();
            Member updater = null;
            if (members.Any())
            {
                updater = members[0];
            }
            if (updater != null && updater.CanDeposit)
            {
                if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Bank) >= amount)
                {
                    Bank = Bank + amount;
                    MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, amount);
                    Utility.Instance.SendChatMessage(player, "[Organizations]",
                        "You have deposited " + amount + " into organization bank account!", 150, 25, 25);
                    UpdateDatabase();
                }
                else
                {
                    Utility.Instance.SendChatMessage(player, "[Organizations]", "Not enough money in bank! (HAVE TO USE BANK MONEY)", 150, 25, 25);
                }
            }
        }

        public void Withdrawl(Player player, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var members = Members.Where(x => x.Character == user.CurrentCharacter.FullName).ToList();
            Member updater = null;
            if (members.Any())
            {
                updater = members[0];
            }
            if (updater != null && updater.CanDeposit)
            {
                if(Bank >= amount)
                {
                    Bank = Bank - amount;
                    MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, amount);
                    Utility.Instance.SendChatMessage(player, "[Organizations]",
                        "You have withdrawled " + amount + " from organization bank account!", 150, 25, 25);
                    UpdateDatabase();
                }
                else
                {
                    Utility.Instance.SendChatMessage(player, "[Organizations]", "Not enough money!", 150, 25, 25);
                }
            }
        }

        public void RemoveMember(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            Members.RemoveAll(x => x.Character == user.CurrentCharacter.FullName);
            Alert(player + " have been added to " + Name);
            UpdateDatabase();
        }

        public void RemoveMember(string player)
        {
            Alert(player + " have been removed from " + Name);
            Members.RemoveAll(x => x.Character == player);
            UpdateDatabase();
        }

        public void UpdateMemberPerms(Player player, bool canInvite, bool canPromote, bool canDeposit, bool canWithdraw)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            foreach (var member in Members)
            {
                if (member.Character == user.CurrentCharacter.FullName)
                {
                    Utility.Instance.SendChatMessage(player, "[Organizations]", "Your permissions have been updated. | Invite:" + canInvite + " Promote:" + canPromote + " Deposit:" + canDeposit + " Withdraw:" + canWithdraw + " |", 150, 25, 25);
                    member.CanPromote = canPromote;
                    member.CanDeposit = canDeposit;
                    member.CanWithdrawl = canWithdraw;
                    member.CanInvite = canInvite;
                    UpdateDatabase();
                    return;
                }
            }
        }


        public void UpdateMemberPerms(string player, bool canInvite, bool canPromote, bool canDeposit, bool canWithdraw)
        {
            foreach (var member in Members)
            {
                if (member.Character == player)
                {
                    var user = UserManager.Instance.GetUserFromPlayer(player);
                    Utility.Instance.SendChatMessage(user.Source, "[Organizations]", "Your permissions have been updated. | Invite:" + canInvite + " Promote:" + canPromote + " Deposit:" + canDeposit + " Withdraw:" + canWithdraw + " |", 150, 25, 25);
                    member.CanPromote = canPromote;
                    member.CanDeposit = canDeposit;
                    member.CanWithdrawl = canWithdraw;
                    member.CanInvite = canInvite;
                    UpdateDatabase();
                    return;
                }
            }
        }

        public void Alert(string message)
        {
            foreach (var member in Members)
            {
                var user = UserManager.Instance.GetUserFromCharacterName(member.Character);
                if (user != null)
                {
                    Utility.Instance.SendChatMessage(user.Source,"["+Name+"]",message,150,25,25);
                }
            }
        }

        public void UpdateDatabase()
        {
            var json = JsonConvert.SerializeObject(this);
            DatabaseManager.Instance.Execute("UPDATE ORGS SET json='"+ json + "' WHERE name='"+Name+"'");
            foreach (var member in Members)
            {
                if (member.Character != null)
                {
                    var user = UserManager.Instance.GetUserFromCharacterName(member.Character);
                    if (user != null)
                    {
                        Manager.Instance.LoadCharactersOrganizations(user.Source);
                    }
                }
            }
        }
    }
}
