using System;
using System.Data;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public class Visit
    {
        private const int MAX_HOUR_IN_DAY = 20;

        public enum VisitStatus { Planed = 1, Complited = 2, Uncomplited = 3 };
        
        private int _id;
        private int _idUser;
        private int _idULPU;
        private VisitStatus _status;
        private int _idActivity;
        private DateTime _dateVisit;
        private string _plan;
        private string _fact;
        private string _next;
        private string _commRD;
        private bool _haveNewVisit;
        private DateTime _dateNewVisit;
        private bool _editPlan;
        private bool _editFact;

        private Sql _sql1;

        public int IdULPU { get { return _idULPU; } }
        public string Plan { get { return _plan; } }
        public string Fact { get { return _fact; } }
        public string Next { get { return _next; } }
        public int IdActivity { get { return _idActivity; } }
        public string CommRD { get { return _commRD; } }
        public VisitStatus Status { get { return _status; } }
        public bool HaveNewVisitDate { get { return _haveNewVisit; } }
        public DateTime NewVisitDate { get { return _dateNewVisit; } }
        public DateTime DateVisit { get { return _dateVisit; } }
        public bool canEditPlan { get { return _editPlan; } }
        public bool canEditFact { get { return _editFact; } }

        public Visit(int idUser, DateTime date)
        {
            _id = 0;
            _idUser = idUser;
            _dateVisit = date;

            _idULPU = 0;
            _plan = string.Empty;
            _fact = string.Empty;
            _next = string.Empty;
            _idActivity = 1;
            _commRD = string.Empty;
            _status = VisitStatus.Planed;
            _haveNewVisit = false;

            Init();
            SetDefaultHour();
            SetEditRightVisit();
        }

        public Visit(Visit visit1, DateTime date)
        {
            _id = 0;
            this._idUser = visit1._idUser;
            this._dateVisit = date;
            this._idULPU = visit1._idULPU;
            this._plan = visit1._plan;

            _fact = string.Empty;
            _next = string.Empty;
            _idActivity = 1;
            _commRD = string.Empty;
            _status = VisitStatus.Planed;
            _haveNewVisit = false;

            Init();
            SetDefaultHour();
            SetEditRightVisit();
        }

        public Visit(int id, int idUser)
        {
            this._id = id;
            this._idUser = idUser;
            Init();
            getVisitAndFillFields();
            SetEditRightVisit();
        }

        private void Init()
        {
            _sql1 = new Sql();
        }

        private void SetDefaultHour()
        {
            if (_dateVisit.Hour == 0)
                _dateVisit = Convert.ToDateTime(_dateVisit.Year.ToString() + "-" + _dateVisit.Month.ToString() + "-" + _dateVisit.Day + " 10:00");
        }

        private void getVisitAndFillFields()
        {
            DataTable dt1 = _sql1.GetRecords("exec VisitPlanDay_Select_ByID @p1, @p2", _id, _idUser);

            int.TryParse(dt1.Rows[0].ItemArray[2].ToString(), out _idULPU);
            DateTime.TryParse(dt1.Rows[0].ItemArray[3].ToString(), out _dateVisit);
            _plan = dt1.Rows[0].ItemArray[4].ToString();
            _fact = dt1.Rows[0].ItemArray[5].ToString();
            _next = dt1.Rows[0].ItemArray[6].ToString();
            int.TryParse(dt1.Rows[0].ItemArray[7].ToString(), out _idActivity);
            _commRD = dt1.Rows[0].ItemArray[8].ToString();
            _status = (VisitStatus)dt1.Rows[0].ItemArray[9];

            if (dt1.Rows[0].ItemArray[10].ToString() != string.Empty)
            {
                _haveNewVisit = true;
                DateTime.TryParse(dt1.Rows[0].ItemArray[10].ToString(), out _dateNewVisit);
            }
            else
                _haveNewVisit = false;
        }

        public DataTable GetULPUList()
        {
            return _sql1.GetRecords("exec SelULPUbyUserIDVisPlan @p1", _idUser);
        }
        
        public bool IsMyVisit(int idUser)
        {
            return _idUser == idUser;
        }

        public bool IsVacantTimeAndSetNewTime()
        {
            if (_id != 0)
                return true;

            while (_sql1.GetRecordsOne("exec VisitPlanDay_IsVacantDateTime @p1, @p2", _idUser, _dateVisit) != "0")
            {
                int hour = _dateVisit.Hour;
                hour++;

                if (hour == MAX_HOUR_IN_DAY)
                    return false;

                SetNewTime(hour);
            }
            return true;
        }

        private void SetNewTime(int hour)
        {
            _dateVisit = Convert.ToDateTime(_dateVisit.Year.ToString() + "-" + _dateVisit.Month.ToString() + "-" + _dateVisit.Day + " " + hour.ToString() + ":00");
        }

        public void Save(int userIDRD, int idStatus, int idULPU, int idActivity, string plan, string fact, string next, string commRD, int haveNewVisit, DateTime dateNewVisit)
        {
            AddSignature(userIDRD, commRD);

            _plan = ReplaceUnprintedSymbols(plan);
            _fact = ReplaceUnprintedSymbols(fact);
            _next = ReplaceUnprintedSymbols(next);

            _sql1.GetRecords("exec VisitPlanDay_Insert @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12", _id, _idUser, idULPU, _dateVisit, _plan, _fact,
                _next, idActivity, _commRD, idStatus, haveNewVisit, dateNewVisit);
        }

        private void AddSignature(int userIDRD, string commRD)
        {
            if ((commRD.Trim() != string.Empty) && (this._commRD.Trim() != commRD.Trim()))
            {
                DataTable dt1 = _sql1.GetRecords("exec GetUserByID @p1", userIDRD);

                if (dt1.Rows.Count > 0)
                    this._commRD = commRD.Trim() + " (доб. " + dt1.Rows[0].ItemArray[0].ToString() + ")";
            }
            else if (commRD.Trim() == string.Empty)
            {
                this._commRD = string.Empty;
            }
        }

        private string ReplaceUnprintedSymbols(string text)
        {
            return text.Replace("\r", "").Replace("\n", " ");
        }

        public bool IsValidNewDate(DateTime newDate)
        {
            return IsDateMoreBaseDate(newDate) && IsDateNowOrFuture(newDate);
        }

        private bool IsDateMoreBaseDate(DateTime newDate)
        {
            return (newDate.Year > _dateVisit.Year) || ((newDate.Year == _dateVisit.Year) && (newDate.DayOfYear > _dateVisit.DayOfYear));
        }

        private bool IsDateNowOrFuture(DateTime newDate)
        {
            DateTime serverDate = Convert.ToDateTime(_sql1.GetRecordsOne("exec GetDateServer"));

            return (newDate.Year > serverDate.Year) || ((newDate.Year == serverDate.Year) && (newDate.DayOfYear >= serverDate.DayOfYear));
        }

        private void SetEditRightVisit()
        {
            DateTime serverDate = Convert.ToDateTime(_sql1.GetRecordsOne("exec GetDateServer"));

            int dayOfYearCurrDay = serverDate.DayOfYear;
            int minDay = dayOfYearCurrDay - GetMinusDay(serverDate.DayOfWeek);
            int dayOfYearVisit = _dateVisit.DayOfYear;

            _editPlan = ((dayOfYearVisit >= minDay) || (_dateVisit.Year > serverDate.Year));

            _editFact = (((dayOfYearVisit >= minDay) && (dayOfYearVisit <= dayOfYearCurrDay)) || (_dateVisit.Year > serverDate.Year));
        }

        private int GetMinusDay(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    {
                        return 2;
                    }
                case DayOfWeek.Monday:
                    {
                        return 3;
                    }
                default:
                    {
                        return 1;
                    }
            }
        }

        public void Delete()
        {
            _sql1.GetRecords("exec VisitPlanDay_Delete @p1, @p2", _id, _idUser);
        }

        public static DataTable GetDataTableForDayByUser(int userID, DateTime date)
        {
            Sql sql1 = new Sql();
            return sql1.GetRecords("exec VisitPlanDay_Select_ByUser @p1, @p2", userID, date);
        }

        public static DataTable GetDataTableForMonthByUser(int userID, DateTime date)
        {
            Sql sql1 = new Sql();
            return sql1.GetRecords("exec VisitPlanDay_Select_ByMonthByUser @p1, @p2", userID, date);
        }

        public string GetPlanAndTime()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_dateVisit.Hour.ToString());
            sb.Append(":00 ");
            sb.Append(_plan);

            return sb.ToString();
        }
    }
}
