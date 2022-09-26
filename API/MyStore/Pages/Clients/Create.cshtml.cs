using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            //save the new client into the database
            try
            {
                String connectionString = "Data Source=DESKTOP-KH043DC;Initial Catalog=mystory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                                 "(name, email, phone, address) VALUES " +
                                 "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        SqlParameter nameParam = command.Parameters.AddWithValue("@name", clientInfo.name);
                        if (clientInfo.name == null)
                        {
                            nameParam.Value = DBNull.Value;
                        }
                        SqlParameter emailParam = command.Parameters.AddWithValue("@email", clientInfo.email);
                        if (clientInfo.email == null)
                        {
                            emailParam.Value = DBNull.Value;
                        }
                        SqlParameter phoneParam = command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        if (clientInfo.phone == null)
                        {
                            phoneParam.Value = DBNull.Value;
                        }
                        SqlParameter addressParam = command.Parameters.AddWithValue("@address", clientInfo.address);
                        if (clientInfo.address == null)
                        {
                            addressParam.Value = DBNull.Value;
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New client added correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}
