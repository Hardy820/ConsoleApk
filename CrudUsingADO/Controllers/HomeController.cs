using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrudUsingADO.Models;

namespace CrudUsingADO.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString);

        // GET: ADOConnectedExamples
        public ActionResult Index()
        {
            List<tblDepartment> departmentList = new List<tblDepartment>();

            connection.Open();
            string query = "SELECT ID, DepartmentName, Location, IsActive FROM tblDepartment";
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                tblDepartment dept = new tblDepartment()
                {
                    DepartmentName = reader["DepartmentName"].ToString(),
                    ID = Convert.ToInt32(reader[0]),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    Location = reader[2].ToString()
                };
                departmentList.Add(dept);
            }
            return View(departmentList);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(tblDepartment model)
        {
            //Using Parameterized Query 
            if (ModelState.IsValid)
            {
                string ParameterizedQuery = "INSERT INTO tblDepartment (DepartmentName, Location, IsActive) VALUES (@DepartmentName, @Location, @IsActive)";

                SqlCommand command = new SqlCommand(ParameterizedQuery, connection);

                connection.Open();
                command.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                command.Parameters.AddWithValue("@Location", model.Location);
                command.Parameters.AddWithValue("@IsActive", model.IsActive);

                int output = command.ExecuteNonQuery();
                if (output > 0)
                {
                    ViewBag.Message = "Record Inserted Successfully";
                }
                else
                {
                    ViewBag.Message = "Record Not Inserted. Try again later.";

                }
            }
            connection.Close();
            return View();
        }

        public ActionResult Edit(int? ID)
        {
            if (ID != null)
            {

                tblDepartment department = new tblDepartment();
                string query = "SELECT ID, DepartmentName, Location, IsActive FROM tblDepartment WHERE ID=" + ID;
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    department.ID = Convert.ToInt32(reader["ID"]);
                    department.DepartmentName = reader["DepartmentName"].ToString();
                    department.IsActive = Convert.ToBoolean(reader["IsActive"]);
                    department.Location = reader["Location"].ToString();
                }
                else
                {
                    ViewBag.Message = "Error : No Record Found";
                }
                return View(department);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpPost]
        public ActionResult Edit(tblDepartment model)
        {
            string query = "UPDATE tblDepartment SET DepartmentName='" + model.DepartmentName + "', Location='" +
                model.Location + "', IsActive='" + model.IsActive + "' WHERE ID=" + model.ID;
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            //int i = command.ExecuteNonQuery();
            //connection.Close();


            ////Using Parameterized Query 

            //string ParameterizedQuery = "UPDATE tblDepartment SET DepartmentName=@DepartmentName, Location=@myLocation, IsActive=@IsActive WHERE ID=@ID";
            //SqlCommand command1 = new SqlCommand(ParameterizedQuery, connection);

            //connection.Open();
            //command1.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
            //command1.Parameters.AddWithValue("@myLocation", model.Location);
            //command1.Parameters.AddWithValue("@IsActive", model.IsActive);
            //command1.Parameters.AddWithValue("@ID", model.ID);

            int output = command.ExecuteNonQuery();
            if (output > 0)
            {
                ViewBag.Message = "Record Updated Successfully";
            }
            else
            {
                ViewBag.Message = "Record Not Updated. Try again later.";

            }
            connection.Close();

            return View(model);
        }

        public ActionResult Delete(int ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int ID)
        {
            string ParameterizedQuery = "DELETE FROM tblDepartment WHERE ID =" + ID;

            SqlCommand command = new SqlCommand(ParameterizedQuery, connection);

            command.Parameters.AddWithValue("@ID", ID);

            connection.Open();

            int output = command.ExecuteNonQuery();

            connection.Close();

            if (output > 0)
            {
                ViewBag.Message = "Record Deleted Successfully";
            }
            else
            {
                ViewBag.Message = "Record Not Deleted. Try again later.";
            }

            return RedirectToAction("Index");
        }
    }
}