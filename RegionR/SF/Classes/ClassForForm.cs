using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;

namespace RegionR.SF
{
    public static class ClassForForm
    {
        public static void LoadDictionary(ComboBox combo, DataTable dt, bool isNeedAddEmpty = true)
        {
            //Добавляем отсутствие значения
            DataTable newDT = new DataTable();
            newDT.Columns.Add(dt.Columns[0].ColumnName);
            newDT.Columns.Add(dt.Columns[1].ColumnName);

            if (isNeedAddEmpty)
            {
                newDT.Rows.Add(0, "");
            }

            foreach (DataRow row in dt.Rows)
            {
                newDT.Rows.Add(row.ItemArray[0], row.ItemArray[1]);
            }

            combo.DataSource = newDT;

            if (newDT == null)
            {
                MessageBox.Show("Отсутствуют данные в зависимых ячейках", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            combo.ValueMember = newDT.Columns[0].ColumnName;
            combo.DisplayMember = newDT.Columns[1].ColumnName;
        }

        public static void CheckFilled(string text, string fieldName)
        {
            if (text == string.Empty)
                throw new NullReferenceException(string.Concat("Не заполнено поле \"", fieldName, "\""));
        }

        public static bool DeleteOrganization(Organization organization)
        {
            if (MessageBox.Show(string.Concat("Вы действительно хотите удалить организацию \"", organization.ShortName, "\"?\n\nПри этом будут удалены входящие в неё подразделения и привязанные к ним персоны, если таковые имеются."), "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                OrganizationList organizationList = OrganizationList.GetUniqueInstance();

                if (organizationList.GetChildList(organization).Count() > 0)
                {
                    if (MessageBox.Show("У филиала имеются зависимые организации, они будут также удалены. Продолжить удаление?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)    
                        return false;
                }

                PersonList personList = PersonList.GetUniqueInstance();

                if (personList.GetItems(organization).Count() > 0)
                {
                    if (MessageBox.Show("В организации имеются сотрудники, они будут также удалены. Продолжить удаление?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return false;
                }

                organization.Delete();

                return true;
            }

            return false;
        }

        public static void CheckINN(Organization organization, string inn)
        {
            if ((inn.Length != 10) && (inn.Length != 12))
                throw new NullReferenceException("Поле ИНН должно содержать 10 или 12 цифр");

            if ((inn != organization.INN) && (inn.Length == 12))
            {
                if (MessageBox.Show("Данная организация является ИП?", "ИНН содержит 12 цифр", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    throw new NullReferenceException("Перед сохранением необходимо исправить поле ИНН");
            }
        }

        public static bool IsEmail(string address)
        {
            address = address.Replace(" ", "");

            if (address == string.Empty)
                return true;

            return address.First() != '@' && address.First() != '.' && address.Last() != '@' && address.Last() != '.' && address.Contains('@') && address.Contains('.');
        }

        public static bool IsWebSite(string address)
        {
            address = address.Replace(" ", "");

            if (address == string.Empty)
                return true;

            return address.Last() != '.' && !address.Contains('@') && address.Contains('.');
        }
    }
}
