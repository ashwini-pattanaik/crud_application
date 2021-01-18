using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace backend.Controllers
{
    
    public class EmployeesController : ApiController
    {
        public IEnumerable<employee> Get()
        {
            using (ashwiniEntities entities = new ashwiniEntities())
            {
                return entities.employees.ToList();
            }
        }

        [Route("api/employees/read/{id}")]
        public HttpResponseMessage Get(int id)
        {
            using (ashwiniEntities entities = new ashwiniEntities())
            {
                var entity=entities.employees.FirstOrDefault(e => e.id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id" + id.ToString() + "not found");
                }
            }
        }
        [HttpPost]
        public HttpResponseMessage create([FromBody] employee employee)
        {
            try
            {
                using (ashwiniEntities entities = new ashwiniEntities())
                {
                    entities.employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.id.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
       
        [Route("api/employees/update/{id}")]
        public HttpResponseMessage Put(int id,[FromBody]employee employee)
        {
            try
            {
                using (ashwiniEntities entities = new ashwiniEntities())
                {
                    var entity = entities.employees.FirstOrDefault(e => e.id == id);
                        if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id" + id.ToString() + "not found to update");
                    }
                    else
                    {
                        entity.name = employee.name;
                        entity.email = employee.email;
                        entity.designation = employee.designation;
                        entity.phoneNumber = employee.phoneNumber;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/employees/delete/{id}")]
        public HttpResponseMessage delete(int id)
        {
            try
            {
                using (ashwiniEntities entities = new ashwiniEntities())
                {
                    var entity = entities.employees.FirstOrDefault(e => e.id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id=" + id.ToString() + "not found to delete");
                    }
                    else
                    {
                        entities.employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
