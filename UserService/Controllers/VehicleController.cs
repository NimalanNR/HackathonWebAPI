using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using VehicleDataAccess;
namespace UserService.Controllers
{
    [RoutePrefix("api/Vehicle")]
    public class VehicleController : ApiController
    {
        VehicleDBEntities ve = new VehicleDBEntities();
        [HttpGet]
        [Route("findall")]
        public HttpResponseMessage findAll()
        {

            try
            {
                /*var response = new HttpResponseMessage();
                response.Content = new StringContent(JsonConvert.SerializeObject(ve.VehicleDetails.ToList(),Formatting.Indented));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response.StatusCode = HttpStatusCode.OK;*/
                string response = JsonConvert.SerializeObject(ve.VehicleDetails.ToList(), Formatting.Indented).ToString();
                response = "{ \"vehicle \":" + response + "}";
                return Request.CreateResponse(HttpStatusCode.OK,response);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage create(VehicleDetail vehicledetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                ve.VehicleDetails.Add(vehicledetail);
                ve.SaveChanges();
                response.StatusCode = HttpStatusCode.OK;
                return Request.CreateResponse(HttpStatusCode.OK, vehicledetail);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }

        }
        [HttpGet]
        [Route("mileage/{bike_id}")]
        public HttpResponseMessage mileage(long bike_id)
        {

            try
            {

                var response = new HttpResponseMessage();
                var Currentuser = ve.VehicleDetails.SingleOrDefault(v => v.Bike_Id == bike_id);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                long? total_fuel = Currentuser.Existing_Fuel + Currentuser.Fuel_Refil;
                long? fuel_consumed = total_fuel - Currentuser.Fuel_Left;
                long? total_kilometer = Currentuser.Current_Odometer_Reading - Currentuser.Initial_Odometer_Reading;
                long? result = total_kilometer / fuel_consumed;

                var obj = new Models.MileageModels();
                obj.Model = Currentuser.Bike_Model;
                obj.Mileage = result.ToString();
                if (result < 30)
                    obj.PerformanceMessage = "Poor Mileage, Must require Service and  proper Maintainance";
                else if (result >= 30 && result <= 45)
                    obj.PerformanceMessage = "Good Mileage, but still proper maintainance need to achieve expected Mileage";
                else
                    obj.PerformanceMessage = "Excellent Mileage as Expected. Keep Continuing";


                response.Content = new StringContent(JsonConvert.SerializeObject(obj));
                response.StatusCode = HttpStatusCode.OK;
                //string response1 = JsonConvert.SerializeObject(obj).ToString();
                // response1 = "{ \"mileage\":" + response1 + "}";
                // return Request.CreateResponse(HttpStatusCode.OK,response);
                return response;

               
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpGet]
        [Route("mileagecomparison/{bike_id}/{bike_id1}")]
        public HttpResponseMessage mileagecomparisondifferentuser(string bike_id,string bike_id1)
        {
            //string[] str = bike_id.Split(',');
            long your_bike_id = Convert.ToInt64(bike_id);
            long compare_bike_id = Convert.ToInt64(bike_id1);

            try
            {

                var response = new HttpResponseMessage();
                var Currentuser = ve.VehicleDetails.SingleOrDefault(v => v.Bike_Id == your_bike_id);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                long? Current_user_total_fuel = Currentuser.Existing_Fuel + Currentuser.Fuel_Refil;
                long? Current_user_fuel_consumed = Current_user_total_fuel - Currentuser.Fuel_Left;
                long? Current_user_total_kilometer = Currentuser.Current_Odometer_Reading - Currentuser.Initial_Odometer_Reading;
                long? Current_user_result = Current_user_total_kilometer / Current_user_fuel_consumed;
                var Compareuser = ve.VehicleDetails.SingleOrDefault(v => v.Bike_Id == compare_bike_id);
                if (Compareuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                long? Compare_user_total_fuel = Compareuser.Existing_Fuel + Compareuser.Fuel_Refil;
                long? Compare_user_fuel_consumed = Compare_user_total_fuel - Compareuser.Fuel_Left;
                long? Compare_user_total_kilometer = Compareuser.Current_Odometer_Reading - Compareuser.Initial_Odometer_Reading;
                long? Compare_user_result = Compare_user_total_kilometer / Compare_user_fuel_consumed;
                var obj = new Models.ComparisonModel();
                if (Current_user_result > Compare_user_result)
                    obj.Performance_Message = "Your Bike Performance is Better than Compared one";
                else if (Current_user_result == Compare_user_result)
                    obj.Performance_Message = "Your Bike Performance is Equal to the Compared one";
                else
                    obj.Performance_Message = "Your Bike Performance is Poor with the Compared one";
                obj.Current_user_Bike_Model = Currentuser.Bike_Model;
                obj.Current_user_Mileage = Current_user_result.ToString();
                obj.Compare_user_Bike_Model = Compareuser.Bike_Model;
                obj.Compare_user_Mileage = Compare_user_result.ToString();
                response.Content = new StringContent(JsonConvert.SerializeObject(obj));
                response.StatusCode = HttpStatusCode.OK;
                //string response = JsonConvert.SerializeObject(obj);
                //response = "{\"mileagecomparison\":" + response + "}";

                //return Request.CreateResponse(HttpStatusCode.OK,response);
                return response;

            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpPut]
        [Route("Update")]
        public HttpResponseMessage update(VehicleDetail vehicledetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                var Currentuser = ve.VehicleDetails.SingleOrDefault(v => v.Bike_Id == vehicledetail.Bike_Id);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                Currentuser.Users_Id = vehicledetail.Users_Id;
                Currentuser.Bike_Model = vehicledetail.Bike_Model;
                Currentuser.Existing_Fuel = vehicledetail.Existing_Fuel;
                Currentuser.Fuel_Refil = vehicledetail.Fuel_Refil;
                Currentuser.Fuel_Left = vehicledetail.Fuel_Left;
                Currentuser.Amount_Paid = vehicledetail.Amount_Paid;
                Currentuser.Initial_Odometer_Reading = vehicledetail.Initial_Odometer_Reading;
                Currentuser.Current_Odometer_Reading = vehicledetail.Current_Odometer_Reading; 
                ve.SaveChanges();
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage delete(VehicleDetail vehicledetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                var Currentuser = ve.VehicleDetails.SingleOrDefault(v => v.Bike_Id == vehicledetail.Bike_Id);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                ve.VehicleDetails.Remove(Currentuser);
                ve.SaveChanges();
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }

        }
    }
}
