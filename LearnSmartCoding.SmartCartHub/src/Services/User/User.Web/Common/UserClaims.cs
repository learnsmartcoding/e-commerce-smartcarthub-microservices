using System.Security.Claims;

namespace User.Web.Common
{
    public interface IUserClaims
    {
        string GetCurrentUserEmail();
        string GetCurrentUserId();
    }
    public class UserClaims : IUserClaims
    {
        public UserClaims(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public string GetCurrentContextUserId()
        {
            return GetCurrentUserId();
        }
        private string GetClaimInfo(string property)
        {
            var propertyData = "";
            var identity = HttpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                propertyData = identity.Claims.FirstOrDefault(d => d.Type.Contains(property)).Value;

            }
            return propertyData;
        }
       
        public string GetCurrentUserEmail()
        {
            return GetClaimInfo("emails");
        }

        public string GetCurrentUserId()
        {
            //return "2n3o4p5q-6r7s-8t9u-10v11w12x13y14";
            return GetClaimInfo("objectidentifier");
        }
    }
}
