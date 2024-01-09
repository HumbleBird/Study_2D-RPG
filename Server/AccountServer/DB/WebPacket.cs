// client - > server
using System.Collections.Generic;

public class CreateAccountPacketReq
{
    public string AccountName { get; set; }
    public string Password { get; set; }
}

// Server -> Client
public class CreateAccountPacketRes
{
    public bool CreateOk { get; set; }
}

public class LoginAccountPacketReq
{
    public string AccountName { get; set; }
    public string Password { get; set; }
}

public class ServerInfo
{
    public string Name { get; set; }
    public string IP { get; set; }
    public int CrowedLevel { get; set; }
}

public class LoginAccountPacketRes
{
    public bool LoginOk { get; set; }
    public List<ServerInfo> ServerList { get; set; } = new List<ServerInfo>();
}