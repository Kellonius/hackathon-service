using Hackathon_Service.Models.Users.Requests;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Responses;
using Hackathon_Service.Repositories;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Users")]
    public class UserController : ApiController
    {
        private UserRepository userRepository;
        private PatientRepository patientRepository;
        private PharmacyRepository pharmacyRepository;
        private MedicalProfessionalRepository medicalProfessionalRepository;
        
        public UserController()
        {
            userRepository = new UserRepository();
            patientRepository = new PatientRepository();
            pharmacyRepository = new PharmacyRepository();
            medicalProfessionalRepository = new MedicalProfessionalRepository();
        }
        
        /// <summary>
        /// Create a new user for system with a UserCreationRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("CreateUser")]
        public IHttpActionResult createUser(UserCreationRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {
                    var exists = userRepository.checkIfUserExists(request.email);
                    if (exists != null && exists.email.Length > 0)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "An account with this email already exists.");
                        throw new HttpResponseException(response);
                    }
                    userRepository.createNewUser(request);
                }
                return Ok("success");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Create a new patient with a PatientCreationRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("CreatePatient")]
        public IHttpActionResult createPatient(PatientCreationRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {
                    var exists = userRepository.checkIfUserExists(request.email);
                    if (exists != null && exists.email.Length > 0)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "An account with this email already exists.");
                        throw new HttpResponseException(response);
                    }
                    patientRepository.createNewUserPatient(request);
                }
                return Ok("success");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Create a new pharmacy with a PharmacyCreationRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("CreatePharmacy")]
        public IHttpActionResult createPharmacy(PharmacyCreationRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {
                    var exists = userRepository.checkIfUserExists(request.email);
                    if (exists != null && exists.email.Length > 0)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "An account with this email already exists.");
                        throw new HttpResponseException(response);
                    }
                    pharmacyRepository.createNewUserPharmacy(request);
                }
                return Ok("success");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Create a new Medical Professional with a MedicalProfessionalRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("CreateMedicalProfessional")]
        public IHttpActionResult createMedicalProfessional(MedicalProfessionalRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {
                    var exists = userRepository.checkIfUserExists(request.email);
                    if (exists != null && exists.email.Length > 0)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "An account with this email already exists.");
                        throw new HttpResponseException(response);
                    }
                    medicalProfessionalRepository.createNewUserMedicalProfessional(request);
                }
                return Ok("success");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Login as a user passing in a UserLoginRequest
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [Route("LoginUser")]
        public UserResponse loginUser(UserLoginRequest userLoginRequest)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {
                    var user = context.users.FirstOrDefault(x => x.email.Equals(userLoginRequest.email) && x.delete_ts == null);
                    if (user == null)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "Email supplied does not match account in our records.");
                        throw new HttpResponseException(response);
                    }
                    var success = userRepository.ValidatePassword(user.password, userLoginRequest.password);

                    if (!success)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "User email and password combination does not match our records.");
                        throw new HttpResponseException(response);
                    }
                    
                    return new UserResponse()
                    {
                        id = user.id,
                        email = user.email,
                        firstName = user.first_name,
                        lastName = user.last_name
                    };
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }

        }
        
    }
}