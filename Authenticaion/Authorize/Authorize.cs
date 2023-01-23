using AppEnvironment;
using Authenticaion.JWT;
using Authentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Authenticaion.Authorize
{
    public class Auth : Attribute, IActionFilter
    {
        private readonly string[] _claims;
        private readonly JwtHelper _jwtHelper;
        public Auth(string claims)
        {
            _jwtHelper = new JwtHelper();
            _claims = claims.Split(',');
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var expireDate = jwtExpireTime(context.HttpContext.Request.Headers.Authorization.ToString());
            if (DateTime.Now > expireDate)
            {
                context.Result = new JsonResult(new Result(AppEnvironment.MessageType.Forbidden));
                return;
            }

            var claims = context.HttpContext.User.ClaimRoles().ToList();
            if (_claims.Any(role => claims.Contains(role)))
            {
                return;
            }
            //UnAuthrozation Claim Exception9
            context.Result = new JsonResult(new Result(AppEnvironment.MessageType.Unauthorized));
        }

        private DateTime? jwtExpireTime(string jwt)
        {
            if (jwt == "") return null;
            var jwtArray = jwt.Split("Bearer ");
            var jsonString = jwtArray[1] ?? "";
            var expire = _jwtHelper.ValidateJwtToken(jsonString).Payload["exp"];
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)expire);
            return dateTimeOffset.LocalDateTime;
        }
    }
}
