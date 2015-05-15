namespace Fanex.CustomerSetting.FormTypes
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public sealed class FormHandlerFactory : IFormHandlerFactory
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IResourceService _resourceService;

        public FormHandlerFactory()
            : this(null, null)
        {
        }

        public FormHandlerFactory(IQueryRepository queryRepository, IResourceService resourceService)
        {
            _queryRepository = queryRepository ?? new QueryRepository();
            _resourceService = resourceService ?? new ResourceService();
        }

        public IFormHandler Create(FormType formType)
        {
            switch (formType)
            {
                case FormType.CompanyEditSuper:
                    return new CompanyEditSuperFormHandler(_queryRepository, _resourceService);

                case FormType.SuperEditMaster:
                    return new SuperEditMasterFormHandler(_queryRepository, _resourceService);

                case FormType.MasterEditAgent:
                    return new MasterEditAgentFormHandler(_queryRepository, _resourceService);

                case FormType.SuperEditMember:
                    return new SuperEditMemberFormHandler(_queryRepository, _resourceService);

                case FormType.MasterEditMember:
                    return new MasterEditMemberFormHandler(_queryRepository, _resourceService);

                case FormType.AgentEditMember:
                    return new AgentEditMemberFormHandler(_queryRepository, _resourceService);

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}