using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class Availability
    {

        public Availability()
        {

            DTSundayStart =
                DTSundayEnd =
                    DTSaturdayStart =
                        DTSundayEnd =
                            DTMondayStart =
                                DTMondayEnd =
                                    DTTuesdayStart =
                                        DTTuesdayEnd =
                                            DTWednesdayStart =
                                                DTWednesdayEnd =
                                                    DTThursdayStart =
                                                        DTThursdayEnd =
                                                            DTFridayStart =
                                                                DTFridayEnd =
                                                                    DTSaturdayStart =
                                                                        DTSaturdayEnd = new DateTime(1970, 1, 1);
        }

        [Key]
        [Required]
        public int ID { get; set; }

        public DateTime DTSundayStart { get; set; }

        [NotMapped]
        public TimeSpan SundayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTSundayStart); }
            set { DTSundayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTMondayStart { get; set; }

        [NotMapped]
        public TimeSpan MondayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTMondayStart); }
            set { DTMondayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTTuesdayStart { get; set; }

        [NotMapped]
        public TimeSpan TuesdayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTTuesdayStart); }
            set { DTTuesdayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTWednesdayStart { get; set; }

        [NotMapped]
        public TimeSpan WednesdayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTWednesdayStart); }
            set { DTWednesdayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTThursdayStart { get; set; }

        [NotMapped]
        public TimeSpan ThursdayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTThursdayStart); }
            set { DTThursdayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTFridayStart { get; set; }

        [NotMapped]
        public TimeSpan FridayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTFridayStart); }
            set { DTFridayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTSaturdayStart { get; set; }

        [NotMapped]
        public TimeSpan SaturdayStart
        {
            get { return DateTimeHelper.ConvertDateTime(DTSaturdayStart); }
            set { DTSaturdayStart = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTSundayEnd { get; set; }

        [NotMapped]
        public TimeSpan SundayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTSundayEnd); }
            set { DTSundayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTMondayEnd { get; set; }

        [NotMapped]
        public TimeSpan MondayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTMondayEnd); }
            set { DTMondayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTTuesdayEnd { get; set; }

        [NotMapped]
        public TimeSpan TuesdayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTTuesdayEnd); }
            set { DTTuesdayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTWednesdayEnd { get; set; }

        [NotMapped]
        public TimeSpan WednesdayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTWednesdayEnd); }
            set { DTWednesdayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTThursdayEnd { get; set; }

        [NotMapped]
        public TimeSpan ThursdayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTThursdayEnd); }
            set { DTThursdayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTFridayEnd { get; set; }

        [NotMapped]
        public TimeSpan FridayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTFridayEnd); }
            set { DTFridayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTSaturdayEnd { get; set; }

        [NotMapped]
        public TimeSpan SaturdayEnd
        {
            get { return DateTimeHelper.ConvertDateTime(DTSaturdayEnd); }
            set { DTSaturdayEnd = DateTimeHelper.ConvertTimeSpan(value); }
        }

    }
}
