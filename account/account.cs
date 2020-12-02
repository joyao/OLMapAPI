using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI;
using System.Data;
using System.Configuration;

namespace OLMapAPI.Models
{
    /// <summary>
    /// account 的摘要描述
    /// </summary>
    public class account
    {
        public account()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }

        #region 帳號操作

        // 判斷角色是否已在存在
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }

        // 判斷使用者是否已在存在
        public bool UserExists(string name)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var user = um.FindByName(name);

            if (user == null)
            {
                return false;

            }
            else
            {
                return true;
            }
        }


        // 新增角色
        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }

        // 新增使用者
        public bool CreateUser(ApplicationUser userName, string password)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var idResult = um.Create(userName, password);
            return idResult.Succeeded;
        }

        // 新增使用者_使用userName
        public bool CreateUser_string(string userName, string password)
        {
            var user = new ApplicationUser() { UserName = userName };
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }

        //刪除使用者
        public bool DeleteUserOnlyASPNET(string username)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == username);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return true;
        }


        //刪除使用者
        public bool DeleteUser(string username)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == username);
            Db.Users.Remove(user);
            Db.SaveChanges();

            System.Web.UI.WebControls.SqlDataSource sds = new System.Web.UI.WebControls.SqlDataSource();
            sds.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            String SQLStr = "DELETE AspNetLinkUserData Where username = @username";
            sds.DeleteParameters.Add("username", username);
            sds.DeleteCommand = SQLStr;
            sds.Delete();

            return true;
        }

        // 將使用者加入角色中
        public bool AddUserToRole(string userName, string roleName)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var user = um.FindByName(userName);
            var idResult = um.AddToRole(user.Id, roleName);
            return idResult.Succeeded;
        }

        // 刪除使用者特定角色
        public void removeUserRole(string userName, string RoleName)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var user = um.FindByName(userName);
            if (user.Id != "")
            {
                um.RemoveFromRole(user.Id, RoleName);
            }
        }

        // 清除所有的角色設定
        public void ClearUserRoles(string userName, string RoleName)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var user = um.FindByName(userName);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(user.Id, rm.FindById(role.RoleId).Name);
            }
        }

        // 列出該使用者的角色設定
        public List<string> ListUserRoles(string userName)
        {
            List<string> list = new List<string>();
            if (userName != "")
            {
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
                var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                var user = um.FindByName(userName);
                if (user != null)
                {
                    var currentRoles = new List<IdentityUserRole>();
                    currentRoles.AddRange(user.Roles);
                    foreach (var role in currentRoles)
                    {
                        list.Add(rm.FindById(role.RoleId).Name);
                    }
                }
            }
            return list;
        }

        public bool checkRoles(string username, string checkrole)
        {
            List<string> roles = ListUserRoles(username);

            foreach (var role in roles)
            {
                if (role == checkrole)
                {
                    return true;
                }
            }
            return false;
        }

        // 列出該角色的使用者
        public List<string> ListRoleUsers(string roleName)
        {
            List<string> list = new List<string>();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = rm.FindByName(roleName);
            var currentUsers = new List<IdentityUserRole>();
            currentUsers.AddRange(role.Users);
            foreach (var user in currentUsers)
            {
                list.Add(um.FindById(user.UserId).UserName);
            }
            return list;
        }

        /// <summary>
        /// 列出該使用者的角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<string> getRolesFromUserId(string UserId)
        {
            List<string> list = new List<string>();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var user = um.FindByName(UserId);
            if (user != null)
            {
                var currentRoles = new List<IdentityUserRole>();
                currentRoles.AddRange(user.Roles);
                foreach (var role in currentRoles)
                {
                    list.Add(rm.FindById(role.RoleId).Name);
                }
            }
            return list;
        }


        //列出使用者列表
        public List<string> getUserList()
        {
            List<string> list = new List<string>();
            var Db = new ApplicationDbContext();
            var users = Db.Users;
            foreach (var user in users)
            {
                list.Add(user.UserName);
            }
            return list;
        }

        //列出權限列表
        public List<string> getRoleList()
        {
            List<string> list = new List<string>();

            var Db = new ApplicationDbContext();
            var roles = Db.Roles;
            foreach (var role in roles)
            {
                list.Add(role.Name);
            }
            return list;
        }
        // 確認使用者是否重複
        public bool UserisExist(string userName)
        {
            var manager = new UserManager();
            ApplicationUser user = manager.FindByName(userName);
            bool bresult = false;
            if (user != null)
            {
                bresult = true;
            }
            return bresult;
        }
        //變更密碼
        public void changePassword(string userName, string Currentpassword, string Newpassword)
        {
            var manager = new UserManager();
            manager.ChangePassword(userName, Currentpassword, Newpassword);
        }

        //admin幫使用者變更密碼
        public void adminChangePassword(string userName, string Newpassword)
        {
            var manager = new UserManager();
            manager.RemovePassword(userName);
            manager.AddPassword(userName, Newpassword);
        }

        public void deluser(string userName)
        {
            var manager = new UserManager();
            ApplicationUser user = manager.FindByName(userName);
            manager.Delete(user);

        }

        public DataView getUserListData()
        {
            System.Web.UI.WebControls.SqlDataSource sds = new System.Web.UI.WebControls.SqlDataSource();
            sds.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            String SQLStr = "SELECT username,name,dep,mail,note FROM AspNetLinkUserData";
            sds.SelectParameters.Clear();
            //sds.SelectParameters.Add("group_name", "%" + group_name + "%");
            sds.SelectCommand = SQLStr;

            DataView dv = (DataView)sds.Select(DataSourceSelectArguments.Empty);

            return dv;
        }
        #endregion
    }
}