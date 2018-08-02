using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_LMS.Models;

namespace Project_LMS.ScheduleUI
{
    public class ScheduleManage
    {
        private ApplicationDbContext DataBase { get; set; }
        private Course TheCourse { get; set; }
        private ScheduleViewModels[] samling;
        public int MaxDates { get; private set; }
        private int CourseId { get; set; }
        private string CourseName { get; set; }

        public ScheduleManage()
        {
            MaxDates = 0;
            CourseId = 0;
            CourseName = "UNKNOW";
        }

        public ScheduleManage(DateTime startDate, DateTime endDate)
        {
            MaxDates = 0;
            InitScheduleSize(startDate, endDate);
            InitScheduleDates(startDate, endDate);
        }


        public ScheduleManage(ApplicationDbContext db, Course course)
        {
            TheCourse = course;
            DataBase = db;
            MaxDates = 0;
            CourseId = course.CourseId;
            CourseName = course.CourseName;
            var courseStartDate = course.StartDate;
            var courseEndDate = course.EndDate;
            InitScheduleSize(courseStartDate, courseEndDate);
            InitScheduleDates(courseStartDate, courseEndDate);
        }

        public ScheduleHeadViewModels RunAndGetList()
        {
            var newSchedule = new ScheduleHeadViewModels();
            newSchedule.CourseId = CourseId;
            newSchedule.CourseName = CourseName;

            // add data from Modules
            var listModules = DataBase.Modules.Where(m => m.CourseId == TheCourse.CourseId).OrderBy(m => m.StartDate).OrderBy(m => m.EndDate);
            foreach (var module in listModules)
            {
                var module_StartDate = module.StartDate;
                var module_StoppDate = module.EndDate;
                var module_Name = module.Name;
                //
                AddModules(module_StartDate, module_StoppDate, module_Name);
            }

            var listActivites = TheCourse.CourseModules.SelectMany(m => m.Activities).OrderBy(a => a.Start).OrderBy(a => a.End);
            foreach (var activity in listActivites)
            {
                var activity_StartDate = activity.Start;
                var activity_StoppDate = activity.End;
                var activity_Name = activity.ActivityName;
                var activity_Type = activity.ActivityType.Type;

                AddActivites(activity_StartDate, activity_StoppDate, activity_Name, activity_Type);
            }

            AddHolidays();

            newSchedule.myList = GetList();

            return newSchedule;
        }


        private void InitScheduleSize(DateTime courseStartDate, DateTime courseEndDate)
        {

            while (courseEndDate >= courseStartDate)
            {
                courseStartDate = courseStartDate.AddDays(1);
                MaxDates += 1;
            }
        }

        private void InitScheduleDates(DateTime courseStartDate, DateTime courseEndDate)
        {
            samling = new ScheduleViewModels[MaxDates];

            for (int i = 0; i < MaxDates; i++)
            {
                samling[i] = new ScheduleViewModels
                {
                    Date = courseStartDate.ToString("yyyy-MM-dd"),
                    Day = courseStartDate.DayOfWeek.ToString(),
                    Modul = "",
                    PM = "",
                    AM = "",
                    ActuallDate = courseStartDate,
                    Year = courseStartDate.ToString("yyyy"),
                    DayOfYear = courseStartDate.DayOfYear,
                    ErrModul = new List<string>(),
                    ErrAM = new List<string>(),
                    ErrPM = new List<string>()
                };

                if (i % 2 == 0) { samling[i].bgColor = "LightSteelBlue"; }
                else samling[i].bgColor = "#ffffff";
                courseStartDate = courseStartDate.AddDays(1);
            }
        }


        // UpdateModelName
        private ScheduleViewModels UpdateModelName(ScheduleViewModels model, string name)
        {
            if (model.Modul == "")
            {
                model.Modul = name;
            }
            else
            {
                model.ErrModul.Add(name);
            }

            return model;
        }

        // UpdatePMName
        private ScheduleViewModels UpdatePMName(ScheduleViewModels model, string name)
        {
            if (model.PM == "")
            {
                model.PM = name;
            }
            else
            {
                model.ErrPM.Add(name);
            }

            return model;
        }

        // UpdateAMName
        private ScheduleViewModels UpdateAMName(ScheduleViewModels model, string name)
        {
            if (model.AM == "")
            {
                model.AM = name;
            }
            else
            {
                model.ErrAM.Add(name);
            }

            return model;
        }

        public void AddModules(DateTime startDate, DateTime endDate, string aName)
        {
            var moduleStartDate = startDate;
            var moduleStoppDate = endDate;

            // Lägg in data från Module för kursen
            while (moduleStoppDate >= moduleStartDate)
            {
                var dag = moduleStartDate.DayOfYear;
                var year = moduleStartDate.ToString("yyyy");
                var modul = aName;
                var found = false;
                var counter = 0;

                while (found == false)
                {
                    if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                    {
                        samling[counter] = UpdateModelName(samling[counter], modul);
                        found = true;
                    }
                    counter += 1;
                    if (counter > MaxDates) found = true; //Säkerhet
                }

                moduleStartDate = moduleStartDate.AddDays(1);
            }
        }

