using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Helper
{
    public static class AppConstants
    {
        public static class ApiVersion
        {
            public const int DefaultApiVersion = 1;
            public const string ApiVersionFormat = "{apiVersion:apiVersion}";
        }
        public static class Environment
        {
            public const string Development = "Development";
            public const string Production = "Production";
            public const string Staging = "Staging";
        }
        public static class Entity
        {
            public const string User = "User";
            public const string Team = "Team";
            public const string Task = "Task";

        }
        public static class CrudOperation
        {
            public const string Add = "add";
            public const string Edit = "edit";
            public const string Delete = "delet";
        }
        public static class CrudResult
        {
            private static string Success(string entity, string operation) => $"{entity} {operation}ed successfully";
            private static string Error(string entity, string operation) => $"Error while {operation}ing {entity}. Please contact with Administrator";
            public static string Message(int affectedRows, string entity, string operation)
                => affectedRows > 0 ? Success(entity, operation) : Error(entity, operation);
        }
    }
}
