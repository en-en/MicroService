using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WangGang.MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<string> Login(UserRequestModel userRequestModel)
        {
            var client = new HttpClient();

            DiscoveryResponse discoveryResponse = await client.GetDiscoveryDocumentAsync("http://127.0.0.1:5001");
            if(discoveryResponse==null)
            {
                return "认证服务未启动";
            }
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = "ServiceAClient",
                ClientSecret = "ServiceAClient",
                UserName = userRequestModel.Name,
                Password = userRequestModel.Password
            });
            return tokenResponse.IsError ? tokenResponse.Error : tokenResponse.AccessToken;
        }


        public class UserRequestModel
        {
            [Required(ErrorMessage = "用户名称不可以为空")]
            public string Name { get; set; }

            [Required(ErrorMessage = "用户密码不可以为空")]
            public string Password { get; set; }
        }
        }
    }
