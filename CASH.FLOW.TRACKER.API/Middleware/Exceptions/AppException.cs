using System.Net;

namespace CASH.FLOW.TRACKER.API.Middleware.Exceptions
{
    public abstract class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        protected AppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    //CUSTOM EXCEPTIONS
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string resource, object key)
            : base($"{resource} with ID '{key}' was not found.", HttpStatusCode.NotFound) { }
    }

    public sealed class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest) { }
    }

    //CATEGORY EXCEPTIONS
    public sealed class CategoryException : AppException
    {
        public CategoryException(string category)
            : base($"Category '{category}' was not registered.", HttpStatusCode.BadRequest) { }
    }
    public sealed class CategoryNotFoundException : AppException
    {
        public CategoryNotFoundException(int categoryId)
            : base($"{categoryId} not found", HttpStatusCode.NotFound) { }
    }


    public sealed class NoCategoryExistingException : AppException
    {
        public NoCategoryExistingException()
            : base($"No registered category", HttpStatusCode.NotFound) { }
    }

    //TRANSACTION EXCEPTIONS
    public sealed class TransactionException : AppException
    {
        public TransactionException(string transaction)
            : base($"Transaction '{transaction}' was not registered.", HttpStatusCode.BadRequest) { }
    }

    public sealed class TransactionNotFoundException : AppException
    {
        public TransactionNotFoundException()
            : base("No transaction found", HttpStatusCode.NotFound) { }
    }
    public sealed class NoTransactionExistingException : AppException
    {
        public NoTransactionExistingException()
            : base($"No registered transaction", HttpStatusCode.NotFound) { }
    }


}
