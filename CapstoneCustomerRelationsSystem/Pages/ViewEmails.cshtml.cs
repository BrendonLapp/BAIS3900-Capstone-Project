using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CapstoneCustomerRelationsSystem.Pages
{
    [Authorize(Roles ="Admin, Employee, Manager")]
    public class ViewEmailsModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 15;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        [BindProperty]
        public List<Message> Inbox { get; set; }

        public void OnGet(int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;

            if (Inbox == null)
            {
                ContactController ContactController = new ContactController();

                Inbox = ContactController.GetEmails();
            }

            Count = Inbox.Count();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            Inbox = Inbox
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

        }//End OnGet

        public void OnPost()
        {
        }//End OnPost
    }
}
