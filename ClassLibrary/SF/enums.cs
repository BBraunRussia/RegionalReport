using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public enum TypeOrg { ЛПУ = 1, Отдел = 2, Отделение = 3, Аптека = 4 }

    public enum Roles
    {
        Администратор = 1, Руководство1 = 2, Руководство2 = 3, Региональный_директор = 4, Региональный_представитель = 5,
        Региональный_менеджер = 6, Просмотр_продаж = 7, Просмотр_Ассортиментого_плана = 8, Руководство2_без_отчётов_для_руководства = 9,
        Просмотр = 10, IT_отдел = 11, Транспортный_отдел = 12, Бухгалтерия = 13
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
}
