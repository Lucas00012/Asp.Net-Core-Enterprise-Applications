using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
    public class IdentityConfiguration
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }

        public SymmetricSecurityKey IssuerSignignKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
    }
}
