using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class RolePermissionDTO
    {
        public string  activeFlag { get; set; }
        public ButtonPermisionDTO buttonPermissionDatas { get; set; }
        public Int32 companyid { get; set; }
        public Int32 displayorder { get; set; }
        public string dynamicPresence { get; set; }
        public string icon { get; set; }
        public Int32 moduleId { get; set; }
        public string moduleName { get; set; }
        public Int32 roleid { get; set; }
        public string rolename { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string browser { get; set; }
    }

    public class ButtonPermisionDTO
     {
        public Boolean add { get; set; }
        public Boolean edit { get; set; }
        public Boolean view { get; set; }
        public Boolean print { get; set; }
        public Boolean delete { get; set; }
        public Boolean export { get; set; }
     }

    public class ButtondefaultDTO
    {
        public Boolean add = false;
        public Boolean edit = false;
        public Boolean view = false;
        public Boolean print = false;
        public Boolean delete = false;
        public Boolean export = false;
    }
    public class RolesDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string browser { get; set; }
        public string ipaddress { get; set; }
    }

    public class RolePermissionEditDTO
    {
        public Int64 id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public Int64 companyid { get; set; }
        public Int64 displayorder { get; set; }
        public bool activeflag { get; set; }
      //  public ButtonPermisionDTO buttonpermissiondata { get; set; }
      public string buttonpermissiondata { get; set; }
        public string dynamicpresence { get; set; }
         public string rolename { get; set; }
        public Int64 roleid { get; set; }
       
    }

    public class RolePermissionLoadDTO
    {
        public Int64 id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public Int64 companyid { get; set; }
        public Int64 displayorder { get; set; }
        public bool activeflag { get; set; }
        //  public ButtonPermisionDTO buttonpermissiondata { get; set; }
        public string buttonpermissiondata { get; set; }
        public string dynamicpresence { get; set; }
        public string rolename { get; set; }
        public Int64 roleid { get; set; }

    }

    public class RolePermissionDTO2
    {
        public Boolean activeFlag { get; set; }
        public ButtonPermisionDTO buttonPermissionDatas { get; set; }
        public Int32 companyid { get; set; }
        public Int32 displayorder { get; set; }
        public string dynamicPresence { get; set; }
        public string icon { get; set; }
        public Int32 moduleId { get; set; }
        public string moduleName { get; set; }
        public Int32 roleid { get; set; }
        public string rolename { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string browser { get; set; }
    }


}
