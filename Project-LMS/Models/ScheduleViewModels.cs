using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class ScheduleViewModels
    {
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }
        [Display(Name = "Modul")]
        public string Modul { get; set; }
        [Display(Name = "AM (08:00 - 12:00)")]
        public String AM { get; set; }
        [Display(Name = "PM (13:00 - 17:00)")]
        public String PM { get; set; }
        [Display(Name = "Comments")]
        public String Extern { get; set; }

        public string Year { get; set; }
        public int DayOfYear { get; set; }
        public DateTime ActuallDate { get; set; }
        public string bgColor { get; set; }
        public List<string> ErrModul { get; set; }
        public List<string> ErrAM { get; set; }
        public List<string> ErrPM { get; set; }


    }

    public class MyScheduleViewModel
    {
        private ScheduleViewModels[] samling;
        public int MaxDates { get; }

        public MyScheduleViewModel(DateTime startDate, DateTime endDate)
        {
            var maxDates = 0;
            while (endDate >= startDate)
            {
                startDate = startDate.AddDays(1);
                maxDates += 1;
            }

            MaxDates = maxDates;
            samling = new ScheduleViewModels[maxDates];

            for (int i = 0; i <= maxDates; i++)
            {
                samling[i] = new ScheduleViewModels();
                samling[i].Date = startDate.ToString("yyyy-MM-dd");
                samling[i].Day = startDate.DayOfWeek.ToString();
                samling[i].Modul = "";
                samling[i].PM = "";
                samling[i].AM = "";
                samling[i].ActuallDate = startDate;
                samling[i].Year = startDate.ToString("yyyy");
                samling[i].DayOfYear = startDate.DayOfYear;
                samling[i].ErrModul = new List<string>();
                samling[i].ErrAM = new List<string>();
                samling[i].ErrPM = new List<string>();

                if (i % 2 == 0) { samling[i].bgColor = "LightSteelBlue"; }
                else samling[i].bgColor = "#ffffff";
                startDate = startDate.AddDays(1);
            }

        }

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
                if (counter > MaxDates) found = true; //Säkerhet
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
                    counter += 1;
                    if (counter > MaxDates) found = true; //Säkerhet
                }

                activiStartDate = activiStartDate.AddDays(1);
            }

            return true;
        }
    }


    public class ScheduleHeadViewModels
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public ScheduleViewModels ViewModels { get; set; }
        public List<ScheduleViewModels> myList;
    }
}