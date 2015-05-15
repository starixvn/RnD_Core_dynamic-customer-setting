namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;

    public class QueryService : IQueryService
    {
        private readonly IValidationService _validationService = new ValidationService();

        public Result Do(QueryInfo queryInfo)
        {
            Result validateResult = _validationService.Validate(queryInfo);
            if (!validateResult.IsSuccess)
            {
                return validateResult;
            }

            var formTypeHandler = FormHandlerFactory.Create(queryInfo.FormType);
            return formTypeHandler.Query(queryInfo);
        }
    }
}