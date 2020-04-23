using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SqlDbService : IPrescriptionsDbService
    {
        private const string connectionStr = "Data Source=db-mssql;" +
                                         "Initial Catalog=s19461;Integrated Security=True";

        public IEnumerable<Prescription> GetPrescriptions(string lekarz)
        {

            {
                var _prescriptions = new List<Prescription>();
                if (lekarz == null)
                {
                    using (var connection = new SqlConnection(connectionStr))
                    using (var commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        commands.CommandText = "SELECT p.IdPrescription, p.Date, p.DueDate, d.LastName 'Doctors LastName', pat.LastName 'Patients LastName' " +
                                               "FROM Doctor d INNER JOIN Prescription p ON d.IdDoctor = p.IdDoctor " +
                                               "INNER JOIN Patient pat ON p.IdPatient = pat.IdPatient Order by Date Desc;";

                        connection.Open();
                        var dr = commands.ExecuteReader();

                        while (dr.Read())
                        {
                            _prescriptions.Add(
                                new Prescription
                                {
                                    IdPrescription = Int32.Parse(dr["IdPrescription"].ToString()),
                                    Date = dr["Date"].ToString(),
                                    DueDate = dr["DueDate"].ToString(),
                                    PatientLastName = dr["Patients LastName"].ToString(),
                                    DoctorLastName = dr["Doctors LastName"].ToString()
                                });
                        }
                        dr.Close();
                    }
                }
                else
                {
                    using (var connection = new SqlConnection(connectionStr))
                    using (var commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        commands.CommandText = "SELECT p.IdPrescription, p.Date, p.DueDate, d.LastName 'Doctors LastName', pat.LastName 'Patients LastName' " +
                                               "FROM Doctor d " +
                                               "INNER JOIN Prescription p ON d.IdDoctor = p.IdDoctor " +
                                               "INNER JOIN Patient pat ON p.IdPatient = pat.IdPatient " +
                                               "WHERE d.LastName = @lekarz Oreder by Date Desc";

                        commands.Parameters.Add(new SqlParameter("lekarz", lekarz));

                        connection.Open();
                        var dr = commands.ExecuteReader();

                        while (dr.Read())
                        {
                            _prescriptions.Add(
                                new Prescription
                                {
                                    IdPrescription = Int32.Parse(dr["IdPrescription"].ToString()),
                                    Date = dr["Date"].ToString(),
                                    DueDate = dr["DueDate"].ToString(),
                                    PatientLastName = dr["Patients LastName"].ToString(),
                                    DoctorLastName = dr["Doctors LastName"].ToString()
                                });
                        }
                        dr.Close();
                    }
                }
                return _prescriptions;
            }

        }

        public PrescriptionsRequest CreatePrescription(PrescriptionsRequest request)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                com.Transaction = transaction;

                com.CommandText = "Insert Into Prescription (Date, DueDate,IdPatient,IdDoctor)" +
                    "Values(@Date,@DueDate,@IdPatient,@IdDoctor)";

                
                com.Parameters.AddWithValue("Date", request.Date);
                com.Parameters.AddWithValue("DueDate", request.DueDate);
                com.Parameters.AddWithValue("IdPatient", request.IdPatient);
                com.Parameters.AddWithValue("IdDoctor", request.IdDoctor);

                com.ExecuteNonQuery();
                transaction.Commit();



            }
            return request;


        }

    }
}