using Microsoft.AspNetCore.Mvc;

namespace SwaggerTokenAuthentication
{
    public class JwtConfiguration
    {
       public string Key { get; set; }
       public string Issuer { get; set; }
    }
}
