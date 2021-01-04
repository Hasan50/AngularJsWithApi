using System.Collections.Generic;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.Common;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IUserCredential
    {
        ResponseModel Save(UserCredentialModel model);
        ResponseModel Update(UserCredentialModel model);
        UserCredentialModel Get(string username, string password);

        ResponseModel ChangePassword(string userInitial, string newPassword);
        ResponseModel DeleteUser(string Id);
        UserCredentialModel GetProfileDetails(string userId);
        UserCredentialModel GetByLoginID(string loginID);
        UserCredentialModel GetByLoginID(string loginID, UserType uType);
        UserCredentialModel GetByLoginID(string loginID, string password, UserType uType);
        UserCredentialModel GetUserFullInfo(string userId);
        List<UserCredentialModel> GetPushToken(int companyId);
    }
}