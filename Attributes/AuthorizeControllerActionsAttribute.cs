using Framework.DAO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.FileManipulations;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Framework.Attributes
{
    public class AuthorizeControllerActionsAttribute : AuthorizeAttribute
    {
        public bool CheckInsert { get; set; } = true;
        public bool CheckIdentityInsert { get; set; } = true;
        public bool CheckFind { get; set; } = true;
        public bool CheckUpdate { get; set; } = true;
        public bool CheckDelete { get; set; } = true;
        public bool CheckActivate { get; set; } = true;
        public bool CheckDeactivate { get; set; } = true;
        public bool CheckList { get; set; } = true;

        public AuthorizeControllerActionsAttribute(
            bool checkInsert = true,
            bool checkIdentityInsert = true,
            bool checkFind = true,
            bool checkUpdate = true,
            bool checkActivate = true,
            bool checkDeactivate = true,
            bool checkDelete = true,
            bool checkList = true)
        {
            CheckInsert = checkInsert;
            CheckIdentityInsert = checkIdentityInsert;
            CheckFind = checkFind;
            CheckUpdate = checkUpdate;
            CheckDelete = checkDelete;
            CheckActivate = checkActivate;
            CheckDeactivate = checkDeactivate;
            CheckList = checkList;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
            
            var actionName = actionContext.ActionDescriptor.ActionName;
            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var controllerActionsToCheck = actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AuthorizeControllerActionsAttribute>().FirstOrDefault();

            bool checkAction = true;

            string[] defaultActions = new string[] { "Insert", "IdentityInsert", "Find", "List", "Update", "Delete", "Activate", "Deactivate" };

            if (defaultActions.Contains(actionName) && (
                (actionName == "Insert" && controllerActionsToCheck.CheckInsert)
                    || (actionName == "IdentityInsert" && controllerActionsToCheck.CheckIdentityInsert)
                        || (actionName == "Find" && controllerActionsToCheck.CheckFind)
                            || (actionName == "List" && controllerActionsToCheck.CheckList)
                                || (actionName == "Update" && controllerActionsToCheck.CheckUpdate)
                                    || (actionName == "Delete" && controllerActionsToCheck.CheckDelete)
                                        || (actionName == "Activate" && controllerActionsToCheck.CheckActivate)
                                            || (actionName == "Deactivate" && controllerActionsToCheck.CheckDeactivate)))
                checkAction = true;
            else if (defaultActions.Contains(actionName))
                checkAction = false;

            if (checkAction)
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}
