using Microsoft.Extensions.Logging;
using System;

namespace AppEnvironment
{
    public class Result
    {
        public Result()
        {

        }

        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public MessageType? MessageType { get; set; }

        public Result(Exception e) //for services 
        {
            IsSuccess = false;
            Code = 500;
            Exception = e;
            MessageType = AppEnvironment.MessageType.ExceptionOccurred;
        }

        public Result(MessageType type) //for services 
        {
            switch (type)
            {
                case AppEnvironment.MessageType.Unauthorized:
                    IsSuccess = false;
                    Code = 401;
                    MessageType = AppEnvironment.MessageType.Unauthorized;
                    break;
                case AppEnvironment.MessageType.OperationFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.OperationFailed;
                    break;
                case AppEnvironment.MessageType.InsertFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.InsertFailed;
                    break;
                case AppEnvironment.MessageType.UpdateFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.UpdateFailed;
                    break;
                case AppEnvironment.MessageType.DeleteFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.DeleteFailed;
                    break;
                case AppEnvironment.MessageType.RecordNotFound:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.RecordNotFound;
                    break;
                case AppEnvironment.MessageType.PhoneNumberExists:
                    IsSuccess = false;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.PhoneNumberExists;
                    break;
                case AppEnvironment.MessageType.EmailExits:
                    IsSuccess = false;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.EmailExits;
                    break;
                case AppEnvironment.MessageType.RecordAlreadyExists:
                    IsSuccess = false;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.RecordAlreadyExists;
                    break;
                case AppEnvironment.MessageType.ExceptionOccurred:
                    IsSuccess = false;
                    Code = 500;
                    MessageType = AppEnvironment.MessageType.ExceptionOccurred;
                    break;
                case AppEnvironment.MessageType.OperationSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.OperationSuccess;
                    break;
                case AppEnvironment.MessageType.InsertSuccess:
                    IsSuccess = true;
                    Code = 200; //201 de koyulabilir
                    MessageType = AppEnvironment.MessageType.InsertSuccess;
                    break;
                case AppEnvironment.MessageType.UpdateSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.UpdateSuccess;
                    break;
                case AppEnvironment.MessageType.DeleteSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.DeleteSuccess;
                    break;

                default:
                    IsSuccess = false;
                    Code = 500;
                    Exception = new ArgumentOutOfRangeException();
                    MessageType = AppEnvironment.MessageType.ExceptionOccurred;
                    break;
            }
        }
    }

    public class Result<T> : Result
    {
        public Result(T data) //messagetype da verilebilir. operation complaeted tarzi
        {
            IsSuccess = true;
            Code = 200;
            Data = data;
        }

        public Result(Exception e) //for services 
        {
            IsSuccess = false;
            Code = 500;
            Exception = e;
            MessageType = AppEnvironment.MessageType.ExceptionOccurred;
        }

        public Result(Exception e, ILogger logger, string userId) //for controller services 
        {
            IsSuccess = false;
            Code = 500;
            Exception = e;
            MessageType = AppEnvironment.MessageType.ExceptionOccurred;
            logger.LogError(e, $"UserId:'{userId}', {e.Message}");
        }

        public Result(MessageType type) //for services 
        {
            switch (type)
            {
                case AppEnvironment.MessageType.Unauthorized:
                    IsSuccess = false;
                    Code = 401;
                    MessageType = AppEnvironment.MessageType.Unauthorized;
                    break;
                case AppEnvironment.MessageType.OperationFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.OperationFailed;
                    break;
                case AppEnvironment.MessageType.InsertFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.InsertFailed;
                    break;
                case AppEnvironment.MessageType.UpdateFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.UpdateFailed;
                    break;
                case AppEnvironment.MessageType.DeleteFailed:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.DeleteFailed;
                    break;
                case AppEnvironment.MessageType.RecordNotFound:
                    IsSuccess = false;
                    Code = 404;
                    MessageType = AppEnvironment.MessageType.RecordNotFound;
                    break;
                case AppEnvironment.MessageType.PhoneNumberExists:
                    IsSuccess = false;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.PhoneNumberExists;
                    break;
                case AppEnvironment.MessageType.EmailExits:
                    IsSuccess = false;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.EmailExits;
                    break;
                case AppEnvironment.MessageType.RecordAlreadyExists:
                    IsSuccess = true;
                    Code = 409; //Conflict
                    MessageType = AppEnvironment.MessageType.RecordAlreadyExists;
                    break;
                case AppEnvironment.MessageType.ExceptionOccurred:
                    IsSuccess = false;
                    Code = 500;
                    MessageType = AppEnvironment.MessageType.ExceptionOccurred;
                    break;
                case AppEnvironment.MessageType.OperationSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.OperationSuccess;
                    break;
                case AppEnvironment.MessageType.InsertSuccess:
                    IsSuccess = true;
                    Code = 200; //201 de koyulabilir
                    MessageType = AppEnvironment.MessageType.InsertSuccess;
                    break;
                case AppEnvironment.MessageType.UpdateSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.UpdateSuccess;
                    break;
                case AppEnvironment.MessageType.DeleteSuccess:
                    IsSuccess = true;
                    Code = 200;
                    MessageType = AppEnvironment.MessageType.DeleteSuccess;
                    break;
                default:
                    IsSuccess = false;
                    Code = 500;
                    Exception = new ArgumentOutOfRangeException();
                    MessageType = AppEnvironment.MessageType.ExceptionOccurred;
                    break;
            }
        }

        public Result()
        {
        }

        public T Data { get; set; }
    }

    public enum MessageType
    {
        OperationFailed,
        OperationSuccess,
        InsertFailed,
        InsertSuccess,
        UpdateFailed,
        UpdateSuccess,
        DeleteFailed,
        DeleteSuccess,
        RecordNotFound,
        PhoneNumberExists,
        StockNotEnough,
        EmailExits,
        RecordAlreadyExists,
        ExceptionOccurred,
        InsufficientBalance,
        Unauthorized,
        Forbidden,
        ContactUs
    }
}