        private void AddHomeWork(DateTime startDate, string aName)
        {
            var dag = startDate.DayOfYear;
            var year = startDate.ToString("yyyy");
            var name = aName;
            var found = false;
            var counter = 0;

            while (found == false)
            {
                if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                {
                    samling[counter].Extern = "HomeWork";
                    found = true;
                }

                counter += 1;
                if (counter >= MaxDates) found = true; //Säkerhet
            }
        }

        private void CheckActivityDateHourAndUpdateNames(int startHour, int stoppHour, int counter, string name)
        {
            if (startHour >= 13)
            {
                samling[counter] = UpdatePMName(samling[counter], name);
            }
            else if (startHour >= 0 && startHour < 13)
            {
                samling[counter] = UpdateAMName(samling[counter], name);

                if (stoppHour > 13 && stoppHour <= 24)
                {
                    samling[counter] = UpdatePMName(samling[counter], name);
                }
            }
            else
            {
                samling[counter] = UpdatePMName(samling[counter], name);
            }
        }


        public bool AddActivites(DateTime startDate, DateTime endDate, string aName, string aType)
        {
            if (aType == "Homework")
            {
                AddHomeWork(endDate, aName);
                return true;
            }

            var activiStartDate = startDate;
            var activiStoppDate = endDate;
            var firstTime = true;

            while (activiStoppDate.Date >= activiStartDate.Date)
            {
                var dag = activiStartDate.DayOfYear;
                var year = activiStartDate.ToString("yyyy");
                var name = aName;
                var found = false;
                var counter = 0;

                while (found == false)
                {
                    if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                    {
                        if (activiStoppDate.Date == activiStartDate.Date)
                        {
                            CheckActivityDateHourAndUpdateNames(activiStartDate.Hour, activiStoppDate.Hour, counter, name);
                        }
                        else if (activiStoppDate.Date > activiStartDate.Date)
                        {
                            if (firstTime == true)
                            {
                                CheckActivityDateHourAndUpdateNames(activiStartDate.Hour, activiStoppDate.Hour, counter, name);
                                firstTime = false;
                            }
                            else
                            {
                                samling[counter] = UpdateAMName(samling[counter], name);
                                samling[counter] = UpdatePMName(samling[counter], name);
                            }
                        }

                        if (aType == "Lecture")
                        {
                            samling[counter].Extern = "Extern";
                        }
                        else if (aType == "Holiday")
                        {
                            samling[counter].Extern = "Holiday";
                        }

                        found = true;
                    }

                    counter += 1;
                    if (counter >= MaxDates) found = true; //Säkerhet
                }

                activiStartDate = activiStartDate.AddDays(1);
            }

            return true;
        }


        private void ClearFields(int i, string bgColor)
        {
            samling[i].Modul = "";
            samling[i].AM = "";
            samling[i].PM = "";
            samling[i].Extern = "";
            samling[i].ErrModul = new List<string>();
            samling[i].ErrAM = new List<string>();
            samling[i].ErrPM = new List<string>();
            samling[i].bgColor = bgColor;
        }

        public void AddHolidays()
        {
            for (int i = 0; i < MaxDates; i++)
            {
                // Om lördag eller söndag, blanka fälten sam sätt backgrundsfärgen till Helgdag
                var bgColorHoliday = "#FFDAB9";
                var dayOfWeek = samling[i].ActuallDate.DayOfWeek.ToString();
                if (dayOfWeek == "Saturday" || dayOfWeek == "Sunday")
                {
                    ClearFields(i, bgColorHoliday);
                }

                var dag = samling[i].DayOfYear;
                var year = samling[i].Year;
                if (year == "2018" && (dag == 1 || dag == 6 || dag == 88 || dag == 89 | dag == 90 || dag == 91 || dag == 92 || dag == 121 || dag == 130 || dag == 140 || dag == 157 || dag == 174 || dag == 307 || dag == 359 || dag == 360))
                {
                    ClearFields(i, bgColorHoliday);
                }

                if (year == "2019" && (dag == 1 || dag == 6 || dag == 109 | dag == 111 || dag == 112 || dag == 150 || dag == 157 || dag == 160 || dag == 173 || dag == 306 || dag == 359 || dag == 360))
                {
                    ClearFields(i, bgColorHoliday);
                }

                if (samling[i].Extern == "Holiday")
                {
                    samling[i].bgColor = bgColorHoliday;
                }
            }
        }

        public List<ScheduleViewModels> GetList()
        {
            var myList = new List<ScheduleViewModels>();

            for (int i = 0; i < MaxDates; i++)
            {
                var model = new ScheduleViewModels();
                model = GetOneLine(i);
                myList.Add(model);
            }

            return myList;
        }

        private ScheduleViewModels GetOneLine(int i)
        {
            return samling[i];
        }

    }
}