using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Availability
    {
        public Availability()
        {
            SundayStart = SundayEnd = MondayStart = MondayEnd = TuesdayStart = TuesdayEnd = WednesdayStart = WednesdayEnd =
                ThursdayStart = ThursdayEnd = FridayStart = FridayEnd = SaturdayStart = SaturdayEnd = new DateTime(1970, 1, 1);
        }

        [Key]
        [Required]
        public int ID { get; set; }

        public DateTime SundayStart { get; set; }

        public DateTime MondayStart { get; set; }

        public DateTime TuesdayStart { get; set; }

        public DateTime WednesdayStart { get; set; }

        public DateTime ThursdayStart { get; set; }

        public DateTime FridayStart { get; set; }

        public DateTime SaturdayStart { get; set; }

        public DateTime SundayEnd { get; set; }

        public DateTime MondayEnd { get; set; }

        public DateTime TuesdayEnd { get; set; }

        public DateTime WednesdayEnd { get; set; }

        public DateTime ThursdayEnd { get; set; }

        public DateTime FridayEnd { get; set; }

        public DateTime SaturdayEnd { get; set; }

    }
}
