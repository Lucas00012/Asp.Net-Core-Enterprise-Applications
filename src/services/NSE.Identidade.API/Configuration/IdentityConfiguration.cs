﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.Identidade.API.Configuration
{
    public class IdentityConfiguration
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public IEnumerable<string> ValidoEm { get; set; }

        public SymmetricSecurityKey IssuerSignignKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
    }
}
