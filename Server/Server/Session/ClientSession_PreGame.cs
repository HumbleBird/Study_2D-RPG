using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Data;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Server.DB;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Server
{
    public partial class ClientSession : PacketSession
    {
        public void HandleLogin(C_Login loginPacket)
        {
            Console.WriteLine($"UniqueId({loginPacket.UniqueId})");

            // TODO 보안 체크
            if (ServerSetate != PlayerServerState.ServerStateLogin)
                return;

            using (AppDbContext db = new AppDbContext())
            {
                AccountDb findAccount = db.Accounts
                    .Include(a => a.Players)
                    .Where(a => a.AccountName == loginPacket.UniqueId).FirstOrDefault();

                if (findAccount != null)
                {
                    S_Login loginOk = new S_Login() { LoginOk = 1 };
                    Send(loginOk);
                }
                else
                {
                    AccountDb newAccount = new AccountDb() { AccountName = loginPacket.UniqueId };
                    db.Accounts.Add(newAccount);
                    db.SaveChanges();

                    S_Login loginOk = new S_Login() { LoginOk = 1 };
                    Send(loginOk);
                }
            }
        }
    }
}
