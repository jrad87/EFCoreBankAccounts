using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

using Newtonsoft.Json;


namespace EFCoreBankAccounts
{
    public class ModelStateTransferValue
    {
        public string Key {get; set;}
        public string AttemptedValue {get; set;}
        public object RawValue {get; set;}
        public ICollection<string> ErrorMessages {get; set;} = new List<string>();
    }
    public class ModelStateTransfer : ActionFilterAttribute 
    {
        protected const string Key = nameof(ModelStateTransfer);
    }
    public class ExportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(!context.ModelState.IsValid && (
                context.Result is RedirectResult ||
                context.Result is RedirectToActionResult ||
                context.Result is RedirectToRouteResult
            ))
            {
                var controller = context.Controller as Controller;
                if(controller != null && context.ModelState != null){
                    var modelState = this.SerializeModelState(context.ModelState);
                    controller.TempData[Key] = modelState;
                }
            }
            base.OnActionExecuted(context);
        }
        private string SerializeModelState(ModelStateDictionary modelState)
        {
            var errorList = modelState.Select(kvp => new ModelStateTransferValue{
                Key = kvp.Key,
                AttemptedValue = kvp.Value.AttemptedValue,
                RawValue = kvp.Value.RawValue,
                ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList()
            });
            return JsonConvert.SerializeObject(errorList);
        }
    }
    public class ImportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            var serializedModelState = controller?.TempData[Key] as string;

            if(serializedModelState != null)
            {
                if(context.Result is ViewResult)
                {
                    var modelState = this.DeserializeModelState(serializedModelState);                    
                    context.ModelState.Merge(modelState);
                }
                else
                {
                    controller.TempData.Remove(Key);
                }
            }
            base.OnActionExecuted(context);
        }   
        private ModelStateDictionary DeserializeModelState(string serializedErrorList)
        {
            var errorList = JsonConvert.DeserializeObject<List<ModelStateTransferValue>>(serializedErrorList);
            var modelState = new ModelStateDictionary();
            foreach(var item in errorList)
            {
                modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
                foreach(string error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                } 
            }
            return modelState;
        }
    }
}