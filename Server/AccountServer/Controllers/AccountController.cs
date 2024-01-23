using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.DB;
using AccountServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		AccountService _account;

		public AccountController(AccountService account)
		{
			_account = account;
		}

		[HttpPost]
		[Route("login/facebook")]
		public async Task<LoginFacebookAccountPacketRes> LoginAccount([FromBody] LoginFacebookAccountPacketReq req)
		{
			LoginFacebookAccountPacketRes res = new LoginFacebookAccountPacketRes();

			string jwtToken = await _account.LoginFacebookAccount(req.Token);
			if (string.IsNullOrEmpty(jwtToken))
			{
				res.LoginOk = false;
				return res;
			}

			res.LoginOk = true;
			res.JwtAccessToken = jwtToken;
			return res;
		}
	}
}
