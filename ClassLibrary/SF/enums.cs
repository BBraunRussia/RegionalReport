using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public enum TypeOrg { ЛПУ = 1, Отдел = 2, Отделение = 3, Аптека = 4, Дистрибьютор = 5, Административное_Учреждение = 6 }
    
    public enum RolesSF
    {
        Администратор = 1, Пользователь = 2
    }

    public enum ContextMenuItem
    {
        Separator,
        AddOrganization, DeleteOrganization, EditOrganization,        
        AddPerson,
        CityList,
        Import, Export,
        Exit,
        Filter, Sort
    }

    public enum Action { Создал = 1, Редактировал = 2, Удалил = 3 }
    public enum Files { rules_lpu = 1, rules_pharmacy = 2, rules_department = 3 }
    public enum HistoryType { organization = 1, person = 2 }
    public enum SDiv { HC = 1, AE = 2, OM = 3 }
}
