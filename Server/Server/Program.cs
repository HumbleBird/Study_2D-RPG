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
	// 1. GameRoom 방식의 간단한 동기화 <- OK
	// 2. 더 넓은 영역 관리
	// 3. 심리스 MMO

	// 1. Recv (N개)     서빙
	// 2. GameLogic (1)  요리사
	// 3. Send (1개)     서빙
	// 4. DB (1)         결제/장부

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

		public static string Name { get; } = "데포르쥬";
		public static int Port { get; } = 7777;
		public static string IpAddress { get; set; }

		static void Main(string[] args)
		{
			ConfigManager.LoadConfig();
			DataManager.LoadData();

			GameLogic.Instance.Push(() => { GameLogic.Instance.Add(1); });

			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[1];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, Port);

			IpAddress = ipAddr.ToString();

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			// DbTask
			{
				Thread t = new Thread(DbTask);
				t.Name = "DB";
				t.Start();
			}

			// NetworkTask
			{
				Thread t = new Thread(NetworkTask);
				t.Name = "Network Send";
				t.Start();
			}

			// GameLogic
			Thread.CurrentThread.Name = "GameLogic";
			GameLogicTask();
		}
	}
}
