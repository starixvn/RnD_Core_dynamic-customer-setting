namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;

    public class QueryService : IQueryService
    {
        private readonly IValidationService _validationService;
        private readonly FormHandlerFactory _formHandlerFactory;

        public QueryService()
            : this(null, null)
        {
        }

        public QueryService(IValidationService validationService, FormHandlerFactory formHandlerFactory)
        {
            _validationService = validationService ?? new ValidationService();
            _formHandlerFactory = formHandlerFactory ?? new FormHandlerFactory();
        }

        public Result Execute(QueryInfo queryInfo)
        {
            Result validateResult = _validationService.Validate(queryInfo);
            if (!validateResult.IsSuccess)
            {
                return validateResult;
            }

            var formTypeHandler = _formHandlerFactory.Create(queryInfo.FormType);
            return formTypeHandler.Query(queryInfo);
        }
    }
}