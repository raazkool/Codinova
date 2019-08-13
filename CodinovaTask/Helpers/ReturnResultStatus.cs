using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.Helpers
{
    public class ReturnResultStatus
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public object Data { get; set; }


        public ReturnResultStatus(Status status, string code, string message, object data = null)
        {
            Status = status.ToString().ToLower();
            Code = code;
            Message = message;
            Data = data;
            Errors = string.Empty;
        }
        public ReturnResultStatus(Status status, string code, string message, object data, string error)
        {
            Status = status.ToString().ToLower();
            Code = code;
            Message = message;
            Data = data;
            Errors = error.ToString();
        }
        public ReturnResultStatus(Status status, string code, string message, object data, ModelStateDictionary error)
        {
            Status = status.ToString().ToLower();
            Code = code;
            Message = message;
            Data = data;
            Errors = GetStateError((error as ModelStateDictionary));
        }

        private Dictionary<string, string[]> GetStateError(ModelStateDictionary modelstate)
        {
            var errorList = modelstate.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return errorList;
        }
    }

    public enum Status
    {
        Success,
        Failed,
        Warning
    }
}
