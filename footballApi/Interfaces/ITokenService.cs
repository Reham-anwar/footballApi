using footballApi.DTO;
using footballApi.Models.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.Interfaces
{
  public  interface ITokenService
    {
        string CreateToken(AppUser appUser);

        AuthDTO GetJWTClams(String JWTToken);
    }
}
