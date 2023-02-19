using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonInfrastructure.Http.Helpers
{
    public static class CommonHttpHelper
    {
        public static CommonHttpResponse BuildErrorResponse(
           HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
           List<string> extErrors = null,
           string initialError = null)
        {
            if (initialError == null) initialError = $"Ошибка при выполнении запроса. Код: {(int)statusCode}";
            var errors = new List<string> { initialError };
            if (extErrors != null)
            {
                errors.AddRange(extErrors);
            }

            return new CommonHttpResponse
            {
                Success = false,
                StatusCode = statusCode,
                Errors = errors.ToArray()
            };
        }

        public static CommonHttpResponse BuildNotFoundErrorResponse(HttpStatusCode statusCode = HttpStatusCode.NotFound,
           List<string> extErrors = null,
           string initialError = null)
        {
            if (initialError == null) initialError = $"Сущность не найдена";
            var errors = new List<string> { initialError };
            if (extErrors != null)
            {
                errors.AddRange(extErrors);
            }

            return new CommonHttpResponse
            {
                Success = false,
                StatusCode = statusCode,
                Errors = errors.ToArray()
            };
        }

        public static CommonHttpResponse<T> BuildNotFoundErrorResponse<T>(HttpStatusCode statusCode = HttpStatusCode.NotFound,
           List<string> extErrors = null,
           string initialError = null)
        {
            if (initialError == null) initialError = $"Сущность не найдена";
            var errors = new List<string> { initialError };
            if (extErrors != null)
            {
                errors.AddRange(extErrors);
            }

            return new CommonHttpResponse<T>
            {
                Success = false,
                Data = default(T),
                StatusCode = statusCode,
                Errors = errors.ToArray()
            };
        }
        public static CommonHttpResponse<T> BuildErrorResponse<T>(
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            List<string> extErrors = null,
            string initialError = null)
        {
            if (initialError == null) initialError = $"Ошибка при выполнении запроса. Код: {statusCode}";
            var errors = new List<string> { initialError };
            if (extErrors != null)
            {
                errors.AddRange(extErrors);
            }

            return new CommonHttpResponse<T>
            {
                Success = false,
                Data = default(T),
                StatusCode = statusCode,
                Errors = errors.ToArray()
            };
        }

        public static CommonHttpResponse BuildSuccessResponse(
            HttpStatusCode statusCode = HttpStatusCode.NoContent
        )
        {
            return new CommonHttpResponse
            {
                Success = true,
                StatusCode = statusCode
            };
        }

        public static CommonHttpResponse<T> BuildSuccessResponse<T>(
            T data,
            HttpStatusCode statusCode = HttpStatusCode.OK
        )
        {
            return new CommonHttpResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Data = data
            };
        }

    }
}
