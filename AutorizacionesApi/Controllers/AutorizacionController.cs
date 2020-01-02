using System;
using System.Linq;
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
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Models.Config;
using WebApi.Repository;
using WebApi.Services;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
   // [ApiController]
   // [Authorize]
    public class AutorizacionController : Controller
    {

        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        private readonly MyConfiguration _myConfiguration;


        public AutorizacionController(
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
        [HttpGet, ActionName("Prestadores")]
        public IEnumerable<Prestador> Prestadores()
        {
            try
            {
                var userId = User.Identity.Name;
                if (userId != null)
                {
                    var prestadores = new PrestadorRepository();
                    var p = prestadores.Prestadores(userId);
                    return p;
                }
                else
                {
                    //Si es null perdio la sesion
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                var error = new Errores();
                error.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, string.Empty, string.Empty);
                return null;
            }
        }

        [HttpGet, ActionName("GetObrasSociales")]
        public List<Os> GetObrasSociales()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Identity.Name;
                    var os = new OSRepository();
                    return os.List(Convert.ToInt32(userId));
                }
                else
                {
                    var error = new Errores();
                    error.SetError("Getos", 0, string.Empty, string.Empty, null , "Modelo invalido");
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = new Errores();
                error.SetError("Getos", GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, null, "Modelo invalido");
                return null;
            }
        }

        
        [HttpPost, ActionName("GetElegibilidad")]
        public Afiliado GetElegibilidad([FromBody] Elegibilidad id)
        {
            var afiliado = new Afiliado();

            try
            {
                if (ModelState.IsValid)
                {
                    OsStatus.Medife = DateTime.Now;
                    id.UserId = User.Identity.Name;
                    var os = new OSRepository();
                    return os.Elegibilidad(id);
                }
                afiliado.ModelError = true;
                var error = ModelState.Values.FirstOrDefault() != null ? ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage : string.Empty;
                afiliado.SetError(GetType().Name, 0, error, string.Empty, id, string.Empty);
            }
            catch (Exception ex)
            {
                afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
            }
            return afiliado;
        }


        [HttpPost, ActionName("GetPractDesc")]
        public Models.Prestacion.PrestacionSearch GetPractDesc([FromBody] Search id)
        {
            try
            {
                if (!ModelState.IsValid) return null;
                var os = new OSRepository();
                var a = os.NombrePractica(id.Cod, id.OsId);
                        return a;
                //Todo review Error
            }
            catch (Exception ex)
            {
                var error = new Errores();
                error.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
                return null;
            }
        }


        [HttpPost, ActionName("GetPractDescList")]
        public List<Models.Prestacion.PrestacionSearch> GetPractDescList([FromBody] Search id)
        {
            try
            {
              if (!ModelState.IsValid) return null;
                var os = new OSRepository();
                return os.GetPracticaByDesc(id.Cod, id.OsId);
                //Todo review Error
            }
            catch (Exception ex)
            {
                var error = new Errores();
                error.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
                return null;
            }
        }

        [HttpPost, ActionName("GetCarnetList")]
        public List<CarnetSearch> GetCarnetList([FromBody] Search id)
        {
            try
            {
              if (!ModelState.IsValid) return null;
                var os = new OSRepository();
                return os.GetCarnetList(id.PrestadorId, id.OsId, id.Cod);
                //Todo review Error
            }
            catch (Exception ex)
            {
                var error = new Errores();
                error.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
                return null;
            }
        }

        [HttpPost, ActionName("Authorize")]
        public AutorizacionVer Authorize([FromBody] AuthorizeIn id)
        {
            var autorizacionVer = new AutorizacionVer();
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var model = new Authorize(id.OSId, id.PrestadorId, id.FacturadorId, id.Credencial, id.Tipo, id.Prestaciones, User.Identity.Name);
            //        var autorizacion = new AutorizacionRepository();
            //        autorizacionVer = autorizacion.Autorizacion(model);
            //    }
            //    else
            //    {
            //        var error = ModelState.Values.FirstOrDefault() != null ? ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage : string.Empty;
            //        autorizacionVer.SetError(GetType().Name, 0, error, string.Empty, id, string.Empty);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    autorizacionVer.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
            //}

            var auth = new AutorizacionVer();
            auth.Afiliado = "Jose Salvador";
            auth.Aseguradora = "Salvador Company";
            auth.AuthNr = "000004518142";
            auth.AuthNrAnulacion = "";
            //auth.Detalle
            auth.Estado = "Autorizada";
            auth.Fecha = "14/06/19";
            auth.IdentificacionNro = "???????";
            auth.Iva = "Excento";
            auth.Matricula = "9999";
            auth.Plan = "El mas Alto";
            auth.Profesional = "Dr. Salvador Jesus Juan Pablo";
            auth.ShowMessage = "No se que mensaje";
            auth.Detalle = new List<AutorizacionVerDet>();
            var authDet = new AutorizacionVerDet();
            authDet.Cantidad = "1";
            authDet.Estado = "Aprobada";
            authDet.Observacion = "Puede hacerce cualquier cosa";
            authDet.Prestacion = "Radiografia de Colon";

            return auth;
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

        //  //[HttpPost]
        //  //[ActionName("Elegibilidad")]
        //  //public Afiliado Elegibilidad(Elegibilidad id)
        //  //{
        //  //    var afiliado = new Afiliado();

        //  //    try
        //  //    {
        //  //        if (ModelState.IsValid)
        //  //        {
        //  //            OsStatus.Medife = DateTime.Now;
        //  //            id.UserId = User.Identity.Name;
        //  //            var os = new OSRepository();
        //  //            //return os.Elegibilidad(id);
        //  //        }
        //  //        var error = ModelState.Values.FirstOrDefault() != null ? ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage : string.Empty;
        //  //        afiliado.SetError(GetType().Name, 0, error, string.Empty, id, string.Empty);
        //  //    }
        //  //    catch (Exception ex)
        //  //    {
        //  //        afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, string.Empty);
        //  //    }
        //  //    return afiliado;
        //  //}

        //  // GET: api/Autorizacion
        //  [HttpPost]
        ////  [ActionName("GetPrestadores")]
        //  public List<Prestador> GetPrestadores()
        //  {
        //      try
        //      {
        //          var userId = User.Identity.Name;
        //          var prestadores = new PrestadorRepository();
        //          var p = prestadores.Prestadores(userId);
        //          return p;
        //      }
        //      catch (Exception ex)
        //      {
        //          var error = new Errores();
        //          error.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, string.Empty, string.Empty);
        //          return null;
        //      }
        //  }


        //  // GET: api/Autorizacion
        //  [HttpGet]
        //  public IEnumerable<string> Get()
        //  {
        //      return new string[] { "value1", "value2" };
        //  }

        //  // GET: api/Autorizacion/5
        //  [HttpGet("{id}", Name = "Get")]
        //  public string Get(int id)
        //  {
        //      return "value";
        //  }

        //  // POST: api/Autorizacion
        //  [HttpPost]
        //  public void Post([FromBody] string value)
        //  {
        //  }

        //  // PUT: api/Autorizacion/5
        //  [HttpPut("{id}")]
        //  public void Put(int id, [FromBody] string value)
        //  {
        //  }

        //  // DELETE: api/ApiWithActions/5
        //  [HttpDelete("{id}")]
        //  public void Delete(int id)
        //  {
        //  }
    }
}
