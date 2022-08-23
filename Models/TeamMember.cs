using System;
using System.Collections.Generic;
using System.Data;

namespace HR.Models
{
    public class TeamMember
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public Position Position { get; set; }

        public Department Department { get; set; }
        
        public string DepartmentName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }

        public Shift Shift { get; set; }

        public Manager Manager { get; set; }
        
        public string ManagerName { get; set; }

        public string Photo { get; set; }

        public string FavoriteColor { get; set; }

        public static List<TeamMember> FetchFromDataTable(DataTable Table)
        {
            List<TeamMember> ls = new List<TeamMember>();

            foreach (DataRow dr in Table.Rows)
            {
                TeamMember tm = new TeamMember();

                if (Table.Columns.Contains("Id"))
                {
                    tm.Id = dr["Id"] is DBNull ? null : (int?)dr["Id"];
                }

                if (Table.Columns.Contains("Name"))
                {
                    tm.Name = dr["Name"] is DBNull ? null : (string)dr["Name"];
                }

                if (Table.Columns.Contains("Address"))
                {
                    tm.Address = dr["Address"] is DBNull ? null : (string)dr["Address"];
                }

                if (Table.Columns.Contains("EmailAddress"))
                {
                    tm.EmailAddress = dr["EmailAddress"] is DBNull ? null : (string)dr["EmailAddress"];
                }

                if (Table.Columns.Contains("PhoneNumber"))
                {
                    tm.PhoneNumber = dr["PhoneNumber"] is DBNull ? null : (string)dr["PhoneNumber"];
                }

                if (Table.Columns.Contains("PositionId"))
                {
                    if (!(dr["PositionId"] is DBNull))
                    {
                        tm.Position = new Position
                        {
                            Id = (int?)dr["PositionId"],
                            Name = (string)dr["PositionName"]
                        };
                    }
                }

                if (Table.Columns.Contains("DepartmentId"))
                {
                    if (!(dr["DepartmentId"] is DBNull))
                    {
                        tm.Department = new Department
                        {
                            Id = (int?)dr["DepartmentId"],
                            Name = (string)dr["DepartmentName"]
                        };

                        tm.DepartmentName = (string)dr["DepartmentName"];
                    }
                }

                if (Table.Columns.Contains("StartDate"))
                {
                    tm.StartDate = dr["StartDate"] is DBNull ? null : (DateTime?)dr["StartDate"];
                }

                if (Table.Columns.Contains("EndDate"))
                {
                    tm.EndDate = dr["EndDate"] is DBNull ? null : (DateTime?)dr["EndDate"];
                }

                if (Table.Columns.Contains("EmploymentStatusId"))
                {
                    if (!(dr["EmploymentStatusId"] is DBNull))
                    {
                        tm.EmploymentStatus = new EmploymentStatus
                        {
                            Id = (int?)dr["EmploymentStatusId"],
                            Name = (string)dr["EmploymentStatusName"]
                        };
                    }
                }

                if (Table.Columns.Contains("ShiftId"))
                {
                    if (!(dr["ShiftId"] is DBNull))
                    {
                        tm.Shift = new Shift
                        {
                            Id = (int?)dr["ShiftId"],
                            Name = (string)dr["ShiftName"]
                        };
                    }
                }

                if (Table.Columns.Contains("ManagerId"))
                {
                    if (!(dr["ManagerId"] is DBNull))
                    {
                        tm.Manager = new Manager
                        {
                            Id = (int?)dr["ManagerId"],
                            Name = (string)dr["ManagerName"]
                        };

                        tm.ManagerName = (string)dr["ManagerName"];
                    }
                }

                if (Table.Columns.Contains("Photo"))
                {
                    tm.Photo = dr["Photo"] is DBNull ? null : (string)dr["Photo"];
                }

                if (Table.Columns.Contains("FavoriteColor"))
                {
                    tm.FavoriteColor = dr["FavoriteColor"] is DBNull ? null : (string)dr["FavoriteColor"];
                }

                ls.Add(tm);
            }

            return ls;
        }
    }
}