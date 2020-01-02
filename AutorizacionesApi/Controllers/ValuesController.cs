using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Helpers;
using WebApi.Models.Config;
using WebApi.Services;
using WebApi.Repository.OS;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        private readonly MyConfiguration _myConfiguration;


        public ValuesController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IOptions<MyConfiguration> myConfiguration)
        {
            _userService = userService;
            _mapper = mapper;
            _myConfiguration = myConfiguration.Value;
            _appSettings = appSettings.Value;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var userId = User.Identity.Name;
            var os = new OSSwiss();
            var resp = os.Eligibilidad("0509769010009");

            //var OSSwiss = new OSSwiss();

            //// var test =  OSSancor.EligibilidadAsync("");

            //var test = OSSwiss.Eligibilidad("0509769010009");

            //try
            //{
            //    var query = "";

            //    var codigo = "180104";
            //    var OsId = 2;

            //    query += " SELECT DISTINCT N.ccodprest, N.CREFPREST";
            //    query += " FROM sistema.nomencla N";
            //    query += " INNER JOIN sistema.osocplan OSP on N.ccodnomen = OSP.ccodnomen OR N.ccodnomen = OSP.csecnomen";
            //    query += " INNER JOIN obrasocial_webservices WS on WS.ncodosoc = OSP.ncodosoc";
            //    query += " WHERE N.ccodprest = '" + codigo + "' AND WS.nid_osocwebservices = " + OsId + " and N.nestado=0";

            //    var c = new Connection();
            //    var dt = c.Query(query);

            //    var descripcion = "Código Inexistente";
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        descripcion = dr.ItemArray[1].ToString().Trim();
            //    }
            //    var qwerty = descripcion;

            //}
            //catch (Exception ex)
            //{
            //    var sdfds = ex;
            //}

            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [AllowAnonymous]
        [HttpPost]
        
        public IActionResult Post([FromBody]userDto value)
        {
            var user = _userService.Authenticate(value.Username, value.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect bla bla bla" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), ///Todo poner 1 hs de expiracion
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
