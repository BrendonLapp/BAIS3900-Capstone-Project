using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class AboutModel : PageModel
    {
        #region Capstone One Properties
        [BindProperty]
        public string CapstoneOneSundayHours { get; set; }
        [BindProperty]
        public string CapstoneOneMondayHours { get; set; }
        [BindProperty]
        public string CapstoneOneTuesdayHours { get; set; }
        [BindProperty]
        public string CapstoneOneWednesdayHours { get; set; }
        [BindProperty]
        public string CapstoneOneThursdayHours { get; set; }
        [BindProperty]
        public string CapstoneOneFridayHours { get; set; }
        [BindProperty]
        public string CapstoneOneSaturdayHours { get; set; }
        [BindProperty]
        public string CapstoneOneHolidaysHours { get; set; }
        [BindProperty]
        public string CapstoneOnePhoneNumber { get; set; }
        [BindProperty]
        public string CapstoneOneEmail { get; set; }
        [BindProperty]
        public string CapstoneOneAddress { get; set; }
        [BindProperty]
        public string CapstoneOneAddressLink { get; set; }
        #endregion

        #region Capstone Two Properties
        [BindProperty]
        public string CapstoneTwoSundayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoMondayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoTuesdayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoWednesdayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoThursdayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoFridayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoSaturdayHours { get; set; }
        [BindProperty]
        public string CapstoneTwoHolidaysHours { get; set; }
        [BindProperty]
        public string CapstoneTwoPhoneNumber { get; set; }
        [BindProperty]
        public string CapstoneTwoEmail { get; set; }
        [BindProperty]
        public string CapstoneTwoAddress { get; set; }
        [BindProperty]
        public string CapstoneTwoAddressLink { get; set; }
        #endregion

        #region Capstone Three Properties
        [BindProperty]
        public string CapstoneThreeSundayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeMondayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeTuesdayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeWednesdayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeThursdayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeFridayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeSaturdayHours { get; set; }
        [BindProperty]
        public string CapstoneThreeHolidaysHours { get; set; }
        [BindProperty]
        public string CapstoneThreePhoneNumber { get; set; }
        [BindProperty]
        public string CapstoneThreeEmail { get; set; }
        [BindProperty]
        public string CapstoneThreeAddress { get; set; }
        [BindProperty]
        public string CapstoneThreeAddressLink { get; set; }
        #endregion

        [BindProperty]
        public string Submit { get; set; }

        private CapstoneInfoController RequestDirector = new CapstoneInfoController();

        public void OnGet()
        {
            PopulateWithCapstoneInfo();
        }//end OnGet

        public void OnPost()
        {
            if (User.IsInRole("Admin"))
            {
                switch (Submit)
                {
                    case "Update":
                        //Declare                  Instantiate   a CapstoneInfo object  
                        CapstoneInfo CapstoneInfoToSend = new CapstoneInfo();
                        //Here, we set the new CapstoneInfoToSend CapstoneInfo object's properties.
                        #region Capstone One to Modify
                        CapstoneInfoToSend.CapstoneStoreName = "Capstone I";
                        CapstoneInfoToSend.CapstoneSunday = CapstoneOneSundayHours;
                        CapstoneInfoToSend.CapstoneMonday = CapstoneOneMondayHours;
                        CapstoneInfoToSend.CapstoneTuesday = CapstoneOneTuesdayHours;
                        CapstoneInfoToSend.CapstoneWednesday = CapstoneOneWednesdayHours;
                        CapstoneInfoToSend.CapstoneThursday = CapstoneOneThursdayHours;
                        CapstoneInfoToSend.CapstoneFriday = CapstoneOneFridayHours;
                        CapstoneInfoToSend.CapstoneSaturday = CapstoneOneSaturdayHours;
                        CapstoneInfoToSend.CapstoneHoliday = CapstoneOneHolidaysHours;
                        CapstoneInfoToSend.PhoneNumber = CapstoneOnePhoneNumber;
                        CapstoneInfoToSend.Email = CapstoneOneEmail;
                        CapstoneInfoToSend.Address = CapstoneOneAddress;
                        CapstoneInfoToSend.AddressLink = CapstoneOneAddressLink;
                        #endregion

                        RequestDirector.ModifyCapstoneInfo(CapstoneInfoToSend);

                        #region Capstone Two to Modify
                        CapstoneInfoToSend.CapstoneStoreName = "Capstone II";
                        CapstoneInfoToSend.CapstoneSunday = CapstoneTwoSundayHours;
                        CapstoneInfoToSend.CapstoneMonday = CapstoneTwoMondayHours;
                        CapstoneInfoToSend.CapstoneTuesday = CapstoneTwoTuesdayHours;
                        CapstoneInfoToSend.CapstoneWednesday = CapstoneTwoWednesdayHours;
                        CapstoneInfoToSend.CapstoneThursday = CapstoneTwoThursdayHours;
                        CapstoneInfoToSend.CapstoneFriday = CapstoneTwoFridayHours;
                        CapstoneInfoToSend.CapstoneSaturday = CapstoneTwoSaturdayHours;
                        CapstoneInfoToSend.CapstoneHoliday = CapstoneTwoHolidaysHours;
                        CapstoneInfoToSend.PhoneNumber = CapstoneTwoPhoneNumber;
                        CapstoneInfoToSend.Email = CapstoneTwoEmail;
                        CapstoneInfoToSend.Address = CapstoneTwoAddress;
                        CapstoneInfoToSend.AddressLink = CapstoneTwoAddressLink;
                        #endregion

                        RequestDirector.ModifyCapstoneInfo(CapstoneInfoToSend);

                        #region Capstone Three to Modify
                        CapstoneInfoToSend.CapstoneStoreName = "Capstone III";
                        CapstoneInfoToSend.CapstoneSunday = CapstoneThreeSundayHours;
                        CapstoneInfoToSend.CapstoneMonday = CapstoneThreeMondayHours;
                        CapstoneInfoToSend.CapstoneTuesday = CapstoneThreeTuesdayHours;
                        CapstoneInfoToSend.CapstoneWednesday = CapstoneThreeWednesdayHours;
                        CapstoneInfoToSend.CapstoneThursday = CapstoneThreeThursdayHours;
                        CapstoneInfoToSend.CapstoneFriday = CapstoneThreeFridayHours;
                        CapstoneInfoToSend.CapstoneSaturday = CapstoneThreeSaturdayHours;
                        CapstoneInfoToSend.CapstoneHoliday = CapstoneThreeHolidaysHours;
                        CapstoneInfoToSend.PhoneNumber = CapstoneThreePhoneNumber;
                        CapstoneInfoToSend.Email = CapstoneThreeEmail;
                        CapstoneInfoToSend.Address = CapstoneThreeAddress;
                        CapstoneInfoToSend.AddressLink = CapstoneThreeAddressLink;
                        #endregion

                        RequestDirector.ModifyCapstoneInfo(CapstoneInfoToSend);
                        break;
                    default:
                        break;
                }//end switch(Submit)
            }//end if(User.IsInRole("Admin"))

            PopulateWithCapstoneInfo();
        }//end OnPost

        public void PopulateWithCapstoneInfo()
        {
            CapstoneInfo CapstoneOneInfo = RequestDirector.RetrieveCapstoneInfo("Capstone I");
            CapstoneInfo CapstoneTwoInfo = RequestDirector.RetrieveCapstoneInfo("Capstone II");
            CapstoneInfo CapstoneThreeInfo = RequestDirector.RetrieveCapstoneInfo("Capstone III");

            #region Capstone One To Populate
            CapstoneOneSundayHours = CapstoneOneInfo.CapstoneSunday;
            CapstoneOneMondayHours = CapstoneOneInfo.CapstoneMonday;
            CapstoneOneTuesdayHours = CapstoneOneInfo.CapstoneTuesday;
            CapstoneOneWednesdayHours = CapstoneOneInfo.CapstoneWednesday;
            CapstoneOneThursdayHours = CapstoneOneInfo.CapstoneThursday;
            CapstoneOneFridayHours = CapstoneOneInfo.CapstoneFriday;
            CapstoneOneSaturdayHours = CapstoneOneInfo.CapstoneSaturday;
            CapstoneOneHolidaysHours = CapstoneOneInfo.CapstoneHoliday;
            CapstoneOnePhoneNumber = CapstoneOneInfo.PhoneNumber;
            CapstoneOneEmail = CapstoneOneInfo.Email;
            CapstoneOneAddress = CapstoneOneInfo.Address;
            CapstoneOneAddressLink = CapstoneOneInfo.AddressLink;
            #endregion

            #region Capstone Two to Populate
            CapstoneTwoSundayHours = CapstoneTwoInfo.CapstoneSunday;
            CapstoneTwoMondayHours = CapstoneTwoInfo.CapstoneMonday;
            CapstoneTwoTuesdayHours = CapstoneTwoInfo.CapstoneTuesday;
            CapstoneTwoWednesdayHours = CapstoneTwoInfo.CapstoneWednesday;
            CapstoneTwoThursdayHours = CapstoneTwoInfo.CapstoneThursday;
            CapstoneTwoFridayHours = CapstoneTwoInfo.CapstoneFriday;
            CapstoneTwoSaturdayHours = CapstoneTwoInfo.CapstoneSaturday;
            CapstoneTwoHolidaysHours = CapstoneTwoInfo.CapstoneHoliday;
            CapstoneTwoPhoneNumber = CapstoneTwoInfo.PhoneNumber;
            CapstoneTwoEmail = CapstoneTwoInfo.Email;
            CapstoneTwoAddress = CapstoneTwoInfo.Address;
            CapstoneTwoAddressLink = CapstoneTwoInfo.AddressLink;
            #endregion

            #region Capstone Three to Populate
            CapstoneThreeSundayHours = CapstoneThreeInfo.CapstoneSunday;
            CapstoneThreeMondayHours = CapstoneThreeInfo.CapstoneMonday;
            CapstoneThreeTuesdayHours = CapstoneThreeInfo.CapstoneTuesday;
            CapstoneThreeWednesdayHours = CapstoneThreeInfo.CapstoneWednesday;
            CapstoneThreeThursdayHours = CapstoneThreeInfo.CapstoneThursday;
            CapstoneThreeFridayHours = CapstoneThreeInfo.CapstoneFriday;
            CapstoneThreeSaturdayHours = CapstoneThreeInfo.CapstoneSaturday;
            CapstoneThreeHolidaysHours = CapstoneThreeInfo.CapstoneHoliday;
            CapstoneThreePhoneNumber = CapstoneThreeInfo.PhoneNumber;
            CapstoneThreeEmail = CapstoneThreeInfo.Email;
            CapstoneThreeAddress = CapstoneThreeInfo.Address;
            CapstoneThreeAddressLink = CapstoneThreeInfo.AddressLink;
            #endregion
        }
    }
}
