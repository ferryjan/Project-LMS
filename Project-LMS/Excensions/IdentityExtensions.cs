using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Project_LMS.Excensions
{
    public static class IdentityExtensions
    {
        public static string GetUserGivenName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("GivenName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetUserFamilyName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FamilyName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetUserProfileRef(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfileImageRef");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}