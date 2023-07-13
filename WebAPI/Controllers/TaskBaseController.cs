using DataAccessEF.Services.LogFile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Shared;
using System.Linq;

namespace WebAPI.Controllers
{
    public class TaskBaseController: Controller
    {
        protected readonly LogFileService Logger;

        public TaskBaseController(LogFileService logger)
        {
            this.Logger = logger;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }




        #region try catch log

        protected DescriptiveResponse<T> TryCatchLog<T>(Func<T> function)
        {
            try
            {
                var res = function.Invoke();
                return new DescriptiveResponse<T>()
                {
                    Result = res,
                    Status = true
                };
            }
            catch (TaskException ex)
            {
                this.Logger.LogValidation(ex.ToString());
                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)ex.ValidationEum).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
            catch (AggregateException aggEx)
            {
                TaskValidationKeysEnum msg;
                if (aggEx.InnerException is TaskException)
                {
                    var valExp = (TaskException)aggEx.InnerException;
                    this.Logger.LogValidation($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    //this.Logger.LogValidation($"{valExp}\nStackTrace:\n{valExp.StackTrace}");
                    msg = valExp.ValidationEum;
                }
                else
                {
                    this.Logger.LogException($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    //this.Logger.LogException($"{innerExp}\nStackTrace:\n{innerExp.StackTrace}");
                    msg = TaskValidationKeysEnum.UnhandeledException;
                }

                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)msg).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
            catch (Exception ex)
            {
                this.Logger.LogException(ex.ToString());
                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)TaskValidationKeysEnum.UnhandeledException).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
        }

        protected async Task<DescriptiveResponse<T>> TryCatchLogAscync<T>(Func<Task<T>> function)
        {
            try
            {
                var res = await function.Invoke();
                return new DescriptiveResponse<T>()
                {
                    Result = res,
                    Status = true
                };
            }
            catch (TaskException ex)
            {
                this.Logger.LogValidation(ex.ToString());
                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)ex.ValidationEum).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
            catch (AggregateException aggEx)
            {
                TaskValidationKeysEnum msg;
                if (aggEx.InnerException is TaskException)
                {
                    var valExp = (TaskException)aggEx.InnerException;
                    this.Logger.LogValidation($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    msg = valExp.ValidationEum;
                }
                else
                {
                    this.Logger.LogException($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    msg = TaskValidationKeysEnum.UnhandeledException;
                }

                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)msg).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
            catch (Exception ex)
            {
                this.Logger.LogException(ex.ToString());
                return new DescriptiveResponse<T>
                {
                    Status = false,
                    Message = ((int)TaskValidationKeysEnum.UnhandeledException).ToString(),
                    ErrorCode = (int)DescriptiveResponseMessagesEnum.ErrorPleaseTryAgain
                };
            }
        }


        protected ActionResult TryCatchLogReturnAction(Func<ActionResult> function)
        {
            try
            {
                return function.Invoke();
            }
            catch (TaskException ex)
            {
                this.Logger.LogValidation(ex.ToString());
                return StatusCode(500, ex.ValidationEum);
            }
            catch (AggregateException aggEx)
            {
                if (aggEx.InnerException is TaskException)
                {
                    this.Logger.LogValidation($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    return StatusCode(500, (aggEx.InnerException as TaskException).ValidationEum);
                }
                else
                {
                    this.Logger.LogException($"{aggEx}\nStackTrace:\n{aggEx.StackTrace}");
                    return StatusCode(500, TaskValidationKeysEnum.UnhandeledException);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogException(ex.ToString());
                return StatusCode(500, TaskValidationKeysEnum.UnhandeledException);
            }
        }


        #endregion

        protected long GetUserId_Token()
        {
            long _userId;
            return long.TryParse(User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value, out _userId)
                ? _userId : throw new TaskException(TaskValidationKeysEnum.NonAuthorized);
        }
    }
}
