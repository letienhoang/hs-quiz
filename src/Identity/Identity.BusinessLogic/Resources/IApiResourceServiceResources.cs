using Identity.BusinessLogic.Helpers;

namespace Identity.BusinessLogic.Resources
{
    public interface IApiResourceServiceResources
    {
        ResourceMessage ApiResourceDoesNotExist();

        ResourceMessage ApiResourceExistsValue();

        ResourceMessage ApiResourceExistsKey();

        ResourceMessage ApiSecretDoesNotExist();

        ResourceMessage ApiResourcePropertyDoesNotExist();

        ResourceMessage ApiResourcePropertyExistsKey();

        ResourceMessage ApiResourcePropertyExistsValue();
    }
}