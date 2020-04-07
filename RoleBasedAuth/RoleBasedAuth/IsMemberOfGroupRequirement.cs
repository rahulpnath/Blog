using Microsoft.AspNetCore.Authorization;

namespace RoleBasedAuth
{
    public class IsMemberOfGroupRequirement : IAuthorizationRequirement
    {
        public readonly string GroupId;
        public readonly string GroupName;

        public IsMemberOfGroupRequirement(string groupName, string groupId)
        {
            GroupName = groupName;
            GroupId = groupId;
        }
    }
}
