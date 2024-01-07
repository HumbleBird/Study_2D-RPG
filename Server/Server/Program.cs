﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Data;
using Server.DB;
using Server.Game;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();

		static void GameLogicTask()
		{
            while (true)
            {
                GameLogic.Instance.Update();
                Thread.Sleep(0);
            }
        }

		static void DbTask()
		{
            while (true)
            {
                DbTransaction.Instance.Flush();
				Thread.Sleep(0);
            }
        }

		static void NetworkTask()
		{
			while (true)
			{
				List<ClientSession> sessions = SessionManager.Instance.GetSessions();

				foreach (ClientSession session in sessions)
				{
					session.FlushSend();
				}

                Thread.Sleep(0);
            }
		}

		static void Main(string[] args)
		{
			ConfigManager.LoadConfig();
			DataManager.LoadData();


            GameLogic.Instance.Push(() =>
            {
                GameRoom room = GameLogic.Instance.Add(1);
            });


			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

            // DbTask
            {
                Thread t = new Thread(DbTask);
				t.Name = "DB";
				t.Start();
			}

			// Network
			{
                Thread t = new Thread(NetworkTask);
                t.Name = "Network Send";
                t.Start();
            }

			// GameLogicTask
			Thread.CurrentThread.Name = "GameLogic";
            GameLogicTask();
		}
	}
}
