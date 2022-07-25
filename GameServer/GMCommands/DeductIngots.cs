﻿using System;
using GameServer.Data;
using GameServer.Networking;

namespace GameServer
{
	
	public sealed class DeductIngots : GMCommand
	{
		
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002865 File Offset: 0x00000A65
		public override ExecutionWay ExecutionWay
		{
			get
			{
				return ExecutionWay.优先后台执行;
			}
		}

		
		public override void Execute()
		{
			GameData GameData;
			if (GameDataGateway.CharacterDataTable.Keyword.TryGetValue(this.CharName, out GameData))
			{
				CharacterData CharacterData = GameData as CharacterData;
				if (CharacterData != null)
				{
					CharacterData.元宝数量 = Math.Max(0, CharacterData.元宝数量 - this.元宝数量);
					客户网络 网络连接 = CharacterData.ActiveConnection;
					if (网络连接 != null)
					{
						网络连接.发送封包(new 同步元宝数量
						{
							元宝数量 = CharacterData.元宝数量
						});
					}
					MainForm.添加命令日志(string.Format("<= @{0} command has been executed, with the current amount of treasure: {1}", base.GetType().Name, CharacterData.元宝数量));
					return;
				}
			}
			MainForm.添加命令日志("<= @" + base.GetType().Name + " Command execution failed, role does not exist");
		}

		
		public DeductIngots()
		{
			
			
		}

		
		[FieldAttribute(0, 排序 = 0)]
		public string CharName;

		
		[FieldAttribute(0, 排序 = 1)]
		public int 元宝数量;
	}
}
