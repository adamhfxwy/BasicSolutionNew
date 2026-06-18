
namespace WebAPI
{
    public class JwtParam
    {
        public bool ValidateIssuerSigningKey { get; set; }

        public string ValidIssuerSigningKey { get; set; }

        public bool ValidateIssuer { get; set; }

        public string ValidIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public string ValidAudience { get; set; }

        public bool ValidateLifetime { get; set; }

        public uint ValidLifetime { get; set; }
    }
}
