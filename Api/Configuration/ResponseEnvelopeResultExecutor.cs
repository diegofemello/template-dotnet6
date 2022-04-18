using Application.Utils;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configuration
{
    public class ResponseEnvelope<T>
    {
        public ResponseEnvelope() { }

        public ResponseEnvelope(int status, string message, T data, string details = null)
        {
            Message = message;
            Details = details;
            Data = data;
            Status = status;
        }

        public ResponseEnvelope(int status, string message, string details = null)
        {
            Message = message;
            Details = details;
            Status = status;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Details { set; get; }

        public int Status { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { set; get; }
    }

    public static class GenerateResponse
    {
        public static ResponseEnvelope<T> Result<T>(
            int status, string message, T data, string details = null)
        {
            return new ResponseEnvelope<T>(status, message, data, details);
        }
        public static ResponseEnvelope<object> Result(
            int status, string message, string details = null)
        {
            return new ResponseEnvelope<object>(status, message, details);
        }
    }

    internal class ResponseEnvelopeResultExecutor : ObjectResultExecutor
    {
        public ResponseEnvelopeResultExecutor(
            OutputFormatterSelector formatterSelector,
            IHttpResponseStreamWriterFactory writerFactory,
            ILoggerFactory loggerFactory,
            IOptions<MvcOptions> mvcOptions) : base(
                formatterSelector, writerFactory, loggerFactory, mvcOptions)
        { }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            if (result.StatusCode == null)
                return base.ExecuteAsync(context, result);

            

            int resultStatus = (int)result.StatusCode;
            Type type = result.Value.GetType();

            Console.WriteLine(type.Name);

            if (type == typeof(ResponseEnvelope<>)) { }
            
            else if (resultStatus >= 400)
            {
                ResponseEnvelope<object> response = new();
                response.Status = resultStatus;

                if (resultStatus >= 500 && type == typeof(string))
                {
                    string[] splitedResponse =
                        result.Value.ToString().Split('.');

                    string responseText = "";

                    response.Message = splitedResponse[0] + ".";
                    for (int i = 1; i < splitedResponse.Length; i++)
                    {
                        responseText += splitedResponse[i] + ".";
                    }

                    response.Details = responseText.Replace("..", ".");
                }
                else if (resultStatus == 401 && type != typeof(string))
                {
                    response.Message = "Não autenticado!";
                }
                else if (type == typeof(ValidationProblemDetails))
                {
                    ValidationProblemDetails problem = (ValidationProblemDetails)result.Value;
                    string[] detail =
                        problem.Errors.Values.FirstOrDefault();

                    response.Details = "Ocorreu um erro de validação em um ou mais campos.";
                    response.Message = detail.FirstOrDefault();
                }
                else if (type == typeof(ProblemDetails))
                {
                    ProblemDetails problem = (ProblemDetails)result.Value;
                    response.Message = problem.Title;
                }
                else
                {
                    response.Message = result.Value.ToString();
                }

                result.Value = response;
            }else if (type == typeof(string) )
            {
                ResponseEnvelope<object> response = new();
                response.Message = (string)result.Value;
                response.Status = resultStatus;
                result.Value = response;
            }

            var typpp = typeof(PageList<>);

            Console.WriteLine(typpp.Name);

            if (type.Name == typeof(PageList<>).Name)
            {
                dynamic pageList = result.Value;

                int currentPage = pageList.CurrentPage;
                int pageSize = pageList.PageSize;
                int totalCount = pageList.TotalCount;
                int totalPages = pageList.TotalPages;

                context.HttpContext.Response.AddPagination(currentPage, pageSize, totalCount, totalPages);
            }

            return base.ExecuteAsync(context, result);
        }
    }
}
